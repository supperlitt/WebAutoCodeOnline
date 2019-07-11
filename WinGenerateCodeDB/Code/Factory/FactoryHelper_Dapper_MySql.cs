using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class FactoryHelper_Dapper_MySql
    {
        public static string CreateFactory()
        {
            StringBuilder facContent = new StringBuilder();
            facContent.Append(CreateFactoryCode());

            return facContent.ToString();
        }

        private static string CreateFactoryCode()
        {
            string template = @"using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace {0}
{{
    public class ConnectionFactory
    {{
        public static IDbConnection {1}
        {{
            get
            {{
                return new MySqlConnection(ConfigurationManager.ConnectionStrings[""{1}""].ConnectionString);
            }}
        }}
    }}
}}";

            return string.Format(template, PageCache.NameSpaceStr, PageCache.DatabaseName);
        }
    }
}
