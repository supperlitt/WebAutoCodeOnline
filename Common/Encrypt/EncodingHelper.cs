using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Common
{
    public class EncodingHelper
    {
        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encodeType"></param>
        /// <returns></returns>
        public static string EncodeBase64(string text, string encodeType)
        {
            Encoding encode = Encoding.GetEncoding(encodeType);
            byte[] bytes = encode.GetBytes(text);
            string result = Convert.ToBase64String(bytes);
            return result;
        }

        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encodeType"></param>
        /// <returns></returns>
        public static string DecodeBase64(string text, string encodeType)
        {
            try
            {
                Encoding encode = Encoding.GetEncoding(encodeType);
                byte[] base64 = Convert.FromBase64String(text);
                return encode.GetString(base64);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UrlEncode(string text)
        {
            return HttpUtility.UrlEncode(text);
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UrlDecode(string text)
        {
            return HttpUtility.UrlDecode(text);
        }

        /// <summary>
        /// 反斜杠U编码 \uXXXX
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string BackslashUEncode(string text, string encodeType)
        {
            Encoding encode = Encoding.GetEncoding(encodeType);
            byte[] bts = encode.GetBytes(text);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2)
            {
                r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            }

            return r;
        }

        /// <summary>
        /// 反斜杠U解码 \uXXXX
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string BackslashUDecode(string text, string encodeType)
        {
            Encoding encode = Encoding.GetEncoding(encodeType);
            Regex regex = new Regex(@"\\u(?<key>\w{4})");
            var matches = regex.Matches(text);
            foreach (Match m in matches)
            {
                string value = m.Groups["key"].Value;
                string first = value.Substring(2, 2);
                string second = value.Substring(0, 2);
                byte[] buffer = new byte[2];
                buffer[0] = (byte)Convert.ToInt32(first, 16);
                buffer[1] = (byte)Convert.ToInt32(second, 16);

                string decodeStr = encode.GetString(buffer);
                text = text.Replace(m.ToString(), decodeStr);
            }

            return text;
        }

        /// <summary>
        /// 百分号F编码 %fXXXX
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encodeType"></param>
        /// <returns></returns>
        public static string PercentFEncode(string text, string encodeType)
        {
            Encoding encode = Encoding.GetEncoding(encodeType);
            byte[] bts = encode.GetBytes(text);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2)
            {
                r += "%f" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            }

            return r;
        }

        /// <summary>
        /// 百分号F解码 %fXXXX
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encodeType"></param>
        /// <returns></returns>
        public static string PercentFDecode(string text, string encodeType)
        {
            Encoding encode = Encoding.GetEncoding(encodeType);
            Regex regex = new Regex(@"%f(?<key>\w{4})");
            var matches = regex.Matches(text);
            foreach (Match m in matches)
            {
                string value = m.Groups["key"].Value;
                string first = value.Substring(2, 2);
                string second = value.Substring(0, 2);
                byte[] buffer = new byte[2];
                buffer[0] = (byte)Convert.ToInt32(first, 16);
                buffer[1] = (byte)Convert.ToInt32(second, 16);

                string decodeStr = encode.GetString(buffer);
                text = text.Replace(m.ToString(), decodeStr);
            }

            return text;
        }
    }
}