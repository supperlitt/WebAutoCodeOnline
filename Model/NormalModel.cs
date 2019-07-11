using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class NormalModel
    {
        /// <summary>
        /// 查询列
        /// </summary>
        private string searchColumnsStr = string.Empty;
        public string SearchColumnsStr
        {
            get { return this.searchColumnsStr; }
            set { this.searchColumnsStr = value; }
        }

        /// <summary>
        /// 作为查询条件的列
        /// </summary>
        private List<ColumnInfo> searchColumns = new List<ColumnInfo>();
        public List<ColumnInfo> SearchColumns
        {
            get { return this.searchColumns; }
            set { this.searchColumns = value; }
        }

        /// <summary>
        /// 列列表
        /// </summary>
        private List<ColumnInfo> columnList = new List<ColumnInfo>();
        public List<ColumnInfo> ColumnList
        {
            get { return this.columnList; }
            set { this.columnList = value; }
        }

        /// <summary>
        /// 表的字符串
        /// </summary>
        private string tableStr = string.Empty;
        public string TableStr
        {
            get { return this.tableStr; }
            set { this.tableStr = value; }
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
        /// </summary>
        private int dbType = 0;
        public int DbType
        {
            get { return this.dbType; }
            set { this.dbType = value; }
        }
    }
}
