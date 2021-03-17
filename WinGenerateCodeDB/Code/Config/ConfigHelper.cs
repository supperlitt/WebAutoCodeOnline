using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class ConfigHelper
    {
        public static string GetConnectStringConfig(string db_name, string connectString)
        {
            string connectionString = string.Format("database={0};{1}", db_name, connectString);
            string template = @"
<configuration>
  <connectionStrings>
    <add name=""{0}"" connectionString=""{1}""/>
  </connectionStrings>
</configuration>";

            return string.Format(template, db_name, connectionString);
        }
    }
}
