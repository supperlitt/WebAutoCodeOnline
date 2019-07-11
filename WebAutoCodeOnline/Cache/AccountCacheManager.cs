using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAutoCodeOnline
{
    /// <summary>
    /// IP访问频率控制器
    /// </summary>
    public class AccountCacheManager
    {
        /// <summary>
        /// IP缓存集合
        /// </summary>
        private static List<AccountCacheInfo> dataList = new List<AccountCacheInfo>();
        private static object lockObj = new object();

        /// <summary>
        /// 一段时间内，最大请求次数,必须大于等于1
        /// </summary>
        private static int maxTimes = 3;

        /// <summary>
        /// 一段时间长度（单位秒)，必须大于等于1
        /// </summary>
        private static int partSecond = 60;

        /// <summary>
        /// 请求被拒绝是否加入请求次数
        /// </summary>
        private static bool isFailAddIn = false;

        static AccountCacheManager()
        {
        }

        /// <summary>
        /// 设置时间，默认maxTimes=3, partSecond=30
        /// </summary>
        /// <param name="_maxTimes">最大请求次数</param>
        /// <param name="_partSecond">请求单位时间</param>
        public static void SetTime(int _maxTimes, int _partSecond)
        {
            maxTimes = _maxTimes;
            partSecond = _partSecond;
        }

        /// <summary>
        /// 检测一段时间内，IP的请求次数是否可以继续请求
        /// 和使用
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CheckIsAble(string username)
        {
            lock (lockObj)
            {
                var item = dataList.Find(p => p.Account == username);
                if (item == null)
                {
                    item = new AccountCacheInfo();
                    item.Account = username;
                    item.ReqTime.Add(DateTime.Now);
                    dataList.Add(item);

                    return true;
                }
                else
                {
                    if (item.ReqTime.Count > maxTimes)
                    {
                        item.ReqTime.RemoveAt(0);
                    }

                    var nowTime = DateTime.Now;
                    if (isFailAddIn)
                    {
                        #region 请求被拒绝也需要加入当次请求
                        item.ReqTime.Add(nowTime);
                        if (item.ReqTime.Count >= maxTimes)
                        {
                            if (item.ReqTime[0].AddSeconds(partSecond) > nowTime)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 请求被拒绝就不需要加入当次请求了
                        if (item.ReqTime.Count >= maxTimes)
                        {
                            if (item.ReqTime[0].AddSeconds(partSecond) > nowTime)
                            {
                                return false;
                            }
                            else
                            {
                                item.ReqTime.Add(nowTime);
                                return true;
                            }
                        }
                        else
                        {
                            item.ReqTime.Add(nowTime);
                            return true;
                        }
                        #endregion
                    }
                }
            }
        }
    }

    public class AccountCacheInfo
    {
        public string Account { get; set; }

        private List<DateTime> reqTime = new List<DateTime>();
        public List<DateTime> ReqTime
        {
            get { return this.reqTime; }
            set { this.reqTime = value; }
        }
    }
}
