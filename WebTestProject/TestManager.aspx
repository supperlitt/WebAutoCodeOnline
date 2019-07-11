<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestManager.aspx.cs" Inherits="Test.TestManager" ValidateRequest="false" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>标题</title>
    <link href="http://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <link href="http://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="http://cdn.bootcss.com/bootstrap-datetimepicker/2.1.30/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="http://cdn.bootcss.com/jquery-confirm/2.0.0/jquery-confirm.min.css" rel="stylesheet" />
    <script src="http://cdn.bootcss.com/jquery/3.1.1/jquery.min.js"></script>
    <script src="http://cdn.bootcss.com/jquery-confirm/2.0.0/jquery-confirm.min.js"></script>
    <script src="http://cdn.bootcss.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="http://cdn.bootcss.com/bootstrap-datetimepicker/2.1.30/js/bootstrap-datetimepicker.min.js"></script>
</head>

<body>
    <div class="container">
        <h2>标题
        </h2>

        <div class="row form-group">
            <div class="col-md-4">
                <label for="txtSearchTestName">测试名称：</label><input type="text" id="txtSearchTestName" />
            </div>
            <div class="col-md-8"></div>
        </div>
        <div class="row form-group">
            <div class="col-md-4">
                <input type="button" id="btnsearch" value="查询" />
            </div>
            <div class="col-md-8"></div>
        </div>
        <div class="row form-group">
            <div class="col-md-9">
                <input type="button" value="新增" onclick='newModel();' />
                <input type="button" value="编辑" onclick='editModel();' />
                <input type="button" value="批量编辑" onclick='batEditModel();' />
                <input type="button" value="删除" onclick='destroyModel();' />
                <input type="button" value="选中导出" onclick='selectExport();' />
                <input type="button" value="导出全部" onclick='exportAll();' />
            </div>
        </div>
        <table id="tbcontent" class="table table-striped">
            <thead>
                <tr>
                    <th>
                        <input type="checkbox" id="chkAll" onclick="checkAll();" /></th>
                    <th>测试名称</th>
                    <th>测试密码</th>
                    <th>测试金额</th>
                    <th>添加时间</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div id="msgPager"></div>


        <!-- 模态框（Modal） -->
        <div class="modal fade" id="add_Modal" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" style="width: auto; max-width: 1200px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                        </button>
                        <h4>添加</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row form-group">
                            <div class="col-md-12">
                                <label for="txtAddTestName">测试名称:</label>
                                <input type="text" id="txtAddTestName" name="txtAddTestName" class="form-control" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12">
                                <label for="txtAddTestPwd">测试密码:</label>
                                <input type="text" id="txtAddTestPwd" name="txtAddTestPwd" class="form-control" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12">
                                <label for="txtAddTestMemory">测试金额:</label>
                                <input type="text" id="txtAddTestMemory" name="txtAddTestMemory" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            onclick="saveAddModel()">
                            添加
                        </button>
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>

        <!-- 模态框（Modal） -->
        <div class="modal fade" id="edit_Modal" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" style="width: auto; max-width: 1200px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                        </button>
                        <h4>编辑</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row form-group">
                            <input type="hidden" id="txtEditTestId" value="" />
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12">
                                <label for="txtEditTestName">测试名称:</label>
                                <input type="text" id="txtEditTestName" name="txtEditTestName" class="form-control" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12">
                                <label for="txtEditTestPwd">测试密码:</label>
                                <input type="text" id="txtEditTestPwd" name="txtEditTestPwd" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            onclick="saveEditModel()">
                            保存
                        </button>
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>

        <!-- 模态框（Modal） -->
        <div class="modal fade" id="batEdit_Modal" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" style="width: auto; max-width: 1200px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                        </button>
                        <h4>批量编辑</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row form-group">
                            <input type="hidden" id="txtBatEditTestId" value="" />
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12">
                                <label for="txtBatEditTestPwd">测试密码:</label>
                                <input type="text" id="txtBatEditTestPwd" name="txtBatEditTestPwd" class="form-control" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12">
                                <label for="txtBatEditTestMemory">测试金额:</label>
                                <input type="text" id="txtBatEditTestMemory" name="txtBatEditTestMemory" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            onclick="saveBatEditModel()">
                            保存
                        </button>
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>

        <div id="dialog" class="modal-dialog" title="提示">
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

            function checkAll() {
                {
                    var result = $("#chkAll")[0].checked;
                    $("#tbcontent tbody").find("input[type='checkbox']").each(function () {
                        {
                            $(this)[0].checked = result;
                        }
                    });
                }
            }
        </script>


        <script type="text/javascript">

            function editModel() {
                var testId = "";
                var testName = "";
                var testPwd = "";
                var checkCount = 0;
                $("#tbcontent tbody").find("input[type='checkbox']").each(function () {
                    if ($(this)[0].checked) {
                        testId = $(this).val();
                        testName = $(this).attr("tag_testName");
                        testPwd = $(this).attr("tag_testPwd");
                        checkCount++;
                    }
                });

                if (checkCount == 0) {
                    alert("请选择一个进行编辑！");
                } else if (checkCount > 1) {
                    alert("只能选择一个进行编辑！");
                } else {
                    $("#txtEditTestId").val(testId);
                    $("#txtEditTestName").val(testName);
                    $("#txtEditTestPwd").val(testPwd);
                    $("#edit_Modal").modal('show');
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
                    data: postData.replace("+", "%2b"), // 解决加号在传递中变成%20问题
                    success: function (msg) {
                        if (msg == "0") {
                            alert("保存成功！");
                            loadData();
                        } else {
                            alert(msg);
                        }

                        $('#edit_Modal').modal('hide');        // close the dialog
                    }
                });
            }

            function newModel() {
                $('#add_Modal').modal('show');
            }

            function saveAddModel() {

                var txtAddTestName = $("#txtAddTestName").val();
                var txtAddTestPwd = $("#txtAddTestPwd").val();
                var txtAddTestMemory = $("#txtAddTestMemory").val();
                var postData = "txtAddTestName=" + encodeURI(txtAddTestName) + "&txtAddTestPwd=" + encodeURI(txtAddTestPwd) + "&txtAddTestMemory=" + encodeURI(txtAddTestMemory);
                $.ajax({
                    type: "POST",
                    url: "TestInfoManager.aspx?type=add",
                    data: postData.replace("+", "%2b"),
                    success: function (msg) {
                        if (msg == "0") {
                            alert("添加成功！");
                            loadData();
                        } else {
                            alert(msg);
                        }

                        $('#add_Modal').modal('hide');
                    }
                });
            }

            function destroyModel() {
                var checkCount = 0;
                $("#tbcontent tbody").find("input[type='checkbox']").each(function () {
                    if ($(this)[0].checked) {
                        checkCount++;
                    }
                });

                if (checkCount > 0) {
                    $.confirm({
                        content: "确定删除选中数据吗?",
                        text: "确定删除选中数据吗?",
                        title: "提示",
                        confirm: function (button) {
                            var ids = "";
                            $("#tbcontent tbody").find("input[type='checkbox']").each(function () {
                                if ($(this)[0].checked) {
                                    ids += $(this).val() + ",";
                                }
                            });

                            ids = ids.substring(0, ids.length - 1);
                            $.post('TestInfoManager.aspx?type=delete', { ids: ids }, function (msg) {
                                if (msg == "0") {
                                    alert("删除成功");
                                    loadData();
                                } else {
                                    alert(msg);
                                }
                            }, 'json');
                        },
                        cancel: function (button) {
                            // nothing to do
                        },
                        confirmButton: "确定",
                        cancelButton: "取消",
                        post: true,
                        confirmButtonClass: "btn-danger",
                        cancelButtonClass: "btn-default",
                        dialogClass: "modal-dialog modal-lg" // Bootstrap classes for large modal
                    });
                } else {
                    alert("请勾选进行删除！");
                }
            }

            function batEditModel() {
                var checkCount = 0;
                var ids = "";
                $("#tbcontent tbody").find("input[type='checkbox']").each(function () {
                    if ($(this)[0].checked) {
                        ids += $(this).val() + ",";
                        checkCount++;
                    }
                });

                if (checkCount > 0) {
                    ids = ids.substring(0, ids.length - 1);
                    $("#txtBatEditTestId").val(ids);
                    $("#batEdit_Modal").modal('show');
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
                    data: postData.replace("+", "%2b"),
                    success: function (msg) {
                        if (msg == "0") {
                            alert("修改成功！");
                            loadData();
                        } else {
                            alert(msg);
                        }

                        $("#batEdit_Modal").modal('hide');      // close the dialog
                    }
                });
            }

            function selectExport() {
                var checkCount = 0;
                var ids = "";
                $("#tbcontent tbody").find("input[type='checkbox']").each(function () {
                    if ($(this)[0].checked) {
                        ids += $(this).val() + ",";
                        checkCount++;
                    }
                });

                if (checkCount > 0) {
                    document.location = "TestInfoManager.aspx?type=down&ids=" + ids;
                }
            }

            function exportAll() {

                var txtSearchTestName = $("#txtSearchTestName").val();
                var pageData = "&txtSearchTestName=" + txtSearchTestName;
                document.location = "TestInfoManager.aspx?type=downall" + pageData.replace("+", "%2b");
            }
        </script>


        <script type="text/javascript">
            var hpageSize = 13;
            var hpage = 1;
            $(document).ready(function () {
                loadData(hpage, hpageSize);

                if (($("#btnsearch") != null && $("#btnsearch") != undefined)) {
                    $("#btnsearch").click(function () { return loadData(hpage, hpageSize); });
                }
            });

            function loadData(page, pageSize) {
                if (page == undefined) {
                    page = hpage;
                }

                if (pageSize == undefined) {
                    pageSize = hpageSize;
                }

                hpage = page;
                hpageSize = pageSize;
                $("#tbcontent tbody").empty();
                $("#tbcontent tbody").append("<tr><td colspan='5'>内容加载中...</td></tr>");

                var txtSearchTestName = $("#txtSearchTestName").val();
                var pageData = "&txtSearchTestName=" + txtSearchTestName;
                var postData = "page=" + page + "&pageSize=" + pageSize + pageData;
                $.ajax({
                    type: "POST",
                    url: "TestInfoManager.aspx?type=loaddata",
                    data: postData.replace("+", "%2b"),
                    success: function (text) {
                        var data = eval("(" + text + ")");
                        var content = "";
                        for (var index in data.Data) {
                            var model = data.Data[index];
                            content += "<tr>";


                            var attr = " tag_testId='" + model.TestId + "'" + " tag_testName='" + model.TestName + "'" + " tag_testPwd='" + model.TestPwd + "'" + " tag_testMemory='" + model.TestMemory + "'" + " tag_addDate='" + model.AddDate + "'";
                            content += "<td><input type=\"checkbox\" id=\"chk" + model.TestId + "\" value=\"" + model.TestId + "\" " + attr + " /></td>";
                            content += "<td>" + model.TestName + "</td>";
                            content += "<td>" + model.TestPwd + "</td>";
                            content += "<td>" + model.TestMemory + "</td>";
                            content += "<td>" + model.AddDateStr + "</td>";
                            content += "</tr>";
                        }

                        $("#tbcontent tbody").empty();
                        $("#tbcontent tbody").append(content);

                        // 构造页码
                        var pageContent = "<ul class=\"pagination\"><li><a href=\"javascript:loadData(1," + pageSize + ");\">&laquo;</a></li>";
                        var startI = 1;
                        var endI = 1;
                        if (data.PageCount > 5) {
                            if ((page - 2) >= 1 && (page + 2) <= data.PageCount) {
                                startI = page - 2;
                                endI = page + 2;
                            }
                            else if ((page - 2) < 1) {
                                startI = 1;
                                endI = 5;
                            }
                            else {
                                startI = (data.PageCount - 4);
                                endI = data.PageCount;
                            }
                        } else {
                            startI = 1;
                            endI = data.PageCount;
                        }

                        for (var i = startI; i <= endI; i++) {
                            if (page == i) {
                                pageContent += "<li class=\"active\"><a href=\"javascript:void();\">" + i + "</a></li>";
                            }
                            else {
                                pageContent += "<li><a href=\"javascript:loadData(" + i + "," + pageSize + ");\">" + i + "</a></li>";
                            }
                        }

                        pageContent += "<li><a href=\"javascript:loadData(" + data.PageCount + "," + pageSize + ");\">&raquo;</a></li></ul>";
                        $("#msgPager").empty();
                        $("#msgPager").append(pageContent);
                    }
                });
            }
        </script>

    </div>
</body>
</html>
