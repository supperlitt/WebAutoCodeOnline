using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CodeHelper;
using Model;
using System.Text.RegularExpressions;
using Common;

namespace WebAutoCodeOnline
{
    /// <summary>
    /// Down 的摘要说明
    /// </summary>
    public class Down : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/octet-stream";
            string type = context.Request["type"];
            switch (type)
            {
                case "cs":
                    {
                        string cs = context.Request["cs"];
                        DownFile(cs + ".cs.file");
                    }
                    break;
                case "dll":
                    {
                        string dll = context.Request["dll"];
                        DownFile(dll + ".dll.file");
                    }
                    break;
                case "xml":
                    {
                        string xml = context.Request["xml"];
                        DownFile(xml + ".xml.file");
                    }
                    break;
                case "zip":
                    {
                        string guid = context.Request["guid"];
                        DownZip(guid);
                    }
                    break;
            }

            context.Response.Flush();
            context.Response.End();
        }

        private void DownZip(string guid)
        {
            var item = DownZipCache.GetZipData(guid);
            if (item == null)
            {
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=null.zip;");
                var bs = new byte[0];
                HttpContext.Current.Response.BinaryWrite(bs);
            }
            else
            {
                string paramStr = item.ParamStr;
                string txtNameSpace = HttpUtility.UrlDecode(GetParams(paramStr, "namespace"));
                string txtDBName = HttpUtility.UrlDecode(GetParams(paramStr, "dbname"));
                string txtSearchColumns = HttpUtility.UrlDecode(GetParams(paramStr, "searchcolumns"));
                string dbType = GetParams(paramStr, "dbtype");
                string sql = HttpUtility.UrlDecode(HttpUtility.UrlDecode(GetParams(paramStr, "sql")));
                string txtadd = HttpUtility.UrlDecode(GetParams(paramStr, "txtadd"));
                string txtsearch = HttpUtility.UrlDecode(GetParams(paramStr, "txtsearch"));
                string txtedit = HttpUtility.UrlDecode(GetParams(paramStr, "txtedit"));
                string txtbatedit = HttpUtility.UrlDecode(GetParams(paramStr, "txtbatedit"));
                string chkadd = GetParams(paramStr, "chkadd");
                string chkdelete = GetParams(paramStr, "chkdelete");
                string chkedit = GetParams(paramStr, "chkedit");
                string chkbatdelete = GetParams(paramStr, "chkbatdelete");
                string chkexport = GetParams(paramStr, "chkexport");

                EasyUIModel model = new EasyUIModel();
                model.DbName = txtDBName;
                model.AddColumnsStr = txtadd;
                model.SearchColumnsStr = txtsearch;
                model.EditColumnsStr = txtedit;
                model.BatEditColumnsStr = txtbatedit;
                model.TableStr = sql;
                model.NameSpace = txtNameSpace;
                model.DbType = dbType == "0" ? 0 : 1;
                model.IsDel = chkadd == "0";
                model.IsBatDel = chkdelete == "0";
                model.IsBatEdit = !string.IsNullOrEmpty(txtbatedit);
                model.IsAdd = chkadd == "0";
                model.IsEdit = chkedit == "0";
                model.IsExport = chkexport == "0";

                ShowEasyUIModel easyModel = new ShowEasyUIModel();

                try
                {
                    // 初始化model对象的属性
                    UIHelper.InitEasyUI(model);
                    if (model.DbType == 0)
                    {
                        EasyUIHelper easyHelper = new EasyUIHelper();
                        easyModel.ClassStr = easyHelper.CreateModel(model);
                        easyModel.AspxStr = easyHelper.CreateASPX(model);
                        easyModel.AspxCsStr = easyHelper.CreateASPXCS(model);
                        easyModel.DALStr = easyHelper.CreateDAL(model);
                        easyModel.FactoryStr = easyHelper.CreateFactory(model);
                    }
                    else if (model.DbType == 1)
                    {
                        EasyUIHelper easyHelper = new EasyUIHelper();
                        MySqlEasyUIHelper mysqlEasyHelper = new MySqlEasyUIHelper();
                        easyModel.ClassStr = easyHelper.CreateModel(model);
                        easyModel.AspxStr = easyHelper.CreateASPX(model);
                        easyModel.AspxCsStr = easyHelper.CreateASPXCS(model);
                        easyModel.DALStr = mysqlEasyHelper.CreateDAL(model);
                        easyModel.FactoryStr = mysqlEasyHelper.CreateFactory(model);
                    }

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(model.TableName.ToFirstUpper() + ".cs", easyModel.ClassStr);
                    dic.Add(model.TableName.ToFirstUpper() + "Manager.aspx", easyModel.AspxStr);
                    dic.Add(model.TableName.ToFirstUpper() + "Manager.aspx.cs", easyModel.AspxCsStr);
                    dic.Add(model.TableName.ToFirstUpper() + "DAL.cs", easyModel.DALStr);
                    dic.Add("ConnectionFactory.cs", easyModel.FactoryStr);
                    byte[] result = ZipHelper.Zip(dic);

                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + model.TableName + ".zip;");
                    HttpContext.Current.Response.BinaryWrite(result);
                }
                catch
                {
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + model.TableName + ".zip;");
                    var bs = new byte[0];
                    HttpContext.Current.Response.BinaryWrite(bs);
                }
            }
        }

        private void DownFile(string fileName)
        {
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + fileName.ToFirstUpper().Substring(0, fileName.LastIndexOf(".")) + ";");
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Down", fileName);
            var bs = File.ReadAllBytes(filePath);
            HttpContext.Current.Response.BinaryWrite(bs);
        }

        private string GetParams(string content, string key)
        {
            Regex regex = new Regex(key + "=?(?<key>[^&]{0,})");
            return regex.Match(content).Groups["key"].Value;
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