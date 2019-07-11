using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAutoCodeOnline
{
    public class DownZipCache
    {
        private static List<ZipInfo> dataList = new List<ZipInfo>();
        private static object lockObj = new object();

        static DownZipCache()
        {
        }

        public static void AddZipInfo(ZipInfo zipInfo)
        {
            lock (lockObj)
            {
                if (dataList.Count > 1000)
                {
                    dataList.RemoveAll(p => p.AddTime.AddHours(1) < DateTime.Now);
                }

                if (dataList.Count > 2000)
                {
                    dataList.RemoveAt(0);
                }

                dataList.Add(zipInfo);
            }
        }

        public static ZipInfo GetZipData(string guid)
        {
            var item = dataList.Find(p => p.Guid == guid);

            return item;
        }
    }

    /// <summary>
    /// Zip信息
    /// </summary>
    public class ZipInfo
    {
        public string Guid { get; set; }

        public ZipType Type { get; set; }

        public string ParamStr { get; set; }

        public string IP { get; set; }

        public DateTime AddTime { get; set; }
    }

    public enum ZipType
    {
        EasyUI = 0,
        NormalCode = 1
    }
}