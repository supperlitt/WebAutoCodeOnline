using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace WebAutoCodeOnline
{
    public class FriendlyDllXml
    {
        private static string title1 = string.Empty;
        private static string title2 = string.Empty;
        private static List<DLLInfo> dllList = new List<DLLInfo>();

        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Adm", "db", "friendlydll.xml");

        // html网页显示
        private static string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Layout", "friendly_dll.html");

        // html网页模板
        private static string htmlTempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Adm", "html", "friendly_dll.html");

        static FriendlyDllXml()
        {
            string xmlContent = File.ReadAllText(path, Encoding.UTF8);
            using (TextReader reader = new StringReader(xmlContent))
            {
                var doc = XDocument.Load(reader);
                var nodes = doc.Element("Root").Element("DLLS").Elements("DLL");
                var ele = doc.Element("Root").Element("DLLS");
                title1 = ele.Attribute("Title1").Value;
                title2 = ele.Attribute("Title2").Value;
                foreach (XElement node in nodes)
                {
                    var item = new DLLInfo();
                    item.Url = node.Attribute("Url").Value;
                    item.Text = node.Attribute("Text").Value;
                    item.Id = Guid.NewGuid().ToString("N");

                    dllList.Add(item);
                }
            }
        }

        public static List<DLLInfo> GetList()
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

        public static List<DLLInfo> GetList(string key)
        {
            lock (dllList)
            {
                return dllList.FindAll(p => p.Text.Contains(key) || p.Url.Contains(key));
            }
        }

        public static void AddList(DLLInfo info)
        {
            lock (dllList)
            {
                info.Id = Guid.NewGuid().ToString("N");
                dllList.Add(info);
                SaveXml();
            }
        }

        public static void EditList(DLLInfo info)
        {
            lock (dllList)
            {
                var item = dllList.Find(p => p.Id == info.Id);
                item = info;

                SaveXml();
            }
        }

        public static void EditTitle(string title1, string title2)
        {
            lock (dllList)
            {
                FriendlyDllXml.title1 = title1;
                FriendlyDllXml.title2 = title2;

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
            content.AppendFormat("  <DLLS Title1=\"{0}\" Title2=\"{1}\">\r\n", title1, title2);
            foreach (var item in dllList)
            {
                content.AppendFormat("    <DLL Url=\"{0}\" Text=\"{1}\" />\r\n", item.Url, item.Text);
            }

            content.AppendLine("  </DLLS>");
            content.AppendLine("</Root>");

            File.WriteAllText(path, content.ToString(), Encoding.UTF8);
            #endregion

            #region 保存html
            StringBuilder contentStr = new StringBuilder();
            foreach (var item in dllList)
            {
                contentStr.AppendFormat(@"
                        <ul class=""list-group"">
                            <li class=""list-group-item""><a target=""_blank"" href=""{0}"">{1}</a></li>
                        </ul>", item.Url, HttpUtility.HtmlEncode(item.Text));
            }

            string htmlContent = File.ReadAllText(htmlTempPath, Encoding.UTF8);
            htmlContent = htmlContent.Replace("{{Title1}}", title1).Replace("{{Title2}}", title2).Replace("{{Content}}", contentStr.ToString()).Replace("{{UpdateTime}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            File.WriteAllText(htmlPath, htmlContent, Encoding.UTF8);
            #endregion
        }
    }

    public class DLLInfo
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public string Text { get; set; }
    }
}