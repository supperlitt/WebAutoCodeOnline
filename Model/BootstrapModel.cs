using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BootstrapModel : NormalModel
    {
        private bool isAdd = false;
        public bool IsAdd
        {
            get { return this.isAdd; }
            set { this.isAdd = value; }
        }

        private string addColumnsStr = string.Empty;
        public string AddColumnsStr
        {
            get { return this.addColumnsStr; }
            set { this.addColumnsStr = value; }
        }

        /// <summary>
        /// 作为添加的列
        /// </summary>
        private List<ColumnInfo> addColumns = new List<ColumnInfo>();
        public List<ColumnInfo> AddColumns
        {
            get { return this.addColumns; }
            set { this.addColumns = value; }
        }

        private bool isEdit = false;
        public bool IsEdit
        {
            get { return isEdit; }
            set { isEdit = value; }
        }

        private string editColumnsStr = string.Empty;
        public string EditColumnsStr
        {
            get { return this.editColumnsStr; }
            set { this.editColumnsStr = value; }
        }

        /// <summary>
        /// 作为编辑的列
        /// </summary>
        private List<ColumnInfo> editColumns = new List<ColumnInfo>();
        public List<ColumnInfo> EditColumns
        {
            get { return this.editColumns; }
            set { this.editColumns = value; }
        }

        private bool isBatEdit = false;
        public bool IsBatEdit
        {
            get { return isBatEdit; }
            set { isBatEdit = value; }
        }

        private string batEditColumnsStr = string.Empty;
        public string BatEditColumnsStr
        {
            get { return this.batEditColumnsStr; }
            set { this.batEditColumnsStr = value; }
        }

        /// <summary>
        /// 作为批量编辑的列
        /// </summary>
        private List<ColumnInfo> batEditColumns = new List<ColumnInfo>();
        public List<ColumnInfo> BatEditColumns
        {
            get { return this.batEditColumns; }
            set { this.batEditColumns = value; }
        }

        private bool isDel = false;
        public bool IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }

        private bool isBatDel = false;
        public bool IsBatDel
        {
            get { return isBatDel; }
            set { isBatDel = value; }
        }

        /// <summary>
        /// 是否包含批量处理
        /// </summary>
        public bool IsBatOperation
        {
            get { return isBatDel | isBatEdit; }
        }

        /// <summary>
        /// 主键字符串
        /// </summary>
        public string MainKeyIdStr { get; set; }

        public string MainKeyIdDBType { get; set; }

        private bool isExport = false;
        public bool IsExport
        {
            get { return this.isExport; }
            set { this.isExport = value; }
        }
    }
}
