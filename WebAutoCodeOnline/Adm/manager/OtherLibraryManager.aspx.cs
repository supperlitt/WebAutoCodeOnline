using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAutoCodeOnline.Adm
{
    public partial class OtherLibraryManager : BasePage
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
                    case "edittitle":
                        EditTitle();
                        break;
                    case "delete":
                        DeleteData();
                        break;
                }

                Response.Flush();
                Response.End();
            }
        }

        private void LoadData()
        {
            string txtKey = HttpUtility.UrlDecode(Request["txtKey"]);
            var list = FriendlyDllXml.GetList(txtKey);

            JavaScriptSerializer js = new JavaScriptSerializer();
            var str = js.Serialize(new { Title1 = FriendlyDllXml.GetTitle(1), Title2 = FriendlyDllXml.GetTitle(2), Data = list });
            Response.Write(str);
        }

        private void AddData()
        {
            string txtAddText = HttpUtility.UrlDecode(Request["txtAddText"]);
            string txtAddAddr = HttpUtility.UrlDecode(Request["txtAddAddr"]);
            FriendlyDllXml.AddList(new DLLInfo() { Text = txtAddText, Url = txtAddAddr });

            Response.Write("0");
        }

        private void EditData()
        {
            string txtEditId = HttpUtility.UrlDecode(Request["txtEditId"]);
            string txtEditText = HttpUtility.UrlDecode(Request["txtEditText"]);
            string txtEditAddr = HttpUtility.UrlDecode(Request["txtEditAddr"]);
            FriendlyDllXml.EditList(new DLLInfo() { Id = txtEditId, Text = txtEditText, Url = txtEditAddr });

            Response.Write("0");
        }

        private void EditTitle()
        {
            string txtEditTitle1 = HttpUtility.UrlDecode(Request["txtEditTitle1"]);
            string txtEditTitle2 = HttpUtility.UrlDecode(Request["txtEditTitle2"]);
            FriendlyDllXml.EditTitle(txtEditTitle1, txtEditTitle2);

            Response.Write("0");
        }

        private void DeleteData()
        {
            string ids = HttpUtility.UrlDecode(Request["ids"]);
            List<string> idList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            FriendlyDllXml.RemoveAll(idList);

            Response.Write("0");
        }
    }
}