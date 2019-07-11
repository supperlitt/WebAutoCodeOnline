using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAutoEasyUI
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class ExtendMethods
    {
        /// <summary>
        /// 小写首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToFirstLower(this string str)
        {
            if (str.Length > 0)
            {
                return str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1);
            }

            return string.Empty;
        }

        /// <summary>
        /// 大写首字母
        /// </summary>
        /// <param name="str">单次</param>
        /// <returns></returns>
        public static string ToFirstUpper(this string str)
        {
            if (str.Length > 0)
            {
                return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1);
            }

            return string.Empty;
        }

        /// <summary>
        /// 左补字符串（一次字符串占一个位置）
        /// </summary>
        /// <param name="str">原字符串-需要处理的字符串</param>
        /// <param name="len">处理后，</param>
        /// <param name="addstr">左边补充的字符串</param>
        /// <returns></returns>
        public static string PadLeft(this string str, int len, string addstr)
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                var addCount = len - str.Length;
                for (int i = 0; i < addCount; i++)
                {
                    result.Append("&#12288;");
                }

                result.Append(str);
            }

            return result.ToString();
        }
    }
}
