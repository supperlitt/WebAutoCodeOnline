using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class MvcControllerHelper
    {
        public static string CreateController()
        {
            StringBuilder aspxcsContent = new StringBuilder();
            aspxcsContent.Append(CreateCSHead());
            aspxcsContent.Append(CreatePageLoad());
            aspxcsContent.Append(CreateLoadData());
            aspxcsContent.Append(CreateAddData());
            aspxcsContent.Append(CreateEditData());
            aspxcsContent.Append(CreateBatEditData());
            aspxcsContent.Append(CreateDeleteData());
            aspxcsContent.Append(CreateDownAndDownAll());
            aspxcsContent.Append(CreateBottom());

            return aspxcsContent.ToString();
        }

        private static string CreateCSHead()
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
", PageCache.NameSpaceStr, PageCache.TableName);
        }

        private static string CreatePageLoad()
        {
            return string.Empty;
        }

        private static string CreateLoadData()
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
            var showModel = PageCache.GetCmd("主显示");
            if (showModel != null)
            {
                foreach (var item in showModel.AttrList)
                {
                    string attribute = item.AttrName;
                    if (item.DbType.ToLower() == "datetime" ||
                        item.DbType.ToLower() == "date" ||
                        item.DbType.ToLower() == "int" ||
                        item.DbType.ToLower() == "bigint" ||
                        item.DbType.ToLower() == "tinyint")
                    {
                        searchContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(txtSearch{0});\r\n", attribute);
                        if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            searchContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", attribute);
                            searchContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", attribute);
                        }
                        else
                        {
                            searchContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", attribute);
                            searchContent.AppendFormat("\t\t\tif(!int.TryParse({0}Str, out {0}))\r\n", attribute);
                            searchContent.Append("\t\t\t{\r\n");
                            searchContent.AppendFormat("\t\t\t\t{0} = -1;\r\n", attribute);
                            searchContent.Append("\t\t\t}\r\n");
                        }
                    }
                    else
                    {
                        searchContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtSearch{0});\r\n", attribute);
                    }

                    searchStrContent.AppendFormat(" {0},", attribute);
                    loadStrContent.AppendFormat(", string txtSearch{0}", attribute);
                }

                StringBuilder encodeContent = new StringBuilder();
                foreach (var item in showModel.AttrList)
                {
                    if (item.Style.FieldName == "Html编码")
                    {
                        string attribute = item.ColName;
                        encodeContent.AppendLine(string.Format("\t\t\tlist.ForEach(p => p.{0} = HttpUtility.HtmlEncode(p.{0}));", attribute));
                    }
                }

                // 移除最后的逗号
                string searchStrCount = searchStrContent.Length > 0 ? searchStrContent.ToString().Substring(0, searchStrContent.Length - 1) : searchStrContent.ToString();
                return string.Format(template,
                    loadStrContent.ToString(),
                    searchContent.ToString(),
                    PageCache.TableName_DAL,
                    searchStrContent.ToString(),
                    searchStrCount,
                    encodeContent.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateAddData()
        {
            StringBuilder addContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            StringBuilder addStrContent = new StringBuilder();
            var addModel = PageCache.GetCmd("添加");
            if (addModel != null)
            {
                foreach (var item in addModel.AttrList)
                {
                    string attribute = item.AttrName;
                    string eval_attribute = item.ColName;
                    if (item.DbType.ToLower() == "datetime" ||
                        item.DbType.ToLower() == "date" ||
                        item.DbType.ToLower() == "int" ||
                     item.DbType.ToLower() == "bigint" ||
                        item.DbType.ToLower() == "tinyint")
                    {
                        addContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(txtAdd{0});\r\n", attribute);
                        if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            addContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", attribute);
                            addContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", attribute);
                        }
                        else
                        {
                            addContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", attribute);
                            addContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", attribute);
                        }

                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", eval_attribute, ExtendMethod.ToStringToType_ExceptIntDate(attribute, item.DbType));
                    }
                    else
                    {
                        addContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtAdd{0});\r\n", attribute);
                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", eval_attribute, ExtendMethod.ToStringToType_ExceptIntDate(attribute, item.DbType));
                    }

                    addStrContent.AppendFormat("string txtAdd{0},", attribute);
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
                    PageCache.TableName_Model,
                    createModel.ToString(),
                    PageCache.TableName_DAL,
                    PageCache.TableName);
            }
            else
            {
                return "";
            }
        }

        private static string CreateEditData()
        {
            StringBuilder editContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            StringBuilder editStrContent = new StringBuilder();
            var editModel = PageCache.GetCmd("编辑");
            if (editModel != null)
            {
                editContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtEdit{1});\r\n", PageCache.KeyId, PageCache.KeyId);
                createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", PageCache.KeyId, ExtendMethod.ToStringToType(PageCache.KeyId, PageCache.KeyId_DbType));
                editStrContent.AppendFormat("string txtEdit{0},", PageCache.KeyId);
                foreach (var item in editModel.AttrList)
                {
                    string attribute = item.AttrName;
                    string eval_attribute = item.ColName;
                    if (item.DbType.ToLower() == "datetime" ||
                        item.DbType.ToLower() == "date" ||
                        item.DbType.ToLower() == "int" ||
                     item.DbType.ToLower() == "bigint" ||
                        item.DbType.ToLower() == "tinyint")
                    {
                        editContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(txtEdit{0});\r\n", attribute);
                        if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            editContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", attribute);
                            editContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", attribute);
                        }
                        else
                        {
                            editContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", attribute);
                            editContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", attribute);
                        }

                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", eval_attribute, ExtendMethod.ToStringToType_ExceptIntDate(attribute, item.DbType));
                    }
                    else
                    {
                        editContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtEdit{1});\r\n", attribute, attribute);
                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", eval_attribute, ExtendMethod.ToStringToType_ExceptIntDate(attribute, item.DbType));
                    }

                    editStrContent.AppendFormat("string txtEdit{0},", attribute);
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
                    PageCache.TableName_Model,
                    createModel.ToString(),
                    PageCache.TableName_DAL,
                    PageCache.TableName);
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateBatEditData()
        {
            StringBuilder batEditContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            StringBuilder batEditStrContent = new StringBuilder();
            var batEditModel = PageCache.GetCmd("批量编辑");
            if (batEditModel != null)
            {
                batEditContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtBatEdit{1});\r\n", PageCache.KeyId, PageCache.KeyId);
                batEditContent.AppendFormat(@"           List<string> idList = {0}.Split(new char[]{{','}}, StringSplitOptions.RemoveEmptyEntries).ToList();{1}", PageCache.KeyId, Environment.NewLine);
                batEditStrContent.AppendFormat("string txtBatEdit{0},", PageCache.KeyId);
                foreach (var item in batEditModel.AttrList)
                {
                    string attribute = item.AttrName;
                    string eval_attribute = item.ColName;
                    if (item.DbType.ToLower() == "datetime" ||
                     item.DbType.ToLower() == "date" ||
                     item.DbType.ToLower() == "int" ||
                     item.DbType.ToLower() == "bigint" ||
                     item.DbType.ToLower() == "tinyint")
                    {
                        batEditContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(txtBatEdit{0});\r\n", attribute);
                        if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            batEditContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", attribute);
                            batEditContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", attribute);
                        }
                        else
                        {
                            batEditContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", attribute);
                            batEditContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", attribute);
                        }

                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", eval_attribute, ExtendMethod.ToStringToType_ExceptIntDate(attribute, item.DbType));
                    }
                    else
                    {
                        batEditContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(txtBatEdit{1});\r\n", attribute, attribute);
                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", eval_attribute, ExtendMethod.ToStringToType_ExceptIntDate(attribute, item.DbType));
                    }

                    batEditStrContent.AppendFormat("string txtBatEdit{0},", attribute);
                }

                if (batEditStrContent.Length > 0)
                {
                    batEditStrContent.Remove(batEditStrContent.Length - 1, 1);
                }

                string template = @"
        [HttpPost]
        public ActionResult batedit({0})
        {{
{1}
            {2} model = new {2}();
            {3} dal = new {3}();
            dal.Update{4}(model);

            return new ContentResult() {{ Content = ""0"" }};
        }}
";

                return string.Format(template,
                    batEditStrContent.ToString(),
                    batEditContent.ToString(),
                    PageCache.TableName_Model,
                    createModel.ToString(),
                    PageCache.TableName_DAL,
                    PageCache.TableName);
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateDeleteData()
        {
            StringBuilder batContent = new StringBuilder();
            var deleteModel = PageCache.GetCmd("删除");
            if (deleteModel != null)
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
                    PageCache.TableName_DAL,
                    PageCache.TableName);
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateDownAndDownAll()
        {
            var down_all_model = PageCache.GetCmd("导出全部");
            var down_select_model = PageCache.GetCmd("导出选中");
            if (down_all_model != null || down_select_model != null)
            {
                StringBuilder down_all_str = new StringBuilder();
                #region 导出全部
                if (down_all_model != null)
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

                    foreach (var item in down_all_model.AttrList)
                    {
                        string attribute = item.AttrName;
                        string eval_attribute = item.ColName;
                        if (item.DbType.ToLower() == "datetime" ||
                            item.DbType.ToLower() == "date" ||
                            item.DbType.ToLower() == "int" ||
                            item.DbType.ToLower() == "bigint" ||
                            item.DbType.ToLower() == "tinyint")
                        {
                            searchContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(txtSearch{0});\r\n", attribute);
                            if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                            {
                                searchContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", attribute);
                                searchContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", attribute);
                            }
                            else
                            {
                                searchContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", attribute);
                                searchContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", attribute);
                            }
                        }
                        else
                        {
                            searchContent.AppendFormat("            string {0} = HttpUtility.UrlDecode(txtSearch{0});\r\n", attribute);
                        }

                        searchStrContent.AppendFormat(" {0},", attribute);
                        searchFieldStrContent.AppendFormat("string txtSearch{0},", attribute);
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
                    foreach (var item in down_all_model.AttrList)
                    {
                        content.AppendFormat("<th>{0}</th>", item.TitleName);
                    }

                    content.Append("</tr></thead>");

                    StringBuilder appendFormat = new StringBuilder();
                    foreach (var item in down_all_model.AttrList)
                    {
                        appendFormat.AppendFormat(@"                content.AppendFormat(""<td>{{0}}</td>"", list[i].{0});{1}", item.AttrName, Environment.NewLine);
                    }

                    down_all_str.AppendFormat(template,
                        batContent.ToString(),
                        PageCache.TableName_DAL,
                        content.ToString(),
                        appendFormat.ToString(),
                        searchStrCount,
                        PageCache.TableName_Model,
                        searchFieldStrContent.ToString());
                }
                #endregion

                StringBuilder down_select_str = new StringBuilder();
                #region 导出选中
                if (down_select_model != null)
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
                    searchContent.AppendFormat("            ids = HttpUtility.UrlDecode(ids);\r\n", PageCache.KeyId, PageCache.KeyId);
                    searchContent.Append("            List<string> idList = ids.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries).ToList();\r\n");
                    batContent.Append(searchContent.ToString());

                    StringBuilder content = new StringBuilder();
                    content.Append("<table border='1'><thead><tr>");
                    foreach (var item in down_select_model.AttrList)
                    {
                        content.AppendFormat("<th>{0}</th>", item.TitleName);
                    }

                    content.Append("</tr></thead>");

                    StringBuilder appendFormat = new StringBuilder();
                    foreach (var item in down_select_model.AttrList)
                    {
                        appendFormat.AppendFormat(@"                content.AppendFormat(""<td>{{0}}</td>"", list[i].{0});{1}", item.AttrName, Environment.NewLine);
                    }

                    down_select_str.AppendFormat(template,
                        batContent.ToString(),
                        PageCache.TableName_DAL,
                        content.ToString(),
                        appendFormat.ToString(),
                        PageCache.TableName_Model);
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
