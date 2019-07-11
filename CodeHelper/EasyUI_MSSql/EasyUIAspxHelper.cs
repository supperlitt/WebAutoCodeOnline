using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    internal class EasyUIAspxHelper
    {
        public static string CreatePageHead(string nameSpace, string pageName)
        {
            return string.Format(@"
<%@ Page Language=""C#"" AutoEventWireup=""true"" CodeBehind=""{0}.aspx.cs"" Inherits=""{1}.{0}"" ValidateRequest=""false"" %>

<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
", pageName, nameSpace);
        }

        public static string CreateHeader(string title)
        {
            return string.Format(@"
<head>
    <title>{0}</title>
    <link rel=""stylesheet"" type=""text/css"" href=""easyui/themes/default/easyui.css"" />
    <link rel=""stylesheet"" type=""text/css"" href=""easyui/themes/icon.css"" />
    <link rel=""stylesheet"" type=""text/css"" href=""easyui/themes/color.css"" />
    <link rel=""stylesheet"" type=""text/css"" href=""easyui/demo/demo.css"" />
    <script type=""text/javascript"" src=""easyui/jquery.min.js""></script>
    <script type=""text/javascript"" src=""easyui/jquery.easyui.min.js""></script>
    <script src=""easyui/locale/easyui-lang-zh_CN.js"" type=""text/javascript""></script>
    <script type=""text/javascript"" src=""easyui/datagrid-detailview.js""></script>
    <style type=""text/css"">
        #dg tbody tr td
        {{
            text-align: center;
        }}
        
        #fm-add div,#fm-edit div,#fm-batedit div
        {{
            padding: 5px;
        }}
    </style>
</head>
", title);
        }

        public static string CreateBodyHead(string title)
        {
            return string.Format(@"
<body>
    <h2>{0}</h2>
", title);
        }

        public static string CreateSearchContent(EasyUIModel model)
        {
            string template = @"
    <div class=""demo-info"" style=""margin-bottom: 10px"">
        <div class=""demo-tip icon-tip"">
            &nbsp;</div>
        <div>{0}</div>
    </div>
";

            StringBuilder searchContent = new StringBuilder();
            foreach (var item in model.SearchColumns)
            {
                string attribute = item.ColumnName.ToFirstUpper();
                searchContent.AppendFormat("&nbsp;&nbsp;<label for=\"txtSearch{0}\">{1}：</label><input type=\"text\" id=\"txtSearch{0}\" />", attribute, item.Comment);
            }

            if (searchContent.ToString() != string.Empty)
            {
                // 非空。添加查询按钮
                searchContent.Append("&nbsp;&nbsp;<input type=\"button\" id=\"btnsearch\" value=\"查询\" />");
            }

            return string.Format(template, searchContent.ToString());
        }

        public static string CreateCmdToolBar(EasyUIModel model)
        {
            StringBuilder toolBarContent = new StringBuilder(@"
<div id=""toolbar"">
");
            if (model.IsAdd && model.AddColumns.Count > 0)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-add"" plain=""true"" onclick=""newModel()"">新增</a>");
            }

            if (model.IsEdit && model.EditColumns.Count > 0)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-edit"" plain=""true"" onclick=""editModel()"">编辑</a>");
            }

            if (model.IsBatEdit && model.BatEditColumns.Count > 0)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-edit"" plain=""true"" onclick=""batEditModel()"">批量编辑</a>");
            }

            if (model.IsDel)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-remove"" plain=""true"" onclick=""destroyModel()"">删除</a>");
            }

            if (model.IsBatDel)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-remove"" plain=""true"" onclick=""destroyBatModel()"">删除</a>");
            }

            if (model.IsExport)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-remove"" plain=""true"" onclick=""selectExport()"">选中导出</a>");
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-remove"" plain=""true"" onclick=""exportAll()"">导出全部</a>");
            }

            toolBarContent.Append(@"
</div>");

            return toolBarContent.ToString();
        }

        public static string CreateDataGrid(EasyUIModel model)
        {
            // singleselect=""true""
            string template = @"
    <table id=""dg"" class=""easyui-datagrid"" style=""width: 1200px; height: auto"" url="""" pagination=""true"" 
         title=""{0}"" fitcolumns=""true"">
        <thead>
            <tr>
{1}
            </tr>
        </thead>
    </table>";

            StringBuilder tbodyContent = new StringBuilder();
            foreach (var item in model.ColumnList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();

                // 初始化table显示数据
                if (item.IsMainKey)
                {
                    tbodyContent.AppendFormat("\t\t\t\t<th data-options=\"field:'{0}',checkbox:true\">{1}</th>\r\n", attribute, item.Comment); //  style=\"display: none;\"
                }
                else
                {
                    if (item.DBType.ToLower().Contains("date"))
                    {
                        tbodyContent.AppendFormat("\t\t\t\t<th data-options=\"field:'{0}',width:120,align:'center'\">{1}</th>\r\n", attribute + "Str", item.Comment);
                    }
                    else
                    {
                        tbodyContent.AppendFormat("\t\t\t\t<th data-options=\"field:'{0}',width:120,align:'center'\">{1}</th>\r\n", attribute, item.Comment);
                    }
                }
            }

            return string.Format(template, model.Title, tbodyContent.ToString());
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string CreateDialog(EasyUIModel model)
        {
            StringBuilder dialogContent = new StringBuilder();
            if (model.AddColumns.Count > 10 || model.EditColumns.Count > 10 || model.BatEditColumns.Count > 10)
            {
                // 编辑等操作需要分成两行进行
                return "";
            }
            else
            {
                #region 添加
                if (model.IsAdd)
                {
                    // 行数过多，分成两行
                    string template = @"
    <div id=""dlg-add"" class=""easyui-dialog"" style=""width: 400px; height: 240px; padding: 10px 20px""
        closed=""true"" buttons=""#dlg-add-buttons"">
        <div class=""ftitle"">
        </div>
        <form id=""fm-add"" method=""post"" enctype=""multipart/form-data"" action=""{0}Manager.aspx?type=add""
        novalidate=""novalidate"">
{1}
        </form>
    </div>
    <div id=""dlg-add-buttons"">
        <a href=""javascript:void(0)"" class=""easyui-linkbutton c6"" iconcls=""icon-ok"" onclick=""saveAddModel()""
            style=""width: 90px"">保 存</a> <a href=""javascript:void(0)"" class=""easyui-linkbutton""
                iconcls=""icon-cancel"" onclick=""javascript:$('#dlg-add').dialog('close')"" style=""width: 90px"">
                取 消</a>
    </div>";

                    StringBuilder content = new StringBuilder();
                    foreach (var item in model.AddColumns)
                    {
                        string attribute = item.ColumnName.ToFirstUpper();

                        // 初始化form显示数据
                        if (item.IsMainKey)
                        {
                            content.AppendFormat(@"        <input type=""hidden"" id=""txtAdd{0}"" value="""" />", attribute);
                        }
                        else
                        {
                            // &#12288; 占一个中文字符
                            content.AppendFormat(@"
        <div class=""fitem"">
            <label for=""txtAdd{0}"">{1}:</label>
            <input type=""text"" id=""txtAdd{0}"" name=""txtAdd{0}"" {2} />
        </div>", attribute, item.Comment.PadLeftStr(4, "&emsp;"), item.DBType.ToEasyUIInputClassOptStr());
                        }
                    }

                    dialogContent.Append(string.Format(template, model.TableName.ToFirstUpper(), content.ToString()));
                }
                #endregion

                #region 编辑
                if (model.IsEdit)
                {
                    // 行数过多，分成两行
                    string template = @"
    <div id=""dlg-edit"" class=""easyui-dialog"" style=""width: 400px; height: 240px; padding: 10px 20px""
        closed=""true"" buttons=""#dlg-edit-buttons"">
        <div class=""ftitle"">
        </div>
        <form id=""fm-edit"" method=""post"" enctype=""multipart/form-data"" action=""{0}Manager.aspx?type=edit""
        novalidate=""novalidate"">
{1}
        </form>
    </div>
    <div id=""dlg-edit-buttons"">
        <a href=""javascript:void(0)"" class=""easyui-linkbutton c6"" iconcls=""icon-ok"" onclick=""saveEditModel()""
            style=""width: 90px"">保 存</a> <a href=""javascript:void(0)"" class=""easyui-linkbutton""
                iconcls=""icon-cancel"" onclick=""javascript:$('#dlg-edit').dialog('close')"" style=""width: 90px"">
                取 消</a>
    </div>";

                    StringBuilder content = new StringBuilder();
                    content.AppendFormat(@"        <input type=""hidden"" id=""txtEdit{0}"" value="""" />", model.MainKeyIdStr.ToFirstUpper());
                    foreach (var item in model.EditColumns)
                    {
                        string attribute = item.ColumnName.ToFirstUpper();

                        // 初始化form显示数据
                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
        <div class=""fitem"">
            <label for=""txtEdit{0}"">{1}:</label>
            <input type=""text"" id=""txtEdit{0}"" name=""txtEdit{0}"" {2} />
        </div>", attribute, item.Comment.PadLeftStr(4, "&emsp;"), item.DBType.ToEasyUIInputClassOptStr());
                    }

                    dialogContent.Append(string.Format(template, model.TableName.ToFirstUpper(), content.ToString()));
                }
                #endregion

                #region 批量编辑
                if (model.IsBatEdit && model.BatEditColumns.Count > 0)
                {
                    string template = @"
    <div id=""dlg-batedit"" class=""easyui-dialog"" style=""width: 400px; height: 240px; padding: 10px 20px""
        closed=""true"" buttons=""#dlg-batedit-buttons"">
        <div class=""ftitle"">
        </div>
        <form id=""fm-batedit"" method=""post"" enctype=""multipart/form-data"" action=""{0}Manager.aspx?type=batedit""
        novalidate=""novalidate"">
{1}
        </form>
    </div>
    <div id=""dlg-batedit-buttons"">
        <a href=""javascript:void(0)"" class=""easyui-linkbutton c6"" iconcls=""icon-ok"" onclick=""saveBatEditModel()""
            style=""width: 90px"">保 存</a> <a href=""javascript:void(0)"" class=""easyui-linkbutton""
                iconcls=""icon-cancel"" onclick=""javascript:$('#dlg-batedit').dialog('close')"" style=""width: 90px"">
                取 消</a>
    </div>";

                    StringBuilder content = new StringBuilder();
                    content.AppendFormat(@"		<input type=""hidden"" id=""txtBatEdit{0}"" value="""" />", model.MainKeyIdStr.ToFirstUpper());
                    foreach (var item in model.BatEditColumns)
                    {
                        string attribute = item.ColumnName.ToFirstUpper();

                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
        <div class=""fitem"">
            <label for=""txtBatEdit{0}"">{1}:</label>
            <input type=""text"" id=""txtBatEdit{0}"" name=""txtBatEdit{0}"" {2} />
        </div>", attribute, item.Comment.PadLeftStr(4, "&emsp;"), item.DBType.ToEasyUIInputClassOptStr());
                    }

                    dialogContent.Append(string.Format(template, model.TableName.ToFirstUpper(), content.ToString()));
                }
                #endregion

                return dialogContent.ToString();
            }
        }

        public static string CreateNotifyMsg()
        {
            return @"
    <div id=""dialog"" title=""提示"">
        <p style=""text-align: center;"">
        </p>
    </div>
";
        }

        /// <summary>
        /// 根据easyui的classname来设置读取对象的值
        /// .numberbox('setValue', 206.12);
        /// .numberbox('getValue');
        /// .datebox('getValue');
        /// .datetimebox('getValue');	
        /// </summary>
        /// <returns></returns>
        public static string CreateJsDateFormat()
        {
            return @"
        <script type=""text/javascript"">
        /* 
        日期格式化 2015-07-09
        */
        function myformatter(date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
        }
        function myparser(s) {
            if (!s) return new Date();
            var ss = (s.split('-'));
            var y = parseInt(ss[0], 10);
            var m = parseInt(ss[1], 10);
            var d = parseInt(ss[2], 10);
            if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                return new Date(y, m - 1, d);
            } else {
                return new Date();
            }
        }
    </script>
";
        }

        public static string CreateJsOperation(EasyUIModel model)
        {
            StringBuilder content = new StringBuilder(@"
    <script type=""text/javascript"">");

            #region edit
            if (model.IsEdit)
            {
                StringBuilder editSubmitContent = new StringBuilder();
                editSubmitContent.AppendFormat("\t\t\tvar txtEdit{0} = $(\"#txtEdit{0}\").textbox(\"getValue\");\r\n", model.MainKeyIdStr.ToFirstUpper());
                StringBuilder postDataContent = new StringBuilder("var postData = ");
                postDataContent.AppendFormat("\"txtEdit{0}=\" + encodeURI(txtEdit{0}) ", model.MainKeyIdStr.ToFirstUpper());
                int index = 0;

                StringBuilder editContent = new StringBuilder();
                editContent.AppendFormat(@"                    $(""#txtEdit{0}"").textbox(""setValue"", row.{0});", model.MainKeyIdStr.ToFirstUpper());
                foreach (var item in model.EditColumns)
                {
                    string attribute = item.ColumnName.ToFirstUpper();
                    editContent.AppendFormat(@"
                    $(""#txtEdit{0}"").textbox(""setValue"", row.{0});", attribute);
                    editSubmitContent.AppendFormat("\t\t\tvar txtEdit{0} = $(\"#txtEdit{0}\").textbox(\"getValue\");\r\n", attribute);
                    postDataContent.AppendFormat(" + \"&txtEdit{0}=\" + encodeURI(txtEdit{0})", attribute);

                    index++;
                }

                if (postDataContent.ToString() == "var postData = ")
                {
                    postDataContent.Append("\"\"");
                }

                postDataContent.Append(";");

                content.AppendFormat(@"

        function editModel() {{
            var row = $('#dg').datagrid('getChecked');
            if (row) {{
                if(row.length==1)
                {{
                    row = row[0];
                    $('#dlg-edit').dialog('open').dialog('setTitle', '编辑成员');
                    $('#fm-edit').form('load', row);
{0}
                }}
                else{{
                    alter(""编辑只能选择一个！"");
                }}
            }}
        }}

        function saveEditModel(){{
            {1}
            {2}
            $.ajax({{
                type: ""POST"",
                url: ""{3}Manager.aspx?type=edit"",
                data: postData,
                success: function (msg) {{
                    if (msg == ""0"") {{
                        $(""#dialog>p"").text(""修改成功！"");
                        SearchTrainee();
                    }} else {{
                        $(""#dialog>p"").text(msg);
                    }}

                    $('#dlg').dialog('close');        // close the dialog

                    // show dialog
                    $(""#dialog"").dialog({{
                        width: 180,
                        height: 100,
                        buttons: {{
                            Ok: function () {{
                                $(this).dialog(""close"");
                            }}
                        }}
                    }});
                }}
            }});
        }}
", editContent.ToString(), editSubmitContent.ToString(), postDataContent.ToString(), model.TableName.ToFirstUpper());
            }
            #endregion

            #region add
            if (model.IsAdd)
            {
                StringBuilder addContent = new StringBuilder();
                StringBuilder postDataContent = new StringBuilder("var postData = ");
                int index = 0;
                foreach (var item in model.AddColumns)
                {
                    string attribute = item.ColumnName.ToFirstUpper();
                    addContent.AppendFormat("\t\t\tvar txtAdd{0} = $(\"#txtAdd{0}\").textbox(\"getValue\");\r\n", attribute);

                    if (index == 0)
                    {
                        postDataContent.AppendFormat("\"txtAdd{0}=\" + encodeURI(txtAdd{0}) ", attribute);
                    }
                    else
                    {
                        postDataContent.AppendFormat(" + \"&txtAdd{0}=\" + encodeURI(txtAdd{0})", attribute);
                    }

                    index++;
                }

                if (postDataContent.ToString() == "var postData = ")
                {
                    postDataContent.Append("\"\"");
                }

                postDataContent.Append(";");

                content.AppendFormat(@"
        function newModel() {{
            $('#dlg-add').dialog('open').dialog('setTitle', '添加成员');
        }}

        function saveAddModel() {{
            {0}
            {1}
            $.ajax({{
                type: ""POST"",
                url: ""{2}Manager.aspx?type=add"",
                data: postData,
                success: function (msg) {{
                    if (msg == ""0"") {{
                        $(""#dialog>p"").text(""添加成功！"");
                        SearchTrainee();
                    }} else {{
                        $(""#dialog>p"").text(msg);
                    }}

                    $('#dlg').dialog('close');        // close the dialog

                    // show dialog
                    $(""#dialog"").dialog({{
                        width: 180,
                        height: 100,
                        buttons: {{
                            Ok: function () {{
                                $(this).dialog(""close"");
                            }}
                        }}
                    }});
                }}
            }});
        }}", addContent.ToString(), postDataContent.ToString(), model.TableName.ToFirstUpper());
            }
            #endregion

            #region del
            if (model.IsDel)
            {
                string template = string.Format(@"

        function destroyUser() {{
            var row = $('#dg').datagrid('getChecked');
            if (row && row.length > 0) {{
                $.messager.confirm('Confirm', '确定删除该数据吗?', function (r) {{
                    if (r) {{
                        var ids = """";
                        for(var index=0;i<r.length;i++){{
                            ids+=r[i].{1}+"","";
                        }}

                        ids = ids.substring(0, ids.length-1);
                        $.post('{0}Manager.aspx?type=delete', {{ ids: ids }}, function (result) {{
                            if (result.success) {{
                                $('#dg').datagrid('reload');    // reload the user data
                            }} else {{
                                $.messager.show({{    // show error message
                                    title: 'Error',
                                    msg: result.errorMsg
                                }});
                            }}
                        }}, 'json');
                    }}
                }});
            }}
        }}", model.TableName.ToFirstUpper(), model.MainKeyIdStr.ToFirstUpper());

                content.Append(template);
            }
            #endregion

            #region bat edit

            if (model.IsBatEdit && model.BatEditColumns.Count > 0)
            {
                StringBuilder batEditContent = new StringBuilder();
                batEditContent.AppendFormat("\t\t\tvar txtBatEdit{0} = $(\"#txtBatEdit{0}\").textbox(\"getValue\");\r\n", model.MainKeyIdStr.ToFirstUpper());
                StringBuilder postDataContent = new StringBuilder("var postData = ");
                postDataContent.AppendFormat("\"txtBatEdit{0}=\" + encodeURI(txtBatEdit{0}) ", model.MainKeyIdStr.ToFirstUpper());
                int index = 0;
                foreach (var item in model.AddColumns)
                {
                    string attribute = item.ColumnName.ToFirstUpper();
                    batEditContent.AppendFormat("\t\t\tvar txtBatEdit{0} = $(\"#txtBatEdit{0}\").textbox(\"getValue\");\r\n", attribute);
                    postDataContent.AppendFormat(" + \"&txtBatEdit{0}=\" + encodeURI(txtBatEdit{0})", attribute);

                    index++;
                }

                if (postDataContent.ToString() == "var postData = ")
                {
                    postDataContent.Append("\"\"");
                }

                postDataContent.Append(";");

                content.AppendFormat(@"

        function batEditModel() {{
            var row = $('#dg').datagrid('getChecked');
            if (row && row.length>0) {{
                $('#dlg-batedit').dialog('open').dialog('setTitle', '批量编辑成员');
                var ids = """";
                for(var i=0;i<row.length;i++){{
                    ids += row[i].{0} + "","";
                }}

                ids = ids.substring(0, ids.length - 1);
                $(""#txtBatEdit{0}"").val(ids);
            }}
        }}

        function saveBatEditModel() {{
            {1}
            {2}
            $.ajax({{
                type: ""POST"",
                url: ""{3}Manager.aspx?type=batedit"",
                data: postData,
                success: function (msg) {{
                    if (msg == ""0"") {{
                        $(""#dialog>p"").text(""批量修改成功！"");
                        SearchTrainee();
                    }} else {{
                        $(""#dialog>p"").text(msg);
                    }}

                    $('#dlg').dialog('close');        // close the dialog

                    // show dialog
                    $(""#dialog"").dialog({{
                        width: 180,
                        height: 100,
                        buttons: {{
                            Ok: function () {{
                                $(this).dialog(""close"");
                            }}
                        }}
                    }});
                }}
            }});
        }}
", model.MainKeyIdStr.ToFirstUpper(), batEditContent.ToString(), postDataContent.ToString(), model.TableName.ToFirstUpper());
            }

            #endregion

            #region export

            if (model.IsExport)
            {
                int index = 0;
                StringBuilder coditionContent = new StringBuilder();
                StringBuilder pagePost = new StringBuilder();
                foreach (var item in model.SearchColumns)
                {
                    string attribute = item.ColumnName.ToFirstUpper();

                    // 找到一个
                    coditionContent.AppendFormat("\t\t\tvar txtSearch{0} = $(\"#txtSearch{0}\").textbox(\"getValue\");\r\n", attribute);

                    // var pagePost = "&xxx=" + xxx + "&yyy=" +yyy;
                    if (index == 0)
                    {
                        pagePost.AppendFormat("\t\t\tvar pageData = \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                    }
                    else
                    {
                        pagePost.AppendFormat("+ \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                    }

                    index++;
                }

                if (model.SearchColumns.Count == 0)
                {
                    pagePost.Append("\t\t\tvar pageData = \"\";\r\n");
                }
                else
                {
                    pagePost.Append(";");
                }

                // selectExport
                content.AppendFormat(@"

        function selectExport() {{
            var row = $('#dg').datagrid('getChecked');
            if (row && row.length>0) {{
                var ids = """";
                for(var i=0;i<row.length;i++){{
                    ids += row[i].{0} + "","";
                }}

                document.location=""{1}Manager.aspx?type=down&ids="" + ids;
            }}
        }}

        function exportAll() {{
{2}
{3}
                document.location=""{1}Manager.aspx?type=downall"" + pageData;
        }}", model.MainKeyIdStr.ToFirstUpper(), model.TableName.ToFirstUpper(), coditionContent.ToString(), pagePost.ToString());
            }

            #endregion

            content.AppendLine("\r\n\t</script>");

            return content.ToString();
        }

        public static string CreateJsLoad(EasyUIModel model)
        {
            StringBuilder content = new StringBuilder();
            string template = @"
    <script type=""text/javascript"">
        $(function () {
            var pager = $('#dg').datagrid('getPager');
            $('#dg').datagrid('getPager').pagination({
                pageSize: 10, //每页显示的记录条数，默认为10    
                pageList: [10, 15, 20, 25], //可以设置每页记录条数的列表 
                onSelectPage: function (pageNumber, pageSize) {
                    var opts = $('#dg').datagrid('options');
                    opts.pageNumber = pageNumber;
                    opts.pageSize = pageSize;
                    pager.pagination('refresh', {
                        pageNumber: pageNumber,
                        pageSize: pageSize
                    });

                    SearchTrainee(pageNumber, pageSize); //每次更换页面时触发更改   
                },
                onBeforeRefresh: function (pageNumber, pageSize) {
                    // 好像已经被执行过了，这里就不需要了
                    SearchTrainee(pageNumber, pageSize); //每次更换页面时触发更改  
                },
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录'
            });
        });

        function SearchTrainee(page_Number, page_Size) {
            if (page_Number == undefined && page_Size == undefined) {
                page_Number = $('#dg').datagrid('getPager').data(""pagination"").options.pageNumber;   //pageNumber为datagrid的当前页码
                page_Size = $('#dg').datagrid('getPager').data(""pagination"").options.pageSize;       //pageSize为datagrid的每页记录条数
            } else {
                $('#dg').datagrid('options').pageNumber = page_Number;   //pageNumber为datagrid的当前页码
                $('#dg').datagrid('options').pageSize = page_Size;       //pageSize为datagrid的每页记录条数
            }
";

            content.Append(template);

            int index = 0;
            StringBuilder coditionContent = new StringBuilder();
            StringBuilder pagePost = new StringBuilder();
            foreach (var item in model.SearchColumns)
            {
                string attribute = item.ColumnName.ToFirstUpper();

                // 找到一个
                coditionContent.AppendFormat("\t\t\tvar txtSearch{0} = $(\"#txtSearch{0}\").val();\r\n", attribute);

                // var pagePost = "&xxx=" + xxx + "&yyy=" +yyy;
                if (index == 0)
                {
                    pagePost.AppendFormat("\t\t\tvar pageData = \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                }
                else
                {
                    pagePost.AppendFormat("+ \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                }

                index++;
            }

            pagePost.Append(";");
            content.Append(coditionContent.ToString());
            content.Append(pagePost);

            if (model.SearchColumns.Count == 0)
            {
                content.Append("\t\t\tvar pageData = \"\";\r\n");
            }

            template = string.Format(@"
            var postData = ""page="" + page_Number + ""&pageSize="" + page_Size + pageData;
            $(""#dg>tbody"").empty();
            $.ajax({{
                type: ""POST"",
                url: ""{0}Manager.aspx?type=load&t="" + new Date().getTime(),
                data: postData,
                success: function (msg) {{
                    var data = eval(""("" + msg + "")"");
                    $('#dg').datagrid('loadData', data.Data);
                    $('#dg').datagrid('getPager').pagination({{
                        //更新pagination的导航列表各参数  
                        total: data.ItemCount, //总数
                        pageSize: page_Size, //行数  
                        pageNumber: page_Number//页数  
                    }});
                }}
            }});
        }}

        $(document).ready(function () {{
            // $(""#dg"").datagrid(""hideColumn"", ""{1}"");
            // 不注释会造成翻页不响应 onSelectPage
            // $(""#dg"").datagrid({{ ""checkOnSelect"": true }});
            // $(""#dg"").datagrid({{ ""selectOnCheck"": false }});
            SearchTrainee(1, 10);

            if ($(""#btnsearch"") != null || $(""#btnsearch"") != undefined) {{
                $(""#btnsearch"").click(function () {{
                    SearchTrainee();
                }});
            }}
        }});
    </script>
", model.TableName.ToFirstUpper(), model.MainKeyIdStr.ToFirstUpper());

            content.Append(template);

            return content.ToString();
        }

        public static string CreateBottomContent()
        {
            return @"
</body>
</html>";
        }
    }
}
