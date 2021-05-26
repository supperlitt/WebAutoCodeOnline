using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class DALHelper_DapperCore
    {
        private string db_name = string.Empty;
        private string name_space = string.Empty;
        private string dal_suffix = string.Empty;
        private string model_suffix = string.Empty;
        private string table_name = string.Empty;
        private string model_name = string.Empty;
        private string dal_name = string.Empty;
        private List<SqlColumnInfo> list = new List<SqlColumnInfo>();

        public DALHelper_DapperCore(string db_name, string name_space, string dal_suffix, string model_suffix)
        {
            this.db_name = db_name;
            this.name_space = name_space;
            this.dal_suffix = dal_suffix;
            this.model_suffix = model_suffix;
        }

        public string CreateDAL(string table_name, List<SqlColumnInfo> list)
        {
            this.table_name = table_name;
            this.model_name = table_name + model_suffix;
            this.dal_name = table_name + dal_suffix;
            this.list = list;

            StringBuilder dalContent = new StringBuilder();
            dalContent.Append(CreateDALHeader());
            dalContent.Append(CreateAddMethod());
            dalContent.Append(CreateBatAddMethod());
            dalContent.Append(CreateEditMethod());
            dalContent.Append(CreateDeleteMethod());
            dalContent.Append(CreateQueryListMethod());

            dalContent.Append(CreateBottom());

            return dalContent.ToString();
        }

        public string CreateDALHeader()
        {
            return string.Format(@"using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace {0}
{{
    public class {1}
    {{", name_space, dal_name);
        }

        public string CreateAddMethod()
        {
            StringBuilder keyContent = new StringBuilder();
            StringBuilder valueContent = new StringBuilder();
            int index = 0;
            foreach (var item in list.ToNotMainIdList())
            {
                if (index == 0)
                {
                    keyContent.Append(item.Name);
                    valueContent.Append("@" + item.Name);
                }
                else
                {
                    keyContent.Append(" ," + item.Name);
                    valueContent.Append(" ,@" + item.Name);
                }

                index++;
            }

            string addContent = string.Format(@"string insertSql = ""insert into {0} ({1}) values ({2});"";", table_name, keyContent.ToString(), valueContent.ToString());

            var checkList = Cache_VMData.GetAddCheckList(table_name, list.ToNotMainIdList());
            if (checkList.Count == 0)
            {
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
                    model_name,
                    addContent,
                    db_name,
                    table_name);
            }
            else
            {
                string select_text = SqlTextHelper.CreateSelectCountSql(table_name, checkList);
                string template = @"
        public bool Add{3}({0} model)
        {{
            {4}
			{1}
            using (IDbConnection sqlcn = ConnectionFactory.{2})
            {{
                int count = sqlcn.QuerySingleOrDefault<int>(selectSql, model);
                if(count == 0)
                {{
                    return sqlcn.Execute(insertSql, model) > 0;
                }}

                return false;
            }}
        }}
";

                return string.Format(template,
                    model_name,
                    addContent,
                    db_name,
                    table_name,
                    select_text);
            }
        }

        public string CreateBatAddMethod()
        {
            StringBuilder keyContent = new StringBuilder();
            StringBuilder valueContent = new StringBuilder();
            int index = 0;
            foreach (var item in list.ToNotMainIdList())
            {
                if (index == 0)
                {
                    keyContent.Append(item.Name);
                    valueContent.Append("@" + item.Name);
                }
                else
                {
                    keyContent.Append(" ," + item.Name);
                    valueContent.Append(" ,@" + item.Name);
                }

                index++;
            }

            string addContent = string.Format(@"string insertSql = ""insert into {0} ({1}) values ({2});"";", table_name, keyContent.ToString(), valueContent.ToString());

            var checkList = Cache_VMData.GetAddCheckList(table_name, list.ToNotMainIdList());
            if (checkList.Count == 0)
            {
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
                    model_name,
                    addContent,
                    db_name,
                    table_name);
            }
            else
            {
                string select_text = SqlTextHelper.CreateSelectCountSql(table_name, checkList);
                string template = @"
        public void Add{3}_list(List<{0}> list)
        {{
            {4}
			{1}
            using (IDbConnection sqlcn = ConnectionFactory.{2})
            {{
                sqlcn.Open();
                foreach(var model in list)
                {{
                    int count = sqlcn.QuerySingleOrDefault<int>(selectSql, model);
                    if(count == 0)
                    {{
                        sqlcn.Execute(insertSql, model);
                    }}
                }}
            }}
        }}
";

                return string.Format(template,
                    model_name,
                    addContent,
                    db_name,
                    table_name,
                    select_text);
            }
        }

        public string CreateEditMethod()
        {
            StringBuilder updateContent = new StringBuilder("string updateSql = \"update ");
            updateContent.AppendFormat(" {0} set ", table_name);
            int index = 0;
            foreach (var item in list)
            {
                if (item.IsMainKey)
                {
                    continue;
                }

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

            var keyInfo = list.Find(p => p.IsMainKey);
            if (keyInfo != null)
            {
                updateContent.AppendFormat(" where {0}=@{0} \";", keyInfo.Name);
            }
            else
            {
                updateContent.AppendFormat(" where {0}=@{0} \";", "主键");
            }

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
                model_name,
                updateContent.ToString(),
                db_name,
                table_name);
        }

        public string CreateDeleteMethod()
        {
            var deleteCol = Cache_VMData.GetDeleteColumnNameAndValue(table_name, list);
            if (deleteCol == null)
            {
                var keyInfo = list.Find(p => p.IsMainKey);

                // 物理删除
                string updateContent = string.Format(@"string deleteSql = ""delete from {0} where {1}=@{1} limit 1;"";",
                    table_name, (keyInfo == null ? "主键" : keyInfo.Name));

                string template = @"
        public bool Update{3}_delete({0} model)
        {{
			{1}
            using (IDbConnection sqlcn = ConnectionFactory.{2})
            {{
                return  sqlcn.Execute(deleteSql, model) > 0;
            }}
        }}
";
                return string.Format(template,
                    model_name,
                    updateContent,
                    db_name,
                    table_name);

            }
            else
            {
                var keyInfo = list.Find(p => p.IsMainKey);
                string updateContent = string.Format(@"string updateSql = ""update {0} set {1}={2} where {3}=@{3} limit 1;""",
                    table_name, deleteCol.Name, deleteCol.DefaultValue,
                    (keyInfo == null ? "主键" : keyInfo.Name));

                string template = @"
        public bool Update{3}_delete({0} model)
        {{
			{1}
            using (IDbConnection sqlcn = ConnectionFactory.{2})
            {{
                return  sqlcn.Execute(updateSql, model) > 0;
            }}
        }}
";
                return string.Format(template,
                    model_name,
                    updateContent,
                    db_name,
                    table_name);
            }
        }

        public string CreateQueryListMethod()
        {
            StringBuilder queryWhereContent = new StringBuilder();
            StringBuilder queryListParams = new StringBuilder();
            StringBuilder setInfoContent = new StringBuilder();
            int index = 0;
            foreach (var item in list)
            {
                if (item.IsMainKey)
                {
                    continue;
                }

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
                model_name,
                queryListParams.ToString(),
                queryWhereContent.ToString(),
                db_name,
                queryCountParams,
                setInfoContent.ToString(),
                table_name,
                selectSqlContent.ToString());
        }

        public string CreateBottom()
        {
            return @"    }
}";
        }
    }
}
