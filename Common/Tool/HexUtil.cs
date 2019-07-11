using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class HexUtil
    {
        public static string Read16Str(string hexStr)
        {
            List<byte> dataList = new List<byte>();
            for (int i = 0; i < hexStr.Length; i += 2)
            {
                int value = Convert.ToInt32(hexStr[i].ToString(), 16) * 16;
                value += Convert.ToInt32(hexStr[i + 1].ToString(), 16);

                dataList.Add((byte)value);
            }

            return Encoding.UTF8.GetString(dataList.ToArray());
        }

        public static byte[] Read16Byte(string hexStr)
        {
            List<byte> dataList = new List<byte>();
            for (int i = 0; i < hexStr.Length; i += 2)
            {
                int value = Convert.ToInt32(hexStr[i].ToString(), 16) * 16;
                value += Convert.ToInt32(hexStr[i + 1].ToString(), 16);

                dataList.Add((byte)value);
            }

            return dataList.ToArray();
        }

        public static string Byte2Hex(byte[] bs)
        {
            StringBuilder ret = new StringBuilder();
            foreach (byte b in bs)
            {
                //{0:X2} 大写
                ret.AppendFormat("{0:x2}", b);
            }

            return ret.ToString();
        }
    }
}
