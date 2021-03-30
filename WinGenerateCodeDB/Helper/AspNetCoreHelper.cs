using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Code;

namespace WinGenerateCodeDB
{
    public class AspNetCoreHelper
    {
        private static string db_name = string.Empty;
        private static List<string> tableList = new List<string>();
        private static Dictionary<string, List<SqlColumnInfo>> tbDic = new Dictionary<string, List<SqlColumnInfo>>();

        public static void Init()
        {
            db_name = Cache_Next.GetDbName();
            tableList = Cache_Next.GetTableList();
            tbDic = Cache_Next.GetColumnAll();
        }

        public static Dictionary<string, string> CreateModel(string name_space, string model_suffix)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            ModelHelper_DefaultCore helper = new ModelHelper_DefaultCore(name_space, model_suffix);
            foreach (var item in tbDic)
            {
                string text = helper.CreateModel(item.Key, item.Value);

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
    }
}
