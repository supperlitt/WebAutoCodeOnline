using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAutoCodeOnline.Adm
{
    public partial class ShareDllManager : BasePage
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
                    case "uploadfile":
                        UpLoadFile();
                        break;
                }

                Response.End();
            }
        }

        private void UpLoadFile()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                HttpPostedFile file = files[0];

                string guid = Guid.NewGuid().ToString("N");
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", guid + ".zip");
                file.SaveAs(path);

                Response.Write(guid);
            }
        }

        private void LoadData()
        {
            string txtKey = HttpUtility.UrlDecode(Request["txtKey"]);
            var list = ShareDLLXml.GetList(txtKey);

            JavaScriptSerializer js = new JavaScriptSerializer();
            var str = js.Serialize(new { Title1 = ShareDLLXml.GetTitle(1), Title2 = ShareDLLXml.GetTitle(2), Data = list });
            Response.Write(str);
        }

        private void AddData()
        {
            string txtAddName = HttpUtility.UrlDecode(Request["txtAddName"]);
            string txtAddPath = HttpUtility.UrlDecode(Request["txtAddPath"]);
            string txtAddDesc = HttpUtility.UrlDecode(Request["txtAddDesc"]);
            string addid = Request["addid"];

            if (!string.IsNullOrEmpty(addid))
            {
                Regex regex = new Regex("[^0-9a-zA-Z\u4e00-\u9fa5]");
                // 如果 如果有这个值，那么久需要替换程序。
                string oldPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", addid + ".zip");
                string saveName = regex.Replace(txtAddName, "_");
                string newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", saveName + ".zip");
                int index = 1;
                while (File.Exists(newPath))
                {
                    index++;
                    newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", saveName + index + ".zip");
                }

                // 名称没有重复了，可以使用了
                string fileName = saveName + ".zip";
                if (index != 1)
                {
                    fileName = saveName + index + ".zip";
                }

                File.Move(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", addid + ".zip"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", fileName));
                ShareDLLXml.AddList(new ShareDLLInfo() { Name = txtAddName, ZipPath = fileName, Desc = txtAddDesc });

                Response.Write("0");
            }
            else
            {
                Response.Write("1");
            }
        }

        private void EditData()
        {
            string txtEditId = HttpUtility.UrlDecode(Request["txtEditId"]);
            string txtEditName = HttpUtility.UrlDecode(Request["txtEditName"]);
            string txtEditPath = HttpUtility.UrlDecode(Request["txtEditPath"]);
            string txtEditDesc = HttpUtility.UrlDecode(Request["txtEditDesc"]);
            string editid = Request["editid"];

            if (!string.IsNullOrEmpty(editid))
            {
                Regex regex = new Regex("[^0-9a-zA-Z\u4e00-\u9fa5]");
                // 如果 如果有这个值，那么久需要替换程序。
                string oldPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", editid + ".zip");
                string saveName = regex.Replace(txtEditName, "_");
                string newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", saveName + ".zip");
                int index = 1;
                while (File.Exists(newPath))
                {
                    index++;
                    newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", saveName + index + ".zip");
                }

                // 如果 如果有这个值，那么久需要替换程序。string fileName = saveName + ".zip";
                string fileName = saveName + ".zip";
                if (index != 1)
                {
                    fileName = saveName + index + ".zip";
                }

                File.Move(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", editid + ".zip"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", fileName));
                ShareDLLXml.EditList(new ShareDLLInfo() { Id = txtEditId, Name = txtEditName, ZipPath = fileName, Desc = txtEditDesc });

                Response.Write("0");
            }
            else
            {
                ShareDLLXml.EditList(new ShareDLLInfo() { Id = txtEditId, Name = txtEditName, Desc = txtEditDesc });
                Response.Write("0");
            }
        }

        private void EditTitle()
        {
            string txtEditTitle1 = HttpUtility.UrlDecode(Request["txtEditTitle1"]);
            string txtEditTitle2 = HttpUtility.UrlDecode(Request["txtEditTitle2"]);
            ShareDLLXml.EditTitle(txtEditTitle1, txtEditTitle2);

            Response.Write("0");
        }

        private void DeleteData()
        {
            string ids = HttpUtility.UrlDecode(Request["ids"]);
            List<string> idList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            ShareDLLXml.RemoveAll(idList);

            Response.Write("0");
        }
    }
}