using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAutoEasyUI
{
    public class Tool
    {
        /// <summary>
        /// 转换成C#变量类型：int,long,等
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string ToClassType(string dbType)
        {
            switch (dbType.ToLower())
            {
                case "int":
                case "tinyint":
                    return "int";
                case "bigint":
                    return "long";
                case "datetime":
                case "date":
                    return "DateTime";
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                    return "string";
                case "decimal":
                    return "decimal";
                case "float":
                    return "float";
                case "double":
                    return "double";
                default:
                    return dbType;
            }
        }

        /// <summary>
        /// 转成字段默认值结果：string.Empty;
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string ToDefaultValue(string dbType)
        {
            switch (dbType.ToLower())
            {
                case "int":
                case "tinyint":
                    return "0";
                case "bigint":
                    return "0l";
                case "datetime":
                case "date":
                    return "DateTime.Parse(\"1970-1-1\")";
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                    return "string.Empty";
                case "decimal":
                    return "0m";
                case "float":
                    return "0f";
                case "double":
                    return "0d";
                default:
                    return "string.Empty";
            }
        }

        /// <summary>
        /// 转换成SqlDbType.Int类似字符串
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string ToDBTypeString(string dbType)
        {
            switch (dbType.ToLower())
            {
                case "int":
                    return "SqlDbType.Int";
                case "tinyint":
                    return "SqlDbType.TinyInt";
                case "bigint":
                    return "SqlDbType.BigInt";
                case "datetime":
                    return "SqlDbType.DateTime";
                case "date":
                    return "SqlDbType.Date";
                case "varchar":
                    return "SqlDbType.VarChar";
                case "nvarchar":
                    return "SqlDbType.NVarChar";
                case "char":
                    return "SqlDbType.Char";
                case "nchar":
                    return "SqlDbType.NChar";
                case "text":
                    return "SqlDbType.Text";
                case "decimal":
                    return "SqlDbType.Decimal";
                case "float":
                    return "SqlDbType.Float";
                default:
                    return "SqlDbType.VarChar";
            }
        }

        /// <summary>
        /// 转成指定格式的变量：Convert.ToInt32(sqldr["Test"])
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string ToDefaultDBValue(string dbType, string columnName, string readerStr = "sqldr")
        {
            switch (dbType.ToLower())
            {
                case "int":
                case "tinyint":
                    return string.Format("Convert.ToInt32({1}[\"{0}\"])", columnName, readerStr);
                case "bigint":
                    return string.Format("Convert.ToInt64({1}[\"{0}\"])", columnName, readerStr);
                case "datetime":
                case "date":
                    return string.Format("Convert.ToDateTime({1}[\"{0}\"])", columnName, readerStr);
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                    return string.Format("{0}[\"{1}\"].ToString()", readerStr, columnName);
                case "decimal":
                    return string.Format("Convert.ToDecimal({0}[\"{1}\"])", readerStr, columnName);
                case "float":
                    return string.Format("Convert.ToSingle({0}[\"{1}\"])", readerStr, columnName);
                case "double":
                    return string.Format("Convert.ToDouble({0}[\"{1}\"])", readerStr, columnName);
                default:
                    return string.Format("{0}[\"{1}\"].ToString()", readerStr, columnName);
            }
        }

        /// <summary>
        /// 把指定string类型变量转换成指定类型变量
        /// name="num"; dbtype="int'
        /// Convert.ToInt32(num);
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string ToStringToType(string name, string dbType)
        {
            switch (dbType.ToLower())
            {
                case "int":
                case "tinyint":
                    return string.Format("Convert.ToInt32({0})", name);
                case "bigint":
                    return string.Format("Convert.ToInt64({0})", name);
                case "datetime":
                case "date":
                    return string.Format("Convert.ToDateTime({0})", name);
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                    return name;
                case "decimal":
                    return string.Format("Convert.ToDecimal({0})", name);
                case "float":
                    return string.Format("Convert.ToSingle({0})", name);
                case "double":
                    return string.Format("Convert.ToDouble({0})", name);
                default:
                    return name;
            }
        }
    }
}
