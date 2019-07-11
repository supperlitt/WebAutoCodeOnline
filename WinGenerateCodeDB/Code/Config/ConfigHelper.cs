using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class ConfigHelper
    {
        public static string GetConnectStringConfig()
        {
            string connectionString = string.Format("database={0};{1}", PageCache.DatabaseName, PageCache.ConnectionString);
            string template = @"
<configuration>
  <connectionStrings>
    <add name=""{0}"" connectionString=""{1}""/>
  </connectionStrings>";

            return string.Format(template, PageCache.DatabaseName, connectionString);
        }
    }
}
