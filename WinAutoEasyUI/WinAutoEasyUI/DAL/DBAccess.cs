using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAutoEasyUI
{
    public class DBAccess
    {
        public static List<string> GetALLDB(string connectionStr)
        {
            List<string> result = new List<string>();
            string selectSql = "select name from master..sysdatabases";
            using (SqlConnection sqlcn = new SqlConnection(connectionStr))
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql))
                {
                    while (sqldr.Read())
                    {
                        result.Add(sqldr["name"].ToString());
                    }
                }
            }

            return result;
        }

        public static List<string> GetAllTables(string connectionStr)
        {
            List<string> result = new List<string>();
            string selectSql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            using (SqlConnection sqlcn = new SqlConnection(connectionStr))
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql))
                {
                    while (sqldr.Read())
                    {
                        result.Add(sqldr["TABLE_NAME"].ToString());
                    }
                }
            }

            return result;
        }

        public static List<TableInfo> GetAllTableInfo(string tableName, string connectionStr)
        {
            string selectSql = string.Format(@"SELECT a.name 字段名,COLUMNPROPERTY(a.id,a.name,'IsIdentity') 标识,
(case when (SELECT count(*) FROM sysobjects WHERE (name in
(SELECT name
FROM sysindexes
WHERE (id = a.id) AND (indid in
(SELECT indid
FROM sysindexkeys
WHERE (id = a.id) AND (colid in
(SELECT colid
FROM syscolumns
WHERE (id = a.id) AND (name = a.name))))))) AND
(xtype = 'PK'))>0 then '1' else '0' end) 主键,
b.name 类型,
COLUMNPROPERTY(a.id,a.name,'PRECISION') as 长度,
isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as 小数位数,
(case when a.isnullable=1 then '1'else '0' end) 允许空,
isnull(g.[value],'') AS 字段说明
FROM  syscolumns  a 
left join systypes b on  a.xtype=b.xusertype
inner join sysobjects d on a.id=d.id  and  d.xtype='U' and  d.name<>'dtproperties'
left join syscomments e on a.cdefault=e.id
left join sys.extended_properties g on a.id=g.major_id AND a.colid = g.minor_id  
where d.name = '{0}'
order by a.id,a.colorder", tableName);

            List<TableInfo> list = new List<TableInfo>();
            using (SqlConnection sqlcn = new SqlConnection(connectionStr))
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql))
                {
                    TableInfo tableInfo = null;
                    while (sqldr.Read())
                    {
                        tableInfo = new TableInfo();
                        tableInfo.ColumnName = sqldr["字段名"].ToString();
                        tableInfo.IsMark = Convert.ToInt32(sqldr["标识"]);
                        tableInfo.IsMainKey = Convert.ToInt32(sqldr["主键"]);
                        tableInfo.DBType = sqldr["类型"].ToString();
                        tableInfo.Length = Convert.ToInt32(sqldr["长度"]);
                        tableInfo.DecimalLength = Convert.ToInt32(sqldr["小数位数"]);
                        tableInfo.IsAllowNull = Convert.ToInt32(sqldr["允许空"]);
                        tableInfo.Comment = sqldr["字段说明"].ToString();

                        list.Add(tableInfo);
                    }
                }
            }

            return list;
        }
    }

    public class TableInfo
    {
        public string ColumnName { get; set; }

        public int IsMark { get; set; }

        public int IsMainKey { get; set; }

        public string DBType { get; set; }

        public int Length { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public int DecimalLength { get; set; }

        public int IsAllowNull { get; set; }

        public string Comment { get; set; }
    }
}
