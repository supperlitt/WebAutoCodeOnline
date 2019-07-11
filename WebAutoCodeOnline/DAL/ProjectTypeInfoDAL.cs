using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data.SqlClient;
using System.Data;

namespace WebAutoCodeOnline
{
    public class ProjectTypeInfoDAL
    {
        /// <summary>
        /// 获取所有集合类型
        /// </summary>
        /// <param name="containsDeleted">是否包含已经删除了的</param>
        /// <returns>返回项目类型集合</returns>
        public List<ProjectTypeInfo> GetAllProjectTypeList(bool containsDeleted)
        {
            List<ProjectTypeInfo> result = new List<ProjectTypeInfo>();
            string whereStr = string.Empty;
            if (!containsDeleted)
            {
                whereStr = " where IsDelete=0";
            }

            string selectAllProject = string.Format("select ProjectTypeId,ProjectTypeName,ProjectTypeDesc,DirName,Status,IsDelete from ProjectTypeInfo{0}", whereStr);
            using (SqlConnection sqlcn = ConnectionFactory.Location)
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, System.Data.CommandType.Text, selectAllProject))
                {
                    ProjectTypeInfo info = null;
                    while (sqldr.Read())
                    {
                        info = new ProjectTypeInfo();
                        info.ProjectTypeId = Convert.ToInt32(sqldr["ProjectTypeId"]);
                        info.ProjectTypeName = sqldr["ProjectTypeName"].ToString();
                        info.ProjectTypeDesc = sqldr["ProjectTypeDesc"].ToString();
                        info.DirName = sqldr["DirName"].ToString();
                        info.Status = Convert.ToInt32(sqldr["Status"]);
                        info.IsDelete = Convert.ToInt32(sqldr["IsDelete"]);

                        result.Add(info);
                    }
                }
            }

            return result;
        }

        public ProjectTypeInfo GetProjectTypeInfo(int proId, bool containsDeleted)
        {
            ProjectTypeInfo result = null;
            string whereStr = string.Empty;
            if (!containsDeleted)
            {
                whereStr = " and IsDelete=0";
            }

            string selectAllProject = string.Format("select ProjectTypeId,ProjectTypeName,ProjectTypeDesc,DirName,Status,IsDelete from ProjectTypeInfo where ProjectTypeId=@ProjectTypeId {0}", whereStr);
            List<SqlParameter> listParams = new List<SqlParameter>();
            listParams.Add(new SqlParameter("@ProjectTypeId", SqlDbType.Int) { Value = proId });
            using (SqlConnection sqlcn = ConnectionFactory.Location)
            {
                using (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, System.Data.CommandType.Text, selectAllProject, listParams.ToArray()))
                {
                    if (sqldr.Read())
                    {
                        result = new ProjectTypeInfo();
                        result.ProjectTypeId = Convert.ToInt32(sqldr["ProjectTypeId"]);
                        result.ProjectTypeName = sqldr["ProjectTypeName"].ToString();
                        result.ProjectTypeDesc = sqldr["ProjectTypeDesc"].ToString();
                        result.DirName = sqldr["DirName"].ToString();
                        result.Status = Convert.ToInt32(sqldr["Status"]);
                        result.IsDelete = Convert.ToInt32(sqldr["IsDelete"]);
                    }
                }
            }

            return result;
        }
    }
}
