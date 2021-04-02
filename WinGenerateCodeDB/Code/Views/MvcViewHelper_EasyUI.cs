using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class MvcViewHelper_EasyUI
    {
        public static string CreateView(string name_space, string table_name, int action, List<SqlColumnInfo> colList, string model_name, string dal_name)
        {
            StringBuilder aspxContent = new StringBuilder();
            aspxContent.Append(CreatePageHead());
            aspxContent.Append(CreateHeader(table_name));
            aspxContent.Append(CreateBodyHead(table_name));
            aspxContent.Append(CreateSearchContent(action, colList, table_name));
            aspxContent.Append(CreateCmdToolBar(action));
            aspxContent.Append(CreateDataGrid(action, colList, table_name));
            aspxContent.Append(CreateDialog(action, colList, table_name));
            aspxContent.Append(CreateNotifyMsg());
            aspxContent.Append(CreateJsDateFormat());
            aspxContent.Append(CreateJsOperation(action, colList, table_name));
            aspxContent.Append(CreateJsLoad(action, colList, table_name));
            aspxContent.Append(CreateBottomContent());

            return aspxContent.ToString();
        }

        private static string CreatePageHead()
        {
            return @"@{
    Layout = null;
}

<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
";
        }

        private static string CreateHeader(string table_name)
        {
            return string.Format(@"
<head>
    <title>{0}</title>
    <link rel=""stylesheet"" type=""text/css"" href=""http://www.jeasyui.net/Public/js/easyui/themes/default/easyui.css"" />
    <link rel=""stylesheet"" type=""text/css"" href=""http://www.jeasyui.net/Public/js/easyui/themes/icon.css"" />
    <link rel=""stylesheet"" type=""text/css"" href=""http://www.jeasyui.net/Public/js/easyui/themes/color.css"" />
    <link rel=""stylesheet"" type=""text/css"" href=""http://www.jeasyui.net/Public/js/easyui/demo/demo.css"" />
    <script type=""text/javascript"" src=""http://cdn.bootcss.com/jquery/3.1.1/jquery.min.js""></script>
    <script type=""text/javascript"" src=""http://www.jeasyui.net/Public/js/easyui/jquery.easyui.min.js""></script>
    <script type=""text/javascript"" src=""http://www.jeasyui.net/Public/js/easyui/locale/easyui-lang-zh_CN.js""></script>
    <script type=""text/javascript"" src=""http://www.jeasyui.net/Public/js/easyui/datagrid-detailview.js""></script>
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
", table_name);
        }

        private static string CreateBodyHead(string table_name)
        {
            return string.Format(@"
<body>
    <h2>{0}</h2>
", table_name);
        }

        private static string CreateSearchContent(int action, List<SqlColumnInfo> colList, string table_name)
        {
            string template = @"
    <div class=""demo-info"" style=""margin-bottom: 10px"">
        <div class=""demo-tip icon-tip"">
            &nbsp;</div>
        <div>{0}</div>
    </div>
";
            StringBuilder searchContent = new StringBuilder();
            if ((action & (int)action_type.query_list) == (int)action_type.query_list)
            {
                var queryList = Cache_VMData.GetVMList(table_name, VMType.Query, colList.ToNotMainIdList());
                foreach (var item in queryList)
                {
                    searchContent.AppendFormat("&nbsp;&nbsp;<label for=\"txtSearch{0}\">{1}：</label><input type=\"text\" id=\"txtSearch{0}\" />", item.Name, item.Name);
                }

                searchContent.Append("&nbsp;&nbsp;<input type=\"button\" id=\"btnsearch\" value=\"查询\" />");
            }

            return string.Format(template, searchContent.ToString());
        }

        private static string CreateCmdToolBar(int action)
        {
            StringBuilder toolBarContent = new StringBuilder(@"
<div id=""toolbar"">
");
            if ((action & (int)action_type.add) == (int)action_type.add)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-add"" plain=""true"" onclick=""newModel()"">新增</a>");
            }

            if ((action & (int)action_type.edit) == (int)action_type.edit)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-edit"" plain=""true"" onclick=""editModel()"">编辑</a>");
            }

            if ((action & (int)action_type.delete) == (int)action_type.delete)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-remove"" plain=""true"" onclick=""destroyModel()"">删除</a>");
            }

            if ((action & (int)action_type.export_all) == (int)action_type.export_all)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-remove"" plain=""true"" onclick=""exportAll()"">导出全部</a>");
            }

            if ((action & (int)action_type.export_select) == (int)action_type.export_select)
            {
                toolBarContent.Append(@"<a href=""javascript:void(0)"" class=""easyui-linkbutton"" iconcls=""icon-remove"" plain=""true"" onclick=""selectExport()"">导出选中</a>");
            }

            toolBarContent.Append(@"
</div>");

            return toolBarContent.ToString();
        }

        private static string CreateDataGrid(int action, List<SqlColumnInfo> colList, string table_name)
        {
            // singleselect=""true""
            if ((action & (int)action_type.query_list) == (int)action_type.query_list)
            {
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
                tbodyContent.AppendFormat("\t\t\t\t<th data-options=\"field:'{0}',checkbox:true\"></th>\r\n", colList.ToKeyId()); //  style=\"display: none;\"
                foreach (var item in colList.ToNotMainIdList())
                {
                    string attribute = item.Name;
                    tbodyContent.AppendFormat("\t\t\t\t<th data-options=\"field:'{0}',width:120,align:'center'\">{1}</th>\r\n", item.Name, item.Comment);
                }

                return string.Format(template, table_name, tbodyContent.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string CreateDialog(int action, List<SqlColumnInfo> colList, string table_name)
        {
            StringBuilder dialogContent = new StringBuilder();

            #region 添加
            if ((action & (int)action_type.add) == (int)action_type.add)
            {
                // 行数过多，分成两行
                string template = @"
    <div id=""dlg-add"" class=""easyui-dialog"" style=""width: 400px; height: 240px; padding: 10px 20px""
        closed=""true"" buttons=""#dlg-add-buttons"">
        <div class=""ftitle"">
        </div>
        <form id=""fm-add"" method=""post"" enctype=""multipart/form-data"" action=""""
        novalidate=""novalidate"">
{0}
        </form>
    </div>
    <div id=""dlg-add-buttons"">
        <a href=""javascript:void(0)"" class=""easyui-linkbutton c6"" iconcls=""icon-ok"" onclick=""saveAddModel()""
            style=""width: 90px"">保 存</a> <a href=""javascript:void(0)"" class=""easyui-linkbutton""
                iconcls=""icon-cancel"" onclick=""javascript:$('#dlg-add').dialog('close')"" style=""width: 90px"">
                取 消</a>
    </div>";

                StringBuilder content = new StringBuilder();
                content.AppendFormat(@"        <input type=""hidden"" id=""txtAdd{0}"" value="""" />", colList.ToKeyId());
                var addList = Cache_VMData.GetVMList(table_name, VMType.Add, colList.ToNotMainIdList());
                foreach (var item in addList)
                {
                    // &#12288; 占一个中文字符
                    content.AppendFormat(@"
        <div class=""fitem"">
            <label for=""txtAdd{0}"">{1}:</label>
            <input type=""text"" id=""txtAdd{0}"" name=""txtAdd{0}"" {2} />
        </div>", item.Name, item.Comment.PadLeftStr(4, "&emsp;"), item.DbType.ToEasyUIInputClassOptStr());
                }

                dialogContent.Append(string.Format(template, content.ToString()));
            }
            #endregion

            #region 编辑
            if ((action & (int)action_type.edit) == (int)action_type.edit)
            {
                // 行数过多，分成两行
                string template = @"
    <div id=""dlg-edit"" class=""easyui-dialog"" style=""width: 400px; height: 240px; padding: 10px 20px""
        closed=""true"" buttons=""#dlg-edit-buttons"">
        <div class=""ftitle"">
        </div>
        <form id=""fm-edit"" method=""post"" enctype=""multipart/form-data"" action=""""
        novalidate=""novalidate"">
{0}
        </form>
    </div>
    <div id=""dlg-edit-buttons"">
        <a href=""javascript:void(0)"" class=""easyui-linkbutton c6"" iconcls=""icon-ok"" onclick=""saveEditModel()""
            style=""width: 90px"">保 存</a> <a href=""javascript:void(0)"" class=""easyui-linkbutton""
                iconcls=""icon-cancel"" onclick=""javascript:$('#dlg-edit').dialog('close')"" style=""width: 90px"">
                取 消</a>
    </div>";

                StringBuilder content = new StringBuilder();
                content.AppendFormat(@"        <input type=""hidden"" id=""txtEdit{0}"" value="""" />", colList.ToKeyId());
                var editList = Cache_VMData.GetVMList(table_name, VMType.Edit, colList.ToNotMainIdList());
                foreach (var item in editList)
                {
                    // 初始化form显示数据
                    // &#12288; 占一个中文字符
                    content.AppendFormat(@"
        <div class=""fitem"">
            <label for=""txtEdit{0}"">{1}:</label>
            <input type=""text"" id=""txtEdit{0}"" name=""txtEdit{0}"" {2} />
        </div>", item.Name, item.Comment.PadLeftStr(4, "&emsp;"), item.DbType.ToEasyUIInputClassOptStr());
                }

                dialogContent.Append(string.Format(template, content.ToString()));
            }
            #endregion

            return dialogContent.ToString();
        }

        private static string CreateNotifyMsg()
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
        private static string CreateJsDateFormat()
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

        private static string CreateJsOperation(int action, List<SqlColumnInfo> colList, string table_name)
        {
            StringBuilder content = new StringBuilder(@"
    <script type=""text/javascript"">");

            #region edit
            if ((action & (int)action_type.edit) == (int)action_type.edit)
            {
                StringBuilder editSubmitContent = new StringBuilder();
                editSubmitContent.AppendFormat("\t\t\tvar txtEdit{0} = $(\"#txtEdit{0}\").val();\r\n", colList.ToKeyId());
                StringBuilder postDataContent = new StringBuilder("var postData = ");
                postDataContent.AppendFormat("\"txtEdit{0}=\" + encodeURI(txtEdit{0}) ", colList.ToKeyId());
                int index = 0;

                StringBuilder editContent = new StringBuilder();
                editContent.AppendFormat(@"                    $(""#txtEdit{0}"").val(row.{0});", colList.ToKeyId());
                var editList = Cache_VMData.GetVMList(table_name, VMType.Edit, colList.ToNotMainIdList());
                foreach (var item in editList)
                {
                    editContent.AppendFormat(@"
                    $(""#txtEdit{0}"").textbox(""setValue"", row.{0});", item.Name);
                    editSubmitContent.AppendFormat("\t\t\tvar txtEdit{0} = $(\"#txtEdit{0}\").textbox(\"getValue\");\r\n", item.Name);
                    postDataContent.AppendFormat(" + \"&txtEdit{0}=\" + encodeURI(txtEdit{0})", item.Name);

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
                url: ""/{3}/edit"",
                data: postData,
                success: function (msg) {{
                    if (msg == ""0"") {{
                        $(""#dialog>p"").text(""修改成功！"");
                        LoadData();
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
", editContent.ToString(),
 editSubmitContent.ToString(),
 postDataContent.ToString(),
 table_name);
            }
            #endregion

            #region add
            if ((action & (int)action_type.add) == (int)action_type.add)
            {
                StringBuilder addContent = new StringBuilder();
                StringBuilder postDataContent = new StringBuilder("var postData = ");
                int index = 0;
                var addList = Cache_VMData.GetVMList(table_name, VMType.Add, colList.ToNotMainIdList());
                foreach (var item in addList)
                {
                    addContent.AppendFormat("\t\t\tvar txtAdd{0} = $(\"#txtAdd{0}\").textbox(\"getValue\");\r\n", item.Name);
                    if (index == 0)
                    {
                        postDataContent.AppendFormat("\"txtAdd{0}=\" + encodeURI(txtAdd{0}) ", item.Name);
                    }
                    else
                    {
                        postDataContent.AppendFormat(" + \"&txtAdd{0}=\" + encodeURI(txtAdd{0})", item.Name);
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
                url: ""/{2}/add"",
                data: postData,
                success: function (msg) {{
                    if (msg == ""0"") {{
                        $(""#dialog>p"").text(""添加成功！"");
                        LoadData();
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
        }}", addContent.ToString(),
           postDataContent.ToString(),
           table_name);
            }
            #endregion

            #region del
            if ((action & (int)action_type.delete) == (int)action_type.delete)
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
                        $.post('/{0}/delete', {{ ids: ids }}, function (result) {{
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
        }}", table_name,
           colList.ToKeyId());

                content.Append(template);
            }
            #endregion

            #region export

            if ((action & (int)action_type.export_all) == (int)action_type.export_all)
            {
                int index = 0;
                StringBuilder coditionContent = new StringBuilder();
                StringBuilder pagePost = new StringBuilder();
                var queryList = Cache_VMData.GetVMList(table_name, VMType.Query, colList.ToNotMainIdList());
                foreach (var item in queryList)
                {
                    // 找到一个
                    coditionContent.AppendFormat("\t\t\tvar txtSearch{0} = $(\"#txtSearch{0}\").textbox(\"getValue\");\r\n", item.Name);

                    // var pagePost = "&xxx=" + xxx + "&yyy=" +yyy;
                    if (index == 0)
                    {
                        pagePost.AppendFormat("\t\t\tvar pageData = \"&txtSearch{0}=\" + txtSearch{0}", item.Name);
                    }
                    else
                    {
                        pagePost.AppendFormat("+ \"&txtSearch{0}=\" + txtSearch{0}", item.Name);
                    }

                    index++;
                }

                if (colList.ToNotMainIdList().Count == 0)
                {
                    pagePost.Append("\t\t\tvar pageData = \"\";\r\n");
                }
                else
                {
                    pagePost.Append(";");
                }

                // selectExport
                content.AppendFormat(@"
        function exportAll() {{
{2}
{3}
                document.location=""/{1}/downall?"" + pageData;
        }}", colList.ToKeyId(),
           table_name,
           coditionContent.ToString(),
           pagePost.ToString());
            }

            #endregion

            #region select export
            if ((action & (int)action_type.export_select) == (int)action_type.export_select)
            {
                int index = 0;
                StringBuilder coditionContent = new StringBuilder();
                StringBuilder pagePost = new StringBuilder();
                var queryList = Cache_VMData.GetVMList(table_name, VMType.Query, colList.ToNotMainIdList());
                foreach (var item in queryList)
                {
                    // 找到一个
                    coditionContent.AppendFormat("\t\t\tvar txtSearch{0} = $(\"#txtSearch{0}\").textbox(\"getValue\");\r\n", item.Name);

                    // var pagePost = "&xxx=" + xxx + "&yyy=" +yyy;
                    if (index == 0)
                    {
                        pagePost.AppendFormat("\t\t\tvar pageData = \"&txtSearch{0}=\" + txtSearch{0}", item.Name);
                    }
                    else
                    {
                        pagePost.AppendFormat("+ \"&txtSearch{0}=\" + txtSearch{0}", item.Name);
                    }

                    index++;
                }

                if (colList.ToNotMainIdList().Count == 0)
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

                document.location=""/{1}/down?ids="" + ids;
            }}
        }}", colList.ToKeyId(),
           table_name);
            }
            #endregion

            content.AppendLine("\r\n\t</script>");

            return content.ToString();
        }

        private static string CreateJsLoad(int action, List<SqlColumnInfo> colList, string table_name)
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

                    LoadData(pageNumber, pageSize); //每次更换页面时触发更改   
                },
                onBeforeRefresh: function (pageNumber, pageSize) {
                    // 好像已经被执行过了，这里就不需要了
                    LoadData(pageNumber, pageSize); //每次更换页面时触发更改  
                },
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录'
            });
        });

        function LoadData(page_Number, page_Size) {
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
            if ((action & (int)action_type.query_list) == (int)action_type.query_list)
            {
                foreach (var item in colList.ToNotMainIdList())
                {
                    // 找到一个
                    coditionContent.AppendFormat("\t\t\tvar txtSearch{0} = $(\"#txtSearch{0}\").val();\r\n", item.Name);

                    // var pagePost = "&xxx=" + xxx + "&yyy=" +yyy;
                    if (index == 0)
                    {
                        pagePost.AppendFormat("\t\t\tvar pageData = \"&txtSearch{0}=\" + txtSearch{0}", item.Name);
                    }
                    else
                    {
                        pagePost.AppendFormat("+ \"&txtSearch{0}=\" + txtSearch{0}", item.Name);
                    }

                    index++;
                }

                pagePost.Append(";");
                content.Append(coditionContent.ToString());
                content.Append(pagePost);

                if (colList.ToNotMainIdList().Count == 0)
                {
                    content.Append("\t\t\tvar pageData = \"\";\r\n");
                }

                template = string.Format(@"
            var postData = ""page="" + page_Number + ""&pageSize="" + page_Size + pageData;
            $(""#dg>tbody"").empty();
            $.ajax({{
                type: ""POST"",
                url: ""/{0}/load"",
                data: postData,
                dataType:""json"",
                success: function (msg) {{
                    var data = msg;
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
            LoadData(1, 10);

            if ($(""#btnsearch"") != null || $(""#btnsearch"") != undefined) {{
                $(""#btnsearch"").click(function () {{
                    LoadData();
                }});
            }}
        }});
    </script>
", table_name,
 colList.ToKeyId());

                content.Append(template);

                return content.ToString();
            }
            else
            {
                return "";
            }
        }

        private static string CreateBottomContent()
        {
            return @"
</body>
</html>";
        }
    }
}
