using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Web.Security;

namespace Common
{
    /// <summary>
    /// Js相关的功能代码
    /// </summary>
    public class JsTool
    {
        /// <summary>
        /// 1970年格林威治时间,,不多计算出来有偏差，差9那个值
        /// </summary>
        private static long lLeft = 621355968000000000;

        /// <summary>
        /// 得到10位js时间值
        /// </summary>
        /// <returns></returns>
        public static long GetIntFromTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime dt1 = dt.ToUniversalTime();
            long Sticks = (dt1.Ticks - lLeft) / 10000000;
            return Sticks;
        }

        /// <summary>
        /// 得到13位js时间值
        /// </summary>
        /// <returns></returns>
        public static long GetLongFromTime()
        {
            DateTime dt = DateTime.UtcNow;
            DateTime dt1 = dt.ToUniversalTime();
            long Sticks = (dt1.Ticks - lLeft) / 10000;
            return Sticks;
        }
    }
}
