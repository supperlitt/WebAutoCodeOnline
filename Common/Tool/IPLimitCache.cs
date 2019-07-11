using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class IPLimitCache
    {
        private static List<IPLimitInfo> dataList = new List<IPLimitInfo>();

        /// <summary>
        /// 锁对象
        /// </summary>
        private static object lockObj = new object();

        /// <summary>
        /// 判断是否可以请求进来
        /// </summary>
        /// <returns></returns>
        public static bool CheckCanRequest(string ip)
        {
            lock (lockObj)
            {
                if (dataList.Exists(p => p.IP == ip))
                {
                    var item = dataList.Find(p => p.IP == ip);
                    if (item.LimitOverTime > DateTime.Now)
                    {
                        item.Count++;

                        // 还在限制时间内
                        return false;
                    }
                    else
                    {
                        item.Count++;
                        if (item.Count % 10 == 0)
                        {
                            // 满足10的整倍数就把开始时间和结束时间设置一下
                            item.StartTime = item.EndTime;
                            item.EndTime = DateTime.Now;
                            if ((item.EndTime - item.StartTime).TotalSeconds < 5)
                            {
                                // 小于10秒，则不允许再次请求了
                                item.LimitOverTime = DateTime.Now.AddSeconds(5);

                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }

                        return true;
                    }
                }
                else
                {
                    dataList.Add(new IPLimitInfo()
                    {
                        IP = ip,
                        Count = 1
                    });

                    return true;
                }
            }
        }
    }

    /// <summary>
    /// IP限制对象
    /// </summary>
    public class IPLimitInfo
    {
        public string IP { get; set; }

        public int Count { get; set; }

        private DateTime limitOverTime = DateTime.Parse("1970-1-1");

        public DateTime LimitOverTime
        {
            get
            {
                return this.limitOverTime;
            }
            set
            {
                this.limitOverTime = value;
            }
        }

        private DateTime startTime = DateTime.Parse("1970-1-1");

        public DateTime StartTime
        {
            get
            {
                return this.startTime;
            }
            set
            {
                this.startTime = value;
            }
        }

        private DateTime endTime = DateTime.Parse("1970-1-1");

        public DateTime EndTime
        {
            get
            {
                return this.endTime;
            }
            set
            {
                this.endTime = value;
            }
        }
    }
}
