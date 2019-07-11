using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool
{
    public class EncodingManager
    {
        private static List<Encoding> encodingList = new List<Encoding>();

        static EncodingManager()
        {
            encodingList.Add(Encoding.UTF8);
            encodingList.Add(Encoding.GetEncoding("gb2312"));
            encodingList.Add(Encoding.GetEncoding("gbk"));
            encodingList.Add(Encoding.GetEncoding("ISO-8859-1"));
            encodingList.Add(Encoding.Unicode);
            encodingList.Add(Encoding.ASCII);
        }

        public static List<Encoding> GetEncodingList()
        {
            return encodingList;
        }
    }
}
