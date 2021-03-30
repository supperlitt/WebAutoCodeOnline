using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class MvcControllerHelper
    {
        public static string CreateController(string name_space, string table_name, int action, List<SqlColumnInfo> colList, string model_name, string dal_name)
        {
            StringBuilder aspxcsContent = new StringBuilder();
            aspxcsContent.Append(CreateCSHead(name_space, table_name));
            aspxcsContent.Append(CreatePageLoad());
            aspxcsContent.Append(CreateLoadData(action, colList, dal_name));
            aspxcsContent.Append(CreateAddData(action, colList, table_name, dal_name));
            aspxcsContent.Append(CreateEditData(action, colList, table_name, dal_name));
            aspxcsContent.Append(CreateDeleteData(action, colList, table_name, dal_name));
            aspxcsContent.Append(CreateDownAndDownAll(action, colList, table_name, dal_name, model_name));
            aspxcsContent.Append(CreateBottom());

            return aspxcsContent.ToString();
        }

        private static string CreateCSHead(string name_space, string table_name)
        {
            return string.Format(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace {0}.Controllers
{{
    public class {1}Controller : Controller
    {{
        public ActionResult Index()
        {{
            return View();
        }}
", name_space, table_name);
        }

        private static string CreatePageLoad()
        {
            return string.Empty;
        }

        private static string CreateLoadData(int action, List<SqlColumnInfo> colList, string dal_name)
        {
            string template = @"
        [HttpPost]
        public ActionResult load(int page, int pageSize{0})
        {{
{1}
            {2} dal = new {2}();
            var list = dal.QueryList({3}page, pageSize);
            int itemCount = dal.QueryListCount({4});
            int pageCount = (int)Math.Ceiling((double)itemCount / (double)pageSize);
{5}
            return new JsonResult() {{ Data = new {{ PageCount = pageCount, ItemCount = itemCount, Data = list }} }};
        }}
";
            StringBuilder searchContent = new StringBuilder();
            StringBuilder searchStrContent = new StringBuilder();
            StringBuilder loadStrContent = new StringBuilder();
            if ((action & (int)action_type.query_list) == (int)action_type.query_list)
            {
                foreach (var item in colList.ToNotMainIdList())
                {
                    if (item.DbType.ToLower() == "datetime" ||
                        item.DbType.ToLower() == "date" ||
                        item.DbType.ToLower() == "int" ||
                        item.DbType.ToLower() == "bigint" ||
                        item.DbType.ToLower() == "tinyint")
                    {
                        searchContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(txtSearch{0});\r\n", item.Name);
                        if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            searchContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", item.Name);
                            searchContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", item.Name);
                        }
                        else
                        {
                            searchContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", item.Name);
                            searchContent.AppendFormat("\t\t\tif(!int.TryParse({0}Str, out {0}))\r\n", item.Name);
                            searchContent.Append("\t\t\t{\r\n");
                            searchContent.AppendFormat("\t\t\t\t{0} = -1;\r\n", item.Name);
                            searchContent.Append("\t\t\t}\r\n");
                        }
                    }
                    else
                    {
                        searchContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtSearch{0});\r\n", item.Name);
                    }

                    searchStrContent.AppendFormat(" {0},", item.Name);
                    loadStrContent.AppendFormat(", string txtSearch{0}", item.Name);
                }

                StringBuilder encodeContent = new StringBuilder();
                foreach (var item in colList.ToNotMainIdList())
                {
                    if (item.DbType.ToLower() == "varchar" || item.DbType.ToLower() == "nvarchar" || item.DbType.ToLower() == "text")
                    {
                        encodeContent.AppendLine(string.Format("\t\t\tlist.ForEach(p => p.{0} = HttpUtility.HtmlEncode(p.{0}));", item.Name));
                    }
                }

                // 移除最后的逗号
                string searchStrCount = searchStrContent.Length > 0 ? searchStrContent.ToString().Substring(0, searchStrContent.Length - 1) : searchStrContent.ToString();
                return string.Format(template,
                    loadStrContent.ToString(),
                    searchContent.ToString(),
                    dal_name,
                    searchStrContent.ToString(),
                    searchStrCount,
                    encodeContent.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateAddData(int action, List<SqlColumnInfo> colList, string table_name, string dal_name)
        {
            StringBuilder addContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            StringBuilder addStrContent = new StringBuilder();
            if ((action & (int)action_type.add) == (int)action_type.add)
            {
                foreach (var item in colList.ToNotMainIdList())
                {
                    if (item.DbType.ToLower() == "datetime" ||
                        item.DbType.ToLower() == "date" ||
                        item.DbType.ToLower() == "int" ||
                     item.DbType.ToLower() == "bigint" ||
                        item.DbType.ToLower() == "tinyint")
                    {
                        addContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(txtAdd{0});\r\n", item.Name);
                        if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            addContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", item.Name);
                            addContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", item.Name);
                        }
                        else
                        {
                            addContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", item.Name);
                            addContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", item.Name);
                        }

                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", item.Name, ExtendMethod.ToStringToType_ExceptIntDate(item.Name, item.DbType));
                    }
                    else
                    {
                        addContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtAdd{0});\r\n", item.Name);
                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", item.Name, ExtendMethod.ToStringToType_ExceptIntDate(item.Name, item.DbType));
                    }

                    addStrContent.AppendFormat("string txtAdd{0},", item.Name);
                }

                if (addStrContent.Length > 0)
                {
                    addStrContent.Remove(addStrContent.Length - 1, 1);
                }

                string template = @"
        [HttpPost]
        public ActionResult add({0})
        {{
{1}
            {2} model = new {2}();
{3}
            {4} dal = new {4}();
            dal.Add{5}(model);

            return new ContentResult() {{ Content = ""0"" }};
        }}
";

                return string.Format(template,
                    addStrContent.ToString(),
                    addContent.ToString(),
                    table_name,
                    createModel.ToString(),
                    dal_name,
                    table_name);
            }
            else
            {
                return "";
            }
        }

        private static string CreateEditData(int action, List<SqlColumnInfo> colList, string table_name, string dal_name)
        {
            StringBuilder editContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            StringBuilder editStrContent = new StringBuilder();
            if ((action & (int)action_type.edit) == (int)action_type.edit)
            {
                editContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtEdit{1});\r\n", colList.ToKeyId(), colList.ToKeyId());
                createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", colList.ToKeyId(), ExtendMethod.ToStringToType(colList.ToKeyId(), colList.ToKeyIdDbType()));
                editStrContent.AppendFormat("string txtEdit{0},", colList.ToKeyId());
                foreach (var item in colList.ToNotMainIdList())
                {
                    if (item.DbType.ToLower() == "datetime" ||
                        item.DbType.ToLower() == "date" ||
                        item.DbType.ToLower() == "int" ||
                     item.DbType.ToLower() == "bigint" ||
                        item.DbType.ToLower() == "tinyint")
                    {
                        editContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(txtEdit{0});\r\n", item.Name);
                        if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            editContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", item.Name);
                            editContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", item.Name);
                        }
                        else
                        {
                            editContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", item.Name);
                            editContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", item.Name);
                        }

                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", item.Name, ExtendMethod.ToStringToType_ExceptIntDate(item.Name, item.DbType));
                    }
                    else
                    {
                        editContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtEdit{1});\r\n", item.Name, item.Name);
                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", item.Name, ExtendMethod.ToStringToType_ExceptIntDate(item.Name, item.DbType));
                    }

                    editStrContent.AppendFormat("string txtEdit{0},", item.Name);
                }

                if (editStrContent.Length > 0)
                {
                    editStrContent.Remove(editStrContent.Length - 1, 1);
                }

                string template = @"
        [HttpPost]
        public ActionResult edit({0})
        {{
{1}
            {2} model = new {2}();
{3}
            {4} dal = new {4}();
            dal.Update{5}(model);

            return new ContentResult() {{ Content = ""0"" }};
        }}
";

                return string.Format(template,
                    editStrContent.ToString(),
                    editContent.ToString(),
                    table_name,
                    createModel.ToString(),
                    dal_name,
                    table_name);
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateDeleteData(int action, List<SqlColumnInfo> colList, string table_name, string dal_name)
        {
            StringBuilder batContent = new StringBuilder();
            if ((action & (int)action_type.bat_real_delete) == (int)action_type.bat_real_delete)
            {
                batContent.Append("            ids = HttpUtility.UrlDecode(ids);\r\n");
                batContent.Append("            List<string> idList = ids.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries).ToList();\r\n");

                string template = @"
        [HttpPost]
        public ActionResult delete(string ids)
        {{
{0}
            {1} dal = new {1}();
            dal.Delete{2}(idList);

            return new ContentResult() {{ Content = ""0"" }};
        }}
";

                return string.Format(template,
                    batContent.ToString(),
                    dal_name,
                    table_name);
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateDownAndDownAll(int action, List<SqlColumnInfo> colList, string table_name, string dal_name, string model_name)
        {
            var down_all_model = (action | (int)action_type.export_all) == (int)action_type.export_all;
            var down_select_model = (action | (int)action_type.export_select) == (int)action_type.export_select;
            if (down_all_model || down_select_model)
            {
                StringBuilder down_all_str = new StringBuilder();
                #region 导出全部
                if (down_all_model)
                {
                    StringBuilder searchContent = new StringBuilder();
                    StringBuilder searchStrContent = new StringBuilder();
                    StringBuilder searchFieldStrContent = new StringBuilder();
                    string template = @"
        [HttpGet]
        public ActionResult downall({6})
        {{
{0}
            {1} dal = new {1}();
            List<{5}> data = dal.GetAll({4});
            string content = CreateTableAll(data);

            return new FileContentResult(Encoding.UTF8.GetBytes(content), ""application/ms-excel;charset=UTF-8"") {{ FileDownloadName = ""{5}.xls"" }};
        }}

        private string CreateTableAll(List<{5}> list)
        {{
            StringBuilder content = new StringBuilder();
            
            // create columns header
            content.Append(""{2}"");
            for (int i = 0, len = list.Count; i < len; i++)
            {{
                content.Append(""<tr>"");
{3}
                content.Append(""</tr>"");
            }}
            content.Append(""</tbody></table>"");

            return content.ToString();            
        }}
";

                    foreach (var item in colList.ToNotMainIdList())
                    {
                        if (item.DbType.ToLower() == "datetime" ||
                            item.DbType.ToLower() == "date" ||
                            item.DbType.ToLower() == "int" ||
                            item.DbType.ToLower() == "bigint" ||
                            item.DbType.ToLower() == "tinyint")
                        {
                            searchContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(txtSearch{0});\r\n", item.Name);
                            if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                            {
                                searchContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", item.Name);
                                searchContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", item.Name);
                            }
                            else
                            {
                                searchContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", item.Name);
                                searchContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", item.Name);
                            }
                        }
                        else
                        {
                            searchContent.AppendFormat("            string {0} = HttpUtility.UrlDecode(txtSearch{0});\r\n", item.Name);
                        }

                        searchStrContent.AppendFormat(" {0},", item.Name);
                        searchFieldStrContent.AppendFormat("string txtSearch{0},", item.Name);
                    }

                    if (searchFieldStrContent.Length > 0)
                    {
                        searchFieldStrContent.Remove(searchFieldStrContent.Length - 1, 1);
                    }

                    // 移除最后的逗号
                    string searchStrCount = searchStrContent.Length > 0 ? searchStrContent.ToString().Substring(0, searchStrContent.Length - 1) : searchStrContent.ToString();

                    StringBuilder batContent = new StringBuilder();
                    batContent.Append(searchContent.ToString());

                    StringBuilder content = new StringBuilder();
                    content.Append("<table border='1'><thead><tr>");
                    foreach (var item in colList.ToNotMainIdList())
                    {
                        content.AppendFormat("<th>{0}</th>", item.Comment);
                    }

                    content.Append("</tr></thead>");

                    StringBuilder appendFormat = new StringBuilder();
                    foreach (var item in colList.ToNotMainIdList())
                    {
                        appendFormat.AppendFormat(@"                content.AppendFormat(""<td>{{0}}</td>"", list[i].{0});{1}", item.Name, Environment.NewLine);
                    }

                    down_all_str.AppendFormat(template,
                        batContent.ToString(),
                        dal_name,
                        content.ToString(),
                        appendFormat.ToString(),
                        searchStrCount,
                        table_name,
                        searchFieldStrContent.ToString());
                }
                #endregion

                StringBuilder down_select_str = new StringBuilder();
                #region 导出选中
                if (down_select_model)
                {
                    StringBuilder searchContent = new StringBuilder();
                    StringBuilder searchStrContent = new StringBuilder();
                    string template = @"
        [HttpGet]
        public ActionResult down(string ids)
        {{
{0}
            {1} dal = new {1}();
            List<{4}> data = dal.GetPartAll(idList);
            string content = CreateTable(data);

            return new FileContentResult(Encoding.UTF8.GetBytes(content), ""application/ms-excel;charset=UTF-8"") {{ FileDownloadName = ""{4}.xls"" }};
        }}

        private string CreateTable(List<{4}> list)
        {{
            StringBuilder content = new StringBuilder();
            
            // create columns header
            content.Append(""{2}"");
            for (int i = 0, len = list.Count; i < len; i++)
            {{
                content.Append(""<tr>"");
{3}
                content.Append(""</tr>"");
            }}
            content.Append(""</tbody></table>"");

            return content.ToString();            
        }}";

                    StringBuilder batContent = new StringBuilder();
                    searchContent.AppendFormat("            ids = HttpUtility.UrlDecode(ids);\r\n", colList.ToKeyId(), colList.ToKeyId());
                    searchContent.Append("            List<string> idList = ids.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries).ToList();\r\n");
                    batContent.Append(searchContent.ToString());

                    StringBuilder content = new StringBuilder();
                    content.Append("<table border='1'><thead><tr>");
                    foreach (var item in colList.ToNotMainIdList())
                    {
                        content.AppendFormat("<th>{0}</th>", item.Comment);
                    }

                    content.Append("</tr></thead>");

                    StringBuilder appendFormat = new StringBuilder();
                    foreach (var item in colList.ToNotMainIdList())
                    {
                        appendFormat.AppendFormat(@"                content.AppendFormat(""<td>{{0}}</td>"", list[i].{0});{1}", item.Name, Environment.NewLine);
                    }

                    down_select_str.AppendFormat(template,
                        batContent.ToString(),
                        dal_name,
                        content.ToString(),
                        appendFormat.ToString(),
                        model_name);
                }
                #endregion

                return down_all_str.ToString() + down_select_str.ToString();
            }
            return string.Empty;
        }

        private static string CreateBottom()
        {
            return @"
    }
}";
        }
    }
}
