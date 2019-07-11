using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    public class BootstrapAspxCsHelper
    {
        public static string CreateCSHead(string nameSpace, string pageName)
        {
            return string.Format(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Text;

namespace {0}
{{
    public partial class {1}Manager : System.Web.UI.Page
    {{
", nameSpace, pageName);
        }

        public static string CreatePageLoad(BootstrapModel model)
        {
            string template = @"
        protected void Page_Load(object sender, EventArgs e)
        {{
            if (Request.HttpMethod.ToUpper() == ""POST"")
            {{
                string type = Request.QueryString[""type""];
                switch (type)
                {{
{0}    
                }}

                Response.Flush();
                Response.End();
            }}{1}
        }}
";
            StringBuilder content = new StringBuilder();
            content.Append(@"
                    case ""loaddata"":
                        LoadData();
                        break;
");
            if (model.IsAdd)
            {
                content.Append(@"
                    case ""add"":
                        AddData();
                        break;
");
            }

            if (model.IsEdit)
            {
                content.Append(@"
                    case ""edit"":
                        EditData();
                        break;
");
            }

            if (model.IsBatEdit)
            {
                content.Append(@"
                    case ""batedit"":
                        BatEditData();
                        break;
");
            }

            if (model.IsDel)
            {
                content.Append(@"
                    case ""delete"":
                        DeleteData();
                        break;
");
            }

            StringBuilder elseContent = new StringBuilder();
            if (model.IsExport)
            {
                elseContent.AppendFormat(@"
            else{{
                string type = Request.QueryString[""type""];
                switch (type)
                {{
                    case ""down"":
                        Down();
                        break;
                    case ""downall"":
                        DownAll();
                        break;
                }}
            }}");
            }

            return string.Format(template, content.ToString(), elseContent.ToString());
        }

        public static string CreateLoadData(BootstrapModel model)
        {
            string template = @"
        private void LoadData()
        {{
            int page = Convert.ToInt32(Request.Form[""page""]);
            int pageSize = Convert.ToInt32(Request.Form[""pageSize""]);
{0}
            {1}DAL dal = new {1}DAL();
            var list = dal.QueryList({2}page, pageSize);
{4}
            int itemCount = dal.QueryListCount({3});
            int pageCount = (int)Math.Ceiling((double)itemCount / (double)pageSize);
			JavaScriptSerializer js = new JavaScriptSerializer();
            var str = js.Serialize(new {{ PageCount = pageCount, ItemCount = itemCount, Data = list }});
            Response.Write(str);
        }}
";
            StringBuilder searchContent = new StringBuilder();
            StringBuilder searchStrContent = new StringBuilder();
            foreach (var item in model.SearchColumns)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();
                if (item.DBType.ToLower() == "datetime" ||
                    item.DBType.ToLower() == "date" ||
                    item.DBType.ToLower() == "int" ||
                    item.DBType.ToLower() == "tinyint")
                {
                    searchContent.AppendFormat("\t\t\tstring {1}Str = HttpUtility.UrlDecode(Request[\"txtSearch{2}\"]);\r\n", item.DBType.ToMsSqlClassType(), field, attribute);
                    if (item.DBType.ToLower() == "datetime" || item.DBType.ToLower() == "date")
                    {
                        searchContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", field);
                        searchContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", field);
                    }
                    else
                    {
                        searchContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", field);
                        searchContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", field);
                    }
                }
                else
                {
                    searchContent.AppendFormat("\t\t\tstring {1} = HttpUtility.UrlDecode(Request[\"txtSearch{2}\"]);\r\n", item.DBType.ToMsSqlClassType(), field, attribute);
                }

                searchStrContent.AppendFormat(" {0},", field);
            }

            StringBuilder encodeContent = new StringBuilder();
            foreach (var item in model.ColumnList)
            {
                if (item.DBType == "varchar")
                {
                    string attribute = item.ColumnName.ToFirstUpper();

                    encodeContent.AppendLine(string.Format("\t\t\tlist.ForEach(p => p.{0} = HttpUtility.HtmlEncode(p.{0}));", attribute));
                }
            }

            // 移除最后的逗号
            string searchStrCount = searchStrContent.Length > 0 ? searchStrContent.ToString().Substring(0, searchStrContent.Length - 1) : searchStrContent.ToString();
            return string.Format(template, searchContent.ToString(), model.TableName.ToFirstUpper(), searchStrContent.ToString(), searchStrCount, encodeContent.ToString());
        }

        public static string CreateAddData(BootstrapModel model)
        {
            StringBuilder addContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            foreach (var item in model.AddColumns)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();
                if (item.DBType.ToLower() == "datetime" ||
                    item.DBType.ToLower() == "date" ||
                    item.DBType.ToLower() == "int" ||
                    item.DBType.ToLower() == "tinyint")
                {
                    addContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(Request[\"txtAdd{1}\"]);\r\n", field, attribute);
                    if (item.DBType.ToLower() == "datetime" || item.DBType.ToLower() == "date")
                    {
                        addContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", field);
                        addContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", field);
                    }
                    else
                    {
                        addContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", field);
                        addContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", field);
                    }

                    createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, ExtendMethod.ToStringToType(field, item.DBType));
                }
                else
                {
                    addContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"txtAdd{1}\"]);\r\n", field, attribute);
                    createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, ExtendMethod.ToStringToType(field, item.DBType));
                }
            }

            string template = @"
        private void AddData()
        {{
{0}
            {1} model = new {1}();
{2}
            {1}DAL dal = new {1}DAL();
            dal.Add{1}(model);

            Response.Write(""0"");
        }}
";

            return string.Format(template, addContent.ToString(), model.TableName.ToFirstUpper(), createModel.ToString());
        }

        public static string CreateEditData(BootstrapModel model)
        {
            StringBuilder editContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            editContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"txtEdit{1}\"]);\r\n", model.MainKeyIdStr.ToFirstLower(), model.MainKeyIdStr.ToFirstUpper());
            createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", model.MainKeyIdStr.ToFirstUpper(), ExtendMethod.ToStringToType(model.MainKeyIdStr.ToFirstLower(), model.MainKeyIdDBType));
            foreach (var item in model.EditColumns)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();
                if (item.DBType.ToLower() == "datetime" ||
                    item.DBType.ToLower() == "date" ||
                    item.DBType.ToLower() == "int" ||
                    item.DBType.ToLower() == "tinyint")
                {
                    createModel.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(Request[\"txtEdit{1}\"]);\r\n", field, attribute);
                    if (item.DBType.ToLower() == "datetime" || item.DBType.ToLower() == "date")
                    {
                        editContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", field);
                        editContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", field);
                    }
                    else
                    {
                        editContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", field);
                        editContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", field);
                    }

                    createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, ExtendMethod.ToStringToType(field, item.DBType));
                }
                else
                {
                    editContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"txtEdit{1}\"]);\r\n", field, attribute);
                    createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, ExtendMethod.ToStringToType(field, item.DBType));
                }
            }

            string template = @"
        private void EditData()
        {{
{0}
            {1} model = new {1}();
{2}
            {1}DAL dal = new {1}DAL();
            dal.Update{1}(model);

            Response.Write(""0"");
        }}
";

            return string.Format(template, editContent.ToString(), model.TableName.ToFirstUpper(), createModel.ToString());
        }

        public static string CreateBatEditData(BootstrapModel model)
        {
            StringBuilder batEditContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            batEditContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"txtBatEdit{1}\"]);\r\n", model.MainKeyIdStr.ToFirstLower(), model.MainKeyIdStr.ToFirstUpper());
            batEditContent.AppendFormat(@"           List<string> idList = {0}.Split(new char[]{{','}}, StringSplitOptions.RemoveEmptyEntries).ToList();{1}", model.MainKeyIdStr.ToFirstLower(), Environment.NewLine);
            foreach (var item in model.BatEditColumns)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();
                if (item.DBType.ToLower() == "datetime" ||
                    item.DBType.ToLower() == "date" ||
                    item.DBType.ToLower() == "int" ||
                    item.DBType.ToLower() == "tinyint")
                {
                    batEditContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(Request[\"txtBatEdit{1}\"]);\r\n", field, attribute);
                    if (item.DBType.ToLower() == "datetime" || item.DBType.ToLower() == "date")
                    {
                        batEditContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", field);
                        batEditContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", field);
                    }
                    else
                    {
                        batEditContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", field);
                        batEditContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", field);
                    }

                    createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, ExtendMethod.ToStringToType(field, item.DBType));
                }
                else
                {
                    batEditContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"txtBatEdit{1}\"]);\r\n", field, attribute);
                    createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, ExtendMethod.ToStringToType(field, item.DBType));
                }
            }

            string template = @"
        private void BatEditData()
        {{
{0}
            {1} model = new {1}();
{2}
            {1}DAL dal = new {1}DAL();
            dal.BatUpdate{1}(idList, model);

            Response.Write(""0"");
        }}
";

            return string.Format(template, batEditContent.ToString(), model.TableName.ToFirstUpper(), createModel.ToString());
        }

        public static string CreateDeleteData(BootstrapModel model)
        {
            StringBuilder batContent = new StringBuilder();
            batContent.AppendFormat("            string ids = HttpUtility.UrlDecode(Request[\"ids\"]);\r\n", model.MainKeyIdStr.ToFirstLower(), model.MainKeyIdStr.ToFirstUpper());
            batContent.Append("            List<string> idList = ids.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries).ToList();\r\n");

            string template = @"
        private void DeleteData()
        {{
{0}
            {1}DAL dal = new {1}DAL();
            dal.Delete{1}(idList);

            Response.Write(""0"");
        }}
";

            return string.Format(template, batContent.ToString(), model.TableName.ToFirstUpper());
        }

        public static string CreateDownAndDownAll(BootstrapModel model)
        {
            StringBuilder searchContent = new StringBuilder();
            StringBuilder searchStrContent = new StringBuilder();
            foreach (var item in model.SearchColumns)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();
                if (item.DBType.ToLower() == "datetime" ||
                    item.DBType.ToLower() == "date" ||
                    item.DBType.ToLower() == "int" ||
                    item.DBType.ToLower() == "tinyint")
                {
                    searchContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(Request[\"txtSearch{1}\"]);\r\n", field, attribute);
                    if (item.DBType.ToLower() == "datetime" || item.DBType.ToLower() == "date")
                    {
                        searchContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", field);
                        searchContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", field);
                    }
                    else
                    {
                        searchContent.AppendFormat("\t\t\tint {0} = 0;;\r\n", field);
                        searchContent.AppendFormat("\t\t\tint.TryParse({0}Str, out {0});\r\n", field);
                    }
                }
                else
                {
                    searchContent.AppendFormat("            string {0} = HttpUtility.UrlDecode(Request[\"txtSearch{1}\"]);\r\n", field, attribute);
                }

                searchStrContent.AppendFormat(" {0},", field);
            }

            // 移除最后的逗号
            string searchStrCount = searchStrContent.Length > 0 ? searchStrContent.ToString().Substring(0, searchStrContent.Length - 1) : searchStrContent.ToString();

            StringBuilder batContent = new StringBuilder();
            batContent.AppendFormat("            string ids = HttpUtility.UrlDecode(Request[\"ids\"]);\r\n", model.MainKeyIdStr.ToFirstLower(), model.MainKeyIdStr.ToFirstUpper());
            batContent.Append("            List<string> idList = ids.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries).ToList();\r\n");
            batContent.Append(searchContent.ToString());

            StringBuilder content = new StringBuilder();
            content.Append("<table border='1'><thead><tr>");
            foreach (var item in model.ColumnList)
            {
                content.AppendFormat("<th>{0}</th>", item.Comment);
            }

            content.Append("</tr></thead>");

            StringBuilder appendFormat = new StringBuilder();
            foreach (var item in model.ColumnList)
            {
                appendFormat.AppendFormat(@"                content.AppendFormat(""<td>{{0}}</td>"", list[i].{0});{1}", item.ColumnName.ToFirstUpper(), Environment.NewLine);
            }

            string template = @"
        private void Down()
        {{
{0}
            {1}DAL dal = new {1}DAL();
            List<{1}> data = dal.GetPartAll({4}idList);
            string content = CreateTable(data);
            Response.Clear(); 
            Response.Buffer = true; 
            Response.Charset = ""UTF-8""; 
            Response.AddHeader(""Content-Disposition"", ""attachment; filename={1}.xls""); 
            Response.ContentEncoding = System.Text.Encoding.GetEncoding(""UTF-8""); 
            Response.ContentType = ""application/ms-excel;charset=UTF-8""; 
            Response.Write(content); 
            Response.Flush(); 
            Response.End(); 
        }}

        private void DownAll()
        {{
{0}
            {1}DAL dal = new {1}DAL();
            List<{1}> data = dal.GetAll({5});
            string content = CreateTable(data);
            Response.Clear(); 
            Response.Buffer = true; 
            Response.Charset = ""UTF-8""; 
            Response.AddHeader(""Content-Disposition"", ""attachment; filename={1}.xls""); 
            Response.ContentEncoding = System.Text.Encoding.GetEncoding(""UTF-8""); 
            Response.ContentType = ""application/ms-excel;charset=UTF-8""; 
            Response.Write(content); 
            Response.Flush(); 
            Response.End(); 
        }}

        private string CreateTable(List<{1}> list)
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

            // TODO
            return string.Format(template, batContent.ToString(), model.TableName.ToFirstUpper(), content.ToString(), appendFormat.ToString(), searchStrContent.ToString(), searchStrCount);
        }

        public static string CreateBottom()
        {
            return @"
    }
}";
        }
    }
}
