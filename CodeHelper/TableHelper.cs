using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeHelper
{
    /// <summary>
    /// 表格帮助信息
    /// </summary>
    public class TableHelper : IHelper
    {
        /// <summary>
        /// 是否需要在类中包含字段
        /// </summary>
        private bool isContainsField = false;

        /// <summary>
        /// 是否第一个字母小写处理：默认是添加下划线
        /// </summary>
        private bool isLowerFirst = false;

        /// <summary>
        /// 是否添加默认值
        /// </summary>
        private bool isDefaultValue = false;

        public TableHelper()
        {
        }

        public TableHelper(bool isContainsField, bool isLowerFirst, bool isDefaultValue)
        {
            this.isContainsField = isContainsField;
            this.isLowerFirst = isLowerFirst;
            this.isDefaultValue = isDefaultValue;
        }

        public string GetClassString(NormalModel model)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("using System;");
            content.AppendLine("using System.Collections.Generic;");
            content.AppendLine("using System.Text;");
            content.AppendLine("namespace " + (string.IsNullOrEmpty(model.NameSpace) ? "命名空间" : model.NameSpace));
            content.AppendLine("{");
            if (!string.IsNullOrEmpty(model.Title))
            {
                content.Append(CreateComment(model.Title, 1));
            }

            content.AppendFormat("\tpublic class {0}\r\n", model.TableName.ToFirstUpper());
            content.AppendLine("\t{");
            for (int i = 0; i < model.ColumnList.Count; i++)
            {
                var item = model.ColumnList.Skip(i).Take(1).First();
                if (isContainsField)
                {
                    if (isLowerFirst)
                    {
                        if (!string.IsNullOrEmpty(item.Comment))
                        {
                            content.Append(CreateComment(item.Comment, 2));
                        }

                        if (isDefaultValue)
                        {
                            content.AppendFormat("\t\tprivate {0} {1} = {2};\r\n", GetFormatString(item.DBType), item.ColumnName.ToFirstLower(), GetDefaultValueStr(item.DBType));
                        }
                        else
                        {
                            content.AppendFormat("\t\tprivate {0} {1};\r\n", GetFormatString(item.DBType), item.ColumnName.ToFirstLower());
                        }

                        content.AppendLine();
                        if (!string.IsNullOrEmpty(item.Comment))
                        {
                            content.Append(CreateComment(item.Comment, 2));
                        }

                        content.AppendFormat("\t\tpublic {0} {1}\r\n", GetFormatString(item.DBType), item.ColumnName.ToFirstUpper());
                        content.AppendLine("\t\t{");
                        content.AppendLine("\t\t\tget { return this." + item.ColumnName.ToFirstLower() + "; }");
                        content.AppendLine("\t\t\tset { this." + item.ColumnName.ToFirstLower() + " = value; }");
                        content.AppendLine("\t\t}");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item.Comment))
                        {
                            content.Append(CreateComment(item.Comment, 2));
                        }
                        if (isDefaultValue)
                        {
                            content.AppendFormat("\t\tprivate {0} _{1} = {2};\r\n", GetFormatString(item.DBType), item.ColumnName, GetDefaultValueStr(item.DBType));
                        }
                        else
                        {
                            content.AppendFormat("\t\tprivate {0} _{1};\r\n", GetFormatString(item.DBType), item.ColumnName.ToFirstLower());
                        }

                        content.AppendLine();
                        if (!string.IsNullOrEmpty(item.Comment))
                        {
                            content.Append(CreateComment(item.Comment, 2));
                        }

                        content.AppendFormat("\t\tpublic {0} {1}\r\n", GetFormatString(item.DBType), item.ColumnName.ToFirstUpper());
                        content.AppendLine("\t\t{");
                        content.AppendLine("\t\t\tget { return this._" + item.ColumnName.ToFirstLower() + "; }");
                        content.AppendLine("\t\t\tset { this._" + item.ColumnName.ToFirstLower() + " = value; }");
                        content.AppendLine("\t\t}");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.Comment))
                    {
                        content.Append(CreateComment(item.Comment, 2));
                    }

                    content.AppendFormat("\t\tpublic {0} {1} ", GetFormatString(item.DBType), item.ColumnName.ToFirstUpper());
                    content.AppendLine("{ get; set; }");
                }

                if ((i + 1) < model.ColumnList.Count)
                {
                    content.AppendLine();
                }
            }

            content.AppendLine("\t}");
            content.AppendLine("}");

            return content.ToString();
        }

        /// <summary>
        /// 创建一个注释，从当前位置开始，回车换行结束
        /// </summary>
        /// <param name="commentStr"></param>
        /// <param name="tabCount">tab的个数</param>
        /// <returns></returns>
        private static string CreateComment(string commentStr, int tabCount)
        {
            string tabStr = string.Empty;
            for (int i = 0; i < tabCount; i++)
            {
                tabStr += "\t";
            }

            StringBuilder content = new StringBuilder();
            content.AppendLine(tabStr + "/// <summary>");
            content.AppendLine(tabStr + "/// " + commentStr);
            content.AppendLine(tabStr + "/// </summary>");

            return content.ToString();
        }

        /// <summary>
        /// 获取格式化之后的字符串
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        private static string GetFormatString(string sqlType)
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
        private static string GetDefaultValueStr(string sqlType)
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
    }
}
