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

        public static string TableName_DAL
        {
            get
            {
                return TableName + DALSuffix;
            }
        }

        public static string KeyId_DbType { get; set; }

        public static string NameSpaceStr { get; set; }

        public static string ModelSuffix { get; set; }

        public static string DALSuffix { get; set; }

        public static string UISuffix { get; set; }

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

        public static void SetParamValue(string namespaceStr, string modelSuffix, string dalSuffix, string uiSuffix)
        {
            NameSpaceStr = namespaceStr;
            ModelSuffix = modelSuffix;
            DALSuffix = dalSuffix;
            UISuffix = uiSuffix;
        }

        private static void SaveConfig()
        {
            // 保存配置信息，下次打开还可以继续
        }
    }

    public class config_info
    {
        public config_info()
        {
            asp_net_core_add_query_model = true;
        }

        public int db_type { get; set; }

        public string server { get; set; }

        public string name { get; set; }

        public string pwd { get; set; }

        public string port { get; set; }

        public string asp_net_ns { get; set; }

        public string asp_net_ui_staff { get; set; }

        public string asp_net_model_staff { get; set; }

        public string asp_net_dal_staff { get; set; }

        public string asp_net_db_tool { get; set; }

        public string asp_net_ui_type { get; set; }

        public string asp_net_model_style { get; set; }

        public bool asp_net_core_add_query_model { get; set; }

        public string asp_net_core_ns { get; set; }

        public string asp_net_core_model_staff { get; set; }

        public string asp_net_core_dal_staff { get; set; }
    }
}