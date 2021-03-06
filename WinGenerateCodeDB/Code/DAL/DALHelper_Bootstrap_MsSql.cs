﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class DALHelper_Bootstrap_MsSql
    {
        public static string CreateDAL(string name_space, string table_name, List<SqlColumnInfo> colList, int action, string dal_name, string model_name, string db_name)
        {
            StringBuilder dalContent = new StringBuilder();
            dalContent.Append(CreateDALHeader(name_space, dal_name));
            dalContent.Append(CreateAddMethod(action, table_name, colList, model_name, db_name));
            dalContent.Append(CreateBatAddMethod(action, table_name, colList, model_name, db_name));
            dalContent.Append(CreateEditMethod(action, table_name, colList, model_name, db_name));
            dalContent.Append(CreateDeleteMethod(action, table_name, colList, db_name));
            dalContent.Append(CreateQueryListMethod(action, table_name, colList, model_name, db_name));
            dalContent.Append(CreateGetAllAndPart(action, table_name, colList, model_name, db_name));

            dalContent.Append(CreateBottom());

            return dalContent.ToString();
        }

        public static string CreateDALHeader(string name_space, string dal_name)
        {
            return string.Format(@"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace {0}
{{
    public class {1}
    {{", name_space, dal_name);
        }

        public static string CreateAddMethod(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            StringBuilder addContent = new StringBuilder();
            StringBuilder valueContent = new StringBuilder(") values (");
            StringBuilder addparamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            addContent.AppendFormat("string insertSql = \"insert {0}(", table_name);
            if ((action & (int)action_type.add) == (int)action_type.add)
            {
                int index = 0;
                var addList = Cache_VMData.GetVMList(table_name, VMType.Add, colList.ToNotMainIdList());
                foreach (var item in addList)
                {
                    if (index == 0)
                    {
                        addContent.Append(item.Name);
                        valueContent.Append("@" + item.Name);
                    }
                    else
                    {
                        addContent.Append(" ," + item.Name);
                        valueContent.Append(" ,@" + item.Name);
                    }

                    addparamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", item.Name, item.DbType.ToMsSqlDbType());

                    index++;
                }

                var checkList = Cache_VMData.GetAddCheckList(table_name, colList.ToNotMainIdList());
                if (checkList.Count == 0)
                {
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

                    return string.Format(template, table_name, addContent.ToString(), addparamsContent.ToString(), db_name);
                }
                else
                {
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

                    return string.Format(template, table_name, addContent.ToString(), addparamsContent.ToString(), db_name);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateBatAddMethod(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            StringBuilder addContent = new StringBuilder();
            StringBuilder valueContent = new StringBuilder(") values (");
            StringBuilder addparamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            addContent.AppendFormat("string insertSql = \"insert {0}(", table_name);
            if ((action & (int)action_type.bat_add) == (int)action_type.bat_add)
            {
                int index = 0;
                var addList = Cache_VMData.GetVMList(table_name, VMType.Add, colList.ToNotMainIdList());
                foreach (var item in addList)
                {
                    if (index == 0)
                    {
                        addContent.Append(item.Name);
                        valueContent.Append("@" + item.Name);
                    }
                    else
                    {
                        addContent.Append(" ," + item.Name);
                        valueContent.Append(" ,@" + item.Name);
                    }

                    addparamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", item.Name, item.DbType.ToMsSqlDbType());

                    index++;
                }

                addContent.Append(valueContent.ToString() + ")\";");
                string template = @"
        public void Add{0}_list(List<{0}> list)
        {{
			{1}
            using (SqlConnection sqlcn = ConnectionFactory.{3})
            {{
                sqlcn.Open();
                foreach(var model in list)
                {{
                    {2}
                    SqlHelper.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
                }}
            }}
        }}
";

                return string.Format(template, table_name, addContent.ToString(), addparamsContent.ToString(), db_name);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateEditMethod(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            StringBuilder updateContent = new StringBuilder("string updateSql = \"update ");
            updateContent.AppendFormat(" {0} set ", table_name);
            StringBuilder updateParamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            if ((action & (int)action_type.edit) == (int)action_type.edit)
            {
                int index = 0;
                var editList = Cache_VMData.GetVMList(table_name, VMType.Edit, colList.ToNotMainIdList());
                foreach (var item in editList)
                {
                    if (index == 0)
                    {
                        updateContent.AppendFormat("{0}=@{0}", item.Name);
                    }
                    else
                    {
                        updateContent.AppendFormat(",{0}=@{0}", item.Name);
                    }

                    updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", item.Name, item.DbType.ToMsSqlDbType());

                    index++;
                }

                updateContent.Append(" where ");
                updateContent.AppendFormat(" {0}=@{0} \";", colList.ToKeyId());
                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", colList.ToKeyId(), colList.ToKeyIdDbType().ToMsSqlDbType());

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
                return string.Format(template, table_name, updateContent.ToString(), updateParamsContent.ToString(), db_name);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateDeleteMethod(int action, string table_name, List<SqlColumnInfo> colList, string db_name)
        {
            StringBuilder deleteContent = new StringBuilder();
            deleteContent.AppendFormat(@"string deleteSql = string.Format(""delete from {0} ", table_name);
            deleteContent.AppendFormat(" where {0} in ({{0}})\", idStr);", colList.ToKeyId());

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
            return string.Format(template, table_name, deleteContent.ToString(), db_name);
        }

        public static string CreateQueryListMethod(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            StringBuilder queryWhereContent = new StringBuilder();
            StringBuilder queryListParams = new StringBuilder();
            if ((action & (int)action_type.query_list) == (int)action_type.query_list)
            {
                int index = 0;
                var queryList = Cache_VMData.GetVMList(table_name, VMType.Query, colList.ToNotMainIdList());
                foreach (var item in queryList)
                {
                    queryListParams.AppendFormat("{0} {1},", item.DbType.ToMsSqlClassType(), item.Name);
                    if (index == 0)
                    {
                        if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint" || item.DbType.ToLower() == "decimal" || item.DbType.ToLower() == "float" || item.DbType.ToLower() == "double")
                        {
                            queryWhereContent.AppendFormat("if ({0} >= 0)\r\n", item.Name);
                        }
                        else if (item.DbType.ToLower() == "tinyint")
                        {
                            queryWhereContent.AppendFormat("if ({0} >= 0)\r\n", item.Name);
                        }
                        else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            queryWhereContent.AppendFormat("if ({0} != DateTime.MinValue)\r\n", item.Name);
                        }
                        else
                        {
                            queryWhereContent.AppendFormat("if (!string.IsNullOrEmpty({0}))\r\n", item.Name);
                        }
                    }
                    else
                    {
                        if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint" || item.DbType.ToLower() == "decimal" || item.DbType.ToLower() == "float" || item.DbType.ToLower() == "double")
                        {
                            queryWhereContent.AppendFormat("\r\n\t\t\tif ({0} >= 0)\r\n", item.Name);
                        }
                        else if (item.DbType.ToLower() == "tinyint")
                        {
                            queryWhereContent.AppendFormat("\r\n\t\t\tif ({0} >= 0)\r\n", item.Name);
                        }
                        else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            queryWhereContent.AppendFormat("\r\n\t\t\tif ({0} != DateTime.MinValue)\r\n", item.Name);
                        }
                        else
                        {
                            queryWhereContent.AppendFormat("\r\n\t\t\tif (!string.IsNullOrEmpty({0}))\r\n", item.Name);
                        }
                    }

                    queryWhereContent.Append("\t\t\t{\r\n");
                    queryWhereContent.AppendFormat("\t\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {0} }});\r\n", item.Name, item.DbType.ToMsSqlDbType());

                    queryWhereContent.AppendFormat("\t\t\t\twhereStr += \" and {0}=@{0} \";\r\n", item.Name);
                    queryWhereContent.Append("\t\t\t}");
                    queryWhereContent.AppendLine();

                    index++;
                }

                StringBuilder selectSqlContent = new StringBuilder();
                selectSqlContent.Append("string selectSql = string.Format(@\"select * from " + table_name + " where 1=1 {0} limit {1},{2};\", whereStr, ((page - 1) * pageSize), pageSize);\r\n");

                var assignContent = new StringBuilder();
                foreach (var item in colList)
                {
                    assignContent.AppendFormat("                        model.{0} = sqldr[\"{0}\"] == DBNull.Value ? {1} : {2};\r\n", item.Name, ExtendMethod.ToDefaultValue(item.DbType), ExtendMethod.ToDefaultDBValue(item.DbType, item.Name));
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
                return string.Format(template, table_name, queryListParams.ToString(), queryWhereContent.ToString(), db_name, queryCountParams, assignContent.ToString(), table_name, selectSqlContent.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateGetAllAndPart(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            int index = 0;
            var down_all_Model = (action | (int)action_type.export_all) == (int)action_type.export_all;
            var down_selelct_Model = (action | (int)action_type.export_select) == (int)action_type.export_select;
            if (down_all_Model || down_selelct_Model)
            {
                StringBuilder resultContent = new StringBuilder();

                #region 导出全部
                if (down_all_Model)
                {
                    StringBuilder queryWhereContent = new StringBuilder();
                    StringBuilder queryListParams = new StringBuilder();
                    var queryList = Cache_VMData.GetVMList(table_name, VMType.Query, colList.ToNotMainIdList());
                    foreach (var item in queryList)
                    {
                        queryListParams.AppendFormat("{0} {1},", item.DbType.ToMsSqlClassType(), item.Name);
                        if (index == 0)
                        {
                            if (item.DbType.ToLower() == "int")
                            {
                                queryWhereContent.AppendFormat("            if ({0} > 0)\r\n", item.Name);
                            }
                            else if (item.DbType.ToLower() == "tinyint")
                            {
                                queryWhereContent.AppendFormat("            if ({0} >= 0)\r\n", item.Name);
                            }
                            else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "datetime")
                            {
                                queryWhereContent.AppendFormat("            if ({0} != DateTime.MinValue)\r\n", item.Name);
                            }
                            else
                            {
                                queryWhereContent.AppendFormat("            if (!string.IsNullOrEmpty({0}))\r\n", item.Name);
                            }
                        }
                        else
                        {
                            if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint")
                            {
                                queryWhereContent.AppendFormat("            if ({0} > 0)\r\n", item.Name);
                            }
                            else if (item.DbType.ToLower() == "tinyint")
                            {
                                queryWhereContent.AppendFormat("            if ({0} >= 0)\r\n", item.Name);
                            }
                            else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "datetime")
                            {
                                queryWhereContent.AppendFormat("            if ({0} != DateTime.MinValue)\r\n", item.Name);
                            }
                            else
                            {
                                queryWhereContent.AppendFormat("            if (!string.IsNullOrEmpty({0}))\r\n", item.Name);
                            }
                        }

                        queryWhereContent.Append("            {\r\n");
                        queryWhereContent.AppendFormat("                listParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {0} }});\r\n", item.Name, item.DbType.ToMsSqlDbType());

                        queryWhereContent.AppendFormat("                whereStr += \" and {0}=@{0} \";\r\n", item.Name);
                        queryWhereContent.Append("            }\r\n");
                        queryWhereContent.AppendLine();

                        index++;
                    }

                    var assignContent = new StringBuilder();
                    foreach (var item in colList)
                    {
                        assignContent.AppendFormat("                        model.{0} = sqldr[\"{0}\"] == DBNull.Value ? {1} : {2};\r\n", item.Name, ExtendMethod.ToDefaultValue(item.DbType), ExtendMethod.ToDefaultDBValue(item.DbType, item.Name));
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
                    resultContent.AppendFormat(template, table_name, queryWhereContent.ToString(), db_name, queryCountParams, assignContent.ToString(), table_name);
                }
                #endregion

                #region 导出选中

                if (down_selelct_Model)
                {
                    StringBuilder selectSqlContent = new StringBuilder();
                    selectSqlContent.Append("            string selectSql = string.Format(@\"select * from\r\n");
                    selectSqlContent.Append("            " + table_name + " where " + colList.ToKeyId() + " in ({1})\r\n");
                    selectSqlContent.Append("            {0};\", whereStr, idArrayStr);\r\n");

                    var assignContent = new StringBuilder();
                    foreach (var item in colList)
                    {
                        assignContent.AppendFormat("                        model.{0} = sqldr[\"{0}\"] == DBNull.Value ? {1} : {2};\r\n", item.Name, ExtendMethod.ToDefaultValue(item.DbType), ExtendMethod.ToDefaultDBValue(item.DbType, item.Name));
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

                    resultContent.AppendFormat(template, table_name, db_name, assignContent.ToString(), selectSqlContent.ToString());
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
