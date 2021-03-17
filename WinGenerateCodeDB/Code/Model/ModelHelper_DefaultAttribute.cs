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
        public static string CreateModel(string name_space, string table_name, List<SqlColumnInfo> colList, string model_name)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("using System;");
            content.AppendLine("using System.Collections.Generic;");
            content.AppendLine("using System.ComponentModel;");
            content.AppendLine("using System.Text;");
            content.AppendLine("namespace " + (string.IsNullOrEmpty(name_space) ? "命名空间" : name_space));
            content.AppendLine("{");
            if (!string.IsNullOrEmpty(table_name))
            {
                content.Append(CommentTool.CreateComment(table_name, 1));
            }

            content.AppendFormat("\tpublic class {0}\r\n", model_name);
            content.AppendLine("\t{");

            bool isCodeSplit = false;
            for (int i = 0; i < colList.Count; i++)
            {
                var item = colList[i];
                if (!string.IsNullOrEmpty(item.Comment))
                {
                    isCodeSplit = true;
                    content.Append(CommentTool.CreateComment(item.Comment, 2));
                }

                content.AppendFormat("\t\tpublic {0} {1} {{ get; set; }} = {2};\r\n",
                    SqlTool.GetFormatString(item.DbType),
                    item.Name,
                    SqlTool.GetDefaultValueStr(item.DbType));
                if (isCodeSplit && i < (colList.Count - 1))
                {
                    content.AppendLine();
                }
            }

            content.AppendLine("\t}");
            content.AppendLine("}");

            return content.ToString();
        }
    }
}
