using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class ModelHelper_Default
    {
        public static string CreateModel(string name_space, string table_name, List<SqlColumnInfo> colList, string model_name)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("using System;");
            content.AppendLine("using System.Collections.Generic;");
            content.AppendLine("using System.Text;");
            content.AppendLine("namespace " + (string.IsNullOrEmpty(name_space) ? "命名空间" : name_space));
            content.AppendLine("{");
            if (!string.IsNullOrEmpty(table_name))
            {
                content.Append(CommentTool.CreateComment(table_name, 1));
            }

            content.AppendFormat("\tpublic class {0}\r\n", model_name);
            content.AppendLine("\t{");

            for (int i = 0; i < colList.Count; i++)
            {
                var item = colList[i];
                if (!string.IsNullOrEmpty(item.Comment))
                {
                    content.Append(CommentTool.CreateComment(item.Comment, 2));
                }

                content.AppendFormat("\t\tprivate {0} _{1} = {2};\r\n",
                    SqlTool.GetFormatString(item.DbType),
                    item.Name,
                    SqlTool.GetDefaultValueStr(item.DbType));

                content.AppendLine();
                if (!string.IsNullOrEmpty(item.Comment))
                {
                    content.Append(CommentTool.CreateComment(item.Comment, 2));
                }

                content.AppendFormat("\t\tpublic {0} {1}\r\n",
                    SqlTool.GetFormatString(item.DbType),
                    item.Name);
                content.AppendLine("\t\t{");
                content.AppendLine("\t\t\tget { return this._" + item.Name + "; }");
                content.AppendLine("\t\t\tset { this._" + item.Name + " = value; }");
                content.AppendLine("\t\t}\r\n");
            }

            content.AppendLine("\t}");
            content.AppendLine("}");

            return content.ToString();
        }
    }
}
