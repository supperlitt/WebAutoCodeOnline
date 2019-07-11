using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public class ExtendAttributeInfo
    {
        private string comment = string.Empty;

        public string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        public string NewAttName
        {
            get;
            set;
        }

        /// <summary>
        /// 依赖列
        /// </summary>
        public string DependColumn { get; set; }

        public string DependColumnType { get; set; }

        /// <summary>
        /// 属性类型
        /// string,int,等
        /// </summary>
        public string AttributeType { get; set; }

        public int FormatType
        {
            get;
            set;
        }

        public string FormatStr
        {
            get;
            set;
        }
    }
}
