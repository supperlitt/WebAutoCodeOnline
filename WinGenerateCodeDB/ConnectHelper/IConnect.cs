using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public interface IConnect
    {
        List<string> GetDbList(string server, string name, string pwd, int port);

        List<string> GetTableList(string server, string name, string pwd, int port, string dbname);

        List<SqlColumnInfo> GetColumnsList(string server, string name, string pwd, int port, string dbname, string tablename);
    }
}
