using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB.Code
{
    public class SqlTool
    {
        /// <summary>
        /// 获取格式化之后的字符串
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static string GetFormatString(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "int":
                case "tinyint":
                case "smallint":
                    return "int";
                case "varchar":
                case "char":
                case "nvarchar":
                case "text":
                    return "string";
                case "datetime":
                case "time":
                case "date":
                case "timestamp":
                    return "DateTime";
                case "float":
                case "decimal":
                    return "decimal";
                case "memory":
                    return "double";
                default:
                    return "string";
            }
        }

        /// <summary>
        /// 获取默认值字符串
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static string GetDefaultValueStr(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "int":
                case "tinyint":
                case "smallint":
                    return "0";
                case "varchar":
                case "char":
                case "nvarchar":
                case "text":
                    return "string.Empty";
                case "datetime":
                case "time":
                case "date":
                case "timestamp":
                    return "DateTime.Parse(\"1970-1-1\")";
                case "float":
                case "decimal":
                    return "0m";
                case "memory":
                    return "0d";
                default:
                    return "string.Empty";
            }
        }

        /// <summary>
        /// 获取默认值字符串
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static string GetDefaultValueAttributeStr(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "int":
                case "tinyint":
                case "smallint":
                    return "0";
                case "varchar":
                case "char":
                case "nvarchar":
                case "text":
                    return "";
                case "datetime":
                case "time":
                case "date":
                case "timestamp":
                    return "1970-1-1";
                case "float":
                case "decimal":
                    return "0";
                case "memory":
                    return "0";
                default:
                    return "";
            }
        }
    }
}
