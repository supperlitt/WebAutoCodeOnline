using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceHelper
{
    /// <summary>
    /// 基础版，最简单的登录（）
    /// </summary>
    public class Back_Bootstrap_Login_01 : IAspx
    {
        public string Create_Aspx(ModuleInfo module)
        {
            StringBuilder content = new StringBuilder();
            content.AppendFormat("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" CodeBehind=\"{0}.aspx.cs\" Inherits=\"{1}.{0}\" %>\r\n", module.PageName, module.NameSpace);

            content.AppendLine(@"<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">");

            // create head
            content.AppendLine(@"<head>
    <meta charset=""utf-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <meta name=""description"" content="""">
    <meta name=""author"" content="""">
    <title>登录</title>
    <link href=""js/bootstrap/css/bootstrap.css"" rel=""stylesheet"" />
    <style type=""text/css"">
        body {{
            padding-top: 40px;
            padding-bottom: 40px;
            background-color: #eee;
        }}
    </style>
    <script src=""js/jquery.min.js""></script>
</head>");

            // create body
            content.AppendFormat(@"<body>
    <script type=""text/javascript"">
        function submitLogin() {{
            var postData = ""name="" + encodeURI($(""#name"").val()) + ""&pwd="" + $(""#pwd"").val();
            $.ajax({{
                type: ""POST"",
                url: ""{0}.aspx?type=login"",
                data: postData.replace(/\+/g, ""%2b""),
                success: function (text) {{
                    if (text == ""0"") {{
                        document.location.href = ""#"";
                    }}
                    else {{
                        $(""#lblMsg"").val(""账号或密码错误"");
                    }}
                }}
            }});
        }}

        $(document).ready(function () {{
            if (window.parent.SetWinHeight) {{
                top.location.href = ""{0}.aspx"";
            }}
        }})
    </script>
    <div class=""container"">
        <div class=""row"">
            <div class=""col-md-4""></div>
            <div class=""col-md-4"">
                <h2 class=""form-signin-heading text-center"">登录</h2>
                <div class=""form-group"">
                    <input id=""name"" type=""text"" class=""form-control"" placeholder=""账号"" />
                </div>
                <div class=""form-group"">
                    <input id=""pwd"" type=""password"" class=""form-control"" placeholder=""密码"" />
                </div>
                <button class=""btn btn-lg btn-primary btn-block"" onclick=""submitLogin();"">登录</button>
                <label id=""lblMsg"" style=""color: red""></label>
            </div>
            <div class=""col-md-4""></div>
        </div>
    </div>
</body>", "");

            // create html
            content.Append(@"</html>");

            return content.ToString();
        }

        public string Create_Aspx_Cs(ModuleInfo module)
        {
            StringBuilder content = new StringBuilder();
            content.AppendFormat(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace {0}
{{
    public partial class {1} : System.Web.UI.Page
    {{", module.NameSpace, module.PageName);

            content.AppendFormat(@"protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod.ToUpper() == ""POST"")
            {
                string type = Request[""type""];
                switch (type)
                {
                    case ""login"":
                        {
                            string name = HttpUtility.UrlDecode(Request[""name""]);
                            string pwd = Request[""pwd""];
                            {2}DAL dal = new {2}DAL();
                            var user = dal.GetUserInfo(name, pwd);
                            if (user == null)
                            {
                                Response.Write(""1"");
                            }
                            else
                            {
                                user.{3} = """";
                                Session[""user""] = user;

                                Response.Write(""0"");
                            }
                        }
                        break;
                    default:
                        break;
                }

                Response.End();
            }
        }", "");

            content.Append(@"}
}");

            return content.ToString();
        }

        public string Create_Model_Cs(ModuleInfo module)
        {
            return string.Empty;
        }

        public string Create_DAL_Cs(ModuleInfo module)
        {
            return string.Empty;
        }

        /// <summary>
        /// 创建一个注释，从当前位置开始，回车换行结束
        /// </summary>
        /// <param name="commentStr"></param>
        /// <param name="tabCount">tab的个数</param>
        /// <returns></returns>
        private static string CreateComment(string commentStr, int tabCount)
        {
            string tabStr = string.Empty;
            for (int i = 0; i < tabCount; i++)
            {
                tabStr += "\t";
            }

            StringBuilder content = new StringBuilder();
            content.AppendLine(tabStr + "/// <summary>");
            content.AppendLine(tabStr + "/// " + commentStr);
            content.AppendLine(tabStr + "/// </summary>");

            return content.ToString();
        }

        /// <summary>
        /// 获取格式化之后的字符串
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        private static string GetFormatString(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "int":
                case "tinyint":
                case "smallint":
                    return "int";
                case "varchar":
                case "char":
                case "nvarchar":
                case "text":
                    return "string";
                case "datetime":
                case "time":
                case "date":
                case "timestamp":
                    return "DateTime";
                case "float":
                case "decimal":
                    return "decimal";
                case "memory":
                    return "double";
                default:
                    return "string";
            }
        }

        /// <summary>
        /// 获取默认值字符串
        /// </summary>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        private static string GetDefaultValueStr(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "int":
                case "tinyint":
                case "smallint":
                    return "0";
                case "varchar":
                case "char":
                case "nvarchar":
                case "text":
                    return "string.Empty";
                case "datetime":
                case "time":
                case "date":
                case "timestamp":
                    return "DateTime.Parse(\"1970-1-1\")";
                case "float":
                case "decimal":
                    return "0m";
                case "memory":
                    return "0d";
                default:
                    return "string.Empty";
            }
        }
    }
}
