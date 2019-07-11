using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WebAutoCodeOnline.MySqlDAL
{
    public class ConnectionFactory
    {
        public static MySqlConnection AliDb
        {
            get
            {
                return new MySqlConnection("server=localhost;uid=root;pwd=Aa123456a;database=AliDb;CharSet=utf8");
            }
        }

        public static MySqlConnection Location
        {
            get
            {
                return new MySqlConnection("server=localhost;uid=root;pwd=Aa123456a;database=Location;CharSet=utf8");
            }
        }
    }
}
