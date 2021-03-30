using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class ModelHelper_DefaultCore
    {
        private string name_space = string.Empty;
        private string model_suffix = string.Empty;
        private string model_name = string.Empty;

        public ModelHelper_DefaultCore(string name_space, string model_suffix)
        {
            this.name_space = name_space;
            this.model_suffix = model_suffix;
        }

        public string CreateModel(string table_name, List<SqlColumnInfo> colList)
        {
            this.model_name = table_name + model_suffix;

            StringBuilder content = new StringBuilder();
            content.AppendLine("using System;");
            content.AppendLine("using System.Collections.Generic;");
            content.AppendLine("using System.Linq;");
            content.AppendLine("using System.Threading.Tasks;");
            content.AppendLine();
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
            content.AppendLine();

            var addList = Cache_VMData.GetVMList(table_name, VMType.Add, colList);
            content.AppendFormat("\tpublic class add_{0}\r\n", model_name);
            content.AppendLine("\t{");
            for (int i = 0; i < addList.Count; i++)
            {
                var item = addList[i];
                if (item.IsMainKey)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(item.Comment))
                {
                    content.Append(CommentTool.CreateComment(item.Comment, 2));
                }

                content.AppendFormat("\t\tpublic {0} {1} {{ get; set; }} = {2};\r\n",
                    SqlTool.GetFormatString(item.DbType),
                    item.Name,
                    SqlTool.GetDefaultValueStr(item.DbType));

                if (isCodeSplit && i < (addList.Count - 1))
                {
                    content.AppendLine();
                }
            }

            content.AppendLine("\t}");
            content.AppendLine();

            var editList = Cache_VMData.GetVMList(table_name, VMType.Edit, colList);
            content.AppendFormat("\tpublic class edit_{0}\r\n", model_name);
            content.AppendLine("\t{");
            for (int i = 0; i < editList.Count; i++)
            {
                var item = editList[i];
                if (!string.IsNullOrEmpty(item.Comment))
                {
                    content.Append(CommentTool.CreateComment(item.Comment, 2));
                }

                content.AppendFormat("\t\tpublic {0} {1} {{ get; set; }} = {2};\r\n",
                    SqlTool.GetFormatString(item.DbType),
                    item.Name,
                    SqlTool.GetDefaultValueStr(item.DbType));

                if (isCodeSplit && i < (editList.Count - 1))
                {
                    content.AppendLine();
                }
            }

            content.AppendLine("\t}");
            content.AppendLine();
            content.AppendFormat("\tpublic class delete_{0}\r\n", model_name);
            content.AppendLine("\t{");
            for (int i = 0; i < colList.Count; i++)
            {
                var item = colList[i];
                if (!item.IsMainKey)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(item.Comment))
                {
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
            content.AppendLine();

            var queryList = Cache_VMData.GetVMList(table_name, VMType.Query, colList);
            content.AppendFormat("\tpublic class query_{0}\r\n", model_name);
            content.AppendLine("\t{");
            for (int i = 0; i < queryList.Count; i++)
            {
                var item = queryList[i];
                if (!string.IsNullOrEmpty(item.Comment))
                {
                    content.Append(CommentTool.CreateComment(item.Comment, 2));
                }

                content.AppendFormat("\t\tpublic {0} {1} {{ get; set; }} = {2};\r\n",
                    SqlTool.GetFormatString(item.DbType),
                    item.Name,
                    SqlTool.GetDefaultValueStr(item.DbType));

                if (isCodeSplit && i < (queryList.Count - 1))
                {
                    content.AppendLine();
                }
            }

            content.AppendLine("\t\tpublic int page { get; set; } = 0;");
            content.AppendLine("\t\tpublic int pageSize { get; set; } = 0;");
            content.AppendLine("\t}");

            content.AppendLine();
            content.AppendFormat("\tpublic class display_{0}\r\n", model_name);
            content.AppendLine("\t{");
            content.AppendLine("\t\tpublic int item_count { get; set; } = 0;");
            content.AppendLine("\t\tpublic int page_count { get; set; } = 0;");
            content.AppendFormat("\t\tpublic List<{0}> list {{ get; set; }} = new List<{0}>();\r\n", model_name);
            content.AppendLine("\t}");
            content.AppendLine("}");

            return content.ToString();
        }
    }
}
