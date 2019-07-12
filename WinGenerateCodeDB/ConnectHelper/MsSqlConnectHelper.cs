using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public class MsSqlConnectHelper : IConnect
    {
        public List<string> GetDbList(string server, string name, string pwd, int port = 3306)
        {
            string selectSql = "select * from master.dbo.SysDatabases";
            string mysqlConnectionStr = string.Format("server={0};uid={1};pwd={2};database=master;", server, name, pwd, port);
            List<string> result = new List<string>();
            using (SqlConnection sqlcn = new SqlConnection(mysqlConnectionStr))
            {
                SqlCommand sqlcm = new SqlCommand();
                sqlcm.Connection = sqlcn;
                sqlcm.CommandText = selectSql;
                sqlcm.CommandType = CommandType.Text;
                sqlcn.Open();
                using (SqlDataReader sqldr = sqlcm.ExecuteReader())
                {
                    while (sqldr.Read())
                    {
                        result.Add(sqldr["name"] == DBNull.Value ? "" : sqldr["name"].ToString());
                    }
                }
            }

            return result;
        }

        public List<string> GetTableList(string server, string name, string pwd, int port, string dbname)
        {
            string mysqlConnectionStr = string.Format("server={0};uid={1};pwd={2};database={4};", server, name, pwd, port, dbname);
            string selectSql = string.Format("SELECT name FROM SysObjects Where XType='U'", dbname);
            List<string> result = new List<string>();
            using (SqlConnection sqlcn = new SqlConnection(mysqlConnectionStr))
            {
                SqlCommand sqlcm = new SqlCommand();
                sqlcm.Connection = sqlcn;
                sqlcm.CommandText = selectSql;
                sqlcm.CommandType = CommandType.Text;
                sqlcn.Open();
                using (SqlDataReader sqldr = sqlcm.ExecuteReader())
                {
                    while (sqldr.Read())
                    {
                        string tablename = sqldr["name"].ToString();

                        result.Add(tablename);
                    }
                }
            }

            return result;
        }

        public List<SqlColumnInfo> GetColumnsList(string server, string name, string pwd, int port, string dbname, string tablename)
        {
            string mysqlConnectionStr = string.Format("server={0};uid={1};pwd={2};database={4};", server, name, pwd, port, dbname);
            string selectSql = string.Format(@"SELECT 
表名=case when a.colorder=1 then d.name else '' end, 
字段名=a.name, 
标识=case when COLUMNPROPERTY(a.id,a.name,'IsIdentity')=1 then '√' else '' end, 
主键=case when exists(SELECT 1 FROM sysobjects where xtype= 'PK' and name in ( 
SELECT name FROM sysindexes WHERE indid in( 
SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid 
))) then '√' else '' end, 
类型=b.name, 
占用字节数=a.length, 
长度=COLUMNPROPERTY(a.id,a.name, 'PRECISION'), 
小数位数=isnull(COLUMNPROPERTY(a.id,a.name, 'Scale'),0), 
允许空=case when a.isnullable=1 then '√'else '' end, 
默认值=isnull(e.text, ''), 
字段说明=isnull(g.[value], '') 
FROM syscolumns a 
left join systypes b on a.xtype=b.xusertype 
inner join sysobjects d on a.id=d.id and d.xtype= 'U' and d.name <> 'dtproperties' and d.name = '{0}'
left join syscomments e on a.cdefault=e.id 
left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id and g.name='MS_Description' 
order by a.id,a.colorder", tablename);
            List<SqlColumnInfo> result = new List<SqlColumnInfo>();
            using (SqlConnection sqlcn = new SqlConnection(mysqlConnectionStr))
            {
                SqlCommand sqlcm = new SqlCommand();
                sqlcm.Connection = sqlcn;
                sqlcm.CommandText = selectSql;
                sqlcm.CommandType = CommandType.Text;
                sqlcn.Open();
                using (SqlDataReader sqldr = sqlcm.ExecuteReader())
                {
                    while (sqldr.Read())
                    {
                        SqlColumnInfo info = new SqlColumnInfo();
                        info.Name = sqldr["字段名"] == DBNull.Value ? "" : sqldr["字段名"].ToString();
                        info.DbType = sqldr["类型"] == DBNull.Value ? "" : sqldr["类型"].ToString();
                        info.Length = sqldr["长度"] == DBNull.Value ? "" : sqldr["长度"].ToString();
                        info.IsMainKey = (sqldr["主键"] == DBNull.Value ? "" : sqldr["主键"].ToString()) == "√";
                        info.IsAllowNull = (sqldr["允许空"] == DBNull.Value ? "" : sqldr["允许空"].ToString()) == "√";
                        info.DefaultValue = sqldr["默认值"] == DBNull.Value ? "[null]" : sqldr["默认值"].ToString();
                        info.IsAutoIncrement = (sqldr["标识"] == DBNull.Value ? "" : sqldr["标识"].ToString()) == "√";
                        info.Comment = sqldr["字段说明"] == DBNull.Value ? "" : sqldr["字段说明"].ToString();

                        result.Add(info);
                    }
                }
            }

            return result;
        }
    }
}
