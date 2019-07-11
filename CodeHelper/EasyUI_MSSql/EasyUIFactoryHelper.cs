using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    public class EasyUIFactoryHelper
    {
        public static string CreateFactory(EasyUIModel model)
        {
            string template = @"using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace {0}
{{
    public class ConnectionFactory
    {{
        public static SqlConnection {1}
        {{
            get
            {{
                return new SqlConnection(ConfigurationManager.ConnectionStrings[""{1}""].ConnectionString);
            }}
        }}
    }}
}}";

            return string.Format(template, model.NameSpace, model.DbName.ToFirstUpper());
        }
    }
}
