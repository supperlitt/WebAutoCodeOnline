using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class FactoryHelper_MsSql
    {
        public static string CreateFactory(string name_space, string db_name)
        {
            StringBuilder facContent = new StringBuilder();
            facContent.Append(CreateFactoryCode(name_space, db_name));

            return facContent.ToString();
        }

        private static string CreateFactoryCode(string name_space, string db_name)
        {
            string template = @"using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace {0}
{{
    public class ConnectionFactory
    {{
        public static MySqlConnection {1}
        {{
            get
            {{
                return new MySqlConnection(ConfigurationManager.ConnectionStrings[""{1}""].ConnectionString);
            }}
        }}
    }}
}}";

            return string.Format(template, name_space, db_name);
        }
    }
}
