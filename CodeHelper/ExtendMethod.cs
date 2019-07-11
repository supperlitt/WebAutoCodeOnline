using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    public static class ExtendMethod
    {
        public static string ToFirstLower(this string key)
        {
            if (key == null)
            {
                return null;
            }
            else if (key.Length == 0)
            {
                return key;
            }
            else
            {
                return key[0].ToString().ToLower() + key.Substring(1, key.Length - 1);
            }
        }

        public static string ToFirstUpper(this string key)
        {
            if (key == null)
            {
                return null;
            }
            else if (key.Length == 0)
            {
                return key;
            }
            else
            {
                return key[0].ToString().ToUpper() + key.Substring(1, key.Length - 1);
            }
        }

        public static string ToCharpCodeShow(this string key)
        {
            string[] keys = { "abstract", "event", "new", "struct", "as", "explicit", "null", "switch", "base", "extern", "object", "this", "bool", "false", "operator", "throw", "break", "finally", "out", "true", "byte", "fixed", "override", "try", "case", "float", "params", "typeof", "catch", "for", "private", "uint", "char", "foreach", "protected", "ulong", "checked", "goto", "public", "unchecked", "class", "if", "readonly", "unsafe", "const", "implicit", "ref", "ushort", "continue", "in", "return", "using", "decimal", "int", "sbyte", "virtual", "default", "interface", "sealed", "volatile", "delegate", "internal", "short", "void", "do", "is", "sizeof", "while", "double", "lock", "stackalloc", "else", "long", "static", "enum", "namespace", "string", "get", "partial", "set", "value", "where-yield" };

            string result = key;
            foreach (var item in keys)
            {
                if (key.Contains(item + " ") || key.Contains(" " + item))
                {
                    result = result.Replace(item, "<span class='csharpkey'>" + item + "</span>");
                }
            }

            return result;
        }

        public static string PadLeftStr(this string msg, int length, string leftStr)
        {
            if (msg.Length >= length)
            {
                return msg;
            }
            else
            {
                int count = length - msg.Length;
                string temp = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    temp += leftStr;
                }

                return temp + msg;
            }
        }

        public static string ToMsSqlDbType(this string type)
        {
            switch (type.ToLower())
            {
                case "char":
                    return "SqlDbType.Char";
                case "varchar":
                    return "SqlDbType.VarChar";
                case "nvarchar":
                    return "SqlDbType.NVarChar";
                case "text":
                    return "SqlDbType.Text";
                case "datetime":
                    return "SqlDbType.DateTime";
                case "date":
                    return "SqlDbType.Date";
                case "int":
                    return "SqlDbType.Int";
                case "decimal":
                    return "SqlDbType.Decimal";
                case "tinyint":
                    return "SqlDbType.TinyInt";
                case "bigint":
                    return "SqlDbType.BigInt";
                default:
                    return "SqlDbType.VarChar";
            }
        }

        public static string ToMsSqlClassType(this string type)
        {
            switch (type)
            {
                case "int":
                case "tinyint":
                    return "int";
                case "bigint":
                    return "long";
                case "varchar":
                case "nvarchar":
                case "char":
                case "text":
                    return "string";
                case "date":
                case "datetime":
                    return "DateTime";
                case "float":
                case "double":
                    return "double";
                case "decimal":
                    return "decimal";
                default:
                    return "string";
            }
        }

        public static string ToMySqlDbType(this string type)
        {
            switch (type.ToLower())
            {
                case "char":
                    return "MySqlDbType.Char";
                case "varchar":
                    return "MySqlDbType.VarChar";
                case "nvarchar":
                    return "MySqlDbType.NVarChar";
                case "text":
                    return "MySqlDbType.Text";
                case "datetime":
                    return "MySqlDbType.DateTime";
                case "date":
                    return "MySqlDbType.Date";
                case "int":
                    return "MySqlDbType.Int32";
                case "decimal":
                    return "MySqlDbType.Decimal";
                case "tinyint":
                    return "MySqlDbType.Int32";
                case "bigint":
                    return "MySqlDbType.Int64";
                default:
                    return "MySqlDbType.VarChar";
            }
        }

        public static string ToMySqlClassType(this string type)
        {
            switch (type)
            {
                case "int":
                case "tinyint":
                    return "int";
                case "bigint":
                    return "long";
                case "varchar":
                case "nvarchar":
                case "char":
                case "text":
                    return "string";
                case "date":
                case "datetime":
                    return "DateTime";
                case "float":
                case "double":
                    return "double";
                case "decimal":
                    return "decimal";
                default:
                    return "string";
            }
        }

        /// <summary>
        /// 转成字段默认值结果：string.Empty;
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string ToDefaultValue(this string dbType)
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

        public static string ToEasyUIInputClassOptStr(this string dbType)
        {
            switch (dbType.ToLower())
            {
                case "int":
                case "tinyint":
                    return "class=\"easyui-numberbox\" data-options=\"required:true\"";
                case "bigint":
                    return "class=\"easyui-numberbox\" data-options=\"required:true\"";
                case "datetime":
                case "date":
                    return "class=\"easyui-datebox\" data-options=\"required:true,formatter:myformatter,parser:myparser\"";
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                    return "class=\"easyui-validatebox\" data-options=\"required:true\"";
                case "decimal":
                case "float":
                case "double":
                    return "class=\"easyui-numberbox\" data-options=\"precision:2,required:true\"";
                default:
                    return "string.Empty";
            }
        }
    }
}
