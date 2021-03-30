using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    /// <summary>
    /// 下一步缓存，保存下一步需要的数据
    /// </summary>
    public class Cache_Next
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

        public static string GetDbName()
        {
            return db_name;
        }

        public static List<string> GetTableList()
        {
            return tableList;
        }

        public static Dictionary<string, List<SqlColumnInfo>> GetColumnAll()
        {
            return tbDic;
        }

        public static List<SqlColumnInfo> GetColumnList(string tbName)
        {
            if (tbDic.ContainsKey(tbName))
            {
                return tbDic[tbName];
            }

            return new List<SqlColumnInfo>();
        }
    }
}
