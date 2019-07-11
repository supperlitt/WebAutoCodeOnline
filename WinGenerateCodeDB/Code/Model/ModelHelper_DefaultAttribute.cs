using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class ModelHelper_DefaultAttribute
    {
        public static string CreateModel()
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("using System;");
            content.AppendLine("using System.Collections.Generic;");
            content.AppendLine("using System.ComponentModel;");
            content.AppendLine("using System.Text;");
            content.AppendLine("namespace " + (string.IsNullOrEmpty(PageCache.NameSpaceStr) ? "命名空间" : PageCache.NameSpaceStr));
            content.AppendLine("{");
            if (!string.IsNullOrEmpty(PageCache.TableName))
            {
                content.Append(CommentTool.CreateComment(PageCache.TableName, 1));
            }

            content.AppendFormat("\tpublic class {0}\r\n", PageCache.TableName_Model);
            content.AppendLine("\t{");

            var colList = PageCache.GetColumnList();
            for (int i = 0; i < colList.Count; i++)
            {
                var item = colList[i];
                content.AppendLine();
                if (!string.IsNullOrEmpty(item.Comment))
                {
                    content.Append(CommentTool.CreateComment(item.Comment, 2));
                }

                content.AppendFormat("\t\t[DefaultValue(typeof({0}), \"{1}\")]\r\n",
                    SqlTool.GetFormatString(item.DbType),
                    SqlTool.GetDefaultValueAttributeStr(item.DbType));
                content.AppendFormat("\t\tpublic {0} {1}\r\n",
                    SqlTool.GetFormatString(item.DbType),
                    item.Name);
                content.AppendLine("\t\t{");
                content.AppendLine("\t\t\tget { return this._" + item.Name + "; }");
                content.AppendLine("\t\t\tset { this._" + item.Name + " = value; }");
                content.AppendLine("\t\t}\r\n");
            }

            var extendList = PageCache.GetExtendList();
            for (int i = 0; i < extendList.Count; i++)
            {
                var item = extendList[i];
                if (!string.IsNullOrEmpty(item.Comment))
                {
                    content.Append(CommentTool.CreateComment(item.Comment, 2));
                }

                content.AppendFormat("\t\tpublic {0} {1}\r\n",
                    item.AttributeType,
                    item.NewAttName);
                content.AppendLine("\t\t{");
                content.AppendLine("\t\t\tget");
                content.AppendLine("\t\t\t{");
                if (item.FormatType == 0)
                {
                    string formatStr = item.FormatStr.Replace("{c1}", item.DependColumn);
                    content.AppendLine("\t\t\treturn " + formatStr + ";");
                }
                else if (item.FormatType == 1)
                {
                    content.AppendLine("\t\t\t\treturn " + item.DependColumn.ToString() + ".ToString(\"" + item.FormatStr + "\");");
                }
                else if (item.FormatType == 2)
                {
                    string formatStr = item.FormatStr;
                    string[] lineArray = item.FormatStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    content.AppendLine("\t\t\t\tswitch (" + item.DependColumn + ")");
                    content.AppendLine("\t\t\t\t{");
                    foreach (var line in lineArray)
                    {
                        var lineItemArray = line.Split(new string[] { "---" }, StringSplitOptions.None);
                        if (lineItemArray[0] != StaticVariable.Switch_Default_Key)
                        {
                            content.AppendLine("\t\t\t\t\tcase " + lineItemArray[0] + ":");
                            content.AppendLine("\t\t\t\t\t\treturn \"" + lineItemArray[1] + "\";");
                        }
                    }

                    foreach (var line in lineArray)
                    {
                        var lineItemArray = line.Split(new string[] { "---" }, StringSplitOptions.None);
                        if (lineItemArray[0] == StaticVariable.Switch_Default_Key)
                        {
                            content.AppendLine("\t\t\t\t\tdefault:");
                            content.AppendLine("\t\t\t\t\t\treturn \"" + lineItemArray[1] + "\";");
                        }
                    }



                    content.AppendLine("\t\t\t\t}");
                }

                content.AppendLine("\t\t\t}");

                if (extendList.Count == i - 1)
                {
                    content.AppendLine("\t\t}");
                }
                else
                {
                    content.AppendLine("\t\t}\r\n");
                }
            }

            content.AppendLine("\t}");
            content.AppendLine("}");

            return content.ToString();
        }
    }
}
