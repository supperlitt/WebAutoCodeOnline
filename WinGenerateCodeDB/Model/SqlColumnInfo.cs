using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public class SqlColumnInfo
    {
        public string Name { get; set; }

        public bool IsMainKey { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsAutoIncrement { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public string Length { get; set; }

        public bool IsAllowNull { get; set; }

        public string DefaultValue { get; set; }

        /// <summary>
        /// 临时存值
        /// </summary>
        public string TempValue { get; set; }

        private string comment = string.Empty;

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }
    }
}
