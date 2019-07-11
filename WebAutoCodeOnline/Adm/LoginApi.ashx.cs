using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebAutoCodeOnline
{
    /// <summary>
    /// LoginApi 的摘要说明
    /// </summary>
    public class LoginApi : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod.ToUpper() == "POST")
            {
                context.Response.ContentType = "html/text";
                string type = context.Request["type"];
                switch (type)
                {
                    case "login":
                        Login();
                        break;
                }

                HttpContext.Current.Response.End();
            }
        }

        private void Login()
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            string username = HttpContext.Current.Request["username"];
            string pwd = HttpContext.Current.Request["pwd"];

            if (IPCacheManager.CheckIsAble(ip))
            {
                if (AccountCacheManager.CheckIsAble(username))
                {
                    // 验证账号密码正确性
                    MySqlDAL.UserInfoDAL dal = new MySqlDAL.UserInfoDAL();
                    var user = dal.GetUserInfo(username, pwd);

                    HttpContext.Current.Session["user"] = user;

                    HttpContext.Current.Response.Write("0");
                    return;
                }
            }

            HttpContext.Current.Response.Write("1");
            return;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}