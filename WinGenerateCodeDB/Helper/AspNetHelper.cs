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
        private static string db_name = string.Empty;
        private static List<string> tableList = new List<string>();
        private static Dictionary<string, List<SqlColumnInfo>> tbDic = new Dictionary<string, List<SqlColumnInfo>>();

        public static void InitDbName(string database_name)
        {
            db_name = database_name;
        }

        public static void InitTables(List<string> tbList)
        {
            tbDic = new Dictionary<string, List<SqlColumnInfo>>();
            tableList = tbList;
        }

        public static void AddColumnList(string tbName, List<SqlColumnInfo> list)
        {
            if (!tbDic.ContainsKey(tbName))
            {
                tbDic.Add(tbName, list);
            }
        }

        public static Dictionary<string, string> CreateModel(string name_space, string model_suffix, bool isAddQueryModel, bool isCodeSplit)
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

        public static Dictionary<string, string> CreateDAL(string name_space, string dal_suffix, string model_suffix)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            DALHelper_DapperCore helper = new DALHelper_DapperCore(db_name, name_space, dal_suffix, model_suffix);
            foreach (var item in tbDic)
            {
                string text = helper.CreateDAL(item.Key, item.Value);

                result.Add(item.Key + dal_suffix, text);
            }

            return result;
        }

        public static Dictionary<string, string> CreateApiController(string name_space, string dal_suffix, string model_suffix)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            AspNetCoreApiController helper = new AspNetCoreApiController(name_space, dal_suffix, model_suffix);
            foreach (var item in tbDic)
            {
                string text = helper.CreateApiController(item.Key, item.Value);

                result.Add(item.Key.ToFirstUpper() + "Controller", text);
            }

            return result;
        }

        public static Dictionary<string, string> CreateOther(string name_space)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            AspNetCoreOther helper = new AspNetCoreOther(name_space);
            result.Add("ConnectionFactory", helper.CreateFactory(db_name));
            result.Add("result_info", helper.CreateResultInfo());

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
    }
}
