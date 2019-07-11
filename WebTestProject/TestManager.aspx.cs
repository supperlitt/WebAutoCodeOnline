using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test
{
    public partial class TestManager : System.Web.UI.Page
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

                    case "batedit":
                        BatEditData();
                        break;

                    case "delete":
                        DeleteData();
                        break;

                }

                Response.End();
            }
            else
            {
                string type = Request.QueryString["type"];
                switch (type)
                {
                    case "down":
                        Down();
                        break;
                    case "downall":
                        DownAll();
                        break;
                }
            }
        }

        private void LoadData()
        {
            int page = Convert.ToInt32(Request.Form["page"]);
            int pageSize = Convert.ToInt32(Request.Form["pageSize"]);
            string testName = HttpUtility.UrlDecode(Request["txtSearchTestName"]);

            TestInfoDAL dal = new TestInfoDAL();
            var list = dal.QueryList(testName, page, pageSize);
            list.ForEach(p => p.TestName = HttpUtility.HtmlEncode(p.TestName));
            int itemCount = dal.QueryListCount(testName);
            int pageCount = (int)Math.Ceiling((double)itemCount / (double)pageSize);
            JavaScriptSerializer js = new JavaScriptSerializer();
            var str = js.Serialize(new { PageCount = pageCount, ItemCount = itemCount, Data = list });
            Response.Write(str);
        }

        private void AddData()
        {
            string testName = HttpUtility.UrlDecode(Request["txtAddTestName"]);
            string testPwd = HttpUtility.UrlDecode(Request["txtAddTestPwd"]);
            string testMemory = HttpUtility.UrlDecode(Request["txtAddTestMemory"]);

            TestInfo model = new TestInfo();
            model.TestName = testName;
            model.TestPwd = testPwd;
            model.TestMemory = Convert.ToDecimal(testMemory);

            TestInfoDAL dal = new TestInfoDAL();
            dal.AddTestInfo(model);

            Response.Write("0");
        }

        private void EditData()
        {
            string testId = HttpUtility.UrlDecode(Request["txtEditTestId"]);
            string testName = HttpUtility.UrlDecode(Request["txtEditTestName"]);
            string testPwd = HttpUtility.UrlDecode(Request["txtEditTestPwd"]);

            TestInfo model = new TestInfo();
            model.TestId = Convert.ToInt32(testId);
            model.TestName = testName;
            model.TestPwd = testPwd;

            TestInfoDAL dal = new TestInfoDAL();
            dal.UpdateTestInfo(model);

            Response.Write("0");
        }

        private void BatEditData()
        {
            string testId = HttpUtility.UrlDecode(Request["txtBatEditTestId"]);
            List<string> idList = testId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string testPwd = HttpUtility.UrlDecode(Request["txtBatEditTestPwd"]);
            string testMemory = HttpUtility.UrlDecode(Request["txtBatEditTestMemory"]);

            TestInfo model = new TestInfo();
            model.TestPwd = testPwd;
            model.TestMemory = Convert.ToDecimal(testMemory);

            TestInfoDAL dal = new TestInfoDAL();
            dal.BatUpdateTestInfo(idList, model);

            Response.Write("0");
        }

        private void DeleteData()
        {
            string ids = HttpUtility.UrlDecode(Request["ids"]);
            List<string> idList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            TestInfoDAL dal = new TestInfoDAL();
            dal.DeleteTestInfo(idList);

            Response.Write("0");
        }

        private void Down()
        {
            string ids = HttpUtility.UrlDecode(Request["ids"]);
            List<string> idList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string testName = HttpUtility.UrlDecode(Request["txtSearchTestName"]);

            TestInfoDAL dal = new TestInfoDAL();
            List<TestInfo> data = dal.GetPartAll(testName, idList);
            string content = CreateTable(data);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "UTF-8";
            Response.AddHeader("Content-Disposition", "attachment; filename=TestInfo.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel;charset=UTF-8";
            Response.Write(content);
            Response.Flush();
            Response.End();
        }

        private void DownAll()
        {
            string ids = HttpUtility.UrlDecode(Request["ids"]);
            List<string> idList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string testName = HttpUtility.UrlDecode(Request["txtSearchTestName"]);

            TestInfoDAL dal = new TestInfoDAL();
            List<TestInfo> data = dal.GetAll(testName);
            string content = CreateTable(data);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "UTF-8";
            Response.AddHeader("Content-Disposition", "attachment; filename=TestInfo.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel;charset=UTF-8";
            Response.Write(content);
            Response.Flush();
            Response.End();
        }

        private string CreateTable(List<TestInfo> list)
        {
            StringBuilder content = new StringBuilder();

            // create columns header
            content.Append("<table border='1'><thead><tr><th>测试Id</th><th>测试名称</th><th>测试密码</th><th>测试金额</th><th>添加时间</th></tr></thead>");
            for (int i = 0, len = list.Count; i < len; i++)
            {
                content.Append("<tr>");
                content.AppendFormat("<td>{0}</td>", list[i].TestId);
                content.AppendFormat("<td>{0}</td>", list[i].TestName);
                content.AppendFormat("<td>{0}</td>", list[i].TestPwd);
                content.AppendFormat("<td>{0}</td>", list[i].TestMemory);
                content.AppendFormat("<td>{0}</td>", list[i].AddDate);

                content.Append("</tr>");
            }
            content.Append("</tbody></table>");

            return content.ToString();
        }
    }
}