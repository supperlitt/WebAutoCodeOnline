using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAutoCodeOnline
{
    /// <summary>
    /// 项目类型信息
    /// </summary>
    public class ProjectTypeInfo
    {
        public int ProjectTypeId { get; set; }

        public string ProjectTypeName { get; set; }

        public string ProjectTypeDesc { get; set; }

        /// <summary>
        /// App_Data下文件夹名称
        /// </summary>
        public string DirName { get; set; }

        public int Status { get; set; }

        public int IsDelete { get; set; }
    }
}