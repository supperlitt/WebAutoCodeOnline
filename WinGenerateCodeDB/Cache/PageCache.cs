using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WinGenerateCodeDB.Cache
{
    public class PageCache
    {
        private static List<SqlColumnInfo> sqlColumnList = new List<SqlColumnInfo>();

        private static List<ExtendAttributeInfo> ExtendList = new List<ExtendAttributeInfo>();

        private static List<CmdInfo> CmdList = new List<CmdInfo>();

        /// <summary>
        /// 0-asp.net
        /// 1-asp.net mvc
        /// 2-html
        /// </summary>
        public static int WebType { get; set; }

        /// <summary>
        /// 0-mysql
        /// 1-mssql
        /// 2-sqlite
        /// </summary>
        public static int DbType { get; set; }

        /// <summary>
        /// 0-SQL语句
        /// 1-ORM(Dapper)
        /// 2-Hibernate
        /// 3-EF
        /// </summary>
        public static int DbTool { get; set; }

        /// <summary>
        /// 0-EasyUI
        /// 1-Bootstrap
        /// 2-Layui
        /// </summary>
        public static int UIType { get; set; }

        public static string ConnectionString = string.Empty;

        public static string DatabaseName = string.Empty;

        public static string TableName = string.Empty;

        public static string TableName_Model
        {
            get
            {
                return TableName + ModelSuffix;
            }
        }

        public static string TableName_UI
        {
            get
            {
                return TableName + UISuffix;
            }
        }

        public static string TableName_DAL
        {
            get
            {
                return TableName + DALSuffix;
            }
        }

        public static string KeyId { get; set; }

        public static string KeyId_DbType { get; set; }

        public static string TiTle { get; set; }

        public static string NameSpaceStr { get; set; }

        public static string ModelSuffix { get; set; }

        public static string DALSuffix { get; set; }

        public static string UISuffix { get; set; }

        public static int FrameworkVersion { get; set; }

        public static int ModelStyle { get; set; }

        public static string server = string.Empty;
        public static string name = string.Empty;
        public static string pwd = string.Empty;
        public static int port = 0;

        public static void SetServer(string server, string name, string pwd, int port)
        {
            PageCache.server = server;
            PageCache.name = name;
            PageCache.pwd = pwd;
            PageCache.port = port;
            ConnectionString = string.Format("server={0};uid={1};pwd={2};port={3};", server, name, pwd, port);
        }

        public static void SetDatabase(string dbname)
        {
            DatabaseName = dbname;
        }

        public static void SetDbType(int dbType)
        {
            DbType = dbType;
        }

        public static void SetTable(string tableName)
        {
            TableName = tableName;
        }

        public static void SetColumnList(List<SqlColumnInfo> list, bool isImport)
        {
            sqlColumnList = list;
            if (!isImport)
            {
                ExtendList = new List<ExtendAttributeInfo>();
                var col = list.Find(p => p.IsMainKey);
                if (col != null)
                {
                    KeyId = col.Name;
                    KeyId_DbType = col.DbType;
                }
            }
        }

        public static void AddAttribute(ExtendAttributeInfo extendInfo)
        {
            ExtendList.Add(extendInfo);
        }

        public static void ChangeExtendInfo(string attrName, ExtendAttributeInfo extendInfo)
        {
            var item = ExtendList.Find(p => p.NewAttName == attrName);
            if (item != null)
            {
                item = extendInfo;
            }
        }

        public static void RemoveAttribute(string attrName)
        {
            ExtendList.RemoveAll(p => p.NewAttName == attrName);
        }

        public static void SetDbTool(int dbTool)
        {
            DbTool = dbTool;
        }

        public static void SetUIType(int uiType)
        {
            UIType = uiType;
        }

        public static void SetModelType(int model_type)
        {
            ModelStyle = model_type;
        }

        public static void SetWebType(int webType)
        {
            WebType = webType;
        }

        public static void SetParamValue(string namespaceStr, string modelSuffix, string dalSuffix, string uiSuffix)
        {
            NameSpaceStr = namespaceStr;
            ModelSuffix = modelSuffix;
            DALSuffix = dalSuffix;
            UISuffix = uiSuffix;
        }

        public static void AddCmd(CmdInfo info)
        {
            CmdList.Add(info);
        }

        public static void RemoveCmd(string guid)
        {
            CmdList.RemoveAll(p => p.Guid == guid);
        }

        public static List<CmdInfo> GetCmdList()
        {
            return CmdList;
        }

        public static CmdInfo GetCmd(string name)
        {
            return CmdList.Find(p => p.CmdName == name);
        }

        public static List<SqlColumnInfo> GetColumnList()
        {
            return sqlColumnList;
        }

        public static List<ExtendAttributeInfo> GetExtendList()
        {
            return ExtendList;
        }

        public static string ToXml()
        {
            StringBuilder content = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            content.AppendLine("<root>");

            content.AppendLine("    <dbinfo>");
            content.AppendLine("        <server>" + PageCache.server + "</server>");
            content.AppendLine("        <uid>" + PageCache.name + "</uid>");
            content.AppendLine("        <pwd>" + PageCache.pwd + "</pwd>");
            content.AppendLine("        <database>" + PageCache.DatabaseName + "</database>");
            content.AppendLine("        <port>" + PageCache.port + "</port>");
            content.AppendLine("        <table>" + PageCache.TableName + "</table>");
            content.AppendLine("    </dbinfo>");

            content.AppendLine("    <cols>");
            foreach (var item in sqlColumnList)
            {
                content.AppendLine("        <col>");
                content.AppendLine("            <name>" + item.Name + "</name>");
                content.AppendLine("            <colname>" + item.Name + "</colname>");
                content.AppendLine("            <dbtype>" + item.DbType + "</dbtype>");
                content.AppendLine("            <attrtype>" + item.DbType + "</attrtype>");
                content.AppendLine("            <title>" + item.Comment + "</title>");
                content.AppendLine("        </col>");
            }
            content.AppendLine("    </cols>");

            content.AppendLine("    <keyinfo>");
            content.AppendLine("        <keyid>" + KeyId + "</keyid>");
            content.AppendLine("        <keytype>" + KeyId_DbType + "</keytype>");
            content.AppendLine("    </keyinfo>");

            content.AppendLine("    <extends>");
            foreach (var item in ExtendList)
            {
                content.AppendLine("    <extend>");
                content.AppendLine("        <name>" + item.NewAttName + "</name>");
                content.AppendLine("        <colname>" + item.DependColumn + "</colname>");
                content.AppendLine("        <dbtype>" + item.DependColumnType + "</dbtype>");
                content.AppendLine("        <attrtype>varchar</attrtype>");
                content.AppendLine("        <title>" + item.Comment + "</title>");
                content.AppendLine("        <formattype>" + item.FormatType + "</formattype>");
                content.AppendLine("        <formatstr><![CDATA[" + item.FormatStr + "]]></formatstr>");
                content.AppendLine("    </extend>");
            }
            content.AppendLine("    </extends>");

            content.AppendLine("    <toolinfo>");
            foreach (var item in CmdList)
            {
                content.AppendLine("        <tool>");
                content.AppendLine("            <name>" + item.CmdName + "</name>");
                content.AppendLine("            <guid>" + item.Guid + "</guid>");
                content.AppendLine("            <attrs>");

                foreach (var attr in item.AttrList)
                {
                    content.AppendLine("                <attr>");
                    content.AppendLine("                    <name>" + attr.AttrName + "</name>");
                    content.AppendLine("                    <title>" + attr.TitleName + "</title>");
                    content.AppendLine("                    <styleinfo>");
                    content.AppendLine("                        <name>" + attr.Style.FieldName + "</name>");
                    content.AppendLine("                        <type>" + attr.Style.FormatType + "</type>");
                    content.AppendLine("                        <str>" + attr.Style.FormatStr + "</str>");
                    content.AppendLine("                    </styleinfo>");
                    content.AppendLine("                </attr>");
                }

                content.AppendLine("            </attrs>");
                content.AppendLine("        </tool>");
            }
            content.AppendLine("    </toolinfo>");

            content.AppendLine("    <othersetting>");
            content.AppendLine("        <dbtype>" + DbType + "</dbtype>");
            content.AppendLine("        <uitype>" + UIType + "</uitype>");
            content.AppendLine("        <webtype>" + WebType + "</webtype>");
            content.AppendLine("        <version>" + FrameworkVersion + "</version>");
            content.AppendLine("        <modeltype>" + ModelStyle + "</modeltype>");
            content.AppendLine("        <namespace>" + NameSpaceStr + "</namespace>");
            content.AppendLine("        <modelsuffix>" + ModelSuffix + "</modelsuffix>");
            content.AppendLine("        <dalsuffix>" + DALSuffix + "</dalsuffix>");
            content.AppendLine("        <uisuffix>" + UISuffix + "</uisuffix>");
            content.AppendLine("    </othersetting>");

            content.Append("</root>");
            return content.ToString();
        }

        public static void ReadXml(string xml)
        {
            using (TextReader reader = new StringReader(xml))
            {
                var doc = XDocument.Load(reader);
                var dbinfo = doc.Element("root").Element("dbinfo");
                var server = dbinfo.Element("server").Value;
                var uid = dbinfo.Element("uid").Value;
                var pwd = dbinfo.Element("pwd").Value;
                var database = dbinfo.Element("database").Value;
                var port = dbinfo.Element("port").Value;
                var table = dbinfo.Element("table").Value;

                PageCache.server = server;
                PageCache.name = uid;
                PageCache.pwd = pwd;
                PageCache.DatabaseName = database;
                int portNum = 0;
                int.TryParse(port, out portNum);
                PageCache.port = portNum;
                PageCache.TableName = table;

                var keyinfo = doc.Element("root").Element("keyinfo");
                var keyid = keyinfo.Element("keyid").Value;
                string keytype = keyinfo.Element("keytype").Value;

                PageCache.KeyId = keyid;
                PageCache.KeyId_DbType = keytype;

                PageCache.sqlColumnList = new List<SqlColumnInfo>();
                var cols = doc.Element("root").Element("cols");
                if (cols.HasElements)
                {
                    var colList = cols.Elements("col");
                    foreach (XElement colEle in colList)
                    {
                        var name = colEle.Element("name").Value;
                        var colname = colEle.Element("colname").Value;
                        var dbtype = colEle.Element("dbtype").Value;
                        var attrtype = colEle.Element("attrtype").Value;
                        var title = colEle.Element("title").Value;

                        PageCache.sqlColumnList.Add(new SqlColumnInfo()
                        {
                            Name = colname,
                            DbType = dbtype,
                            Comment = title,
                            IsMainKey = colname == keyid,
                        });
                    }
                }

                PageCache.ExtendList = new List<ExtendAttributeInfo>();
                var extends = doc.Element("root").Element("extends");
                var extendList = extends.Elements("extend");
                foreach (var extendEle in extendList)
                {
                    var name = extendEle.Element("name").Value;
                    var colname = extendEle.Element("colname").Value;
                    var dbtype = extendEle.Element("dbtype").Value;
                    var attrtype = extendEle.Element("attrtype").Value;
                    var title = extendEle.Element("title").Value;
                    var formattype = extendEle.Element("formattype").Value;
                    int formatNum = 0;
                    var formatstr = extendEle.Element("formatstr").Value;
                    int.TryParse(formattype, out formatNum);

                    PageCache.ExtendList.Add(new ExtendAttributeInfo()
                    {
                        NewAttName = name,
                        DependColumn = colname,
                        Comment = title,
                        DependColumnType = dbtype,
                        AttributeType = attrtype,
                        FormatStr = formatstr,
                        FormatType = formatNum
                    });
                }

                PageCache.CmdList = new List<CmdInfo>();
                var toolinfo = doc.Element("root").Element("toolinfo");
                var tools = toolinfo.Elements("tool");
                foreach (var toolEle in tools)
                {
                    CmdInfo cmdInfo = new CmdInfo();
                    var name = toolEle.Element("name").Value;
                    var guid = toolEle.Element("guid").Value;
                    cmdInfo.CmdName = name;
                    cmdInfo.Guid = guid;
                    cmdInfo.AttrList = new List<ClassAttributeInfo>();

                    var attrs = toolEle.Element("attrs");
                    var attrList = attrs.Elements("attr");
                    foreach (var attrEle in attrList)
                    {
                        var attrname = attrEle.Element("name").Value;
                        var title = attrEle.Element("title").Value;
                        var styleinfo = attrEle.Element("styleinfo");
                        string style_name = styleinfo.Element("name").Value;
                        int style_type_Num = 0;
                        string style_type = styleinfo.Element("type").Value;
                        int.TryParse(style_type, out style_type_Num);
                        string style_str = styleinfo.Element("str").Value;

                        var colInfo = PageCache.GetColumnList().Find(p => p.Name == attrname);
                        if (colInfo != null)
                        {
                            cmdInfo.AttrList.Add(new ClassAttributeInfo()
                            {
                                AttrName = attrname,
                                ColName = colInfo.Name,
                                DbType = colInfo.DbType,
                                AttrType = colInfo.DbType,
                                TitleName = colInfo.Comment,
                                Style = new FieldStyleInfo()
                                {
                                    FieldName = style_name,
                                    FormatType = style_type_Num,
                                    FormatStr = style_str
                                }
                            });
                        }
                        else
                        {
                            var extendInfo = PageCache.ExtendList.Find(p => p.NewAttName == attrname);
                            cmdInfo.AttrList.Add(new ClassAttributeInfo()
                            {
                                AttrName = attrname,
                                ColName = extendInfo.DependColumn,
                                DbType = extendInfo.DependColumnType,
                                AttrType = extendInfo.AttributeType,
                                TitleName = extendInfo.Comment,
                                Style = new FieldStyleInfo()
                                {
                                    FieldName = style_name,
                                    FormatType = style_type_Num,
                                    FormatStr = style_str
                                }
                            });
                        }
                    }

                    PageCache.CmdList.Add(cmdInfo);
                }

                var othersetting = doc.Element("root").Element("othersetting");
                PageCache.DbType = int.Parse(othersetting.Element("dbtype").Value);
                PageCache.UIType = int.Parse(othersetting.Element("uitype").Value);
                PageCache.WebType = int.Parse(othersetting.Element("webtype").Value);
                PageCache.FrameworkVersion = int.Parse(othersetting.Element("version").Value);
                PageCache.ModelStyle = int.Parse(othersetting.Element("modeltype").Value);
                PageCache.NameSpaceStr = othersetting.Element("namespace").Value;
                PageCache.ModelSuffix = othersetting.Element("modelsuffix").Value;
                PageCache.DALSuffix = othersetting.Element("dalsuffix").Value;
                PageCache.UISuffix = othersetting.Element("uisuffix").Value;
            }
        }
    }

    public class CmdInfo
    {
        public string Guid { get; set; }

        public string CmdName { get; set; }

        public List<ClassAttributeInfo> AttrList { get; set; }
    }

    public class ClassAttributeInfo
    {
        public string AttrName { get; set; }

        public string ColName { get; set; }

        public string TitleName { get; set; }

        public string AttrType { get; set; }

        public string DbType { get; set; }

        public FieldStyleInfo Style { get; set; }
    }
}