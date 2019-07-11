using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public class MySqlConnectHelper : IConnect
    {
        public List<string> GetDbList(string server, string name, string pwd, int port = 3306)
        {
            string selectSql = "show databases";
            string mysqlConnectionStr = string.Format("server={0};uid={1};pwd={2};database=mysql;port={3};", server, name, pwd, port);
            List<string> result = new List<string>();
            using (MySqlConnection sqlcn = new MySqlConnection(mysqlConnectionStr))
            {
                using (MySqlDataReader mysqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql))
                {
                    while (mysqldr.Read())
                    {
                        result.Add(mysqldr["Database"] == DBNull.Value ? "" : mysqldr["Database"].ToString());
                    }
                }
            }

            return result;
        }

        public List<string> GetTableList(string server, string name, string pwd, int port, string dbname)
        {
            string mysqlConnectionStr = string.Format("server={0};uid={1};pwd={2};database={4};port={3};", server, name, pwd, port, dbname);
            string selectSql = string.Format("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}'", dbname);
            List<string> result = new List<string>();
            using (MySqlConnection sqlcn = new MySqlConnection(mysqlConnectionStr))
            {
                using (MySqlDataReader mysqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql))
                {
                    while (mysqldr.Read())
                    {
                        string tablename = mysqldr["TABLE_NAME"].ToString();

                        result.Add(tablename);
                    }
                }
            }

            return result;
        }

        public List<SqlColumnInfo> GetColumnsList(string server, string name, string pwd, int port, string dbname, string tablename)
        {
            string mysqlConnectionStr = string.Format("server={0};uid={1};pwd={2};database={4};port={3};", server, name, pwd, port, dbname);
            string selectSql = string.Format("select COLUMN_NAME,COLUMN_DEFAULT,IS_NULLABLE,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,COLUMN_KEY,EXTRA,COLUMN_COMMENT from information_schema.columns where TABLE_SCHEMA='{1}' and table_name='{0}'", tablename, dbname);
            List<SqlColumnInfo> result = new List<SqlColumnInfo>();
            using (MySqlConnection sqlcn = new MySqlConnection(mysqlConnectionStr))
            {
                using (MySqlDataReader mysqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql))
                {
                    while (mysqldr.Read())
                    {
                        SqlColumnInfo info = new SqlColumnInfo();
                        info.Name = mysqldr["COLUMN_NAME"] == DBNull.Value ? "" : mysqldr["COLUMN_NAME"].ToString();
                        info.DbType = mysqldr["DATA_TYPE"] == DBNull.Value ? "" : mysqldr["DATA_TYPE"].ToString();
                        info.Length = mysqldr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? "" : mysqldr["CHARACTER_MAXIMUM_LENGTH"].ToString();
                        info.IsMainKey = (mysqldr["COLUMN_KEY"] == DBNull.Value ? "" : mysqldr["COLUMN_KEY"].ToString()) == "PRI";
                        info.IsAllowNull = (mysqldr["IS_NULLABLE"] == DBNull.Value ? "" : mysqldr["IS_NULLABLE"].ToString()) == "YES";
                        info.DefaultValue = mysqldr["COLUMN_DEFAULT"] == DBNull.Value ? "[null]" : mysqldr["COLUMN_DEFAULT"].ToString();
                        info.IsAutoIncrement = (mysqldr["EXTRA"] == DBNull.Value ? "" : mysqldr["EXTRA"].ToString()) == "auto_increment";
                        info.Comment = mysqldr["COLUMN_COMMENT"] == DBNull.Value ? "" : mysqldr["COLUMN_COMMENT"].ToString();

                        result.Add(info);
                    }
                }
            }

            return result;
        }
    }
}
