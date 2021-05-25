using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class DALHelper_Dapper
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
using System.Linq;
using Dapper;

namespace {0}
{{
    public class {1}
    {{", name_space, dal_name);
        }

        public static string CreateAddMethod(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            if ((action & (int)action_type.add) == (int)action_type.add)
            {
                StringBuilder addContent = new StringBuilder();
                StringBuilder valueContent = new StringBuilder(") values (");
                addContent.AppendFormat("string insertSql = \"insert into {0}(", table_name);
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

                    index++;
                }

                addContent.Append(valueContent.ToString() + ")\";");
                string template = @"
        public bool Add{3}({0} model)
        {{
			{1}
            using (IDbConnection sqlcn = ConnectionFactory.{2})
            {{
                return sqlcn.Execute(insertSql, model) > 0;
            }}
        }}
";

                return string.Format(template,
                    table_name,
                    addContent.ToString(),
                    db_name,
                    table_name);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateBatAddMethod(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            if ((action & (int)action_type.bat_add) == (int)action_type.bat_add)
            {
                StringBuilder addContent = new StringBuilder();
                StringBuilder valueContent = new StringBuilder(") values (");
                addContent.AppendFormat("string insertSql = \"insert into {0}(", table_name);
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

                    index++;
                }

                addContent.Append(valueContent.ToString() + ")\";");
                string template = @"
        public void Add{3}_list(List<{0}> list)
        {{
			{1}
            using (IDbConnection sqlcn = ConnectionFactory.{2})
            {{
                foreach(var model in list)
                {{
                    sqlcn.Execute(insertSql, model);
                }}
            }}
        }}
";

                return string.Format(template,
                    table_name,
                    addContent.ToString(),
                    db_name,
                    table_name);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateEditMethod(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            if ((action & (int)action_type.edit) == (int)action_type.edit)
            {
                StringBuilder updateContent = new StringBuilder("string updateSql = \"update ");
                updateContent.AppendFormat(" {0} set ", table_name);
                int index = 0;
                var editList = Cache_VMData.GetVMList(table_name, VMType.Edit, colList.ToNotMainIdList());
                foreach (var item in colList.ToNotMainIdList())
                {
                    if (index == 0)
                    {
                        updateContent.AppendFormat("{0}=@{0}", item.Name);
                    }
                    else
                    {
                        updateContent.AppendFormat(",{0}=@{0}", item.Name);
                    }

                    index++;
                }

                updateContent.Append(" where ");
                updateContent.AppendFormat(" {0}=@{0} \";", colList.ToKeyId());

                string template = @"
        public bool Update{3}({0} model)
        {{
			{1}
            using (IDbConnection sqlcn = ConnectionFactory.{2})
            {{
                return sqlcn.Execute(updateSql, model) > 0;
            }}
        }}
";
                return string.Format(template,
                    table_name,
                    updateContent.ToString(),
                    db_name,
                    table_name);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateDeleteMethod(int action, string table_name, List<SqlColumnInfo> colList, string db_name)
        {
            if ((action & (int)action_type.delete) == (int)action_type.delete)
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
            using (IDbConnection sqlcn = ConnectionFactory.{2})
            {{
                return sqlcn.Execute(deleteSql) > 0;
            }}
        }}
";
                return string.Format(template,
                    table_name,
                    deleteContent.ToString(),
                    db_name);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateQueryListMethod(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            if ((action & (int)action_type.query_list) == (int)action_type.query_list)
            {
                StringBuilder queryWhereContent = new StringBuilder();
                StringBuilder queryListParams = new StringBuilder();
                StringBuilder setInfoContent = new StringBuilder();
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
                            queryWhereContent.AppendFormat("\t\t\tif ({0} >= 0)\r\n", item.Name);
                        }
                        else if (item.DbType.ToLower() == "tinyint")
                        {
                            queryWhereContent.AppendFormat("\t\t\tif ({0} >= 0)\r\n", item.Name);
                        }
                        else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                        {
                            queryWhereContent.AppendFormat("\t\t\tif ({0} != DateTime.MinValue)\r\n", item.Name);
                        }
                        else
                        {
                            queryWhereContent.AppendFormat("\t\t\tif (!string.IsNullOrEmpty({0}))\r\n", item.Name);
                        }
                    }

                    setInfoContent.AppendFormat("\t\t\tinfo.{0} = {0};\r\n", item.Name);
                    queryWhereContent.Append("\t\t\t{\r\n");
                    queryWhereContent.AppendFormat("\t\t\t\twhereStr += \" and {0}=@{0} \";\r\n", item.Name);
                    queryWhereContent.Append("\t\t\t}\r\n");
                    queryWhereContent.AppendLine();

                    index++;
                }

                StringBuilder selectSqlContent = new StringBuilder();
                selectSqlContent.Append("\tstring selectSql = string.Format(@\"select * from " + table_name + " where 1=1 {0} limit {1},{2};\", whereStr, ((page - 1) * pageSize), pageSize);\r\n");

                string template = @"
        public List<{0}> QueryList({1}int page, int pageSize)
        {{
            {0} info = new {0}();
{5}
            string whereStr = string.Empty;
            {2}
            {7}
            List<{0}> result = new List<{0}>();
            using (IDbConnection sqlcn = ConnectionFactory.{3})
            {{
                result = sqlcn.Query<{0}>(selectSql, info).ToList();
            }}

            return result;
        }}

        public int QueryListCount({4})
        {{
            {0} info = new {0}();
{5}
            string whereStr = string.Empty;
            {2}
            string selectCountSql = string.Format(""select count(0) from {6} where 1=1 {{0}} limit 1"", whereStr);
            using (IDbConnection sqlcn = ConnectionFactory.{3})
            {{
                return sqlcn.QuerySingle<int>(selectCountSql, info);
            }}
        }}
";

                string queryCountParams = queryListParams.Length > 0 ? queryListParams.ToString().Substring(0, queryListParams.Length - 1) : queryListParams.ToString();
                return string.Format(template,
                    table_name,
                    queryListParams.ToString(),
                    queryWhereContent.ToString(),
                    db_name,
                    queryCountParams,
                    setInfoContent.ToString(),
                    table_name,
                    selectSqlContent.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CreateGetAllAndPart(int action, string table_name, List<SqlColumnInfo> colList, string model_name, string db_name)
        {
            var down_allModel = (action | (int)action_type.export_all) == (int)action_type.export_all;
            var down_selelctModel = (action | (int)action_type.export_select) == (int)action_type.export_select;
            if (down_allModel || down_selelctModel)
            {
                StringBuilder down_allModel_Str = new StringBuilder();
                #region 导出全部
                if (down_allModel)
                {
                    StringBuilder queryWhereContent = new StringBuilder();
                    StringBuilder queryListParams = new StringBuilder();
                    StringBuilder setInfoContent = new StringBuilder();
                    int index = 0;
                    var queryList = Cache_VMData.GetVMList(table_name, VMType.Query, colList.ToNotMainIdList());
                    foreach (var item in queryList)
                    {
                        queryListParams.AppendFormat("{0} {1},", item.DbType.ToMsSqlClassType(), item.Name);

                        if (index == 0)
                        {
                            if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint")
                            {
                                queryWhereContent.AppendFormat("if ({0} > 0)\r\n", item.Name);
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
                            if (item.DbType.ToLower() == "int" || item.DbType.ToLower() == "bigint")
                            {
                                queryWhereContent.AppendFormat("\t\t\tif ({0} > 0)\r\n", item.Name);
                            }
                            else if (item.DbType.ToLower() == "tinyint")
                            {
                                queryWhereContent.AppendFormat("\t\t\tif ({0} >= 0)\r\n", item.Name);
                            }
                            else if (item.DbType.ToLower() == "datetime" || item.DbType.ToLower() == "date")
                            {
                                queryWhereContent.AppendFormat("\t\t\tif ({0} != DateTime.MinValue)\r\n", item.Name);
                            }
                            else
                            {
                                queryWhereContent.AppendFormat("\t\t\tif (!string.IsNullOrEmpty({0}))\r\n", item.Name);
                            }
                        }

                        setInfoContent.AppendFormat("\t\t\tinfo.{0} = {0};\r\n", item.Name);
                        queryWhereContent.Append("            {\r\n");
                        queryWhereContent.AppendFormat("                whereStr += \" and {0}=@{0} \";\r\n", item.Name);
                        queryWhereContent.Append("            }\r\n");
                        queryWhereContent.AppendLine();

                        index++;
                    }

                    string template = @"
        public List<{0}> GetAll({3})
        {{
            {0} info = new {0}();
{4}
            string whereStr = string.Empty;
{1}
            string selectAllSql = string.Format(""select * from {5} where 1=1 {{0}} limit 10000"", whereStr);
            List<{0}> result = new List<{0}>();
            using (IDbConnection sqlcn = ConnectionFactory.{2})
            {{
                result = sqlcn.Query<{0}>(selectAllSql, info).ToList();
            }}

            return result;
        }}
";

                    string queryCountParams = queryListParams.Length > 0 ? queryListParams.ToString().Substring(0, queryListParams.Length - 1) : queryListParams.ToString();
                    down_allModel_Str.AppendFormat(template,
                        table_name,
                        queryWhereContent.ToString(),
                        db_name,
                        queryCountParams,
                        setInfoContent.ToString(),
                        table_name);
                }
                #endregion

                StringBuilder down_selectModel_Str = new StringBuilder();
                #region 导出选中
                if (down_selelctModel)
                {
                    StringBuilder selectSqlContent = new StringBuilder();
                    selectSqlContent.Append("            string selectSql = string.Format(@\"select * from\r\n");
                    selectSqlContent.Append("            " + table_name + " where " + colList.ToKeyId() + " in ({1})\r\n");
                    selectSqlContent.Append("            {0} limit {2};\", whereStr, idArrayStr, idList.Count);\r\n");

                    string template = @"
        public List<{0}> GetPartAll(List<string> idList)
        {{
            var idArrayStr = string.Join("","", (from f in idList
                                select ""'""+ f + ""'"").ToArray());
            string whereStr = string.Empty;
{2}
            List<{0}> result = new List<{0}>();
            using (IDbConnection sqlcn = ConnectionFactory.{1})
            {{
                result = sqlcn.Query<{0}>(selectSql).ToList();
            }}

            return result;
        }}

";
                    down_selectModel_Str.AppendFormat(template,
                        table_name,
                        db_name,
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
