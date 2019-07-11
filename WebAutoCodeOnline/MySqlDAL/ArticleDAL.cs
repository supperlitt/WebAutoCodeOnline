using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebAutoCodeOnline.MySqlDAL
{
    public class ArticleDAL
    {

        public bool AddArticle(Article model)
        {
            string insertSql = "insert Article(Title ,Content ,Tags ,GroupIds ,PublishTime ,LastChangeTime ,IsShow ,IsTop) values (@Title ,@Content ,@Tags ,@GroupIds ,@PublishTime ,@LastChangeTime ,@IsShow ,@IsTop)";
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@Title", MySqlDbType.VarChar) { Value = model.Title });
            listParams.Add(new MySqlParameter("@Content", MySqlDbType.Text) { Value = model.Content });
            listParams.Add(new MySqlParameter("@Tags", MySqlDbType.VarChar) { Value = model.Tags });
            listParams.Add(new MySqlParameter("@GroupIds", MySqlDbType.Int32) { Value = model.GroupIds });
            listParams.Add(new MySqlParameter("@PublishTime", MySqlDbType.DateTime) { Value = model.PublishTime });
            listParams.Add(new MySqlParameter("@LastChangeTime", MySqlDbType.DateTime) { Value = model.LastChangeTime });
            listParams.Add(new MySqlParameter("@IsShow", MySqlDbType.Int32) { Value = model.IsShow });
            listParams.Add(new MySqlParameter("@IsTop", MySqlDbType.Int32) { Value = model.IsTop });

            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
            }
        }

        public bool UpdateArticle(Article model)
        {
            string updateSql = "update top(1) Article set  where  Id=@Id ";
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@Id", MySqlDbType.Int32) { Value = model.Id });

            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }
        }

        public bool BatUpdateArticle(List<string> list, Article model)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string updateSql = string.Format("update  Article set  where  Id in ({0})", idStr);
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@Id", MySqlDbType.Int32) { Value = model.Id });

            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }
        }

        public bool DeleteArticle(List<string> list)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string deleteSql = string.Format("delete from Article  where Id in ({0})", idStr);
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, deleteSql, null) > 0;
            }
        }

        public List<Article> QueryList(string tags, int page, int pageSize)
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(tags))
            {
                listParams.Add(new MySqlParameter("@Tags", MySqlDbType.VarChar) { Value = tags });
                whereStr += " and Tags=@Tags ";
            }


            string selectSql = string.Format(@"select * from
	        (select top 100 percent *,ROW_NUMBER() over(order by Id) as rownumber from
	        Article where 1=1 {0}) as T
	        where rownumber between {1} and {2};", whereStr, ((page - 1) * pageSize + 1), page * pageSize);

            List<Article> result = new List<Article>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        Article model = new Article();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.Title = sqldr["Title"] == DBNull.Value ? string.Empty : sqldr["Title"].ToString();
                        model.Content = sqldr["Content"] == DBNull.Value ? string.Empty : sqldr["Content"].ToString();
                        model.Tags = sqldr["Tags"] == DBNull.Value ? string.Empty : sqldr["Tags"].ToString();
                        model.GroupIds = sqldr["GroupIds"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["GroupIds"]);
                        model.PublishTime = sqldr["PublishTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["PublishTime"]);
                        model.LastChangeTime = sqldr["LastChangeTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["LastChangeTime"]);
                        model.IsShow = sqldr["IsShow"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["IsShow"]);
                        model.IsTop = sqldr["IsTop"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["IsTop"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public int QueryListCount(string tags)
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(tags))
            {
                listParams.Add(new MySqlParameter("@Tags", MySqlDbType.VarChar) { Value = tags });
                whereStr += " and Tags=@Tags ";
            }


            string selectCountSql = string.Format("select count(0) from Article where 1=1 {0}", whereStr);
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return Convert.ToInt32(MySqlHelper2.ExecuteScalar(sqlcn, CommandType.Text, selectCountSql, listParams.ToArray()));
            }
        }

        public List<Article> GetPartAll(string tags, List<string> idList)
        {
            var idArrayStr = string.Join(",", (from f in idList
                                               select "'" + f + "'").ToArray());
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(tags))
            {
                listParams.Add(new MySqlParameter("@Tags", MySqlDbType.VarChar) { Value = tags });
                whereStr += " and Tags=@Tags ";
            }


            string selectSql = string.Format(@"select * from
            Article where Id in ({1})
            {0};", whereStr, idArrayStr);

            List<Article> result = new List<Article>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        Article model = new Article();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.Title = sqldr["Title"] == DBNull.Value ? string.Empty : sqldr["Title"].ToString();
                        model.Content = sqldr["Content"] == DBNull.Value ? string.Empty : sqldr["Content"].ToString();
                        model.Tags = sqldr["Tags"] == DBNull.Value ? string.Empty : sqldr["Tags"].ToString();
                        model.GroupIds = sqldr["GroupIds"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["GroupIds"]);
                        model.PublishTime = sqldr["PublishTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["PublishTime"]);
                        model.LastChangeTime = sqldr["LastChangeTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["LastChangeTime"]);
                        model.IsShow = sqldr["IsShow"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["IsShow"]);
                        model.IsTop = sqldr["IsTop"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["IsTop"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public List<Article> GetAll(string tags)
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(tags))
            {
                listParams.Add(new MySqlParameter("@Tags", MySqlDbType.VarChar) { Value = tags });
                whereStr += " and Tags=@Tags ";
            }

            string selectAllSql = string.Format("select * from Article where 1=1 {0}", whereStr);
            List<Article> result = new List<Article>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectAllSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        Article model = new Article();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.Title = sqldr["Title"] == DBNull.Value ? string.Empty : sqldr["Title"].ToString();
                        model.Content = sqldr["Content"] == DBNull.Value ? string.Empty : sqldr["Content"].ToString();
                        model.Tags = sqldr["Tags"] == DBNull.Value ? string.Empty : sqldr["Tags"].ToString();
                        model.GroupIds = sqldr["GroupIds"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["GroupIds"]);
                        model.PublishTime = sqldr["PublishTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["PublishTime"]);
                        model.LastChangeTime = sqldr["LastChangeTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["LastChangeTime"]);
                        model.IsShow = sqldr["IsShow"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["IsShow"]);
                        model.IsTop = sqldr["IsTop"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["IsTop"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

    }
}