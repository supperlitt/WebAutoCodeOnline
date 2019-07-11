using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAutoCodeOnline
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("login.html", true);

                return;
            }
            else
            {
                var userInfo = (Session["user"] as UserInfo);
                if (userInfo == null)
                {
                    Response.Redirect("login.html", true);

                    return;
                }
            }

            base.OnPreInit(e);
        }
    }
}