using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WinAutoCode
{
    public class ConfigManager
    {
        private static string defaultPath = string.Empty;

        static ConfigManager()
        {
            defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "default.ini");
        }

        public static void InitConfig(ref int sport, ref int fport)
        {
            try
            {
                // 创建配置文件
            }
            catch { }
        }

        public static string GetDefaultStartFrm()
        {
            IniFiles iniFile = new IniFiles(defaultPath);
            string startFrm = iniFile.ReadString("Remote", "Start", "EasyUIAutoFrm");

            return startFrm;
        }

        public static void SetDefaultStartFrm(string frmKey)
        {
            IniFiles iniFile = new IniFiles(defaultPath);
            iniFile.WriteString("Remote", "Start", frmKey);
        }
    }
}
