using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB.Code
{
    public class AspNetCoreOther
    {
        private string name_space = string.Empty;

        public AspNetCoreOther(string name_space)
        {
            this.name_space = name_space;
        }

        public string CreateOtherClass(string db_name)
        {
            StringBuilder dalContent = new StringBuilder();
            dalContent.Append(CreateResultInfo());

            return dalContent.ToString();
        }

        public string CreateFactory(string db_name)
        {
            return string.Format(@"using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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
}}
", name_space, db_name);
        }

        public string CreateResultInfo()
        {
            StringBuilder content = new StringBuilder(@"
        using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NAMESPACE
{
    /// <summary>
    /// 结果消息对象
    /// </summary>
    public class result_info<T> where T : class
    {
        public int code { get; set; }
        public string msg { get; set; }
        public T data { get; set; }

        public result_info()
        {
        }

        public result_info(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public result_info(int code, string msg, T data)
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }

        /// <summary>
        /// success + data
        /// </summary>
        public static result_info<T> Success(T data)
        {
            return new result_info<T>(0, ""success"", data);
        }

        /// <summary>
        /// success
        /// </summary>
        public static result_info<object> success
        {
            get
            {
                return new result_info<object>(0, ""success"");
            }
        }

        /// <summary>
        /// data null
        /// </summary>
        public static result_info<T> data_null
        {
            get
            {
                return new result_info<T>(1, ""data null"");
            }
        }

        /// <summary>
        /// fail
        /// </summary>
        public static result_info<T> fail
        {
            get
            {
                return new result_info<T>(2, ""fail"");
            }
        }
    }
}

".Replace("NAMESPACE", name_space));

            return content.ToString();
        }
    }
}
