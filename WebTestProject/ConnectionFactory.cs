using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Test
{
    public class ConnectionFactory
    {
        public static SqlConnection TestDB
        {
            get
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);
            }
        }
    }
}