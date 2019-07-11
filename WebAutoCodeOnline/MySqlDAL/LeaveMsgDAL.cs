using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebAutoCodeOnline.MySqlDAL
{
    public class LeaveMsgDAL
    {
        public bool AddLeaveMsg(LeaveMsg model)
        {
            string insertSql = "insert into LeaveMsg(NickName, Email ,Msg ,IP ,LeaveTime, IsShow) values (@NickName, @Email ,@Msg ,@IP ,now(), 1)";
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@NickName", MySqlDbType.VarChar) { Value = model.NickName });
            listParams.Add(new MySqlParameter("@Email", MySqlDbType.VarChar) { Value = model.Email });
            listParams.Add(new MySqlParameter("@Msg", MySqlDbType.VarChar) { Value = model.Msg });
            listParams.Add(new MySqlParameter("@IP", MySqlDbType.VarChar) { Value = model.IP });

            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
            }
        }
        public List<LeaveMsg> QueryList(int page, int pageSize)
        {
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            string selectSql = string.Format(@"select * from LeaveMsg
	         where IsShow=1 order by LeaveTime desc limit {0},{1};", ((page - 1) * pageSize), pageSize);

            List<LeaveMsg> result = new List<LeaveMsg>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
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
            List<MySqlParameter> listParams = new List<MySqlParameter>();

            string selectCountSql = string.Format("select count(0) from LeaveMsg where IsShow=1 {0}", whereStr);
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return Convert.ToInt32(MySqlHelper2.ExecuteScalar(sqlcn, CommandType.Text, selectCountSql, listParams.ToArray()));
            }
        }
    }
}