using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebAutoCodeOnline
{
    public class LeaveMsgDAL
    {
        public bool AddLeaveMsg(LeaveMsg model)
        {
            string insertSql = "insert into LeaveMsg(NickName, Email ,Msg ,IP ,LeaveTime, IsShow) values (@NickName, @Email ,@Msg ,@IP ,getdate(), 1)";
            List<SqlParameter> listParams = new List<SqlParameter>();
            listParams.Add(new SqlParameter("@NickName", SqlDbType.VarChar) { Value = model.NickName });
            listParams.Add(new SqlParameter("@Email", SqlDbType.VarChar) { Value = model.Email });
            listParams.Add(new SqlParameter("@Msg", SqlDbType.VarChar) { Value = model.Msg });
            listParams.Add(new SqlParameter("@IP", SqlDbType.VarChar) { Value = model.IP });

            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
            }
        }
        public List<LeaveMsg> QueryList(int page, int pageSize)
        {
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();

            string selectSql = string.Format(@"select * from
	        (select top 100 percent *,ROW_NUMBER() over(order by Id) as rownumber from
	        LeaveMsg where IsShow=1 {0}) as T
	        where rownumber between {1} and {2} order by LeaveTime desc;", whereStr, ((page - 1) * pageSize + 1), page * pageSize);

            List<LeaveMsg> result = new List<LeaveMsg>();
            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        LeaveMsg model = new LeaveMsg();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.NickName = sqldr["NickName"] == DBNull.Value ? string.Empty : sqldr["NickName"].ToString();
                        model.Email = sqldr["Email"] == DBNull.Value ? string.Empty : sqldr["Email"].ToString();
                        model.Msg = sqldr["Msg"] == DBNull.Value ? string.Empty : sqldr["Msg"].ToString();
                        model.IP = sqldr["IP"] == DBNull.Value ? string.Empty : sqldr["IP"].ToString();
                        model.LeaveTime = sqldr["LeaveTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["LeaveTime"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public int QueryListCount()
        {
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();

            string selectCountSql = string.Format("select count(0) from LeaveMsg where IsShow=1 {0}", whereStr);
            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(sqlcn, CommandType.Text, selectCountSql, listParams.ToArray()));
            }
        }
    }
}