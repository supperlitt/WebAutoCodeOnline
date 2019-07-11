using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["type"];
            switch (type)
            {
                case "loaddata":
                    break;
                case "edit":
                    break;
                case "add":
                    break;
                case "delete":
                    break;
                case "batedit":
                    break;
                case "down":
                    break;
                case "downall":
                    break;
            }
        }


    }
}