using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Test
{
    public class TestInfoDAL
    {
        public bool AddTestInfo(TestInfo model)
        {
            string insertSql = "insert TestInfo(TestName ,TestPwd ,TestMemory ,AddDate) values (@TestName ,@TestPwd ,@TestMemory ,@AddDate)";
            List<SqlParameter> listParams = new List<SqlParameter>();
            listParams.Add(new SqlParameter("@TestName", SqlDbType.VarChar) { Value = model.TestName });
            listParams.Add(new SqlParameter("@TestPwd", SqlDbType.VarChar) { Value = model.TestPwd });
            listParams.Add(new SqlParameter("@TestMemory", SqlDbType.Decimal) { Value = model.TestMemory });
            listParams.Add(new SqlParameter("@AddDate", SqlDbType.DateTime) { Value = model.AddDate });

            using (SqlConnection sqlcn = ConnectionFactory.TestDB)
            {
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
            }
        }

        public bool UpdateTestInfo(TestInfo model)
        {
            string updateSql = "update top(1) TestInfo set TestName=@TestName,TestPwd=@TestPwd where  TestId=@TestId ";
            List<SqlParameter> listParams = new List<SqlParameter>();
            listParams.Add(new SqlParameter("@TestName", SqlDbType.VarChar) { Value = model.TestName });
            listParams.Add(new SqlParameter("@TestPwd", SqlDbType.VarChar) { Value = model.TestPwd });
            listParams.Add(new SqlParameter("@TestId", SqlDbType.Int) { Value = model.TestId });

            using (SqlConnection sqlcn = ConnectionFactory.TestDB)
            {
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }
        }

        public bool BatUpdateTestInfo(List<string> list, TestInfo model)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string updateSql = string.Format("update  TestInfo set TestPwd=@TestPwd,TestMemory=@TestMemory where  TestId in ({0})", idStr);
            List<SqlParameter> listParams = new List<SqlParameter>();
            listParams.Add(new SqlParameter("@TestPwd", SqlDbType.VarChar) { Value = model.TestPwd });
            listParams.Add(new SqlParameter("@TestMemory", SqlDbType.Decimal) { Value = model.TestMemory });
            listParams.Add(new SqlParameter("@TestId", SqlDbType.Int) { Value = model.TestId });

            using (SqlConnection sqlcn = ConnectionFactory.TestDB)
            {
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }
        }

        public bool DeleteTestInfo(List<string> list)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string deleteSql = string.Format("delete from TestInfo  where TestId in ({0})", idStr);
            using (SqlConnection sqlcn = ConnectionFactory.TestDB)
            {
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, deleteSql, null) > 0;
            }
        }

        public List<TestInfo> QueryList(string testName, int page, int pageSize)
        {
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(testName))
            {
                listParams.Add(new SqlParameter("@TestName", SqlDbType.VarChar) { Value = testName });
                whereStr += " and TestName=@TestName ";
            }


            string selectSql = string.Format(@"select * from
	        (select top 100 percent *,ROW_NUMBER() over(order by TestId) as rownumber from
	        TestInfo where 1=1 {0}) as T
	        where rownumber between {1} and {2};", whereStr, ((page - 1) * pageSize + 1), page * pageSize);

            List<TestInfo> result = new List<TestInfo>();
            using (SqlConnection sqlcn = ConnectionFactory.TestDB)
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        TestInfo model = new TestInfo();
                        model.TestId = sqldr["TestId"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["TestId"]);
                        model.TestName = sqldr["TestName"] == DBNull.Value ? string.Empty : sqldr["TestName"].ToString();
                        model.TestPwd = sqldr["TestPwd"] == DBNull.Value ? string.Empty : sqldr["TestPwd"].ToString();
                        model.TestMemory = sqldr["TestMemory"] == DBNull.Value ? 0m : Convert.ToDecimal(sqldr["TestMemory"]);
                        model.AddDate = sqldr["AddDate"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["AddDate"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public int QueryListCount(string testName)
        {
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(testName))
            {
                listParams.Add(new SqlParameter("@TestName", SqlDbType.VarChar) { Value = testName });
                whereStr += " and TestName=@TestName ";
            }


            string selectCountSql = string.Format("select count(0) from TestInfo where 1=1 {0}", whereStr);
            using (SqlConnection sqlcn = ConnectionFactory.TestDB)
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(sqlcn, CommandType.Text, selectCountSql, listParams.ToArray()));
            }
        }

        public List<TestInfo> GetPartAll(string testName, List<string> idList)
        {
            var idArrayStr = string.Join(",", (from f in idList
                                               select "'" + f + "'").ToArray());
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(testName))
            {
                listParams.Add(new SqlParameter("@TestName", SqlDbType.VarChar) { Value = testName });
                whereStr += " and TestName=@TestName ";
            }


            string selectSql = string.Format(@"select * from
            TestInfo where TestId in ({1})
            {0};", whereStr, idArrayStr);

            List<TestInfo> result = new List<TestInfo>();
            using (SqlConnection sqlcn = ConnectionFactory.TestDB)
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        TestInfo model = new TestInfo();
                        model.TestId = sqldr["TestId"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["TestId"]);
                        model.TestName = sqldr["TestName"] == DBNull.Value ? string.Empty : sqldr["TestName"].ToString();
                        model.TestPwd = sqldr["TestPwd"] == DBNull.Value ? string.Empty : sqldr["TestPwd"].ToString();
                        model.TestMemory = sqldr["TestMemory"] == DBNull.Value ? 0m : Convert.ToDecimal(sqldr["TestMemory"]);
                        model.AddDate = sqldr["AddDate"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["AddDate"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public List<TestInfo> GetAll(string testName)
        {
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(testName))
            {
                listParams.Add(new SqlParameter("@TestName", SqlDbType.VarChar) { Value = testName });
                whereStr += " and TestName=@TestName ";
            }


            string selectAllSql = string.Format("select * from TestInfo where 1=1 {0}", whereStr);
            List<TestInfo> result = new List<TestInfo>();
            using (SqlConnection sqlcn = ConnectionFactory.TestDB)
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectAllSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        TestInfo model = new TestInfo();
                        model.TestId = sqldr["TestId"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["TestId"]);
                        model.TestName = sqldr["TestName"] == DBNull.Value ? string.Empty : sqldr["TestName"].ToString();
                        model.TestPwd = sqldr["TestPwd"] == DBNull.Value ? string.Empty : sqldr["TestPwd"].ToString();
                        model.TestMemory = sqldr["TestMemory"] == DBNull.Value ? 0m : Convert.ToDecimal(sqldr["TestMemory"]);
                        model.AddDate = sqldr["AddDate"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["AddDate"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

    }
}