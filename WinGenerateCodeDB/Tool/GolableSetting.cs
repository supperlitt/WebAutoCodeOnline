using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public class GolableSetting
    {
        private static Dictionary<string, string> allDic = new Dictionary<string, string>();

        public static void AddSetting(string key, string value)
        {
            lock (allDic)
            {
                if (allDic.ContainsKey(key))
                {
                    allDic[key] = value;
                }
                else
                {
                    allDic.Add(key, value);
                }
            }
        }

        public static Dictionary<string, string> GetDic()
        {
            lock (allDic)
            {
                return allDic;
            }
        }
    }
}
