using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Configuration;

namespace WebAutoCodeOnline
{
    public class Logs
    {
        private static object lockObj = new object();

        public static void WriteLog(LogType type, string msg)
        {
            lock (lockObj)
            {
                string pdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(pdir))
                {
                    Directory.CreateDirectory(pdir);
                }

                string dir = Path.Combine(pdir, type.ToString());
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string path = Path.Combine(dir, DateTime.Now.ToString("yyyyMMddHH") + ".txt");
                File.AppendAllText(path, string.Format("{0}\t{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg), Encoding.UTF8);
            }
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        Info = 0,
        Error = 1
    }
}
