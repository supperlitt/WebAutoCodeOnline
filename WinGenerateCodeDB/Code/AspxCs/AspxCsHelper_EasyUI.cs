﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class AspxCsHelper_EasyUI
    {
        public static string CreateASPXCS(string name_space, string table_name, int action, List<SqlColumnInfo> colList, string model_name, string dal_name)
        {
            StringBuilder aspxcsContent = new StringBuilder();
            aspxcsContent.Append(CreateCSHead(name_space, table_name));
            aspxcsContent.Append(CreatePageLoad(action));
            aspxcsContent.Append(CreateLoadData(action, colList, table_name));
            aspxcsContent.Append(CreateAddData(action, colList, table_name, model_name, dal_name));
            aspxcsContent.Append(CreateEditData(action, colList, table_name, model_name, dal_name));
            aspxcsContent.Append(CreateDeleteData(action, colList, table_name, dal_name));
            aspxcsContent.Append(CreateDownAndDownAll(action, colList, table_name, model_name, dal_name));
            aspxcsContent.Append(CreateBottom());

            return aspxcsContent.ToString();
        }

        private static string CreateCSHead(string name_space, string table_name)
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
    public partial class {1} : System.Web.UI.Page
    {{", name_space, table_name);
        }

        private static string CreatePageLoad(int action)
        {
            string template = @"
        protected void Page_Load(object sender, EventArgs e)
        {{
            if (Request.HttpMethod.ToUpper() == ""POST"")
            {{
                string type = Request.QueryString[""type""];
                switch (type)
                {{{0}    
                }}

                Response.End();
            }}{1}
        }}
";
            StringBuilder content = new StringBuilder();
            content.Append(@"
                    case ""load"":
                        LoadData();
                        break;");
            if ((action & (int)action_type.add) == (int)action_type.add)
            {
                content.Append(@"
                    case ""add"":
                        AddData();
                        break;");
            }

            if ((action & (int)action_type.edit) == (int)action_type.edit)
            {
                content.Append(@"
                    case ""edit"":
                        EditData();
                        break;");
            }

            if ((action & (int)action_type.real_delete) == (int)action_type.real_delete)
            {
                content.Append(@"
                    case ""delete"":
                        DeleteData();
                        break;");
            }

            StringBuilder elseContent = new StringBuilder();
            if ((action & (int)action_type.export_select) == (int)action_type.export_select ||
                (action | (int)action_type.export_all) == (int)action_type.export_all)
            {
                string export_all = @"
                    case ""downall"":
                        DownAll();
                        break;";
                string export_select = @"
                    case ""down"":
                        Down();
                        break;";

                elseContent.AppendFormat(@"
            else{{
                string type = Request.QueryString[""type""];
                switch (type)
                {{{0}{1}
                }}
            }}", (action | (int)action_type.export_select) == (int)action_type.export_select ? export_select : "",
               (action | (int)action_type.export_all) == (int)action_type.export_all ? export_all : "");
            }

            return string.Format(template, content.ToString(), elseContent.ToString());
        }

        private static string CreateLoadData(int action, List<SqlColumnInfo> colList, string table_name)
        {
            string template = @"
        private void LoadData()
        {{
            int page = Convert.ToInt32(Request.Form[""page""]);
            int pageSize = Convert.ToInt32(Request.Form[""pageSize""]);
{0}
            {1} dal = new {1}();
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
            if ((action & (int)action_type.query_list) == (int)action_type.query_list)
            {
                var queryList = Cache_VMData.GetVMList(table_name, VMType.Query, colList.ToNotMainIdList());
                foreach (var item in queryList)
                {
                    if (item.DbType.ToLower() == "datetime" ||
                        item.DbType.ToLower() == "date" ||
                        item.DbType.ToLower() == "int" ||
                        item.DbType.ToLower() == "bigint" ||
                        item.DbType.ToLower() == "tinyint")
                    {
                        searchContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(Request[\"txtSearch{0}\"]);\r\n", item.Name);
                        if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            searchContent.AppendFormat("\t\t\tDateTime {0} = DateTime.MinValue;\r\n", item.Name);
                            searchContent.AppendFormat("\t\t\tDateTime.TryParse({0}Str, out {0});\r\n", item.Name);
                        }
                        else
                        {
                            searchContent.AppendFormat("\t\t\tint {0} = 0;\r\n", item.Name);
                            searchContent.AppendFormat("\t\t\tif(!int.TryParse({0}Str, out {0}))\r\n", item.Name);
                            searchContent.Append("\t\t\t{\r\n");
                            searchContent.AppendFormat("\t\t\t\t{0} = -1;\r\n", item.Name);
                            searchContent.Append("\t\t\t}\r\n");
                        }
                    }
                    else
                    {
                        searchContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"txtSearch{0}\"]);\r\n", item.Name);
                    }

                    searchStrContent.AppendFormat(" {0},", item.Name);
                }

                StringBuilder encodeContent = new StringBuilder();
                foreach (var item in queryList)
                {
                    if (item.DbType.ToLower() == "varchar" || item.DbType.ToLower() == "nvarchar" || item.DbType.ToLower() == "text")
                    {
                        encodeContent.AppendLine(string.Format("\t\t\tlist.ForEach(p => p.{0} = HttpUtility.HtmlEncode(p.{0}));", item.Name));
                    }
                }

                // 移除最后的逗号
                string searchStrCount = searchStrContent.Length > 0 ? searchStrContent.ToString().Substring(0, searchStrContent.Length - 1) : searchStrContent.ToString();
                return string.Format(template,
                    searchContent.ToString(),
                    table_name,
                    searchStrContent.ToString(),
                    searchStrCount,
                    encodeContent.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateAddData(int action, List<SqlColumnInfo> colList, string table_name, string model_name, string dal_name)
        {
            StringBuilder addContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            if ((action & (int)action_type.add) == (int)action_type.add)
            {
                var addList = Cache_VMData.GetVMList(table_name, VMType.Add, colList.ToNotMainIdList());
                foreach (var item in addList)
                {
                    if (item.DbType.ToLower() == "datetime" ||
                        item.DbType.ToLower() == "date" ||
                        item.DbType.ToLower() == "int" ||
                     item.DbType.ToLower() == "bigint" ||
                        item.DbType.ToLower() == "tinyint")
                    {
                        addContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(Request[\"txtAdd{0}\"]);\r\n", item.Name);
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
                        addContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"txtAdd{0}\"]);\r\n", item.Name);
                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", item.Name, ExtendMethod.ToStringToType_ExceptIntDate(item.Name, item.DbType));
                    }
                }

                string template = @"
        private void AddData()
        {{
{0}
            {1} model = new {1}();
{2}
            {3} dal = new {3}();
            dal.Add{4}(model);

            Response.Write(""0"");
        }}
";

                return string.Format(template,
                    addContent.ToString(),
                    model_name,
                    createModel.ToString(),
                    dal_name,
                    table_name);
            }
            else
            {
                return "";
            }
        }

        private static string CreateEditData(int action, List<SqlColumnInfo> colList, string table_name, string model_name, string dal_name)
        {
            StringBuilder editContent = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            if ((action & (int)action_type.edit) == (int)action_type.edit)
            {
                editContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"txtEdit{1}\"]);\r\n", colList.ToKeyId(), colList.ToKeyId());
                createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", colList.ToKeyId(), ExtendMethod.ToStringToType(colList.ToKeyId(), colList.ToKeyIdDbType()));
                var editList = Cache_VMData.GetVMList(table_name, VMType.Edit, colList.ToNotMainIdList());
                foreach (var item in editList)
                {
                    if (item.DbType.ToLower() == "datetime" ||
                        item.DbType.ToLower() == "date" ||
                        item.DbType.ToLower() == "int" ||
                     item.DbType.ToLower() == "bigint" ||
                        item.DbType.ToLower() == "tinyint")
                    {
                        editContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(Request[\"txtEdit{0}\"]);\r\n", item.Name);
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
                        editContent.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"txtEdit{1}\"]);\r\n", item.Name, item.Name);
                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", item.Name, ExtendMethod.ToStringToType_ExceptIntDate(item.Name, item.DbType));
                    }
                }

                string template = @"
        private void EditData()
        {{
{0}
            {1} model = new {1}();
{2}
            {3} dal = new {3}();
            dal.Update{4}(model);

            Response.Write(""0"");
        }}
";

                return string.Format(template,
                    editContent.ToString(),
                    model_name,
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
                batContent.AppendFormat("            string ids = HttpUtility.UrlDecode(Request[\"ids\"]);\r\n", colList.ToKeyId(), colList.ToKeyId());
                batContent.Append("            List<string> idList = ids.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries).ToList();\r\n");

                string template = @"
        private void DeleteData()
        {{
{0}
            {1} dal = new {1}();
            dal.Delete{2}(idList);

            Response.Write(""0"");
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

        private static string CreateDownAndDownAll(int action, List<SqlColumnInfo> colList, string table_name, string model_name, string dal_name)
        {
            bool is_down_all_model = (action | (int)action_type.export_all) == (int)action_type.export_all;
            bool is_down_select_model = (action | (int)action_type.export_select) == (int)action_type.export_select;
            if (is_down_all_model || is_down_select_model)
            {
                StringBuilder down_all_str = new StringBuilder();
                #region 导出全部
                if (is_down_all_model)
                {
                    StringBuilder searchContent = new StringBuilder();
                    StringBuilder searchStrContent = new StringBuilder();
                    string template = @"

        private void DownAll()
        {{
{0}
            {1} dal = new {1}();
            List<{5}> data = dal.GetAll({4});
            string content = CreateTableAll(data);
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
        }}";

                    var queryList = Cache_VMData.GetVMList(table_name, VMType.Query, colList.ToNotMainIdList());
                    foreach (var item in queryList)
                    {
                        if (item.DbType.ToLower() == "datetime" ||
                            item.DbType.ToLower() == "date" ||
                            item.DbType.ToLower() == "int" ||
                            item.DbType.ToLower() == "bigint" ||
                            item.DbType.ToLower() == "tinyint")
                        {
                            searchContent.AppendFormat("\t\t\tstring {0}Str = HttpUtility.UrlDecode(Request[\"txtSearch{0}\"]);\r\n", item.Name);
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
                            searchContent.AppendFormat("            string {0} = HttpUtility.UrlDecode(Request[\"txtSearch{0}\"]);\r\n", item.Name);
                        }

                        searchStrContent.AppendFormat(" {0},", item.Name);
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
                        model_name);
                }
                #endregion

                StringBuilder down_select_str = new StringBuilder();
                #region 导出选中
                if (is_down_select_model)
                {
                    StringBuilder searchContent = new StringBuilder();
                    StringBuilder searchStrContent = new StringBuilder();
                    string template = @"
        private void Down()
        {{
{0}
            {1} dal = new {1}();
            List<{4}> data = dal.GetPartAll(idList);
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
                    searchContent.AppendFormat("            string ids = HttpUtility.UrlDecode(Request[\"ids\"]);\r\n", colList.ToKeyId(), colList.ToKeyId());
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
