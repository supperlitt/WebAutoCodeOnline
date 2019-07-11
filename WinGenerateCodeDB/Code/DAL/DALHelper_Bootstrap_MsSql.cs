using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class DALHelper_Bootstrap_MsSql
    {
        public static string CreateDAL()
        {
            StringBuilder dalContent = new StringBuilder();
            dalContent.Append(CreateDALHeader());
            dalContent.Append(CreateAddMethod());
            dalContent.Append(CreateEditMethod());
            dalContent.Append(CreateBatEditMethod());
            dalContent.Append(CreateDeleteMethod());
            dalContent.Append(CreateQueryListMethod());
            dalContent.Append(CreateGetAllAndPart());

            dalContent.Append(CreateBottom());

            return dalContent.ToString();
        }

        public static string CreateDALHeader()
        {
            return string.Format(@"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace {0}
{{
    public class {1}
    {{", PageCache.NameSpaceStr, PageCache.TableName_DAL);
        }

        public static string CreateAddMethod()
        {
            StringBuilder addContent = new StringBuilder();
            StringBuilder valueContent = new StringBuilder(") values (");
            StringBuilder addparamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            addContent.AppendFormat("string insertSql = \"insert {0}(", PageCache.TableName);
            var addModel = PageCache.GetCmd("添加");
            if (addModel != null)
            {
                int index = 0;
                foreach (var item in addModel.AttrList)
                {
                    string attribute = item.ColName;
                    if (index == 0)
                    {
                        addContent.Append(attribute);
                        valueContent.Append("@" + attribute);
                    }
                    else
                    {
                        addContent.Append(" ," + attribute);
                        valueContent.Append(" ,@" + attribute);
                    }

                    addparamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", attribute, item.DbType.ToMsSqlDbType());

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

                return string.Format(template, PageCache.TableName_Model, addContent.ToString(), addparamsContent.ToString(), PageCache.DatabaseName);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateEditMethod()
        {
            StringBuilder updateContent = new StringBuilder("string updateSql = \"update ");
            updateContent.AppendFormat(" {0} set ", PageCache.TableName);
            StringBuilder updateParamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            var editModel = PageCache.GetCmd("编辑");
            if (editModel != null)
            {
                int index = 0;
                foreach (var item in editModel.AttrList)
                {
                    string attribute = item.ColName;
                    if (index == 0)
                    {
                        updateContent.AppendFormat("{0}=@{0}", attribute);
                    }
                    else
                    {
                        updateContent.AppendFormat(",{0}=@{0}", attribute);
                    }

                    updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", attribute, item.DbType.ToMsSqlDbType());

                    index++;
                }

                updateContent.Append(" where ");
                updateContent.AppendFormat(" {0}=@{0} \";", PageCache.KeyId);
                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", PageCache.KeyId, PageCache.KeyId_DbType.ToMsSqlDbType());

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
                return string.Format(template, PageCache.TableName_Model, updateContent.ToString(), updateParamsContent.ToString(), PageCache.DatabaseName);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateBatEditMethod()
        {
            StringBuilder updateContent = new StringBuilder(@"string updateSql = string.Format(""update ");
            updateContent.AppendFormat(" {0} set ", PageCache.TableName);
            StringBuilder updateParamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            var batEditModel = PageCache.GetCmd("批量编辑");
            if (batEditModel != null)
            {
                int index = 0;
                foreach (var item in batEditModel.AttrList)
                {
                    string attribute = item.ColName;
                    if (index == 0)
                    {
                        updateContent.AppendFormat("{0}=@{0}", attribute);
                    }
                    else
                    {
                        updateContent.AppendFormat(",{0}=@{0}", attribute);
                    }

                    updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", attribute, item.DbType.ToMsSqlDbType());
                    index++;
                }

                updateContent.Append(" where ");
                updateContent.AppendFormat(" {0} in ({{0}})\", idStr);", PageCache.KeyId);
                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", PageCache.KeyId, PageCache.KeyId_DbType.ToMsSqlDbType());

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
                return string.Format(template, PageCache.TableName_Model, updateContent.ToString(), updateParamsContent.ToString(), PageCache.DatabaseName);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateDeleteMethod()
        {
            StringBuilder deleteContent = new StringBuilder();
            deleteContent.AppendFormat(@"string deleteSql = string.Format(""delete from {0} ", PageCache.TableName);
            deleteContent.AppendFormat(" where {0} in ({{0}})\", idStr);", PageCache.KeyId);

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
            return string.Format(template, PageCache.TableName, deleteContent.ToString(), PageCache.DatabaseName);
        }

        public static string CreateQueryListMethod()
        {
            StringBuilder queryWhereContent = new StringBuilder();
            StringBuilder queryListParams = new StringBuilder();
            var showModel = PageCache.GetCmd("主显示");
            if (showModel != null)
            {
                int index = 0;
                foreach (var item in showModel.AttrList)
                {
                    string attribute = item.ColName;
                    queryListParams.AppendFormat("{0} {1},", item.DbType.ToMsSqlClassType(), attribute);
                    if (index == 0)
                    {
                        if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint" || item.DbType.ToLower() == "decimal" || item.DbType.ToLower() == "float" || item.DbType.ToLower() == "double")
                        {
                            queryWhereContent.AppendFormat("if ({0} >= 0)\r\n", attribute);
                        }
                        else if (item.DbType.ToLower() == "tinyint")
                        {
                            queryWhereContent.AppendFormat("if ({0} >= 0)\r\n", attribute);
                        }
                        else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            queryWhereContent.AppendFormat("if ({0} != DateTime.MinValue)\r\n", attribute);
                        }
                        else
                        {
                            queryWhereContent.AppendFormat("if (!string.IsNullOrEmpty({0}))\r\n", attribute);
                        }
                    }
                    else
                    {
                        if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint" || item.DbType.ToLower() == "decimal" || item.DbType.ToLower() == "float" || item.DbType.ToLower() == "double")
                        {
                            queryWhereContent.AppendFormat("\t\t\tif ({0} >= 0)\r\n", attribute);
                        }
                        else if (item.DbType.ToLower() == "tinyint")
                        {
                            queryWhereContent.AppendFormat("\t\t\tif ({0} >= 0)\r\n", attribute);
                        }
                        else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            queryWhereContent.AppendFormat("\t\t\tif ({0} != DateTime.MinValue)\r\n", attribute);
                        }
                        else
                        {
                            queryWhereContent.AppendFormat("\t\t\tif (!string.IsNullOrEmpty({0}))\r\n", attribute);
                        }
                    }

                    queryWhereContent.Append("\t\t\t{\r\n");
                    queryWhereContent.AppendFormat("\t\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {0} }});\r\n", attribute, item.DbType.ToMsSqlDbType());

                    queryWhereContent.AppendFormat("\t\t\t\twhereStr += \" and {0}=@{0} \";\r\n", attribute);
                    queryWhereContent.Append("\t\t\t}\r\n");
                    queryWhereContent.AppendLine();

                    index++;
                }

                StringBuilder selectSqlContent = new StringBuilder();
                selectSqlContent.Append("\tstring selectSql = string.Format(@\"select * from " + PageCache.TableName + " where 1=1 {0} limit {1},{2};\", whereStr, ((page - 1) * pageSize), pageSize);\r\n");

                var assignContent = new StringBuilder();
                if (!showModel.AttrList.Exists(p => p.ColName == PageCache.KeyId))
                {
                    assignContent.AppendFormat("                        model.{0} = sqldr[\"{0}\"] == DBNull.Value ? {1} : {2};\r\n", PageCache.KeyId, ExtendMethod.ToDefaultValue(PageCache.KeyId_DbType), ExtendMethod.ToDefaultDBValue(PageCache.KeyId_DbType, PageCache.KeyId));
                }

                foreach (var item in showModel.AttrList)
                {
                    string attribute = item.ColName;
                    assignContent.AppendFormat("                        model.{0} = sqldr[\"{0}\"] == DBNull.Value ? {1} : {2};\r\n", attribute, ExtendMethod.ToDefaultValue(item.DbType), ExtendMethod.ToDefaultDBValue(item.DbType, attribute));
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
                using(SqlDataReader sqldr = SqlHelper.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
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
                return string.Format(template, PageCache.TableName_Model, queryListParams.ToString(), queryWhereContent.ToString(), PageCache.DatabaseName, queryCountParams, assignContent.ToString(), PageCache.TableName, selectSqlContent.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateGetAllAndPart()
        {
            int index = 0;
            var down_all_Model = PageCache.GetCmd("导出全部");
            var down_selelct_Model = PageCache.GetCmd("导出选中");
            if (down_all_Model != null || down_selelct_Model != null)
            {
                StringBuilder resultContent = new StringBuilder();

                #region 导出全部
                if (down_all_Model != null)
                {
                    StringBuilder queryWhereContent = new StringBuilder();
                    StringBuilder queryListParams = new StringBuilder();
                    foreach (var item in down_all_Model.AttrList)
                    {
                        string attribute = item.ColName;
                        queryListParams.AppendFormat("{0} {1},", item.DbType.ToMsSqlClassType(), attribute);
                        if (index == 0)
                        {
                            if (item.DbType.ToLower() == "int")
                            {
                                queryWhereContent.AppendFormat("            if ({0} > 0)\r\n", attribute);
                            }
                            else if (item.DbType.ToLower() == "tinyint")
                            {
                                queryWhereContent.AppendFormat("            if ({0} >= 0)\r\n", attribute);
                            }
                            else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "datetime")
                            {
                                queryWhereContent.AppendFormat("            if ({0} != DateTime.MinValue)\r\n", attribute);
                            }
                            else
                            {
                                queryWhereContent.AppendFormat("            if (!string.IsNullOrEmpty({0}))\r\n", attribute);
                            }
                        }
                        else
                        {
                            if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint")
                            {
                                queryWhereContent.AppendFormat("            if ({0} > 0)\r\n", attribute);
                            }
                            else if (item.DbType.ToLower() == "tinyint")
                            {
                                queryWhereContent.AppendFormat("            if ({0} >= 0)\r\n", attribute);
                            }
                            else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "datetime")
                            {
                                queryWhereContent.AppendFormat("            if ({0} != DateTime.MinValue)\r\n", attribute);
                            }
                            else
                            {
                                queryWhereContent.AppendFormat("            if (!string.IsNullOrEmpty({0}))\r\n", attribute);
                            }
                        }

                        queryWhereContent.Append("            {\r\n");
                        queryWhereContent.AppendFormat("                listParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {0} }});\r\n", attribute, item.DbType.ToMsSqlDbType());

                        queryWhereContent.AppendFormat("                whereStr += \" and {0}=@{0} \";\r\n", attribute);
                        queryWhereContent.Append("            }\r\n");
                        queryWhereContent.AppendLine();

                        index++;
                    }

                    var assignContent = new StringBuilder();
                    foreach (var item in down_all_Model.AttrList)
                    {
                        string attribute = item.ColName;
                        assignContent.AppendFormat("                        model.{0} = sqldr[\"{0}\"] == DBNull.Value ? {1} : {2};\r\n", attribute, ExtendMethod.ToDefaultValue(item.DbType), ExtendMethod.ToDefaultDBValue(item.DbType, attribute));
                    }

                    string template = @"
        public List<{0}> GetAll({3})
        {{
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
{1}
            string selectAllSql = string.Format(""select * from {5} where 1=1 {{0}}"", whereStr);
            List<{0}> result = new List<{0}>();
            using (SqlConnection sqlcn = ConnectionFactory.{2})
            {{
                using(SqlDataReader sqldr = SqlHelper.ExecuteDataReader(sqlcn, CommandType.Text, selectAllSql, listParams.ToArray()))
                {{
                    while(sqldr.Read())
                    {{
                        {0} model = new {0}();
{4}
                        result.Add(model);
                    }}
                }}
            }}

            return result;
        }}
";

                    string queryCountParams = queryListParams.Length > 0 ? queryListParams.ToString().Substring(0, queryListParams.Length - 1) : queryListParams.ToString();
                    resultContent.AppendFormat(template, PageCache.TableName_Model, queryWhereContent.ToString(), PageCache.DatabaseName, queryCountParams, assignContent.ToString(), PageCache.TableName);
                }
                #endregion

                #region 导出选中

                if (down_selelct_Model != null)
                {
                    StringBuilder selectSqlContent = new StringBuilder();
                    selectSqlContent.Append("            string selectSql = string.Format(@\"select * from\r\n");
                    selectSqlContent.Append("            " + PageCache.TableName + " where " + PageCache.KeyId + " in ({1})\r\n");
                    selectSqlContent.Append("            {0};\", whereStr, idArrayStr);\r\n");

                    var assignContent = new StringBuilder();
                    foreach (var item in down_selelct_Model.AttrList)
                    {
                        string colName = item.ColName;
                        assignContent.AppendFormat("                        model.{0} = sqldr[\"{0}\"] == DBNull.Value ? {1} : {2};\r\n", colName, ExtendMethod.ToDefaultValue(item.DbType), ExtendMethod.ToDefaultDBValue(item.DbType, colName));
                    }

                    string template = @"
        public List<{0}> GetPartAll(List<string> idList)
        {{
            var idArrayStr = string.Join("","", (from f in idList
                                select ""'""+ f + ""'"").ToArray());
            string whereStr = string.Empty;
            List<SqlParameter> listParams = new List<SqlParameter>();
{3}
            List<{0}> result = new List<{0}>();
            using (SqlConnection sqlcn = ConnectionFactory.{1})
            {{
                using(SqlDataReader sqldr = SqlHelper.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {{
                    while(sqldr.Read())
                    {{
                        {0} model = new {0}();
{2}
                        result.Add(model);
                    }}
                }}
            }}

            return result;
        }}
";

                    resultContent.AppendFormat(template, PageCache.TableName_Model, PageCache.DatabaseName, assignContent.ToString(), selectSqlContent.ToString());
                }

                #endregion

                return resultContent.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateBottom()
        {
            return @"    }
}";
        }
    }
}
