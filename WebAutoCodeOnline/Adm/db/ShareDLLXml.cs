using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace WebAutoCodeOnline.Adm
{
    public class ShareDLLXml
    {
        private static string title1 = string.Empty;
        private static string title2 = string.Empty;
        private static List<ShareDLLInfo> dllList = new List<ShareDLLInfo>();

        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Adm", "db", "sharedll.xml");

        // html网页显示
        private static string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Layout", "share_dll_download.html");

        // html网页模板
        private static string htmlTempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Adm", "html", "share_dll_download.html");

        static ShareDLLXml()
        {
            string xmlContent = File.ReadAllText(path, Encoding.UTF8);
            using (TextReader reader = new StringReader(xmlContent))
            {
                var doc = XDocument.Load(reader);
                var nodes = doc.Element("Root").Element("ShareDLLS").Elements("ShareDLL");
                var ele = doc.Element("Root").Element("ShareDLLS");
                title1 = ele.Attribute("Title1").Value;
                title2 = ele.Attribute("Title2").Value;
                foreach (XElement node in nodes)
                {
                    var item = new ShareDLLInfo();
                    item.ZipPath = node.Attribute("ZipPath").Value;
                    item.Name = node.Attribute("Name").Value;
                    item.Id = Guid.NewGuid().ToString("N");
                    item.Desc = (node.FirstNode as XCData).Value;

                    dllList.Add(item);
                }
            }
        }

        public static List<ShareDLLInfo> GetList()
        {
            lock (dllList)
            {
                return dllList;
            }
        }

        public static string GetTitle(int index)
        {
            switch (index)
            {
                case 1:
                    return title1;
                case 2:
                    return title2;
                default:
                    return string.Empty;
            }
        }

        public static List<ShareDLLInfo> GetList(string key)
        {
            lock (dllList)
            {
                return dllList.FindAll(p => p.Desc.Contains(key) || p.Name.Contains(key));
            }
        }

        public static void AddList(ShareDLLInfo info)
        {
            lock (dllList)
            {
                info.Id = Guid.NewGuid().ToString("N");
                dllList.Add(info);
                SaveXml();
            }
        }

        public static void EditList(ShareDLLInfo info)
        {
            lock (dllList)
            {
                var item = dllList.Find(p => p.Id == info.Id);
                if (string.IsNullOrEmpty(info.ZipPath))
                {
                    info.ZipPath = item.ZipPath;
                }

                dllList.RemoveAll(p => p.Id == info.Id);
                dllList.Add(info);
                SaveXml();
            }
        }

        public static void EditTitle(string title1, string title2)
        {
            lock (dllList)
            {
                ShareDLLXml.title1 = title1;
                ShareDLLXml.title2 = title2;

                SaveXml();
            }
        }

        public static void RemoveAll(List<string> idList)
        {
            lock (dllList)
            {
                dllList.RemoveAll(p => idList.Contains(p.Id));

                SaveXml();
            }
        }

        private static void SaveXml()
        {
            #region 保存xml
            StringBuilder content = new StringBuilder();
            content.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            content.AppendLine("<Root>");
            content.AppendFormat("  <ShareDLLS Title1=\"{0}\" Title2=\"{1}\">\r\n", title1, title2);
            foreach (var item in dllList)
            {
                content.AppendFormat("    <ShareDLL ZipPath=\"{0}\" Name=\"{1}\">\r\n", item.ZipPath, item.Name);
                content.AppendFormat("      <![CDATA[{0}]]>\r\n", item.Desc);
                content.Append("    </ShareDLL>\r\n");
            }

            content.AppendLine("  </ShareDLLS>");
            content.AppendLine("</Root>");

            File.WriteAllText(path, content.ToString(), Encoding.UTF8);
            #endregion

            #region 保存html
            StringBuilder contentStr = new StringBuilder();
            foreach (var item in dllList)
            {
                contentStr.AppendFormat(@"
                            <li class=""list-group-item"">
                                <div class=""row""><a href=""../Down/{0}"" class=""col-md-4"">{1}</a><span class=""label label-info pull-right col-md-4"">{2}</span></div>
                            </li>", item.ZipPath, item.Name, item.Desc);
            }

            string htmlContent = File.ReadAllText(htmlTempPath, Encoding.UTF8);
            htmlContent = htmlContent.Replace("{{Title1}}", title1).Replace("{{Title2}}", title2).Replace("{{Content}}", contentStr.ToString()).Replace("{{UpdateTime}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            File.WriteAllText(htmlPath, htmlContent, Encoding.UTF8);
            #endregion
        }
    }

    public class ShareDLLInfo
    {
        public string Id { get; set; }

        public string ZipPath { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }
    }
}

/*
 
                            <li class="list-group-item">
                                <div class="row"><a href="../Down/DotRas.zip" class="col-md-4">DotRas</a><span class="label label-info pull-right col-md-4">用于创建vpn或ADSL拨号连接<a href="http://dotras.codeplex.com/">官网下载</a></span></div>
                            </li>
                            <li class="list-group-item">
                                <div class="row"><a href="../Down/NPOI.zip" class="col-md-4">NPOI</a><span class="label label-info pull-right col-md-4">在不依赖于Office的环境中可以直接操作Excel</span></div>
                            </li>
                            <li class="list-group-item">
                                <div class="row"><a href="../Down/OpenPop.zip" class="col-md-4">OpenPop</a><span class="label label-info pull-right col-md-4">可以用于开通了pop邮箱的账户进行pop接收邮件</span></div>
                            </li>
                            <li class="list-group-item">
                                <div class="row"><a href="../Down/System.Data.SQLite.zip" class="col-md-4">System.Data.SQLite 合集</a><span class="label label-info pull-right col-md-4">sqlite本地数据库，不需要额外安装数据库软件</span></div>
                            </li>
                            <li class="list-group-item">
                                <div class="row"><a href="../Down/Sqlite_2.0_and_4.0.zip" class="col-md-4">System.Data.SQLite 2 and 4</a><span class="label label-info pull-right col-md-4">sqlite本地数据库，DLL,包含2.0和4.0，请使用x86模式</span></div>
                            </li>
 */