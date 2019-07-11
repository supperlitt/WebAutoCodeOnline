using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebAutoCodeOnline
{
    public class ArticleDAL
    {

        public bool AddArticle(Article model)
        {
            string insertSql = "insert Article(Title ,Content ,Tags ,GroupIds ,PublishTime ,LastChangeTime ,IsShow ,IsTop) values (@Title ,@Content ,@Tags ,@GroupIds ,@PublishTime ,@LastChangeTime ,@IsShow ,@IsTop)";
            List<SqlParameter> listParams = new List<SqlParameter>();
            listParams.Add(new SqlParameter("@Title", SqlDbType.VarChar) { Value = model.Title });
            listParams.Add(new SqlParameter("@Content", SqlDbType.Text) { Value = model.Content });
            listParams.Add(new SqlParameter("@Tags", SqlDbType.VarChar) { Value = model.Tags });
            listParams.Add(new SqlParameter("@GroupIds", SqlDbType.Int) { Value = model.GroupIds });
            listParams.Add(new SqlParameter("@PublishTime", SqlDbType.DateTime) { Value = model.PublishTime });
            listParams.Add(new SqlParameter("@LastChangeTime", SqlDbType.DateTime) { Value = model.LastChangeTime });
            listParams.Add(new SqlParameter("@IsShow", SqlDbType.TinyInt) { Value = model.IsShow });
            listParams.Add(new SqlParameter("@IsTop", SqlDbType.TinyInt) { Value = model.IsTop });

            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
            }
        }

        public bool UpdateArticle(Article model)
        {
            string updateSql = "update top(1) Article set  where  Id=@Id ";
            List<SqlParameter> listParams = new List<SqlParameter>();
            listParams.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = model.Id });

            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }
        }

        public bool BatUpdateArticle(List<string> list, Article model)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string updateSql = string.Format("update  Article set  where  Id in ({0})", idStr);
            List<SqlParameter> listParams = new List<SqlParameter>();
            listParams.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = model.Id });

            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }
        }

        public bool DeleteArticle(List<string> list)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string deleteSql = string.Format("delete from Article  where Id in ({0})", idStr);
            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, deleteSql, null) > 0;
            }
        }

        public List<Article> QueryList(string tags, int page, int pageSize)
        {
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(tags))
            {
                listParams.Add(new SqlParameter("@Tags", SqlDbType.VarChar) { Value = tags });
                whereStr += " and Tags=@Tags ";
            }


            string selectSql = string.Format(@"select * from
	        (select top 100 percent *,ROW_NUMBER() over(order by Id) as rownumber from
	        Article where 1=1 {0}) as T
	        where rownumber between {1} and {2};", whereStr, ((page - 1) * pageSize + 1), page * pageSize);

            List<Article> result = new List<Article>();
            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
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
            List<SqlParameter> listParams = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(tags))
            {
                listParams.Add(new SqlParameter("@Tags", SqlDbType.VarChar) { Value = tags });
                whereStr += " and Tags=@Tags ";
            }


            string selectCountSql = string.Format("select count(0) from Article where 1=1 {0}", whereStr);
            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(sqlcn, CommandType.Text, selectCountSql, listParams.ToArray()));
            }
        }

        public List<Article> GetPartAll(string tags, List<string> idList)
        {
            var idArrayStr = string.Join(",", (from f in idList
                                               select "'" + f + "'").ToArray());
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(tags))
            {
                listParams.Add(new SqlParameter("@Tags", SqlDbType.VarChar) { Value = tags });
                whereStr += " and Tags=@Tags ";
            }


            string selectSql = string.Format(@"select * from
            Article where Id in ({1})
            {0};", whereStr, idArrayStr);

            List<Article> result = new List<Article>();
            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
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
            List<SqlParameter> listParams = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(tags))
            {
                listParams.Add(new SqlParameter("@Tags", SqlDbType.VarChar) { Value = tags });
                whereStr += " and Tags=@Tags ";
            }

            string selectAllSql = string.Format("select * from Article where 1=1 {0}", whereStr);
            List<Article> result = new List<Article>();
            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectAllSql, listParams.ToArray()))
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