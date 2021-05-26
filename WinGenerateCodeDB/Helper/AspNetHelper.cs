using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;
using WinGenerateCodeDB.Code;

namespace WinGenerateCodeDB
{
    public class AspNetHelper
    {
        private static string guid = string.Empty;
        private static string db_name = string.Empty;
        private static List<string> tableList = new List<string>();
        private static Dictionary<string, List<SqlColumnInfo>> tbDic = new Dictionary<string, List<SqlColumnInfo>>();

        public static void Init(string guid)
        {
            AspNetHelper.guid = guid;
            db_name = Cache_Next.GetDbName();
            tableList = Cache_Next.GetTableList();
            tbDic = Cache_Next.GetColumnAll();
        }

        public static Dictionary<string, string> CreateModel(string name_space, string model_suffix)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in tbDic)
            {
                string text = string.Empty;
                if (PageCache.ModelStyle == 0)
                {
                    text = ModelHelper_Default.CreateModel(name_space, item.Key, item.Value, item.Key + model_suffix);
                }
                else if (PageCache.ModelStyle == 1)
                {
                    text = ModelHelper_DefaultLowerField.CreateModel(name_space, item.Key, item.Value, item.Key + model_suffix);
                }
                else
                {
                    text = ModelHelper_DefaultAttribute.CreateModel(name_space, item.Key, item.Value, item.Key + model_suffix);
                }

                result.Add(item.Key + model_suffix, text);
            }

            return result;
        }

        public static Dictionary<string, string> CreateAspx(string name_space, string model_staff, int action)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in tbDic)
            {
                string text = string.Empty;
                if (PageCache.UIType == 0)
                {
                    text = AspxHelper_EasyUI.CreateASPX(name_space, item.Key, action, item.Value);
                }
                else if (PageCache.UIType == 1)
                {
                    text = AspxHelper_Bootstrap.CreateASPX(name_space, item.Key, action, item.Value);
                }

                result.Add(item.Key + ".aspx", text);
            }

            return result;
        }

        public static Dictionary<string, string> CreateAspxcs(string name_space, string model_staff, string dal_staff, int action)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in tbDic)
            {
                string text = string.Empty;
                if (PageCache.UIType == 0)
                {
                    text = AspxCsHelper_EasyUI.CreateASPXCS(name_space, item.Key, action, item.Value, item.Key + model_staff, item.Key + dal_staff);
                }
                else if (PageCache.UIType == 1)
                {
                    text = AspxCsHelper_Bootstrap.CreateASPXCS(name_space, item.Key, action, item.Value, item.Key + model_staff, item.Key + dal_staff);
                }

                result.Add(item.Key + ".aspx.cs", text);
            }

            return result;
        }

        public static Dictionary<string, string> CreateDAL(string name_space, string model_staff, string dal_staff, int action)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in tbDic)
            {
                string text = string.Empty;
                if (PageCache.DbTool == 0)
                {
                    if (PageCache.UIType == 0 && PageCache.DbType == 0)
                    {
                        // easyui mysql
                        text = DALHelper_EasyUI_MySql.CreateDAL(name_space, item.Key, item.Value, action, item.Key + dal_staff, item.Key + model_staff, db_name);
                    }
                    else if (PageCache.UIType == 0 && PageCache.DbType == 1)
                    {
                        // easyui mssql
                        text = DALHelper_EasyUI_MsSql.CreateDAL(name_space, item.Key, item.Value, action, item.Key + dal_staff, item.Key + model_staff, db_name);
                    }
                    else if (PageCache.UIType == 1 && PageCache.DbType == 0)
                    {
                        // bootstarp mysql
                        text = DALHelper_Bootstrap_MySql.CreateDAL(name_space, item.Key, item.Value, action, item.Key + dal_staff, item.Key + model_staff, db_name);
                    }
                    else if (PageCache.UIType == 1 && PageCache.DbType == 1)
                    {
                        // bootstarp mssql
                        text = DALHelper_Bootstrap_MsSql.CreateDAL(name_space, item.Key, item.Value, action, item.Key + dal_staff, item.Key + model_staff, db_name);
                    }
                }
                else if (PageCache.DbTool == 1)
                {
                    text = DALHelper_Dapper.CreateDAL(name_space, item.Key, item.Value, action, item.Key + dal_staff, item.Key + model_staff, db_name);
                }

                result.Add(item.Key + dal_staff + ".cs", text);
            }

            return result;
        }

        public static Dictionary<string, string> CreateFactory(string name_space)
        {
            string text = string.Empty;
            if (PageCache.DbTool == 0)
            {
                if (PageCache.DbType == 0)
                {
                    text = FactoryHelper_MySql.CreateFactory(name_space, db_name);
                }
                else if (PageCache.DbType == 1)
                {
                    text = FactoryHelper_MsSql.CreateFactory(name_space, db_name);
                }
            }
            else if (PageCache.DbTool == 1 && PageCache.DbType == 0)
            {
                text = FactoryHelper_Dapper_MySql.CreateFactory(name_space, db_name);
            }
            else if (PageCache.DbTool == 1 && PageCache.DbType == 1)
            {
                text = FactoryHelper_Dapper_MsSql.CreateFactory(name_space, db_name);
            }

            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("ConnectionFactory.cs", text);

            return result;
        }

        public static Dictionary<string, string> CreateConfig()
        {
            string text = ConfigHelper.GetConnectStringConfig(db_name, PageCache.ConnectionString);
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("Web.config", text);

            return result;
        }

        public static Dictionary<string, string> CreateView(string name_space, string model_staff, string dal_staff, string ui_staff, int action)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in tbDic)
            {
                string text = string.Empty;
                if (PageCache.UIType == 0)
                {
                    text = MvcViewHelper_EasyUI.CreateView(name_space, item.Key, action, item.Value, item.Key + model_staff, item.Key + dal_staff);
                }
                else if (PageCache.UIType == 1)
                {
                    text = MvcViewHelper_Bootstrap.CreateView(name_space, item.Key, action, item.Value, item.Key + model_staff, item.Key + dal_staff);
                }

                result.Add(item.Key + ui_staff + ".shtml", text);
            }

            return result;
        }

        public static Dictionary<string, string> CreateController(string name_space, string model_staff, string dal_staff, int action)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in tbDic)
            {
                string text = MvcControllerHelper.CreateController(name_space, item.Key, action, item.Value, item.Key + model_staff, item.Key + dal_staff);

                result.Add(item.Key + dal_staff + ".cs", text);
            }

            return result;
        }
    }
}
