using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 列对象
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsAutoIncrement { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsMainKey { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DBType { get; set; }

        /// <summary>
        /// 列注释
        /// </summary>
        public string Comment { get; set; }
    }
}
