<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestInfoManager.aspx.cs" Inherits="Test.TestInfoManager" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>标题</title>
    <link rel="stylesheet" type="text/css" href="easyui/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="easyui/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="easyui/themes/color.css" />
    <link rel="stylesheet" type="text/css" href="easyui/demo/demo.css" />
    <script type="text/javascript" src="easyui/jquery.min.js"></script>
    <script type="text/javascript" src="easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="easyui/datagrid-detailview.js"></script>
    <script src="easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <style type="text/css">
        #dg tbody tr td {
            text-align: center;
        }

        #fm-add div, #fm-edit div, #fm-batedit div {
            padding: 5px;
        }
    </style>
</head>
<body>
    <h2>标题</h2>

    <div class="demo-info" style="margin-bottom: 10px">
        <div class="demo-tip icon-tip">
            &nbsp;
        </div>
        <div>&nbsp;&nbsp;<label for="txtSearchTestName">测试名称：</label><input type="text" id="txtSearchTestName" />&nbsp;&nbsp;<input type="button" id="btnsearch" value="查询" /></div>
    </div>
    <div id="toolbar">
        <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="newModel()">新增</a><a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="editModel()">编辑</a><a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="batEditModel()">批量编辑</a><a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="destroyModel()">删除</a><a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="selectExport()">选中导出</a><a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="exportAll()">导出全部</a>
    </div>
    <table id="dg" class="easyui-datagrid" style="width: 1200px; height: auto" pagination="true" sortname="itemid"
        sortorder="desc" title="标题" fitcolumns="true">
        <thead>
            <tr>
                <th data-options="field:'TestId',checkbox:true">测试Id</th>
                <th data-options="field:'TestName',width:120,align:'center'">测试名称</th>
                <th data-options="field:'TestPwd',width:120,align:'center'">测试密码</th>
                <th data-options="field:'TestMemory',width:120,align:'center'">测试金额</th>
                <th data-options="field:'AddDateStr',width:120,align:'center'">添加时间</th>

            </tr>
        </thead>
    </table>
    <div id="dlg-add" class="easyui-dialog" style="width: 400px; height: 240px; padding: 10px 20px"
        closed="true" buttons="#dlg-add-buttons">
        <div class="ftitle">
        </div>
        <form id="fm-add" method="post" enctype="multipart/form-data" action="TestInfoManager.aspx?type=add"
            novalidate="novalidate">

            <div class="fitem">
                <label for="txtAddTestName">测试名称:</label>
                <input type="text" id="txtAddTestName" name="txtAddTestName" class="easyui-validatebox" data-options="required:true" />
            </div>
            <div class="fitem">
                <label for="txtAddTestPwd">测试密码:</label>
                <input type="text" id="txtAddTestPwd" name="txtAddTestPwd" class="easyui-validatebox" data-options="required:true" />
            </div>
            <div class="fitem">
                <label for="txtAddTestMemory">测试金额:</label>
                <input type="text" id="txtAddTestMemory" name="txtAddTestMemory" class="easyui-numberbox" data-options="precision:2,required:true" />
            </div>
        </form>
    </div>
    <div id="dlg-add-buttons">
        <a href="javascript:void(0)" class="easyui-linkbutton c6" iconcls="icon-ok" onclick="saveAddModel()"
            style="width: 90px">保 存</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-cancel" onclick="javascript:$('#dlg-add').dialog('close')" style="width: 90px">取 消</a>
    </div>
    <div id="dlg-edit" class="easyui-dialog" style="width: 400px; height: 240px; padding: 10px 20px"
        closed="true" buttons="#dlg-edit-buttons">
        <div class="ftitle">
        </div>
        <form id="fm-edit" method="post" enctype="multipart/form-data" action="TestInfoManager.aspx?type=edit"
            novalidate="novalidate">
            <input type="hidden" id="txtEditTestId" value="" />
            <div class="fitem">
                <label for="txtEditTestName">测试名称:</label>
                <input type="text" id="txtEditTestName" name="txtEditTestName" class="easyui-validatebox" data-options="required:true" />
            </div>
            <div class="fitem">
                <label for="txtEditTestPwd">测试密码:</label>
                <input type="text" id="txtEditTestPwd" name="txtEditTestPwd" class="easyui-validatebox" data-options="required:true" />
            </div>
        </form>
    </div>
    <div id="dlg-edit-buttons">
        <a href="javascript:void(0)" class="easyui-linkbutton c6" iconcls="icon-ok" onclick="saveEditModel()"
            style="width: 90px">保 存</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-cancel" onclick="javascript:$('#dlg-edit').dialog('close')" style="width: 90px">取 消</a>
    </div>
    <div id="dlg-batedit" class="easyui-dialog" style="width: 400px; height: 240px; padding: 10px 20px"
        closed="true" buttons="#dlg-batedit-buttons">
        <div class="ftitle">
        </div>
        <form id="fm-batedit" method="post" enctype="multipart/form-data" action="TestInfoManager.aspx?type=batedit"
            novalidate="novalidate">
            <input type="hidden" id="txtBatEditTestId" value="" />
            <div class="fitem">
                <label for="txtBatEditTestPwd">测试密码:</label>
                <input type="text" id="txtBatEditTestPwd" name="txtBatEditTestPwd" class="easyui-validatebox" data-options="required:true" />
            </div>
            <div class="fitem">
                <label for="txtBatEditTestMemory">测试金额:</label>
                <input type="text" id="txtBatEditTestMemory" name="txtBatEditTestMemory" class="easyui-numberbox" data-options="precision:2,required:true" />
            </div>
        </form>
    </div>
    <div id="dlg-batedit-buttons">
        <a href="javascript:void(0)" class="easyui-linkbutton c6" iconcls="icon-ok" onclick="saveBatEditModel()"
            style="width: 90px">保 存</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-cancel" onclick="javascript:$('#dlg-batedit').dialog('close')" style="width: 90px">取 消</a>
    </div>
    <div id="dialog" title="提示">
        <p style="text-align: center;">
        </p>
    </div>

    <script type="text/javascript">
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

    <script type="text/javascript">

        function editModel() {
            var row = $('#dg').datagrid('getChecked');
            if (row) {
                if (row.length == 1) {
                    row = row[0];
                    $('#dlg-edit').dialog('open').dialog('setTitle', '编辑成员');
                    $('#fm-edit').form('load', row);
                    $("#txtEditTestId").val(row.TestId);
                    $("#txtEditTestName").val(row.TestName);
                    $("#txtEditTestPwd").val(row.TestPwd);
                }
                else {
                    alter("编辑只能选择一个！");
                }
            }
        }

        function saveEditModel() {
            var txtEditTestId = $("#txtEditTestId").val();
            var txtEditTestName = $("#txtEditTestName").val();
            var txtEditTestPwd = $("#txtEditTestPwd").val();

            var postData = "txtEditTestId=" + encodeURI(txtEditTestId) + "&txtEditTestName=" + encodeURI(txtEditTestName) + "&txtEditTestPwd=" + encodeURI(txtEditTestPwd);
            $.ajax({
                type: "POST",
                url: "TestInfoManager.aspx?type=edit",
                data: postData,
                success: function (msg) {
                    if (msg == "0") {
                        $("#dialog>p").text("修改成功！");
                        SearchTrainee();
                    } else {
                        $("#dialog>p").text(msg);
                    }

                    $('#dlg').dialog('close');        // close the dialog

                    // show dialog
                    $("#dialog").dialog({
                        width: 180,
                        height: 100,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            });
        }

        function newModel() {
            $('#dlg-add').dialog('open').dialog('setTitle', '添加成员');
        }

        function saveAddModel() {
            var txtAddTestName = $("#txtAddTestName").val();
            var txtAddTestPwd = $("#txtAddTestPwd").val();
            var txtAddTestMemory = $("#txtAddTestMemory").val();

            var postData = "txtAddTestName=" + encodeURI(txtAddTestName) + "&txtAddTestPwd=" + encodeURI(txtAddTestPwd) + "&txtAddTestMemory=" + encodeURI(txtAddTestMemory);
            $.ajax({
                type: "POST",
                url: "TestInfoManager.aspx?type=add",
                data: postData,
                success: function (msg) {
                    if (msg == "0") {
                        $("#dialog>p").text("添加成功！");
                        SearchTrainee();
                    } else {
                        $("#dialog>p").text(msg);
                    }

                    $('#dlg').dialog('close');        // close the dialog

                    // show dialog
                    $("#dialog").dialog({
                        width: 180,
                        height: 100,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            });
        }

        function destroyUser() {
            var row = $('#dg').datagrid('getChecked');
            if (row && row.length > 0) {
                $.messager.confirm('Confirm', '确定删除该数据吗?', function (r) {
                    if (r) {
                        var ids = "";
                        for (var index = 0; i < r.length; i++) {
                            ids += r[i].TestId + ",";
                        }

                        ids = ids.substring(0, ids.length - 1);
                        $.post('TestInfoManager.aspx?type=delete', { ids: ids }, function (result) {
                            if (result.success) {
                                $('#dg').datagrid('reload');    // reload the user data
                            } else {
                                $.messager.show({    // show error message
                                    title: 'Error',
                                    msg: result.errorMsg
                                });
                            }
                        }, 'json');
                    }
                });
            }
        }

        function batEditModel() {
            var row = $('#dg').datagrid('getChecked');
            if (row && row.length > 0) {
                $('#dlg-batedit').dialog('open').dialog('setTitle', '批量编辑成员');
                var ids = "";
                for (var i = 0; i < row.length; i++) {
                    ids += row[i].TestId + ",";
                }

                ids = ids.substring(0, ids.length - 1);
                $("#txtBatEditTestId").val(ids);
            }
        }

        function saveBatEditModel() {
            var txtBatEditTestId = $("#txtBatEditTestId").val();
            var txtBatEditTestName = $("#txtBatEditTestName").val();
            var txtBatEditTestPwd = $("#txtBatEditTestPwd").val();
            var txtBatEditTestMemory = $("#txtBatEditTestMemory").val();

            var postData = "txtBatEditTestId=" + encodeURI(txtBatEditTestId) + "&txtBatEditTestName=" + encodeURI(txtBatEditTestName) + "&txtBatEditTestPwd=" + encodeURI(txtBatEditTestPwd) + "&txtBatEditTestMemory=" + encodeURI(txtBatEditTestMemory);
            $.ajax({
                type: "POST",
                url: "TestInfoManager.aspx?type=batedit",
                data: postData,
                success: function (msg) {
                    if (msg == "0") {
                        $("#dialog>p").text("批量修改成功！");
                        SearchTrainee();
                    } else {
                        $("#dialog>p").text(msg);
                    }

                    $('#dlg').dialog('close');        // close the dialog

                    // show dialog
                    $("#dialog").dialog({
                        width: 180,
                        height: 100,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            });
        }


        function selectExport() {
            var row = $('#dg').datagrid('getChecked');
            if (row && row.length > 0) {
                var ids = "";
                for (var i = 0; i < row.length; i++) {
                    ids += row[i].TestId + ",";
                }

                document.location = "TestInfoManager.aspx?type=down&ids=" + ids;
            }
        }

        function exportAll() {
            var txtSearchTestName = $("#txtSearchTestName").val();
            var pageData = "&txtSearchTestName=" + txtSearchTestName;

            document.location = "TestInfoManager.aspx?type=downall" + pageData;
        }

    </script>

    <script type="text/javascript">

        $(function () {
            var pager = $('#dg').datagrid('getPager');
            $('#dg').datagrid('getPager').pagination({
                //pageSize: 10, //每页显示的记录条数，默认为10    
                //pageList: [10, 15, 20, 25], //可以设置每页记录条数的列表 
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
                    // SearchTrainee(pageNumber, pageSize); //每次更换页面时触发更改  
                },
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录'
            });
            alert("4");
        });

        function SearchTrainee(page_Number, page_Size) {
            if (page_Number == undefined && page_Size == undefined) {
                page_Number = $('#dg').datagrid('getPager').data("pagination").options.pageNumber;   //pageNumber为datagrid的当前页码
                page_Size = $('#dg').datagrid('getPager').data("pagination").options.pageSize;       //pageSize为datagrid的每页记录条数
            } else {
                $('#dg').datagrid('options').pageNumber = page_Number;   //pageNumber为datagrid的当前页码
                $('#dg').datagrid('options').pageSize = page_Size;       //pageSize为datagrid的每页记录条数
            }

            var txtSearchTestName = $("#txtSearchTestName").val();
            var pageData = "&txtSearchTestName=" + txtSearchTestName;
            var postData = "page=" + page_Number + "&pageSize=" + page_Size + pageData;
            $("#dg>tbody").empty();
            $.ajax({
                type: "POST",
                url: "TestInfoManager.aspx?type=load&t=" + new Date().getTime(),
                data: postData,
                success: function (msg) {
                    var data = eval("(" + msg + ")");
                    $('#dg').datagrid('loadData', data.Data);
                    $('#dg').datagrid('getPager').pagination({
                        //更新pagination的导航列表各参数  
                        total: data.ItemCount, //总数
                        pageSize: page_Size, //行数  
                        pageNumber: page_Number//页数  
                    });
                }
            });
        }

        $(document).ready(function () {
            // $("#dg").datagrid("hideColumn", "TestId");
            // 不注释会造成翻页不响应 onSelectPage
            //$("#dg").datagrid({ "checkOnSelect": false });
            //$("#dg").datagrid({ "selectOnCheck": false });
            SearchTrainee(1, 10);

            if ($("#btnsearch") != null || $("#btnsearch") != undefined) {
                $("#btnsearch").click(function () {
                    SearchTrainee();
                });
            }
        });
    </script>

</body>
</html>
