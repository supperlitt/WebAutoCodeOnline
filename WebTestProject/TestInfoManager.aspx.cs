using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Text;

namespace Test
{
    public partial class TestInfoManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod.ToUpper() == "POST")
            {
                string type = Request.QueryString["type"];
                switch (type)
                {
                    case "loaddata":
                        LoadData();
                        break;

                    case "add":
                        AddData();
                        break;

                    case "edit":
                        EditData();
                        break;

                    case "delete":
                        DeleteData();
                        break;

                }

                Response.Flush();
                Response.End();
            }
            else
            {
            }
        }

        private void LoadData()
        {
            string txtKey = HttpUtility.UrlDecode(Request["txtKey"]);

            JavaScriptSerializer js = new JavaScriptSerializer();
            var str = "";
            Response.Write(str);
        }

        private void AddData()
        {
            string testName = HttpUtility.UrlDecode(Request["txtAddTestName"]);
            string testPwd = HttpUtility.UrlDecode(Request["txtAddTestPwd"]);

            Response.Write("0");
        }

        private void EditData()
        {
            string testId = HttpUtility.UrlDecode(Request["txtEditTestId"]);
            string testName = HttpUtility.UrlDecode(Request["txtEditTestName"]);

            Response.Write("0");
        }

        private void DeleteData()
        {
            string ids = HttpUtility.UrlDecode(Request["ids"]);
            List<string> idList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Response.Write("0");
        }
    }
}