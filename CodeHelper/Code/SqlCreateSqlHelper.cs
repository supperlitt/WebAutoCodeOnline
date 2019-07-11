using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using Model;

namespace CodeHelper
{
    public class SqlCreateSqlHelper
    {
        private NormalModel tempModel = null;

        public SqlCreateSqlHelper(NormalModel model)
        {
            this.tempModel = model;
        }

        public string CreateInsertMethod()
        {
            StringBuilder content = new StringBuilder();
            content.AppendFormat("public bool Add_{0}({0} model)\r\n", tempModel.TableName.ToFirstUpper());
            content.Append("{\r\n");
            content.Append("\tstring insertSql = ");
            int index = 0;
            foreach (var item in tempModel.ColumnList)
            {
                if (!item.IsAutoIncrement)
                {
                    if (index == 0)
                    {
                        content.AppendFormat("\"insert {0}({1}", tempModel.TableName, item.ColumnName);
                    }
                    else
                    {
                        content.AppendFormat(" ,{0}", item.ColumnName);
                    }

                    index++;
                }
            }

            content.Append(") values (");

            index = 0;
            foreach (var item in tempModel.ColumnList)
            {
                if (!item.IsAutoIncrement)
                {
                    if (index == 0)
                    {
                        content.AppendFormat("@{0}", item.ColumnName.ToFirstUpper());
                    }
                    else
                    {
                        content.AppendFormat(" ,@{0}", item.ColumnName.ToFirstUpper());
                    }

                    index++;
                }
            }

            content.Append(");\";\r\n");
            content.Append("\tList<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            foreach (var item in tempModel.ColumnList)
            {
                if (!item.IsAutoIncrement)
                {
                    content.AppendFormat("\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", item.ColumnName.ToFirstUpper(), item.DBType.ToMsSqlDbType());
                }
            }
            content.AppendLine();
            content.Append("\t" + GetDBConnection(tempModel.DbName) + "\r\n");
            content.Append("\t{\r\n");
            content.Append("\t\treturn SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;\r\n");
            content.Append("\t}\r\n");
            content.Append("}\r\n");

            return content.ToString();
        }

        public string CreateDeleteMethod()
        {
            ColumnInfo keyColumn = tempModel.ColumnList[0];
            StringBuilder content = new StringBuilder();
            content.AppendFormat("public bool Delete_{0}({1} {2})\r\n", tempModel.TableName.ToFirstUpper(), keyColumn.DBType.ToMsSqlClassType(), keyColumn.ColumnName.ToFirstLower());
            content.Append("{\r\n");
            content.AppendFormat("\tstring deleteSql = \"delete from where {0}=@{1}\";\r\n", keyColumn.ColumnName, keyColumn.ColumnName.ToFirstUpper());
            content.Append("\tList<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            content.AppendFormat("\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", keyColumn.ColumnName.ToFirstUpper(), keyColumn.DBType.ToMsSqlDbType(), keyColumn.ColumnName.ToFirstLower());

            content.AppendLine();
            content.Append("\t" + GetDBConnection(tempModel.DbName) + "\r\n");
            content.Append("\t{\r\n");
            content.Append("\t\treturn SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, deleteSql, listParams.ToArray()) > 0;\r\n");
            content.Append("\t}\r\n");
            content.Append("}\r\n");

            return content.ToString();
        }

        public string CreateBatDeleteMethod()
        {
            ColumnInfo keyColumn = tempModel.ColumnList[0];
            StringBuilder content = new StringBuilder();
            content.AppendFormat("public void DeleteAll_{0}(List<{1}> {2}List)\r\n", tempModel.TableName.ToFirstUpper(), keyColumn.DBType.ToMsSqlClassType(), keyColumn.ColumnName.ToFirstLower());
            content.Append("{\r\n");
            content.AppendFormat("\tstring deleteSql = \"delete from where {0}=@{1}\";\r\n", keyColumn.ColumnName, keyColumn.ColumnName.ToFirstUpper());
            content.Append("\tList<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            content.AppendFormat("\tlistParams.Add(new SqlParameter(\"@{0}\", {1}));\r\n", keyColumn.ColumnName.ToFirstUpper(), keyColumn.DBType.ToMsSqlDbType());

            content.AppendLine();
            content.Append("\t" + GetDBConnection(tempModel.DbName) + "\r\n");
            content.Append("\t{\r\n");
            content.Append("\t\tsqlcn.Open();\r\n");
            content.Append("\t\tforeach(var item in " + keyColumn.ColumnName.ToFirstLower() + "List)\r\n");
            content.Append("\t\t{\r\n");
            content.Append("\t\t\tlistParams[0].Value = item;\r\n");
            content.Append("\t\t\tSqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, deleteSql, listParams.ToArray());\r\n");
            content.Append("\t\t}\r\n");
            content.Append("\t}\r\n");
            content.Append("}\r\n");

            return content.ToString();
        }

        public string CreateUpdateMethod()
        {
            StringBuilder content = new StringBuilder();
            content.AppendFormat("public bool Update_{0}({0} model)\r\n", tempModel.TableName.ToFirstUpper());
            content.Append("{\r\n");
            content.AppendFormat("\tstring updateSql = \"update {0} set ", tempModel.TableName);
            int index = 0;
            string whereStr = string.Empty;
            foreach (var item in tempModel.ColumnList)
            {
                if (index == 0)
                {
                    whereStr = string.Format(" where {0}=@{1};\";\r\n", item.ColumnName, item.ColumnName.ToFirstUpper());
                }
                else
                {
                    content.AppendFormat("{0}=@{1},", item.ColumnName, item.ColumnName.ToFirstUpper());
                }

                index++;
            }

            if (content.ToString().EndsWith(","))
            {
                content = new StringBuilder(content.ToString().Substring(0, content.ToString().Length - 1));
            }

            content.Append(whereStr);
            content.Append("\tList<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            foreach (var item in tempModel.ColumnList)
            {
                content.AppendFormat("\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", item.ColumnName.ToFirstUpper(), item.DBType.ToMsSqlDbType());
            }
            content.AppendLine();
            content.Append("\t" + GetDBConnection(tempModel.DbName) + "\r\n");
            content.Append("\t{\r\n");
            content.Append("\t\treturn SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;\r\n");
            content.Append("\t}\r\n");
            content.Append("}\r\n");

            return content.ToString();
        }

        public string CreateSelectByPageAndSize()
        {
            StringBuilder content = new StringBuilder();
            content.AppendFormat("public List<{0}> QueryList(", tempModel.TableName.ToFirstUpper());

            // 添加条件
            foreach (var col in tempModel.SearchColumns)
            {
                string typeStr = col.DBType.ToMsSqlClassType();
                string arg = col.ColumnName.ToFirstLower();
                content.AppendFormat("{0} {1}, ", typeStr, arg);
            }

            content.Append("int page, int pageSize)\r\n");
            content.Append("{\r\n");

            content.AppendFormat("\tstring whereStr = string.Empty;\r\n");
            content.AppendFormat("\tList<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            foreach (var col in tempModel.SearchColumns)
            {
                string typeStr = col.DBType.ToMsSqlClassType();
                string arg = col.ColumnName.ToFirstLower();

                if (typeStr == "string")
                {
                    content.AppendFormat("\tif (!string.IsNullOrEmpty({0}))\r\n", arg);
                }
                else if (typeStr == "int" || typeStr == "long")
                {
                    content.AppendFormat("\tif ({0} >= 0)\r\n", arg);
                }
                else
                {
                    content.AppendFormat("\tif(true)\r\n");
                }

                content.Append("\t{\r\n");
                content.AppendFormat("\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", col.ColumnName.ToFirstUpper(), col.DBType.ToMsSqlDbType(), arg);
                content.AppendFormat("\t\twhereStr += \" and {0}=@{1} \";\r\n", col.ColumnName, col.ColumnName.ToFirstUpper());
                content.Append("\t}\r\n");
                content.AppendLine();
            }

            content.AppendFormat("\tList<{0}> result = new List<{0}>();\r\n", tempModel.TableName.ToFirstUpper());
            content.Append("\tstring selectSql = string.Format(@\"select * from\r\n");
            content.AppendFormat("\t        (select top 100 percent *,ROW_NUMBER() over(order by {0}) as rownumber from\r\n", tempModel.ColumnList.Find(p => p.IsMainKey).ColumnName);
            content.Append("\t        " + tempModel.TableName + " where 1=1 {0}) as T\r\n");
            content.Append("\t        where rownumber between {1} and {2};\", whereStr, ((page - 1) * pageSize + 1), page * pageSize);\r\n");
            content.Append("\t" + GetDBConnection(tempModel.DbName) + "\r\n");
            content.Append("\t{\r\n");
            content.Append("\t\tusing (SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))\r\n");
            content.Append("\t\t{\r\n");
            content.AppendFormat("\t\t\t{0} model = null;\r\n", tempModel.TableName.ToFirstUpper());
            content.Append("\t\t\twhile (sqldr.Read())\r\n");
            content.Append("\t\t\t{\r\n");
            content.AppendFormat("\t\t\t\tmodel = new {0}();\r\n", tempModel.TableName.ToFirstUpper());
            foreach (var item in tempModel.ColumnList)
            {
                string typeStr = item.DBType;
                if (typeStr == "int" || typeStr == "tinyint")
                {
                    content.AppendFormat("\t\t\t\tmodel.{0} = sqldr[\"{1}\"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr[\"{1}\"]);\r\n", item.ColumnName.ToFirstUpper(), item.ColumnName);
                }
                else if (typeStr == "varchar" || typeStr == "nvarchar" || typeStr == "text" || typeStr == "char")
                {
                    content.AppendFormat("\t\t\t\tmodel.{0} = sqldr[\"{1}\"] == DBNull.Value ? string.Empty : sqldr[\"{1}\"].ToString();\r\n", item.ColumnName.ToFirstUpper(), item.ColumnName);
                }
                else if (typeStr == "datetime" || typeStr == "date")
                {
                    content.AppendFormat("\t\t\t\tmodel.{0} = sqldr[\"{1}\"] == DBNull.Value ? DateTime.Parse(\"1900-1-1\") : Convert.ToDateTime(sqldr[\"{1}\"]);\r\n", item.ColumnName.ToFirstUpper(), item.ColumnName);
                }
                else if (typeStr == "decimal")
                {
                    content.AppendFormat("\t\t\t\tmodel.{0} = sqldr[\"{1}\"] == DBNull.Value ? 0m : Convert.ToDecimal(sqldr[\"{1}\"]);\r\n", item.ColumnName.ToFirstUpper(), item.ColumnName);
                }
                else if (typeStr == "float" || typeStr == "double")
                {
                    content.AppendFormat("\t\t\t\tmodel.{0} = sqldr[\"{1}\"] == DBNull.Value ? 0 : Convert.ToDouble(sqldr[\"{1}\"]);\r\n", item.ColumnName.ToFirstUpper(), item.ColumnName);
                }
                else if (typeStr == "bigint")
                {
                    content.AppendFormat("\t\t\t\tmodel.{0} = sqldr[\"{1}\"] == DBNull.Value ? 0 : Convert.ToInt64(sqldr[\"{1}\"]);\r\n", item.ColumnName.ToFirstUpper(), item.ColumnName);
                }
            }

            content.Append("\t\t\t\tresult.Add(model);\r\n");
            content.Append("\t\t\t}\r\n");
            content.Append("\t\t}\r\n");
            content.Append("\t}\r\n");
            content.AppendLine();
            content.AppendFormat("\treturn result;\r\n");
            content.AppendLine("}");

            return content.ToString();
        }

        public string CreateSelectByPageAndSizeAddCount()
        {
            StringBuilder content = new StringBuilder();
            content.Append("public int QueryListCount(");

            // 添加条件
            foreach (var col in tempModel.SearchColumns)
            {
                string typeStr = col.DBType.ToMsSqlClassType();
                string arg = col.ColumnName.ToFirstLower();
                content.AppendFormat("{0} {1}, ", typeStr, arg);
            }

            if (content.ToString().EndsWith(", "))
            {
                content = new StringBuilder(content.ToString().Substring(0, content.Length - 2));
            }

            content.Append(")\r\n");
            content.Append("{\r\n");

            content.AppendFormat("\tstring whereStr = string.Empty;\r\n");
            content.AppendFormat("\tList<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            foreach (var col in tempModel.SearchColumns)
            {
                string typeStr = col.DBType.ToMsSqlClassType();
                string arg = col.ColumnName.ToFirstLower();

                if (typeStr == "string")
                {
                    content.AppendFormat("\tif (!string.IsNullOrEmpty({0}))\r\n", arg);
                }
                else if (typeStr == "int" || typeStr == "long")
                {
                    content.AppendFormat("\tif ({0} >= 0)\r\n", arg);
                }
                else
                {
                    content.AppendFormat("\tif(true)\r\n");
                }

                content.Append("\t{\r\n");
                content.AppendFormat("\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", col.ColumnName.ToFirstUpper(), col.DBType.ToMsSqlDbType(), arg);
                content.AppendFormat("\t\twhereStr += \" and {0}=@{1} \";\r\n", col.ColumnName, col.ColumnName.ToFirstUpper());
                content.Append("\t}\r\n");
                content.AppendLine();
            }

            content.Append("\tstring selectSql = string.Format(@\"select count(0) from " + tempModel.TableName + " where 1=1 {0};\", whereStr);\r\n");
            content.Append("\t" + GetDBConnection(tempModel.DbName) + "\r\n");
            content.Append("\t{\r\n");
            content.Append("\t\treturn Convert.ToInt32(SqlHelper.ExecuteScalar(sqlcn, CommandType.Text, selectSql, listParams.ToArray()));\r\n");
            content.Append("\t}\r\n");
            content.AppendLine("}");

            return content.ToString();
        }

        public string CreateConnectionFactory()
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("public class ConnectionFactory");
            content.AppendLine("{");
            content.AppendFormat("\tpublic static SqlConnection {0}", (string.IsNullOrEmpty(tempModel.DbName) ? "数据库名称" : tempModel.DbName));
            content.AppendLine("\t{");
            content.AppendLine("\t\tget");
            content.AppendLine("\t\t{");
            content.AppendLine("\t\t\treturn new SqlConnection(\"连接字符串\");");
            content.AppendLine("\t\t}");
            content.AppendLine("\t}");
            content.AppendLine("}");

            return content.ToString();
        }

        private static string GetDBConnection(string dbName)
        {
            return "using (SqlConnection sqlcn = ConnectionFactory." + (string.IsNullOrEmpty(dbName) ? "数据库名称" : dbName) + ")";
        }
    }
}