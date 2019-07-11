<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Test.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Test</title>
    <link href="http://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <link href="http://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" rel="stylesheet" />
    <script src="http://cdn.bootcss.com/jquery/3.1.1/jquery.min.js"></script>
    <script src="http://cdn.bootcss.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href="http://cdn.bootcss.com/bootstrap-datetimepicker/2.1.30/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <script src="http://cdn.bootcss.com/bootstrap-datetimepicker/2.1.30/js/bootstrap-datetimepicker.min.js"></script>
    <script src="http://cdn.bootcss.com/bootstrap-datetimepicker/2.1.30/js/locales/bootstrap-datetimepicker.zh-CN.js"></script>
</head>

<body>
    <div class="container">
        <h2></h2>
        <div class="row">
            <div class="col-md-4">
                TestDate：<input size="19" type="text" value="" id="txtSearchTestDate" placeholder="选择指定日期">
                <script type="text/javascript">
                    $('#txtSearchTestDate').datetimepicker({
                        minView: "month", //选择日期后，不会再跳转去选择时分秒 
                        format: "yyyy-mm-dd hh:ii:ss", //选择日期后，文本框显示的日期格式 
                        language: 'zh-CN', //汉化 
                        autoclose: true //选择日期后自动关闭 
                    }).on('changeDate', function (ev) {
                        loadData(1, pageSize);
                    });
                </script>
            </div>
            <div class="col-md-4">
                <label for="txtSearchTestName">TestName：</label><input type="text" id="txtSearchTestName" />
            </div>
            <div class="col-md-4">
                <label for="txtSearchTestType">TestType：</label><input type="text" id="txtSearchTestType" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-4"><input type="button" id="btnsearch" value="查询" /></div>
            <div class="col-md-8"></div></div>
        <div class="row">
            <div class="col-md-11">
                <input type="button" value="新增" onclick="newModel();" />
                <input type="button" value="编辑" onclick="editModel();" />
                <input type="button" value="批量编辑" onclick="batEditModel();" />
                <input type="button" value="删除" onclick="destroyModel();" />
                <input type="button" value="删除选中" onclick="destroyBatModel();" />
                <input type="button" value="选中导出" onclick="selectExport();" />
                <input type="button" value="导出全部" onclick="exportAll();" />
            </div>
            <div class="col-md-1"></div>
        </div>
        <table id="tbcontent" class="table table-striped">
            <thead>
                <tr>
                    <th><input type="checkbox" id="chkAll" onclick="checkAll();" /></th>
                    <th>TestName</th>
                    <th>TestDate</th>
                    <th>TestType</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <!-- 模态框（Modal） -->
        <div class="modal fade" id="add_Modal" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" style="width: auto; max-width: 1200px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true"></button>
                        <h4>添加</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <input type="hidden" id="txtAddId" value="" />
                            <div class="col-md-4">
                                <label for="txtAddTestName">TestName:</label>
                                <input type="text" id="txtAddTestName" name="txtAddTestName"/>
                            </div>
                            <div class="col-md-4">
                                <label for="txtAddTestDate">TestDate:</label>
                                <input type="text" id="txtAddTestDate" name="txtAddTestDate"/>
                            </div>
                            <div class="col-md-4">
                                <label for="txtAddTestType">TestType:</label>
                                <input type="text" id="txtAddTestType" name="txtAddTestType"/>
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
                            data-dismiss="modal" aria-hidden="true"></button>
                        <h4>编辑</h4>
                    </div>
                    <div class="modal-body">
                        <input type="hidden" id="txtEditId" value="" />
                        <div class="col-md-4">
                            <label for="txtEditTestName">TestName:</label>
                            <input type="text" id="txtEditTestName" name="txtEditTestName"/>
                        </div>
                        <div class="col-md-4">
                            <label for="txtEditTestDate">TestDate:</label>
                            <input type="text" id="txtEditTestDate" name="txtEditTestDate"/>
                        </div>
                        <div class="col-md-4">
                            <label for="txtEditTestType">TestType:</label>
                            <input type="text" id="txtEditTestType" name="txtEditTestType"/>
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
                            data-dismiss="modal" aria-hidden="true"></button>
                        <h4>批量编辑</h4>
                    </div>
                    <div class="modal-body">
                        <input type="hidden" id="txtBatEditId" value="" />
                        <div class="col-md-4">
                            <label for="txtBatEditTestDate">TestDate:</label>
                            <input type="text" id="txtBatEditTestDate" name="txtBatEditTestDate"/>
                        </div>
                        <div class="col-md-4">
                            <label for="txtBatEditTestType">TestType:</label>
                            <input type="text" id="txtBatEditTestType" name="txtBatEditTestType"/>
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
        
        <div id="dialog" class="modal fade" role="dialog">
            <div class="modal-dialog" style="width: auto; max-width: 500px;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div id="dialog-msg"></div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
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
            function newModel() {
                $('#add_Modal').modal('show');
            }

            function editModel() {
                var id = "";
                var testName = "";
                var testDate = "";
                var testType = "";

                var checkCount=0;
                $("#tbcontent tbody").find("input[type='checkbox']").each(function () {
                    if ($(this)[0].checked) {
                        id = $(this).val();
                        testName = $(this).attr("tag_testName");
                        testDate = $(this).attr("tag_testDate");
                        testType = $(this).attr("tag_testType");
                        checkCount++;
                    }
                });

                if (checkCount == 0) {
                    alert("请选择一个进行编辑！");
                } else if (checkCount > 1) {
                    alert("只能选择一个进行编辑！");
                } else {
                    $("#txtEditId").val(id);
                    $("#txtEditTestName").val(testName);
                    $("#txtEditTestDate").val(testDate);
                    $("#txtEditTestType").val(testType);
                    $("#edit_Modal").modal('show');
                }
            }

            function saveEditModel() {
                var txtEditId = $("#txtEditId").val();
                var txtEditTestName = $("#txtEditTestName").val();
                var txtEditTestDate = $("#txtEditTestDate").val();
                var txtEditTestType = $("#txtEditTestType").val();
                var postData = "txtEditId=" + encodeURI(txtEditId) + "&txtEditTestName=" + encodeURI(txtEditTestName) + "&txtEditTestDate=" + encodeURI(txtEditTestDate) + "&txtEditTestType=" + encodeURI(txtEditTestType);

                $.ajax({
                    type: "POST",
                    url: "Test.aspx?type=edit",
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
            
            function batEditModel() {
                var row = $('#dg').datagrid('getChecked');
                if (row && row.length>0) {
                    $('#dlg-batedit').dialog('open').dialog('setTitle', '批量编辑成员');
                    var ids = "";
                    for(var i=0;i<row.length;i++){
                        ids += row[i].{0} + ",";
                    }

                    ids = ids.substring(0, ids.length - 1);
                    $("#txtBatEdit{0}").val(ids);
                }
            }

            function saveBatEditModel() {
            }

            function destroyModel() {
            }

            function destroyBatModel() {
            }

            function selectExport() {
            }

            function exportAll() {
            }
        </script>
        <script type="text/javascript">
            var pageSize = 5;
            var hpage = 1;
            $(document).ready(function () {
                loadData(hpage, pageSize);
            });

            function loadData(page, pageSize) {
                hpage = page;
                $("#tbcontent tbody").empty();
                $("#tbcontent tbody").append("<tr><td colspan='6'>内容加载中...</td></tr>");
                $.post("Manager.aspx?type=loaddata", { "page": page, "pagesize": pageSize, "date": $("#datetimepicker").val() }, function (data) {
                    var content = "";
                    for (var index in data.Result) {
                        var model = data.Result[index];
                        var addr = model.Camera.Area + model.Camera.Township + model.Camera.Village;
                        content += "<tr><td><input type=\"checkbox\" id=\"chk" + model.Id + "\" value=\"" + model.Id + "\" /></td><td>" + model.Id + "</td>";
                        content += "<td>" + addr + "</td>";
                        content += "<td>" + model.AlarmDateStr + "</td>";
                        content += "<td>" + model.AlarmPrividerName + "</td>";
                        content += "</tr>";
                    }

                    $("#tbcontent tbody").empty();
                    $("#tbcontent tbody").append(content);
                    $("#lblUserName").html(data.UserName);

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
                }, 'json');
            }
        </script>
    </div>
</body>
</html>
