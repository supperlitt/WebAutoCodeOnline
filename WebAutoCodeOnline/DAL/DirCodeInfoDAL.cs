using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WebAutoCodeOnline
{
    /// <summary>
    /// 源码文件检查类
    /// </summary>
    public class DirCodeInfoDAL
    {
        public string GetDirName(int dirId, bool isNeedDeleted)
        {
            string andWhere = string.Empty;
            if (!isNeedDeleted)
            {
                andWhere = " and IsDelete=0";
            }

            string sql = "select DirName from ProjectTypeInfo where ProjectTypeId=@ProjectTypeId" + andWhere;
            List<SqlParameter> listParams = new List<SqlParameter>();
            listParams.Add(new SqlParameter("@ProjectTypeId", SqlDbType.Int) { Value = dirId });
            using (SqlConnection sqlcn = ConnectionFactory.AliDB)
            {
                object obj = SqlHelper.ExecuteScalar(sqlcn, CommandType.Text, sql, listParams.ToArray());
                if (obj == DBNull.Value || obj == null)
                {
                    return string.Empty;
                }
                else
                {
                    return obj.ToString();
                }
            }
        }
    }
}
