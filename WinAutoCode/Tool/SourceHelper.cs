using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WinAutoCode
{
    /// <summary>
    /// 资源管理帮助类
    /// </summary>
    public class SourceHelper
    {
        /// <summary>
        /// 读取资源文件内容
        /// </summary>
        /// <param name="fileName">资源文件名称
        /// aspx.cs.txt
        /// aspx.designer.cs.txt
        /// aspx.txt
        /// model.txt
        /// sqlhelper.cs.txt
        /// web.config.txt
        /// dal.txt
        /// assemblyinfo.cs.txt
        /// </param>
        /// <returns></returns>
        public static string GetResource(string fileName)
        {
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAutoCode.source." + fileName);
            if (sm == null)
            {
                return null;
            }
            else
            {
                byte[] bs = new byte[sm.Length];
                sm.Read(bs, 0, (int)sm.Length);
                sm.Close();
                return Encoding.UTF8.GetString(bs);
            }
        }
    }
}
