using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceHelper
{
    public class ModuleInfo
    {
        private string moduleName = string.Empty;

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName
        {
            get { return this.moduleName; }
            set { this.moduleName = value; }
        }

        private string moduleType = string.Empty;
        /// <summary>
        /// 模块类型：
        /// 系统自带：login nav manager
        /// 自定义：
        /// </summary>
        public string ModuleType
        {
            get { return this.moduleType; }
            set { this.moduleType = value; }
        }

        /// <summary>
        /// 1-bootstrap
        /// 2-easyui
        /// 4-xxxx
        /// </summary>
        public int AdapterType { get; set; }

        /// <summary>
        /// 命名空间-UI层
        /// </summary>
        private string nameSpace = string.Empty;
        public string NameSpace
        {
            get { return this.nameSpace; }
            set { this.nameSpace = value; }
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        private string dbName = string.Empty;
        public string DbName
        {
            get { return this.dbName; }
            set { this.dbName = value; }
        }

        /// <summary>
        /// 数据库类型
        /// 0-Sql Server
        /// 1-MySql
        /// </summary>
        private int dbType = 0;
        public int DbType
        {
            get { return this.dbType; }
            set { this.dbType = value; }
        }

        private string tableName = string.Empty;
        /// <summary>
        /// 关联表名
        /// 对于分表的表名使用
        /// TableYYYYMM 按月分表
        /// TableYYYY 按年份表
        /// TableXX 按数字分表00-99
        /// TableXXXXX 按名称XXXXX分表
        /// 暂不支持分库操作
        /// </summary>
        public string TableName
        {
            get { return this.tableName; }
            set { this.tableName = value; }
        }

        public string Desc { get; set; }

        private string pageName = string.Empty;
        public string PageName
        {
            get { return this.pageName; }
            set { this.pageName = value; }
        }

        private string tableStr = string.Empty;
        public string TableStr
        {
            get { return this.tableStr; }
            set { this.tableStr = value; }
        }

        public List<string> settingList = new List<string>();

        /// <summary>
        /// 设置，额外配置的一些细节，内容，可用于扩展
        /// </summary>
        public List<string> SettingList
        {
            get { return this.settingList; }
            set { this.settingList = value; }
        }
    }
}
