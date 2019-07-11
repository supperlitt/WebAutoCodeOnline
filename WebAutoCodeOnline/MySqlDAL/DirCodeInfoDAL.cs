using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace WebAutoCodeOnline.MySqlDAL
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
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@ProjectTypeId", MySqlDbType.Int32) { Value = dirId });
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                object obj = MySqlHelper2.ExecuteScalar(sqlcn, CommandType.Text, sql, listParams.ToArray());
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
