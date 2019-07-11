using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace WebAutoCodeOnline
{
    public class ConnectionFactory
    {
        public static SqlConnection AliDB
        {
            get
            {
                return new SqlConnection("server=qds169535689.my3w.com;uid=qds169535689;pwd=mort464212863;database=qds169535689_db;");
            }
        }

        public static SqlConnection Location
        {
            get
            {
                return new SqlConnection("server=mssql.sql84.cdncenter.net;uid=sq_iwantdebug;pwd=mort199051;database=sq_iwantdebug;");
            }
        }
    }
}