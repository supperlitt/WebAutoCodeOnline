using CodeHelper;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace WebAutoCodeOnline
{
    /// <summary>
    /// Api 的摘要说明
    /// </summary>
    public class Api : IHttpHandler
    {
        private static JavaScriptSerializer js = new JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod.ToUpper() == "POST")
            {
                context.Response.ContentType = "application/json";
                string type = context.Request["type"];
                switch (type)
                {
                    case "normal":
                        Normal();
                        break;
                    case "easyui":
                        EasyUI();
                        break;
                    case "bootstrap":
                        BootStrap();
                        break;
                    case "encode":
                        Encode();
                        break;
                    case "encrptynormal":
                        EncrptyNormal();
                        break;
                    case "encrptycomplex":
                        EncrptyComplex();
                        break;
                    case "encrptyrsa":
                        EncrptyRSA();
                        break;
                    case "loadalldemo":
                        LoadAllDemo();
                        break;
                    case "json2csharp":
                        Json2Csharp();
                        break;
                    case "leavemsg":
                        LeaveMsgInfo();
                        break;
                    case "loadmsg":
                        LoadMsg();
                        break;
                    case "loaddemo":
                        LoadDemo();
                        break;
                    case "encrpty_new":
                        EncryptNew();
                        break;
                    case "upload_exe_to_add_shell":
                        Uplaod_Exe_To_Add_Shell();
                        break;
                    case "add_shell":
                        Add_Shell();
                        break;
                }

                context.Response.Flush();
                context.Response.End();
            }
        }

        private void Add_Shell()
        {
            HttpContext.Current.Response.ContentType = "text/html";
            HttpFileCollection files = HttpContext.Current.Request.Files;
            if (files.Count > 0 && files[0].InputStream.Length > 200)
            {
                HttpPostedFile file = files[0];
                byte[] buffer = new byte[4];
                file.InputStream.Read(buffer, 0, 4);
                if (buffer[0] == 0x4D && buffer[1] == 0x5A && buffer[2] == 0x90 && buffer[3] == 0x00)
                {
                    file.InputStream.Seek(0, SeekOrigin.Begin);
                    string guid = Guid.NewGuid().ToString("N");
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp_Exe", guid + ".exe");
                    file.SaveAs(path);

                    HttpContext.Current.Response.Write(guid);
                }
                else
                {
                    HttpContext.Current.Response.Write("");
                }
            }
            else
            {
                HttpContext.Current.Response.Write("");
            }
        }

        private void Uplaod_Exe_To_Add_Shell()
        {
            HttpContext.Current.Response.ContentType = "text/html";
            string key = HttpContext.Current.Request["key"];
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TEMP_EXE", key + ".exe")))
            {
                HttpContext.Current.Response.Write("../TEMP_EXE/" + key + "_result.exe");
            }
            else
            {
                HttpContext.Current.Response.Write("");
            }
        }

        private void EncryptNew()
        {
            string ctype = HttpContext.Current.Request["ctype"];
            if (ctype == "md5")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                HttpContext.Current.Response.Write(js.Serialize(new { Result = EncryptHelper.GetMD5(text) }));
            }
            else if (ctype == "sha1")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                HttpContext.Current.Response.Write(js.Serialize(new { Result = EncryptHelper.GetMD5(text) }));
            }
            else if (ctype == "en_des")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");

                HttpContext.Current.Response.Write(js.Serialize(new { Result = EncryptHelper.Encrypt(text, key) }));
            }
            else if (ctype == "de_des")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");

                HttpContext.Current.Response.Write(js.Serialize(new { Result = EncryptHelper.Decrypt(text, key) }));
            }
            else if (ctype == "random")
            {
                HttpContext.Current.Response.Write(js.Serialize(new { Result = AESHelper.GetIv(16) }));
            }
            else if (ctype == "en_aes")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");
                try
                {
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = AESHelper.AESEncrypt(text, key) }));
                }
                catch
                {
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = "密钥：长度不对，应该为16位" }));
                }
            }
            else if (ctype == "de_aes")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");
                try
                {
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = AESHelper.AESDecrypt(text, key) }));
                }
                catch
                {
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = "密钥：长度不对，应该为16位" }));
                }
            }
            else if (ctype == "random_rsa_xml")
            {
                HttpContext.Current.Response.Write(js.Serialize(new { Result = RSAHelper.GenerateKeys() }));
            }
            else if (ctype == "en_rsa_xml")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");

                HttpContext.Current.Response.Write(js.Serialize(new { Result = RSANewHelper.Encrypt(text, key) }));
            }
            else if (ctype == "de_rsa_xml")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");

                HttpContext.Current.Response.Write(js.Serialize(new { Result = RSANewHelper.Decrypt(text, key) }));
            }
            else if (ctype == "random_xxtea_normal")
            {
                HttpContext.Current.Response.Write(js.Serialize(new { Result = XXTEA.RandomKey() }));
            }
            else if (ctype == "en_xxtea_normal")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");

                HttpContext.Current.Response.Write(js.Serialize(new { Result = text.Encrypt(key) }));
            }
            else if (ctype == "de_xxtea_normal")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");

                HttpContext.Current.Response.Write(js.Serialize(new { Result = text.Decrypt(key) }));
            }
            else if (ctype == "en_xxtea_normal2")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");

                HttpContext.Current.Response.Write(js.Serialize(new { Result = XXTEA_CSDN.Encrypt(text, key) }));
            }
            else if (ctype == "de_xxtea_normal2")
            {
                string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
                string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");
                bool isbase64 = HttpContext.Current.Request["isbase64"] == "1";
                try
                {
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = XXTEA_CSDN.Decrypt(text, key, isbase64) }));
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = ex.Message }));
                }
            }
        }

        private void LoadDemo()
        {
            var treeInfo = new TreeInfo();
            string key = HttpContext.Current.Request["key"];
            string id = HttpContext.Current.Request["id"];
            if (string.IsNullOrEmpty(id))
            {
                var project = ProjectFileCache.GetByKey(key);
                if (project != null)
                {
                    HttpContext.Current.Response.Write(js.Serialize(project.TreeList));
                }
                else
                {
                    HttpContext.Current.Response.Write(js.Serialize(new List<TreeInfo>()));
                }
            }
            else
            {
                var project = ProjectFileCache.GetByKey(key);
                if (project != null)
                {
                    var singleTree = project.SingleTreeList.Find(p => p.id == id);
                    if (singleTree != null)
                    {
                        string txt = File.ReadAllText(singleTree.path, Encoding.UTF8);
                        HttpContext.Current.Response.Write(js.Serialize(new { Result = txt.Replace("\r\n", "\n") }));
                    }
                    else
                    {
                        HttpContext.Current.Response.Write(js.Serialize(new { Result = "找不到数据" }));
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = "找不到数据" }));
                }
            }
        }

        private void LoadMsg()
        {
            string page = HttpContext.Current.Request["page"];
            string pagesize = HttpContext.Current.Request["pagesize"];
            int pageNum = 0;
            int pageSizeNum = 0;
            int.TryParse(page, out pageNum);
            int.TryParse(pagesize, out pageSizeNum);

            if (pageNum <= 0)
            {
                pageNum = 1;
            }

            pageSizeNum = 5;

            MySqlDAL.LeaveMsgDAL dal = new MySqlDAL.LeaveMsgDAL();
            var list = dal.QueryList(pageNum, pageSizeNum);
            int count = dal.QueryListCount();
            var resultList = (from f in list
                              select new MsgInfo() { NickName = HttpUtility.HtmlEncode(f.NickName), LeaveContent = HttpUtility.HtmlEncode(f.Msg), LeaveTime = f.LeaveTimeStr }).ToList();

            HttpContext.Current.Response.Write(js.Serialize(new { Result = resultList, ItemCount = count, PageCount = (int)Math.Ceiling(((double)count / pageSizeNum)) }));
        }

        private void LeaveMsgInfo()
        {
            string nickname = HttpUtility.UrlDecode(HttpContext.Current.Request["nickname"]).Replace(" ", "+");
            string email = HttpUtility.UrlDecode(HttpContext.Current.Request["email"]).Replace(" ", "+");
            string content = HttpUtility.UrlDecode(HttpContext.Current.Request["content"]).Replace(" ", "+");
            string ip = HttpContext.Current.Request.UserHostAddress == "::1" ? "127.0.0.1" : HttpContext.Current.Request.UserHostAddress;

            MsgResult msgResult = new MsgResult();
            if (IPLimitCache.CheckCanRequest(ip))
            {
                var leaveMsg = new LeaveMsg();
                leaveMsg.NickName = nickname;
                leaveMsg.Email = email;
                leaveMsg.Msg = content;
                leaveMsg.IP = ip;

                try
                {
                    MySqlDAL.LeaveMsgDAL dal = new MySqlDAL.LeaveMsgDAL();
                    dal.AddLeaveMsg(leaveMsg);

                    msgResult.Result = 0;
                    msgResult.Msg = "留言成功！";
                }
                catch (Exception ex)
                {
                    msgResult.Result = 1;
                    msgResult.Msg = ex.Message;
                }
            }
            else
            {
                msgResult.Result = 1;
                msgResult.Msg = "提交太频繁，请稍后重试！";
            }

            HttpContext.Current.Response.Write(js.Serialize(msgResult));
        }

        private void Json2Csharp()
        {
            string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
            JsonHelper jsonHelper = new JsonHelper();
            var content = string.Empty;
            try
            {
                content = jsonHelper.GetClassString(text).Replace("\r\n", "\n");
            }
            catch (Exception ex)
            {
                content = ex.Message;
            }

            HttpContext.Current.Response.Write(js.Serialize(new { Result = content }));
        }

        private void LoadAllDemo()
        {
            // 加载Info信息
            var list = ProjectCache.GetDataList();
            var result = (from f in list
                          orderby f.ProjectTypeName
                          select new { Key = f.ProjectTypeName, Value = HttpUtility.HtmlEncode(f.ProjectTypeDesc.Length > 20 ? f.ProjectTypeDesc.Substring(0, 20) : f.ProjectTypeDesc), Title = f.ProjectTypeDesc }).ToList();

            HttpContext.Current.Response.Write(js.Serialize(result));
        }

        private void EncrptyRSA()
        {
            string ctype = HttpContext.Current.Request["ctype"];
            string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
            string publickey = HttpUtility.UrlDecode(HttpContext.Current.Request["publickey"]).Replace(" ", "+");
            string privatekey = HttpUtility.UrlDecode(HttpContext.Current.Request["privatekey"]).Replace(" ", "+");
            switch (ctype)
            {
                case "random":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = RSAHelper.GenerateKeys() }));
                    break;
                case "rsa":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = RSANewHelper.Encrypt(text, publickey) }));
                    break;
                case "unrsa":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = RSANewHelper.Decrypt(text, privatekey) }));
                    break;
            }
        }

        private void EncrptyNormal()
        {
            string ctype = HttpContext.Current.Request["ctype"];
            string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
            switch (ctype)
            {
                case "md5":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = EncryptHelper.GetMD5(text) }));
                    break;
                case "sha1":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = EncryptHelper.GetSHA1(text) }));
                    break;
            }
        }

        private void EncrptyComplex()
        {
            string ctype = HttpContext.Current.Request["ctype"];
            string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
            string key = HttpUtility.UrlDecode(HttpContext.Current.Request["key"]).Replace(" ", "+");
            switch (ctype)
            {
                case "des":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = EncryptHelper.Encrypt(text, key) }));
                    break;
                case "undes":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = EncryptHelper.Decrypt(text, key) }));
                    break;
                case "random":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = AESHelper.GetIv(16) }));
                    break;
                case "aes":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = AESHelper.AESEncrypt(text, key) }));
                    break;
                case "unaes":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = AESHelper.AESDecrypt(text, key) }));
                    break;
            }
        }

        private void Encode()
        {
            string ctype = HttpContext.Current.Request["ctype"];
            string text = HttpUtility.UrlDecode(HttpContext.Current.Request["text"]).Replace(" ", "+");
            string seltype = HttpContext.Current.Request["seltype"];
            switch (ctype)
            {
                case "0":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = HttpUtility.UrlEncode(text) }));
                    break;
                case "1":
                    HttpContext.Current.Response.Write(js.Serialize(new { Result = HttpUtility.UrlDecode(text) }));
                    break;
                case "2":
                    {
                        byte[] bts = Encoding.Unicode.GetBytes(text);
                        string r = "";
                        for (int i = 0; i < bts.Length; i += 2)
                        {
                            r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
                        }

                        HttpContext.Current.Response.Write(js.Serialize(new { Result = r }));
                    }
                    break;
                case "3":
                    {
                        Regex regex = new Regex(@"\\u(?<key>\w{4})");
                        var matches = regex.Matches(text);
                        foreach (Match m in matches)
                        {
                            string value = m.Groups["key"].Value;
                            string first = value.Substring(2, 2);
                            string second = value.Substring(0, 2);
                            byte[] buffer = new byte[2];
                            buffer[0] = (byte)Convert.ToInt32(first, 16);
                            buffer[1] = (byte)Convert.ToInt32(second, 16);

                            string decodeStr = Encoding.Unicode.GetString(buffer);
                            text = text.Replace(m.ToString(), decodeStr);
                        }

                        HttpContext.Current.Response.Write(js.Serialize(new { Result = text }));
                    }
                    break;
                case "4":
                    {
                        byte[] bts = Encoding.Unicode.GetBytes(text);
                        string r = "";
                        for (int i = 0; i < bts.Length; i += 2)
                        {
                            r += "%f" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
                        }

                        HttpContext.Current.Response.Write(js.Serialize(new { Result = r }));
                    }
                    break;
                case "5":
                    {
                        Regex regex = new Regex(@"%f(?<key>\w{4})");
                        var matches = regex.Matches(text);
                        foreach (Match m in matches)
                        {
                            string value = m.Groups["key"].Value;
                            string first = value.Substring(2, 2);
                            string second = value.Substring(0, 2);
                            byte[] buffer = new byte[2];
                            buffer[0] = (byte)Convert.ToInt32(first, 16);
                            buffer[1] = (byte)Convert.ToInt32(second, 16);

                            string decodeStr = Encoding.Unicode.GetString(buffer);
                            text = text.Replace(m.ToString(), decodeStr);
                        }

                        HttpContext.Current.Response.Write(js.Serialize(new { Result = text }));
                    }
                    break;
                case "6":
                    {
                        byte[] buffer = Encoding.GetEncoding(seltype).GetBytes(text);
                        string result = Convert.ToBase64String(buffer);

                        HttpContext.Current.Response.Write(js.Serialize(new { Result = result }));
                    }
                    break;
                case "7":
                    {
                        byte[] buffer = Convert.FromBase64String(text);
                        string result = Encoding.GetEncoding(seltype).GetString(buffer);

                        HttpContext.Current.Response.Write(js.Serialize(new { Result = result }));
                    }
                    break;
            }
        }

        private void Normal()
        {
            // "dbtype": dbtype, "sql": text, "namespace": namespace, "dbname": dbname, "searchcolumns": searchcolumns
            string txtNameSpace = HttpUtility.UrlDecode(HttpContext.Current.Request["namespace"]);
            string txtDBName = HttpUtility.UrlDecode(HttpContext.Current.Request["dbname"]);
            string txtSearchColumns = HttpUtility.UrlDecode(HttpContext.Current.Request["searchcolumns"]);
            string dbType = HttpContext.Current.Request["dbtype"];
            string sql = HttpUtility.UrlDecode(HttpContext.Current.Request["sql"]);

            NormalModel model = new NormalModel();
            model.DbName = txtDBName;
            model.SearchColumnsStr = txtSearchColumns;
            model.TableStr = sql;
            model.NameSpace = txtNameSpace;
            model.DbType = int.Parse(dbType);

            UIHelper.InitNormalCode(model);

            ShowNormalModel showModel = new ShowNormalModel();

            try
            {
                // 创建类代码
                TableHelper tableHelper = new TableHelper(true, true, true);
                showModel.ClassStr = tableHelper.GetClassString(model).Replace("\r\n", "\n");
                if (model.DbType == 0)
                {
                    SqlCreateSqlHelper sqlHelper = new SqlCreateSqlHelper(model);
                    showModel.AddStr = sqlHelper.CreateInsertMethod().Replace("\r\n", "\n");
                    showModel.DeleteStr = sqlHelper.CreateDeleteMethod().Replace("\r\n", "\n");
                    showModel.BatDeleteStr = sqlHelper.CreateBatDeleteMethod().Replace("\r\n", "\n");
                    showModel.EditStr = sqlHelper.CreateUpdateMethod().Replace("\r\n", "\n");
                    showModel.SearchCountStr = sqlHelper.CreateSelectByPageAndSizeAddCount().Replace("\r\n", "\n");
                    showModel.SearchStr = sqlHelper.CreateSelectByPageAndSize().Replace("\r\n", "\n");
                    showModel.FactoryStr = sqlHelper.CreateConnectionFactory().Replace("\r\n", "\n");
                }
                else if (model.DbType == 1)
                {
                    MySqlCreateMySqlHelper sqlHelper = new MySqlCreateMySqlHelper(model);
                    showModel.AddStr = sqlHelper.CreateInsertMethod().Replace("\r\n", "\n");
                    showModel.DeleteStr = sqlHelper.CreateDeleteMethod().Replace("\r\n", "\n");
                    showModel.BatDeleteStr = sqlHelper.CreateBatDeleteMethod().Replace("\r\n", "\n");
                    showModel.EditStr = sqlHelper.CreateUpdateMethod().Replace("\r\n", "\n");
                    showModel.SearchCountStr = sqlHelper.CreateSelectByPageAndSizeAddCount().Replace("\r\n", "\n");
                    showModel.SearchStr = sqlHelper.CreateSelectByPageAndSize().Replace("\r\n", "\n");
                    showModel.FactoryStr = sqlHelper.CreateConnectionFactory().Replace("\r\n", "\n");
                }
            }
            catch (Exception ex)
            {
                showModel.ClassStr = ex.Message;
            }

            HttpContext.Current.Response.Write(js.Serialize(showModel));
        }

        private void EasyUI()
        {
            // "type": "easyui", "dbtype": dbtype, "sql": text, "namespace": namespace, "dbname": dbname,
            // "txtadd": txtadd, "txtsearch": txtsearch, "txtedit": txtedit, "txtbatedit": txtbatedit,
            // "chkadd": chkadd, "chkdelete": chkdelete, "chkedit": chkedit, "chkbatdelete": chkbatdelete, "chkexport": chkexport
            string txtNameSpace = HttpUtility.UrlDecode(HttpContext.Current.Request["namespace"]);
            string txtDBName = HttpUtility.UrlDecode(HttpContext.Current.Request["dbname"]);
            string txtSearchColumns = HttpUtility.UrlDecode(HttpContext.Current.Request["searchcolumns"]);
            string dbType = HttpContext.Current.Request["dbtype"];
            string sql = HttpUtility.UrlDecode(HttpContext.Current.Request["sql"]);
            string txtadd = HttpUtility.UrlDecode(HttpContext.Current.Request["txtadd"]);
            string txtsearch = HttpUtility.UrlDecode(HttpContext.Current.Request["txtsearch"]);
            string txtedit = HttpUtility.UrlDecode(HttpContext.Current.Request["txtedit"]);
            string txtbatedit = HttpUtility.UrlDecode(HttpContext.Current.Request["txtbatedit"]);
            string chkadd = HttpContext.Current.Request["chkadd"];
            string chkdelete = HttpContext.Current.Request["chkdelete"];
            string chkedit = HttpContext.Current.Request["chkedit"];
            string chkbatdelete = HttpContext.Current.Request["chkbatdelete"];
            string chkexport = HttpContext.Current.Request["chkexport"];

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
                    easyModel.ClassStr = easyHelper.CreateModel(model).Replace("\r\n", "\n");
                    easyModel.AspxStr = easyHelper.CreateASPX(model).Replace("\r\n", "\n");
                    easyModel.AspxCsStr = easyHelper.CreateASPXCS(model).Replace("\r\n", "\n");
                    easyModel.DALStr = easyHelper.CreateDAL(model).Replace("\r\n", "\n");
                    easyModel.FactoryStr = easyHelper.CreateFactory(model).Replace("\r\n", "\n");
                }
                else if (model.DbType == 1)
                {
                    EasyUIHelper easyHelper = new EasyUIHelper();
                    MySqlEasyUIHelper mysqlEasyHelper = new MySqlEasyUIHelper();
                    easyModel.ClassStr = easyHelper.CreateModel(model).Replace("\r\n", "\n");
                    easyModel.AspxStr = easyHelper.CreateASPX(model).Replace("\r\n", "\n");
                    easyModel.AspxCsStr = easyHelper.CreateASPXCS(model).Replace("\r\n", "\n");
                    easyModel.DALStr = mysqlEasyHelper.CreateDAL(model).Replace("\r\n", "\n");
                    easyModel.FactoryStr = mysqlEasyHelper.CreateFactory(model).Replace("\r\n", "\n");
                }

                easyModel.Guid = Guid.NewGuid().ToString("N");
                var paramStr = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
                DownZipCache.AddZipInfo(new ZipInfo()
                {
                    Guid = easyModel.Guid,
                    IP = HttpContext.Current.Request.UserHostAddress,
                    Type = ZipType.EasyUI,
                    ParamStr = paramStr,
                    AddTime = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                easyModel.ClassStr = ex.Message;
            }

            HttpContext.Current.Response.Write(js.Serialize(easyModel));
        }

        private void BootStrap()
        {
            // "type": "easyui", "dbtype": dbtype, "sql": text, "namespace": namespace, "dbname": dbname,
            // "txtadd": txtadd, "txtsearch": txtsearch, "txtedit": txtedit, "txtbatedit": txtbatedit,
            // "chkadd": chkadd, "chkdelete": chkdelete, "chkedit": chkedit, "chkbatdelete": chkbatdelete, "chkexport": chkexport
            string txtNameSpace = HttpUtility.UrlDecode(HttpContext.Current.Request["namespace"]);
            string txtDBName = HttpUtility.UrlDecode(HttpContext.Current.Request["dbname"]);
            string txtSearchColumns = HttpUtility.UrlDecode(HttpContext.Current.Request["searchcolumns"]);
            string dbType = HttpContext.Current.Request["dbtype"];
            string sql = HttpUtility.UrlDecode(HttpContext.Current.Request["sql"]);
            string txtadd = HttpUtility.UrlDecode(HttpContext.Current.Request["txtadd"]);
            string txtsearch = HttpUtility.UrlDecode(HttpContext.Current.Request["txtsearch"]);
            string txtedit = HttpUtility.UrlDecode(HttpContext.Current.Request["txtedit"]);
            string txtbatedit = HttpUtility.UrlDecode(HttpContext.Current.Request["txtbatedit"]);
            string chkadd = HttpContext.Current.Request["chkadd"];
            string chkdelete = HttpContext.Current.Request["chkdelete"];
            string chkedit = HttpContext.Current.Request["chkedit"];
            string chkbatdelete = HttpContext.Current.Request["chkbatdelete"];
            string chkexport = HttpContext.Current.Request["chkexport"];

            BootstrapModel model = new BootstrapModel();
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

            ShowBootStrapModel bootModel = new ShowBootStrapModel();

            try
            {
                // 初始化model对象的属性
                UIHelper.InitBootstrap(model);
                if (model.DbType == 0)
                {
                    BootstrapHelper bootHelper = new BootstrapHelper();
                    bootModel.ClassStr = bootHelper.CreateModel(model).Replace("\r\n", "\n");
                    bootModel.AspxStr = bootHelper.CreateASPX(model).Replace("\r\n", "\n");
                    bootModel.AspxCsStr = bootHelper.CreateASPXCS(model).Replace("\r\n", "\n");
                    bootModel.DALStr = bootHelper.CreateDAL(model).Replace("\r\n", "\n");
                    bootModel.FactoryStr = bootHelper.CreateFactory(model).Replace("\r\n", "\n");
                }
                else if (model.DbType == 1)
                {
                    BootstrapHelper bootHelper = new BootstrapHelper();
                    MySqlBootstrapHelper mysqlBootHelper = new MySqlBootstrapHelper();
                    bootModel.ClassStr = bootHelper.CreateModel(model).Replace("\r\n", "\n");
                    bootModel.AspxStr = bootHelper.CreateASPX(model).Replace("\r\n", "\n");
                    bootModel.AspxCsStr = bootHelper.CreateASPXCS(model).Replace("\r\n", "\n");
                    bootModel.DALStr = mysqlBootHelper.CreateDAL(model).Replace("\r\n", "\n");
                    bootModel.FactoryStr = mysqlBootHelper.CreateFactory(model).Replace("\r\n", "\n");
                }

                bootModel.Guid = Guid.NewGuid().ToString("N");
                var paramStr = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
                DownZipCache.AddZipInfo(new ZipInfo()
                {
                    Guid = bootModel.Guid,
                    IP = HttpContext.Current.Request.UserHostAddress,
                    Type = ZipType.EasyUI,
                    ParamStr = paramStr,
                    AddTime = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                bootModel.ClassStr = ex.Message;
            }

            HttpContext.Current.Response.Write(js.Serialize(bootModel));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class ShowNormalModel
    {
        public string ClassStr { get; set; }

        public string AddStr { get; set; }

        public string DeleteStr { get; set; }

        public string BatDeleteStr { get; set; }

        public string EditStr { get; set; }

        public string SearchCountStr { get; set; }

        public string SearchStr { get; set; }

        public string FactoryStr { get; set; }
    }

    public class ShowEasyUIModel
    {
        public string ClassStr { get; set; }

        public string AspxStr { get; set; }

        public string AspxCsStr { get; set; }

        public string DALStr { get; set; }

        public string FactoryStr { get; set; }

        private string guid = string.Empty;

        public string Guid
        {
            get { return this.guid; }
            set { this.guid = value; }
        }
    }

    public class ShowBootStrapModel
    {
        public string ClassStr { get; set; }

        public string AspxStr { get; set; }

        public string AspxCsStr { get; set; }

        public string DALStr { get; set; }

        public string FactoryStr { get; set; }

        private string guid = string.Empty;

        public string Guid
        {
            get { return this.guid; }
            set { this.guid = value; }
        }
    }

    public class MsgResult
    {
        public int Result { get; set; }

        public string Msg { get; set; }
    }

    public class MsgInfo
    {
        public string NickName { get; set; }

        public string LeaveTime { get; set; }

        public string LeaveContent { get; set; }
    }
}