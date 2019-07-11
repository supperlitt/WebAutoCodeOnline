using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAutoEasyUI
{
    public class TempModel
    {
        /// <summary>
        /// 作为查询条件的列
        /// </summary>
        private string searchColumns = string.Empty;
        public string SearchColumns
        {
            get { return this.searchColumns; }
            set { this.searchColumns = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        private string title = string.Empty;
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        private string tableName = string.Empty;
        public string TableName
        {
            get { return this.tableName; }
            set { this.tableName = value; }
        }

        /// <summary>
        /// 列列表
        /// </summary>
        private List<TableInfo> tableList = new List<TableInfo>();
        public List<TableInfo> TableList
        {
            get { return this.tableList; }
            set { this.tableList = value; }
        }

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
        /// 目标存放URI
        /// </summary>
        private string targetDir = string.Empty;
        public string TargetDir
        {
            get { return this.targetDir; }
            set { this.targetDir = value; }
        }

        /// <summary>
        /// 模板文件路径
        /// </summary>
        private string sourceDir = string.Empty;
        public string SourceDir
        {
            get { return this.sourceDir; }
            set { this.sourceDir = value; }
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
        /// 连接字符串
        /// </summary>
        private string connectionStr = string.Empty;
        public string ConnectionStr
        {
            get { return this.connectionStr; }
            set { this.connectionStr = value; }
        }

        /// <summary>
        /// 是否每层独立一个DLL
        /// </summary>
        public bool IsDenpendDLL
        {
            get;
            set;
        }
    }
}
