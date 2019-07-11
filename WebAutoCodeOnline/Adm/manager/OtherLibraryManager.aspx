<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherLibraryManager.aspx.cs" Inherits="WebAutoCodeOnline.Adm.OtherLibraryManager" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <!-- 新 Bootstrap 核心 CSS 文件 -->
    <link href="../../js/bootstrap-3.3.0-dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- 可选的Bootstrap主题文件（一般不使用） -->
    <link href="../../js/bootstrap-3.3.0-dist/css/bootstrap-theme.min.css" rel="stylesheet" />

    <!-- jQuery文件。务必在bootstrap.min.js 之前引入 -->
    <script src="../../js/jquery.min.js"></script>

    <!-- 最新的 Bootstrap 核心 JavaScript 文件 -->
    <script src="../../js/bootstrap-3.3.0-dist/js/bootstrap.min.js"></script>
</head>

<body>
    <div class="container">
        <h2></h2>
        <div class="row form-group">
            <div class="col-md-4">
                <label for="txtKey">关键字：</label><input type="text" id="txtKey" />
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
            <div class="col-md-7">
                <input type="hidden" value="hidTitle1" />
                <input type="hidden" value="hidTitle2" />
                <input type="button" value="新增" onclick='newModel();' />
                <input type="button" value="编辑" onclick='editModel();' />
                <input type="button" value="编辑标题" onclick='editTitle();' />
                <input type="button" value="删除选中" onclick='destroyModel();' />
            </div>
        </div>
        <table id="tbcontent" class="table table-striped">
            <thead>
                <tr>
                    <th>
                        <input type="checkbox" id="chkAll" onclick="checkAll();" /></th>
                    <th>名称</th>
                    <th>URL地址</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div id="msgPager"></div>

        <!-- 模态框（Modal） -->
        <div class="modal fade" id="add_Modal" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" style="width: auto; max-width: 400px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                        </button>
                        <h4>添加</h4>
                    </div>
                    <div class="modal-body">
                        <div class="container">
                            <div class="row form-group">
                                <div class="col-md-12">
                                    <label for="txtAddText">名称:</label>
                                    <input type="text" id="txtAddText" name="txtAddText" />
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-12">
                                    <label for="txtAddAddr">网址:</label>
                                    <input type="text" id="txtAddAddr" name="txtAddAddr" />
                                </div>
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
            <div class="modal-dialog" style="width: auto; max-width: 400px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                        </button>
                        <h4>编辑</h4>
                    </div>
                    <div class="modal-body">
                        <div class="container">
                            <div class="row form-group">
                                <input type="hidden" id="txtEditId" value="" />
                            </div>
                            <div class="row form-group">
                                <div class="col-md-12">
                                    <label for="txtEditText">名称:</label>
                                    <input type="text" id="txtEditText" name="txtEditText" />
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-12">
                                    <label for="txtEditAddr">网址:</label>
                                    <input type="text" id="txtEditAddr" name="txtEditAddr" />
                                </div>
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

        <!-- 模态框（Modal） 编辑标题 -->
        <div class="modal fade" id="edit_Title" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" style="width: auto; max-width: 400px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                        </button>
                        <h4>编辑标题</h4>
                    </div>
                    <div class="modal-body">
                        <div class="container">
                            <div class="row form-group">
                                <input type="hidden" id="Hidden1" value="" />
                            </div>
                            <div class="row form-group">
                                <div class="col-md-12">
                                    <label for="txtTitle1">标题1:</label>
                                    <input type="text" id="txtTitle1" name="txtTitle1" />
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-12">
                                    <label for="txtTitle2">标题2:</label>
                                    <input type="text" id="txtTitle2" name="txtTitle2" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            onclick="saveEditTitle()">
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
            function editTitle() {
                $("#txtEditTitle1").val($("#hidTitle1").val());
                $("#txtEditTitle2").val($("#hidTitle2").val());

                $("#edit_Title").modal('show');
            }

            function saveEditTitle() {
                var txtEditTitle1 = $("#txtEditTitle1").val();
                var txtEditTitle2 = $("#txtEditTitle2").val();
                var postData = "txtEditTitle1=" + encodeURI(txtEditTitle1) + "&txtEditTitle2=" + encodeURI(txtEditTitle2);
                $.ajax({
                    type: "POST",
                    url: "OtherLibraryManager.aspx?type=edittitle",
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

            function editModel() {
                var id = "";
                var checkCount = 0;
                $("#tbcontent tbody").find("input[type='checkbox']").each(function () {
                    if ($(this)[0].checked) {
                        id = $(this).val();
                        checkCount++;
                    }
                });

                if (checkCount == 0) {
                    alert("请选择一个进行编辑！");
                } else if (checkCount > 1) {
                    alert("只能选择一个进行编辑！");
                } else {
                    $("#txtEditId").val(id);
                    $("#edit_Modal").modal('show');
                }
            }

            function saveEditModel() {
                var txtEditId = $("#txtEditId").val();
                var txtEditText = $("#txtEditText").val();
                var txtEditUrl = $("#txtEditUrl").val();
                var postData = "txtEditId=" + encodeURI(txtEditId) + "&txtEditText=" + encodeURI(txtEditText) + "&txtEditUrl=" + encodeURI(txtEditUrl);
                $.ajax({
                    type: "POST",
                    url: "OtherLibraryManager.aspx?type=edit",
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
                var txtAddText = $("#txtAddText").val();
                var txtAddAddr = $("#txtAddAddr").val();
                var postData = "txtAddText=" + encodeURI(txtAddText) + "&txtAddAddr=" + encodeURI(txtAddAddr);
                $.ajax({
                    type: "POST",
                    url: "OtherLibraryManager.aspx?type=add",
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
                            $.post('OtherLibraryManager.aspx?type=delete', { ids: ids }, function (msg) {
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
        </script>

        <script type="text/javascript">
            $(document).ready(function () {
                loadData();

                if (($("#btnsearch") != null && $("#btnsearch") != undefined)) {
                    $("#btnsearch").click(function () { return loadData(); });
                }
            });

            function loadData() {
                $("#tbcontent tbody").empty();
                $("#tbcontent tbody").append("<tr><td colspan='3'>内容加载中...</td></tr>");

                var txtKey = $("#txtKey").val();
                var postData = "txtKey=" + txtKey;
                $.ajax({
                    type: "POST",
                    url: "OtherLibraryManager.aspx?type=loaddata",
                    data: postData.replace("+", "%2b"),
                    success: function (text) {
                        var data = eval("(" + text + ")");
                        var content = "";
                        $("#hidTitle1").val(data.Title1);
                        $("#hidTitle2").val(data.Title2);
                        for (var index in data.Data) {
                            var model = data.Data[index];
                            content += "<tr>";

                            var attr = " tag_id='" + model.Id + "'" + " tag_Text='" + model.Text + "' tag_Url='" + model.Url + "' ";
                            content += "<td><input type=\"checkbox\" id=\"chk" + model.Id + "\" value=\"" + model.Id + "\" " + attr + " /></td>";
                            content += "<td>" + model.Text + "</td>";
                            content += "<td>" + model.Url + "</td>";
                            content += "</tr>";
                        }

                        $("#tbcontent tbody").empty();
                        $("#tbcontent tbody").append(content);
                        if (window.parent.SetWinHeight)
                            window.parent.SetWinHeight();
                    }
                });
            }
        </script>
    </div>
</body>
</html>
