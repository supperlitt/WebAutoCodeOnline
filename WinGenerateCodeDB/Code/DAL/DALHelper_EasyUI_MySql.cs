using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class DALHelper_EasyUI_MySql
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
using MySql.Data.MySqlClient;
using System.Linq;

namespace {0}
{{
    public class {1}
    {{", PageCache.NameSpaceStr, PageCache.TableName_DAL);
        }

        public static string CreateAddMethod()
        {
            var addModel = PageCache.GetCmd("添加");
            if (addModel != null)
            {
                StringBuilder addContent = new StringBuilder();
                StringBuilder valueContent = new StringBuilder(") values (");
                StringBuilder addparamsContent = new StringBuilder("List<MySqlParameter> listParams = new List<MySqlParameter>();\r\n");
                addContent.AppendFormat("string insertSql = \"insert into {0}(", PageCache.TableName);
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

                    addparamsContent.AppendFormat("\t\t\tlistParams.Add(new MySqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", attribute, item.DbType.ToMySqlDbType());

                    index++;
                }

                addContent.Append(valueContent.ToString() + ")\";");
                string template = @"
        public bool Add{4}({0} model)
        {{
			{1}
            {2}
            using (MySqlConnection sqlcn = ConnectionFactory.{3})
            {{
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
            }}
        }}
";

                return string.Format(template,
                    PageCache.TableName_Model,
                    addContent.ToString(),
                    addparamsContent.ToString(),
                    PageCache.DatabaseName,
                    PageCache.TableName);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateEditMethod()
        {
            var editModel = PageCache.GetCmd("编辑");
            if (editModel != null)
            {
                StringBuilder updateContent = new StringBuilder("string updateSql = \"update ");
                updateContent.AppendFormat(" {0} set ", PageCache.TableName);
                StringBuilder updateParamsContent = new StringBuilder("List<MySqlParameter> listParams = new List<MySqlParameter>();\r\n");

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

                    updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new MySqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", attribute, item.DbType.ToMySqlDbType());
                    index++;
                }

                updateContent.Append(" where ");
                updateContent.AppendFormat(" {0}=@{0} \";", PageCache.KeyId);
                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new MySqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", PageCache.KeyId, PageCache.KeyId_DbType.ToMySqlDbType(), PageCache.KeyId);

                string template = @"
        public bool Update{4}({0} model)
        {{
			{1}
            {2}
            using (MySqlConnection sqlcn = ConnectionFactory.{3})
            {{
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }}
        }}
";
                return string.Format(template,
                    PageCache.TableName_Model,
                    updateContent.ToString(),
                    updateParamsContent.ToString(),
                    PageCache.DatabaseName,
                    PageCache.TableName);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateBatEditMethod()
        {
            var batEditModel = PageCache.GetCmd("批量编辑");
            if (batEditModel != null)
            {
                StringBuilder updateContent = new StringBuilder(@"string updateSql = string.Format(""update ");
                updateContent.AppendFormat(" {0} set ", PageCache.TableName);
                StringBuilder updateParamsContent = new StringBuilder("List<MySqlParameter> listParams = new List<MySqlParameter>();\r\n");

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

                    updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new MySqlParameter(\"@{0}\", {1}) {{ Value = model.{0} }});\r\n", attribute, item.DbType.ToMySqlDbType());
                    index++;
                }

                updateContent.Append(" where ");
                updateContent.AppendFormat(" {0} in ({{0}})\", idStr);", PageCache.KeyId);
                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new MySqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", PageCache.KeyId, PageCache.KeyId_DbType.ToMySqlDbType(), PageCache.KeyId);

                string template = @"
        public bool BatUpdate{4}(List<string> list, {0} model)
        {{
            var array = (from f in list
                        select ""'"" + f + ""'"").ToArray();
            string idStr = string.Join("","", array);
			{1}
            {2}
            using (MySqlConnection sqlcn = ConnectionFactory.{3})
            {{
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }}
        }}
";
                return string.Format(template,
                    PageCache.TableName_Model,
                    updateContent.ToString(),
                    updateParamsContent.ToString(),
                    PageCache.DatabaseName,
                    PageCache.TableName);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateDeleteMethod()
        {
            var deleteModel = PageCache.GetCmd("删除");
            if (deleteModel != null)
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
            using (MySqlConnection sqlcn = ConnectionFactory.{2})
            {{
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, deleteSql, null) > 0;
            }}
        }}
";
                return string.Format(template,
                    PageCache.TableName,
                    deleteContent.ToString(),
                    PageCache.DatabaseName);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateQueryListMethod()
        {
            var showModel = PageCache.GetCmd("主显示");
            if (showModel != null)
            {
                StringBuilder queryWhereContent = new StringBuilder();
                StringBuilder queryListParams = new StringBuilder();
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
                    queryWhereContent.AppendFormat("\t\t\t\tlistParams.Add(new MySqlParameter(\"@{0}\", {1}) {{ Value = {0} }});\r\n", attribute, item.DbType.ToMySqlDbType());

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
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            {2}
            {7}
            List<{0}> result = new List<{0}>();
            using (MySqlConnection sqlcn = ConnectionFactory.{3})
            {{
                using(MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
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
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            {2}
            string selectCountSql = string.Format(""select count(0) from {6} where 1=1 {{0}}"", whereStr);
            using (MySqlConnection sqlcn = ConnectionFactory.{3})
            {{
                return Convert.ToInt32(MySqlHelper2.ExecuteScalar(sqlcn, CommandType.Text, selectCountSql, listParams.ToArray()));
            }}
        }}
";

                string queryCountParams = queryListParams.Length > 0 ? queryListParams.ToString().Substring(0, queryListParams.Length - 1) : queryListParams.ToString();
                return string.Format(template,
                    PageCache.TableName_Model,
                    queryListParams.ToString(),
                    queryWhereContent.ToString(),
                    PageCache.DatabaseName,
                    queryCountParams,
                    assignContent.ToString(),
                    PageCache.TableName,
                    selectSqlContent.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateGetAllAndPart()
        {
            var down_allModel = PageCache.GetCmd("导出全部");
            var down_selelctModel = PageCache.GetCmd("导出选中");
            if (down_allModel != null || down_selelctModel != null)
            {
                StringBuilder down_allModel_Str = new StringBuilder();
                #region 导出全部
                if (down_allModel != null)
                {
                    StringBuilder queryWhereContent = new StringBuilder();
                    StringBuilder queryListParams = new StringBuilder();
                    int index = 0;
                    foreach (var item in down_allModel.AttrList)
                    {
                        string attribute = item.ColName;
                        queryListParams.AppendFormat("{0} {1},", item.DbType.ToMsSqlClassType(), attribute);

                        if (index == 0)
                        {
                            if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint")
                            {
                                queryWhereContent.AppendFormat("if ({0} > 0)\r\n", attribute);
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
                            if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint")
                            {
                                queryWhereContent.AppendFormat("\t\t\tif ({0} > 0)\r\n", attribute);
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

                        queryWhereContent.Append("            {\r\n");
                        queryWhereContent.AppendFormat("                listParams.Add(new MySqlParameter(\"@{0}\", {1}) {{ Value = {0} }});\r\n", attribute, item.DbType.ToMySqlDbType());

                        queryWhereContent.AppendFormat("                whereStr += \" and {0}=@{0} \";\r\n", attribute);
                        queryWhereContent.Append("            }\r\n");
                        queryWhereContent.AppendLine();

                        index++;
                    }

                    var assignContent = new StringBuilder();
                    foreach (var item in down_allModel.AttrList)
                    {
                        string attribute = item.ColName;
                        assignContent.AppendFormat("                        model.{0} = sqldr[\"{0}\"] == DBNull.Value ? {1} : {2};\r\n", attribute, ExtendMethod.ToDefaultValue(item.DbType), ExtendMethod.ToDefaultDBValue(item.DbType, attribute));
                    }

                    string template = @"
        public List<{0}> GetAll({3})
        {{
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
{1}
            string selectAllSql = string.Format(""select * from {5} where 1=1 {{0}}"", whereStr);
            List<{0}> result = new List<{0}>();
            using (MySqlConnection sqlcn = ConnectionFactory.{2})
            {{
                using(MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectAllSql, listParams.ToArray()))
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
                    down_allModel_Str.AppendFormat(template,
                        PageCache.TableName_Model,
                        queryWhereContent.ToString(),
                        PageCache.DatabaseName,
                        queryCountParams,
                        assignContent.ToString(),
                        PageCache.TableName);
                }
                #endregion

                StringBuilder down_selectModel_Str = new StringBuilder();
                #region 导出选中
                if (down_selelctModel != null)
                {
                    StringBuilder selectSqlContent = new StringBuilder();
                    selectSqlContent.Append("            string selectSql = string.Format(@\"select * from\r\n");
                    selectSqlContent.Append("            " + PageCache.TableName + " where " + PageCache.KeyId + " in ({1})\r\n");
                    selectSqlContent.Append("            {0};\", whereStr, idArrayStr);\r\n");

                    var assignContent = new StringBuilder();
                    foreach (var item in down_selelctModel.AttrList)
                    {
                        string attribute = item.ColName;
                        assignContent.AppendFormat("                        model.{0} = sqldr[\"{0}\"] == DBNull.Value ? {1} : {2};\r\n", attribute, ExtendMethod.ToDefaultValue(item.DbType), ExtendMethod.ToDefaultDBValue(item.DbType, attribute));
                    }

                    string template = @"
        public List<{0}> GetPartAll(List<string> idList)
        {{
            var idArrayStr = string.Join("","", (from f in idList
                                select ""'""+ f + ""'"").ToArray());
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();
{3}
            List<{0}> result = new List<{0}>();
            using (MySqlConnection sqlcn = ConnectionFactory.{1})
            {{
                using(MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
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
                    down_selectModel_Str.AppendFormat(template,
                        PageCache.TableName_Model,
                        PageCache.DatabaseName,
                        assignContent.ToString(),
                        selectSqlContent.ToString());
                }
                #endregion

                return down_allModel_Str.ToString() + down_selectModel_Str.ToString();
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