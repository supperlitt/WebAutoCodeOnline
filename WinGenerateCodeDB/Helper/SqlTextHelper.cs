using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public class SqlTextHelper
    {
        public static string CreateSelectCountSql(string table_name, List<SqlColumnInfo> checkList)
        {
            string andStr = "";
            string whereStr = string.Empty;
            foreach (var item in checkList)
            {
                if (string.IsNullOrEmpty(item.TempValue))
                {
                    whereStr += (andStr + item.Name + "=@" + item.Name);
                }
                else
                {
                    whereStr += (andStr + item.Name + "=" + item.TempValue);
                }

                if (string.IsNullOrEmpty(andStr))
                {
                    andStr = " and ";
                }
            }

            return string.Format(@"string selectSql = ""select count(0) from {0} where {1}"";", table_name, whereStr);
        }
    }
}
