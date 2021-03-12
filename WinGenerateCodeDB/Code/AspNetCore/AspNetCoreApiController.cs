﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB.Code
{
    public class AspNetCoreApiController
    {
        private string name_space = string.Empty;
        private string dal_suffix = string.Empty;
        private string model_suffix = string.Empty;
        private string table_name = string.Empty;
        private string model_name = string.Empty;
        private string dal_name = string.Empty;
        private List<SqlColumnInfo> list = new List<SqlColumnInfo>();

        public AspNetCoreApiController(string name_space, string dal_suffix, string model_suffix)
        {
            this.name_space = name_space;
            this.dal_suffix = dal_suffix;
            this.model_suffix = model_suffix;
        }

        public string CreateApiController(string table_name, List<SqlColumnInfo> list)
        {
            this.table_name = table_name;
            this.model_name = table_name + model_suffix;
            this.dal_name = table_name + dal_suffix;
            this.list = list;

            StringBuilder dalContent = new StringBuilder();
            dalContent.Append(CreateHeader());
            dalContent.Append(CreateAddMethod());
            dalContent.Append(CreateEditMethod());
            dalContent.Append(CreateBatEditMethod());
            dalContent.Append(CreateQueryListMethod());

            dalContent.Append(CreateBottom());

            return dalContent.ToString();
        }

        public string CreateHeader()
        {
            return string.Format(@"using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAuth.Controllers
{{
    [ApiController]
    public class {0}Controller : ControllerBase
    {{", table_name.ToFirstUpper());
        }

        public string CreateAddMethod()
        {
            StringBuilder valueContent = new StringBuilder("");
            foreach (var item in list)
            {
                if (item.IsMainKey)
                {
                    continue;
                }

                valueContent.AppendFormat(@"                    {0} = model.{0},
", item.Name);
            }

            string template = @"
        [Route(""{0}/add"")]
        [HttpPost]
        public result_info<object> add_{0}([FromBody] {1} model)
        {{
            if (model != null)
            {{
                {2} dal = new {2}();
                var result = dal.Add{0}(new {1}()
                {{
{3}
                }});

                if (result)
                {{
                    return result_info<object>.success;
                }}
                else
                {{
                    return result_info<object>.fail;
                }}
            }}
            else
            {{
                return result_info<object>.data_null;
            }}
        }}
";

            return string.Format(template,
                table_name,
                model_name,
                dal_name,
                valueContent.ToString());
        }

        public string CreateEditMethod()
        {
            StringBuilder valueContent = new StringBuilder("");
            foreach (var item in list)
            {
                if (item.IsMainKey)
                {
                    continue;
                }

                valueContent.AppendFormat(@"                    {0} = model.{0},
", item.Name);
            }

            string template = @"
        [Route(""{0}/edit"")]
        [HttpPost]
        public result_info<object> edit_{0}([FromBody] {1} model)
        {{
            if (model != null)
            {{
                {2} dal = new {2}();
                var result = dal.Update{0}(new {1}()
                {{
{3}
                }});

                if (result)
                {{
                    return result_info<object>.success;
                }}
                else
                {{
                    return result_info<object>.fail;
                }}
            }}
            else
            {{
                return result_info<object>.data_null;
            }}
        }}
";

            return string.Format(template,
                table_name,
                model_name,
                dal_name,
                valueContent.ToString());
        }

        public string CreateBatEditMethod()
        {
            StringBuilder valueContent = new StringBuilder("");
            foreach (var item in list)
            {
                if (item.IsMainKey)
                {
                    continue;
                }

                valueContent.AppendFormat(@"                    {0} = model.{0},
", item.Name);
            }

            var keyInfo = list.Find(p => p.IsMainKey);
            if (keyInfo != null && (keyInfo.DbType.ToLower() == "int" || keyInfo.DbType.ToLower() == "bigint"))
            {
                string template = @"
        [Route(""{0}/batedit"")]
        [HttpPost]
        public result_info<object> batedit_{0}([FromBody] batedit_{1} model)
        {{
            if (model != null)
            {{
                var idList = (from f in model.ids.Split(new char[] {{ ',' }}, StringSplitOptions.RemoveEmptyEntries).ToList() select {4}.Parse(f)).ToList();
                {2} dal = new {2}();
                var result = dal.BatUpdate{0}(idList, new {1}()
                {{
{3}
                }});

                if (result)
                {{
                    return result_info<object>.success;
                }}
                else
                {{
                    return result_info<object>.fail;
                }}
            }}
            else
            {{
                return result_info<object>.data_null;
            }}
        }}
";

                return string.Format(template,
                    table_name,
                    model_name,
                    dal_name,
                    valueContent.ToString(),
                    keyInfo.DbType.ToLower() == "int" ? "int" : "long");
            }
            else
            {
                string template = @"
        [Route(""{0}/batedit"")]
        [HttpPost]
        public result_info<object> batedit_{0}([FromBody] bitedit_{1} model)
        {{
            if (model != null)
            {{
                var idList = model.ids.Split(new char[] {{ ',' }}, StringSplitOptions.RemoveEmptyEntries).ToList();
                {2} dal = new {2}();
                var result = dal.BatUpdate{0}(idList, new {1}()
                {{
{3}
                }});

                if (result)
                {{
                    return result_info<object>.success;
                }}
                else
                {{
                    return result_info<object>.fail;
                }}
            }}
            else
            {{
                return result_info<object>.data_null;
            }}
        }}
";

                return string.Format(template,
                    table_name,
                    model_name,
                    dal_name,
                    valueContent.ToString());
            }
        }

        public string CreateQueryListMethod()
        {
            StringBuilder queryContent1 = new StringBuilder("");
            StringBuilder queryContent2 = new StringBuilder("");
            foreach (var item in list)
            {
                if (item.IsMainKey)
                {
                    continue;
                }

                queryContent1.AppendFormat("model.{0},", item.Name);
                queryContent2.AppendFormat("model.{0},", item.Name);
            }

            if (queryContent2.Length > 0)
            {
                queryContent2.Remove(queryContent2.Length - 1, 1);
            }

            string template = @"
        [Route(""{0}/query_list"")]
        [HttpPost]
        public result_info<display_{1}> query_list_{0}([FromBody] query_{1} model)
        {{
            if (model != null)
            {{
                {2} dal = new {2}();
                var list = dal.QueryList({3}model.page, model.pageSize);
                int count = dal.QueryListCount({4});

                display_{1} result = new display_{1}();
                result.item_count = count;
                result.page_count = (int)Math.Ceiling((double)count / model.pageSize);
                result.list.AddRange(list);

                return result_info<display_{1}>.Success(result);
            }}
            else
            {{
                return result_info<display_{1}>.data_null;
            }}
        }}
";

            return string.Format(template,
                table_name,
                model_name,
                dal_name,
                queryContent1.ToString(),
                queryContent2.ToString());
        }

        public string CreateBottom()
        {
            return @"    }
}";
        }
    }
}
