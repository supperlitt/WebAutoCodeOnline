using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebAutoCodeOnline.MySqlDAL
{
    public class UserInfoDAL
    {
        public bool AddUserInfo(UserInfo model)
        {
            string insertSql = "insert UserInfo(UserName ,UserPwd ,LastLoginTime) values (@UserName ,@UserPwd ,@LastLoginTime)";
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@UserName", MySqlDbType.VarChar) { Value = model.UserName });
            listParams.Add(new MySqlParameter("@UserPwd", MySqlDbType.VarChar) { Value = model.UserPwd });
            listParams.Add(new MySqlParameter("@LastLoginTime", MySqlDbType.DateTime) { Value = model.LastLoginTime });

            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
            }
        }

        public bool BatUpdateUserInfo(List<string> list, UserInfo model)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string updateSql = string.Format("update  UserInfo set  where  Id in ({0})", idStr);
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@Id", MySqlDbType.Int32) { Value = model.Id });

            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }
        }

        public bool DeleteUserInfo(List<string> list)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string deleteSql = string.Format("delete from UserInfo  where Id in ({0})", idStr);
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, deleteSql, null) > 0;
            }
        }

        public List<UserInfo> QueryList(string userName, string userPwd, int page, int pageSize)
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(userName))
            {
                listParams.Add(new MySqlParameter("@UserName", MySqlDbType.VarChar) { Value = userName });
                whereStr += " and UserName=@UserName ";
            }

            if (!string.IsNullOrEmpty(userPwd))
            {
                listParams.Add(new MySqlParameter("@UserPwd", MySqlDbType.VarChar) { Value = userPwd });
                whereStr += " and UserPwd=@UserPwd ";
            }


            string selectSql = string.Format(@"select * from
	        (select top 100 percent *,ROW_NUMBER() over(order by Id) as rownumber from
	        UserInfo where 1=1 {0}) as T
	        where rownumber between {1} and {2};", whereStr, ((page - 1) * pageSize + 1), page * pageSize);

            List<UserInfo> result = new List<UserInfo>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        UserInfo model = new UserInfo();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.UserName = sqldr["UserName"] == DBNull.Value ? string.Empty : sqldr["UserName"].ToString();
                        model.UserPwd = sqldr["UserPwd"] == DBNull.Value ? string.Empty : sqldr["UserPwd"].ToString();
                        model.LastLoginTime = sqldr["LastLoginTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["LastLoginTime"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public int QueryListCount(string userName, string userPwd)
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(userName))
            {
                listParams.Add(new MySqlParameter("@UserName", MySqlDbType.VarChar) { Value = userName });
                whereStr += " and UserName=@UserName ";
            }

            if (!string.IsNullOrEmpty(userPwd))
            {
                listParams.Add(new MySqlParameter("@UserPwd", MySqlDbType.VarChar) { Value = userPwd });
                whereStr += " and UserPwd=@UserPwd ";
            }


            string selectCountSql = string.Format("select count(0) from UserInfo where 1=1 {0}", whereStr);
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return Convert.ToInt32(MySqlHelper2.ExecuteScalar(sqlcn, CommandType.Text, selectCountSql, listParams.ToArray()));
            }
        }

        public List<UserInfo> GetPartAll(string userName, string userPwd, List<string> idList)
        {
            var idArrayStr = string.Join(",", (from f in idList
                                               select "'" + f + "'").ToArray());
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(userName))
            {
                listParams.Add(new MySqlParameter("@UserName", MySqlDbType.VarChar) { Value = userName });
                whereStr += " and UserName=@UserName ";
            }

            if (!string.IsNullOrEmpty(userPwd))
            {
                listParams.Add(new MySqlParameter("@UserPwd", MySqlDbType.VarChar) { Value = userPwd });
                whereStr += " and UserPwd=@UserPwd ";
            }


            string selectSql = string.Format(@"select * from
            UserInfo where Id in ({1})
            {0};", whereStr, idArrayStr);

            List<UserInfo> result = new List<UserInfo>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        UserInfo model = new UserInfo();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.UserName = sqldr["UserName"] == DBNull.Value ? string.Empty : sqldr["UserName"].ToString();
                        model.UserPwd = sqldr["UserPwd"] == DBNull.Value ? string.Empty : sqldr["UserPwd"].ToString();
                        model.LastLoginTime = sqldr["LastLoginTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["LastLoginTime"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public List<UserInfo> GetAll(string userName, string userPwd)
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(userName))
            {
                listParams.Add(new MySqlParameter("@UserName", MySqlDbType.VarChar) { Value = userName });
                whereStr += " and UserName=@UserName ";
            }

            if (!string.IsNullOrEmpty(userPwd))
            {
                listParams.Add(new MySqlParameter("@UserPwd", MySqlDbType.VarChar) { Value = userPwd });
                whereStr += " and UserPwd=@UserPwd ";
            }


            string selectAllSql = string.Format("select * from UserInfo where 1=1 {0}", whereStr);
            List<UserInfo> result = new List<UserInfo>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectAllSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        UserInfo model = new UserInfo();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.UserName = sqldr["UserName"] == DBNull.Value ? string.Empty : sqldr["UserName"].ToString();
                        model.UserPwd = sqldr["UserPwd"] == DBNull.Value ? string.Empty : sqldr["UserPwd"].ToString();
                        model.LastLoginTime = sqldr["LastLoginTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["LastLoginTime"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public UserInfo GetUserInfo(string userName, string userPwd)
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@UserName", MySqlDbType.VarChar) { Value = userName });
            whereStr += " and UserName=@UserName ";

            listParams.Add(new MySqlParameter("@UserPwd", MySqlDbType.VarChar) { Value = userPwd });
            whereStr += " and UserPwd=@UserPwd ";

            string selectAllSql = string.Format("select * from UserInfo where 1=1 {0}", whereStr);
            UserInfo model = null;
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectAllSql, listParams.ToArray()))
                {
                    if (sqldr.Read())
                    {
                        model = new UserInfo();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.UserName = sqldr["UserName"] == DBNull.Value ? string.Empty : sqldr["UserName"].ToString();
                        model.LastLoginTime = sqldr["LastLoginTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["LastLoginTime"]);
                    }
                }
            }

            return model;
        }
    }
}