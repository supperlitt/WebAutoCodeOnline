using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace WebAutoCodeOnline
{
    public class Tool
    {
        /// <summary>
        /// 用户IP限制集合
        /// </summary>
        public static Dictionary<string, DateTime> IpRequestData = new Dictionary<string, DateTime>();

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string conStr = "server=101.226.179.224;uid=sq_iwantdebug;pwd=mort199051;database=sq_iwantdebug;";

        /// <summary>
        /// 更新用户最新位置
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="imei"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string UpdateLocation(string phone, string pwd, string lat, string lng, string imei,string content)
        {
            string sqlCheckExist = "select count(1) from LocationInfo where Phone=@Phone and IMEI<>@IMEI";
            string sqlCheckAdd = "select count(1) from LocationInfo where Phone=@Phone and IMEI=@IMEI";
            using (SqlConnection sqlcn = new SqlConnection(conStr))
            {
                sqlcn.Open();
                SqlCommand sqlcm = new SqlCommand();
                sqlcm.Connection = sqlcn;
                sqlcm.CommandText = sqlCheckExist;
                sqlcm.CommandType = System.Data.CommandType.Text;
                sqlcm.Parameters.Add(new SqlParameter("@IMEI", SqlDbType.VarChar) { Value = imei });
                sqlcm.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar) { Value = phone });
                int count = Convert.ToInt32(sqlcm.ExecuteScalar());
                if (count > 0)
                {
                    return "1";
                }
                else
                {
                    sqlcm.CommandText = sqlCheckAdd;
                    count = Convert.ToInt32(sqlcm.ExecuteScalar());
                    if (count > 0)
                    {
                        // 存在就修改
                        string sqlUpdate = "update LocationInfo set Pwd=@Pwd,Lat=@Lat,Lng=@Lng,Content=@Content,UpdateTime=getdate() where IMEI=@IMEI and Phone=@Phone;";
                        sqlcm.CommandText = sqlUpdate;
                        sqlcm.Parameters.Add(new SqlParameter("@Pwd", SqlDbType.VarChar) { Value = pwd });
                        sqlcm.Parameters.Add(new SqlParameter("@Lat", SqlDbType.VarChar) { Value = lat });
                        sqlcm.Parameters.Add(new SqlParameter("@Lng", SqlDbType.VarChar) { Value = lng });
                        sqlcm.Parameters.Add(new SqlParameter("@Content", SqlDbType.Text) { Value= content });
                        sqlcm.ExecuteNonQuery();

                        return "2";
                    }
                    else
                    {
                        // 不存在，走添加
                        string sqlInsert = "insert into LocationInfo(IMEI,Phone,Pwd,Lat,Lng,Content,UpdateTime) values(@IMEI,@Phone,@Pwd,@Lat,@Lng,@Content,getdate());";
                        sqlcm.CommandText = sqlInsert;
                        sqlcm.Parameters.Add(new SqlParameter("@Pwd", SqlDbType.VarChar) { Value = pwd });
                        sqlcm.Parameters.Add(new SqlParameter("@Lat", SqlDbType.VarChar) { Value = lat });
                        sqlcm.Parameters.Add(new SqlParameter("@Lng", SqlDbType.VarChar) { Value = lng });
                        sqlcm.Parameters.Add(new SqlParameter("@Content", SqlDbType.Text) { Value = content });
                        sqlcm.ExecuteNonQuery();

                        return "3";
                    }
                }
            }
        }

        /// <summary>
        /// 查询用户位置
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string QueryLocation(string phone, string pwd)
        {
            string sqlSelect = "select Lat,Lng,Content from LocationInfo where Phone=@Phone and Pwd=@Pwd";
            using (SqlConnection sqlcn = new SqlConnection(conStr))
            {
                SqlCommand sqlcm = new SqlCommand();
                sqlcm.Connection = sqlcn;
                sqlcn.Open();
                sqlcm.CommandText = sqlSelect;
                sqlcm.CommandType = System.Data.CommandType.Text;
                sqlcm.Parameters.Add(new SqlParameter("@Pwd", SqlDbType.VarChar) { Value = pwd });
                sqlcm.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar) { Value = phone });
                using (SqlDataReader sqldr = sqlcm.ExecuteReader())
                {
                    if (sqldr.Read())
                    {
                        return string.Format("{0}#{1}#{2}", sqldr["Lat"].ToString(), sqldr["Lng"].ToString(), sqldr["Content"].ToString());
                    }

                    return string.Empty;
                }
            }
        }
    }
}