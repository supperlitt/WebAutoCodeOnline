using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    public class EasyUIDALHelper
    {
        public static string CreateDALHeader(string nameSpace, string tableName)
        {
            return string.Format(@"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace {0}
{{
    public class {1}DAL
    {{
", nameSpace, tableName);
        }

        public static string CreateAddMethod(EasyUIModel model)
        {
            StringBuilder addContent = new StringBuilder();
            StringBuilder valueContent = new StringBuilder(") values (");
            StringBuilder addparamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            addContent.AppendFormat("string insertSql = \"insert {0}(", model.TableName);
            var addlist = (from f in model.ColumnList where !f.IsMainKey select f).ToList();
            int index = 0;
            foreach (var item in addlist)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();

                if (index == 0)
                {
                    addContent.Append(item.ColumnName);
                    valueContent.Append("@" + item.ColumnName);
                }
                else
                {
                    addContent.Append(" ," + item.ColumnName);
                    valueContent.Append(" ,@" + item.ColumnName);
                }

                addparamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, item.DBType.ToMsSqlDbType(), attribute);

                index++;
            }

            addContent.Append(valueContent.ToString() + ")\";");
            string template = @"
        public bool Add{0}({0} model)
        {{
			{1}
            {2}
            using (SqlConnection sqlcn = ConnectionFactory.{3})
            {{
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
            }}
        }}
";

            return string.Format(template, model.TableName.ToFirstUpper(), addContent.ToString(), addparamsContent.ToString(), model.DbName.ToFirstUpper());
        }

        public static string CreateEditMethod(EasyUIModel model)
        {
            StringBuilder updateContent = new StringBuilder("string updateSql = \"update top(1)");
            updateContent.AppendFormat(" {0} set ", model.TableName);
            StringBuilder updateParamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            int index = 0;
            foreach (var item in model.EditColumns)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();

                if (index == 0)
                {
                    updateContent.AppendFormat("{0}=@{0}", item.ColumnName);
                }
                else
                {
                    updateContent.AppendFormat(",{0}=@{0}", item.ColumnName);
                }

                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, item.DBType.ToMsSqlDbType(), attribute);

                index++;
            }

            updateContent.Append(" where ");
            updateContent.AppendFormat(" {0}=@{1} \";", model.MainKeyIdStr, model.MainKeyIdStr.ToFirstUpper());
            updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", model.MainKeyIdStr.ToFirstUpper(), model.MainKeyIdDBType.ToMsSqlDbType(), model.MainKeyIdStr.ToFirstUpper());

            string template = @"
        public bool Update{0}({0} model)
        {{
			{1}
            {2}
            using (SqlConnection sqlcn = ConnectionFactory.{3})
            {{
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }}
        }}
";
            return string.Format(template, model.TableName.ToFirstUpper(), updateContent.ToString(), updateParamsContent.ToString(), model.DbName.ToFirstUpper());
        }

        public static string CreateBatEditMethod(EasyUIModel model)
        {
            StringBuilder updateContent = new StringBuilder(@"string updateSql = string.Format(""update ");
            updateContent.AppendFormat(" {0} set ", model.TableName);
            StringBuilder updateParamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            int index = 0;
            foreach (var item in model.BatEditColumns)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();

                if (index == 0)
                {
                    updateContent.AppendFormat("{0}=@{0}", item.ColumnName);
                }
                else
                {
                    updateContent.AppendFormat(",{0}=@{0}", item.ColumnName);
                }

                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, item.DBType.ToMsSqlDbType(), attribute);
                index++;
            }

            updateContent.Append(" where ");
            updateContent.AppendFormat(" {0} in ({{0}})\", idStr);", model.MainKeyIdStr);
            updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", model.MainKeyIdStr.ToFirstUpper(), model.MainKeyIdDBType.ToMsSqlDbType(), model.MainKeyIdStr.ToFirstUpper());

            string template = @"
        public bool BatUpdate{0}(List<string> list, {0} model)
        {{
            var array = (from f in list
                        select ""'"" + f + ""'"").ToArray();
            string idStr = string.Join("","", array);
			{1}
            {2}
            using (SqlConnection sqlcn = ConnectionFactory.{3})
            {{
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }}
        }}
";
            return string.Format(template, model.TableName.ToFirstUpper(), updateContent.ToString(), updateParamsContent.ToString(), model.DbName.ToFirstUpper());
        }

        public static string CreateDeleteMethod(EasyUIModel model)
        {
            StringBuilder deleteContent = new StringBuilder();
            deleteContent.AppendFormat(@"string deleteSql = string.Format(""delete from {0} ", model.TableName);
            deleteContent.AppendFormat(" where {0} in ({{0}})\", idStr);", model.MainKeyIdStr);

            string template = @"
        public bool Delete{0}(List<string> list)
        {{
            var array = (from f in list
                        select ""'"" + f + ""'"").ToArray();
            string idStr = string.Join("","", array);
			{1}
            using (SqlConnection sqlcn = ConnectionFactory.{2})
            {{
                return SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, deleteSql, null) > 0;
            }}
        }}
";
            return string.Format(template, model.TableName.ToFirstUpper(), deleteContent.ToString(), model.DbName.ToFirstUpper());
        }

        public static string CreateQueryListMethod(EasyUIModel model)
        {
            StringBuilder queryWhereContent = new StringBuilder();
            StringBuilder queryListParams = new StringBuilder();
            int index = 0;
            foreach (var item in model.SearchColumns)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();
                queryListParams.AppendFormat("{0} {1},", item.DBType.ToMsSqlClassType(), field);

                if (index == 0)
                {
                    queryWhereContent.AppendFormat("if (!string.IsNullOrEmpty({0}))\r\n", field);
                }
                else
                {
                    queryWhereContent.AppendFormat("\t\t\tif (!string.IsNullOrEmpty({0}))\r\n", field);
                }

                queryWhereContent.Append("\t\t\t{\r\n");
                queryWhereContent.AppendFormat("\t\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", item.ColumnName, item.DBType.ToMsSqlDbType(), field);

                queryWhereContent.AppendFormat("\t\t\t\twhereStr += \" and {0}=@{0} \";\r\n", item.ColumnName.ToFirstUpper());
                queryWhereContent.Append("\t\t\t}\r\n");
                queryWhereContent.AppendLine();

                index++;
            }

            StringBuilder selectSqlContent = new StringBuilder();
            selectSqlContent.Append("\tstring selectSql = string.Format(@\"select * from\r\n");
            selectSqlContent.AppendFormat("\t        (select top 100 percent *,ROW_NUMBER() over(order by {0}) as rownumber from\r\n", model.MainKeyIdStr);
            selectSqlContent.Append("\t        " + model.TableName + " where 1=1 {0}) as T\r\n");
            selectSqlContent.Append("\t        where rownumber between {1} and {2};\", whereStr, ((page - 1) * pageSize + 1), page * pageSize);\r\n");

            var assignContent = new StringBuilder();
            foreach (var item in model.ColumnList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();
                assignContent.AppendFormat("                        model.{0} = sqldr[\"{1}\"] == DBNull.Value ? {2} : {3};\r\n", attribute, item.ColumnName, ExtendMethod.ToDefaultValue(item.DBType), ExtendMethod.ToDefaultDBValue(item.DBType, item.ColumnName));
            }

            string template = @"
        public List<{0}> QueryList({1}int page, int pageSize)
        {{
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
            {2}
            {7}
            List<{0}> result = new List<{0}>();
            using (SqlConnection sqlcn = ConnectionFactory.{3})
            {{
                using(SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {{
                    while(sqldr.Read())
                    {{
                        {0} model = new {0}();
{5}
                        result.Add(model);
                    }}
                }}
            }}

            return result;
        }}

        public int QueryListCount({4})
        {{
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
            {2}
            string selectCountSql = string.Format(""select count(0) from {6} where 1=1 {{0}}"", whereStr);
            using (SqlConnection sqlcn = ConnectionFactory.{3})
            {{
                return Convert.ToInt32(SqlHelper.ExecuteScalar(sqlcn, CommandType.Text, selectCountSql, listParams.ToArray()));
            }}
        }}
";

            string queryCountParams = queryListParams.Length > 0 ? queryListParams.ToString().Substring(0, queryListParams.Length - 1) : queryListParams.ToString();
            return string.Format(template, model.TableName.ToFirstUpper(), queryListParams.ToString(), queryWhereContent.ToString(), model.DbName.ToFirstUpper(), queryCountParams, assignContent.ToString(), model.TableName, selectSqlContent.ToString());
        }

        public static string CreateGetAllAndPart(EasyUIModel model)
        {
            StringBuilder queryWhereContent = new StringBuilder();
            StringBuilder queryListParams = new StringBuilder();
            int index = 0;
            foreach (var item in model.SearchColumns)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();
                queryListParams.AppendFormat("{0} {1},", item.DBType.ToMsSqlClassType(), field);

                if (index == 0)
                {
                    queryWhereContent.AppendFormat("            if (!string.IsNullOrEmpty({0}))\r\n", field);
                }
                else
                {
                    queryWhereContent.AppendFormat("            if (!string.IsNullOrEmpty({0}))\r\n", field);
                }

                queryWhereContent.Append("            {\r\n");
                queryWhereContent.AppendFormat("                listParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", item.ColumnName, item.DBType.ToMsSqlDbType(), field);

                queryWhereContent.AppendFormat("                whereStr += \" and {0}=@{0} \";\r\n", item.ColumnName.ToFirstUpper());
                queryWhereContent.Append("            }\r\n");
                queryWhereContent.AppendLine();

                index++;
            }

            StringBuilder selectSqlContent = new StringBuilder();
            selectSqlContent.Append("            string selectSql = string.Format(@\"select * from\r\n");
            selectSqlContent.Append("            " + model.TableName + " where " + model.MainKeyIdStr + " in ({1})\r\n");
            selectSqlContent.Append("            {0};\", whereStr, idArrayStr);\r\n");

            var assignContent = new StringBuilder();
            foreach (var item in model.ColumnList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName.ToFirstUpper();
                assignContent.AppendFormat("                        model.{0} = sqldr[\"{1}\"] == DBNull.Value ? {2} : {3};\r\n", attribute, item.ColumnName, ExtendMethod.ToDefaultValue(item.DBType), ExtendMethod.ToDefaultDBValue(item.DBType, item.ColumnName));
            }

            string template = @"
        public List<{0}> GetPartAll({1}List<string> idList)
        {{
            var idArrayStr = string.Join("","", (from f in idList
                                select ""'""+ f + ""'"").ToArray());
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
{2}
{7}
            List<{0}> result = new List<{0}>();
            using (SqlConnection sqlcn = ConnectionFactory.{3})
            {{
                using(SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {{
                    while(sqldr.Read())
                    {{
                        {0} model = new {0}();
{5}
                        result.Add(model);
                    }}
                }}
            }}

            return result;
        }}

        public List<{0}> GetAll({4})
        {{
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
{2}
            string selectAllSql = string.Format(""select * from {6} where 1=1 {{0}}"", whereStr);
            List<{0}> result = new List<{0}>();
            using (SqlConnection sqlcn = ConnectionFactory.{3})
            {{
                using(SqlDataReader sqldr = SqlHelper.ExecuteReader(sqlcn, CommandType.Text, selectAllSql, listParams.ToArray()))
                {{
                    while(sqldr.Read())
                    {{
                        {0} model = new {0}();
{5}
                        result.Add(model);
                    }}
                }}
            }}

            return result;
        }}
";

            string queryCountParams = queryListParams.Length > 0 ? queryListParams.ToString().Substring(0, queryListParams.Length - 1) : queryListParams.ToString();
            return string.Format(template, model.TableName.ToFirstUpper(), queryListParams.ToString(), queryWhereContent.ToString(), model.DbName.ToFirstUpper(), queryCountParams, assignContent.ToString(), model.TableName, selectSqlContent.ToString());
        }

        public static string CreateBottom()
        {
            return @"
    }
}";
        }
    }
}
