using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAutoEasyUI
{
    public class EasyUIHelper : IHelper
    {
        public void ReleaseFile(TempModel tempModel)
        {
            string[] searchArray = tempModel.SearchColumns.Trim().Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string title = tempModel.Title.Trim();
            // 读取表名，读取内容，读取CheckListBox构造页面,构造Model,构造DAL层
            string pageName = string.Format("{0}Manager", tempModel.TableName);
            string modelName = tempModel.TableName;
            string dalName = string.Format("{0}DAL", tempModel.TableName);

            string nameSpace = tempModel.NameSpace.Trim();
            int tableWidth = tempModel.TableList.Count * 100 + 50;
            int formHeight = tempModel.TableList.Count * 35 + 50;

            // 小写开头的主键Id
            string keyId = string.Empty;

            // 大写开头的主键Id
            string KeyId = string.Empty;
            StringBuilder thinfo = new StringBuilder();
            StringBuilder formInfo = new StringBuilder();
            StringBuilder setValue = new StringBuilder();
            StringBuilder createParams = new StringBuilder();
            StringBuilder postData = new StringBuilder("\t\t\tvar postData = ");
            int index = 0;
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                // 初始化table显示数据
                if (item.IsMainKey == 1)
                {
                    thinfo.AppendFormat("<th data-options=\"field:'{0}'\" style=\"display: none;\">{1}</th>\r\n", attribute, item.Comment);
                }
                else
                {
                    thinfo.AppendFormat("<th data-options=\"field:'{0}',width:80,align:'center'\">{1}</th>\r\n", attribute, item.Comment);
                }

                // 初始化form显示数据
                if (item.IsMainKey == 1)
                {
                    keyId = item.ColumnName;
                    KeyId = item.ColumnName.ToFirstUpper();
                    formInfo.AppendFormat(@"<input type=""hidden"" id=""{0}"" value="""" />", field);
                }
                else
                {
                    // &#12288; 占一个中文字符
                    formInfo.AppendFormat(@"<div class=""fitem"">
                <label for=""{0}"">
                    {1}:</label>
                <input class=""easyui-validatebox"" type=""text"" id=""{0}"" name=""{0}""
                    data-options=""required:true"" />
            </div>", field, item.Comment.PadLeft(4, "&#12288;"));
                }

                setValue.AppendFormat("\t\t\t\t$(\"#{0}\").val(row.{1});\r\n", field, attribute);
                createParams.AppendFormat("\t\t\tvar {0} = $(\"#{0}\").val();\r\n", field);
                if (index == 0)
                {
                    postData.AppendFormat("\"{0}=\" + encodeURI({0}) ", field);
                }
                else
                {
                    postData.AppendFormat(" + \"&{0}=\" + encodeURI({0})", field);
                }

                index++;
            }

            postData.Append(";");

            StringBuilder searchContent = new StringBuilder();
            StringBuilder coditionContent = new StringBuilder();
            StringBuilder pagePost = new StringBuilder();
            StringBuilder queryParamsContent = new StringBuilder();
            StringBuilder queryWhereContent = new StringBuilder();
            StringBuilder queryData = new StringBuilder();
            StringBuilder queryListParams = new StringBuilder();
            StringBuilder queryListCountParams = new StringBuilder();
            /*
             <label for="txtOrderId">订单号：</label><input type="text" id="txtOrderId" /> <input type="button" id="btnsearch" value="查询" />
             */
            index = 0;
            foreach (var item in searchArray)
            {
                var model = tempModel.TableList.Find(p => p.ColumnName.ToLower() == item.ToLower());
                if (model != null)
                {
                    string field = model.ColumnName.ToFirstLower();
                    string attribute = model.ColumnName;

                    // 如果两个一样了。。。
                    if (field == attribute)
                    {
                        attribute = attribute.ToFirstUpper();
                    }

                    // 找到一个
                    searchContent.AppendFormat("&nbsp;&nbsp;<label for=\"txt{0}\">{1}：</label><input type=\"text\" id=\"txt{0}\" />", attribute, model.Comment);
                    coditionContent.AppendFormat("\t\t\tvar txt{0} = $(\"#txt{0}\").val();\r\n", attribute);

                    // var pagePost = "&xxx=" + xxx + "&yyy=" +yyy;
                    if (index == 0)
                    {
                        pagePost.AppendFormat("\t\t\tvar pageData = \"&{0}=\" + txt{0}", attribute);
                    }
                    else
                    {
                        pagePost.AppendFormat("+ \"&{0}=\" + txt{0}", attribute);
                    }

                    queryParamsContent.AppendFormat(", {0} {1}", Tool.ToClassType(model.DBType), field);
                    if (index == 0)
                    {
                        queryData.AppendFormat("string {0} = HttpUtility.UrlDecode(Request[\"{0}\"]);\r\n", field);
                    }
                    else
                    {
                        queryData.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"{0}\"]);\r\n", field);
                    }

                    queryData.AppendFormat("\t\t\t{0} {1}Data = {2};\r\n", Tool.ToClassType(model.DBType), field, Tool.ToStringToType(field, model.DBType));
                    if (index == 0)
                    {
                        queryListParams.AppendFormat(", {0}Data", field);
                        queryListCountParams.AppendFormat("{0}Data", field);
                    }
                    else
                    {
                        queryListParams.AppendFormat(", {0}Data", field);
                        queryListCountParams.AppendFormat(", {0}Data", field);
                    }

                    /*
                     if (!string.IsNullOrEmpty(orderId))
                    {
                        listParams.Add(new SqlParameter("@OrderId", SqlDbType.VarChar) { Value = orderId });
                        whereStr += " and OrderId=@OrderId ";
                    }
                     */

                    if (index == 0)
                    {
                        queryWhereContent.AppendFormat("if (!string.IsNullOrEmpty({0}))\r\n", field);
                    }
                    else
                    {
                        queryWhereContent.AppendFormat("\t\t\tif (!string.IsNullOrEmpty({0}))\r\n", field);
                    }

                    queryWhereContent.Append("\t\t\t{\r\n");
                    queryWhereContent.AppendFormat("\t\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", model.ColumnName, Tool.ToDBTypeString(model.DBType), field);
                    queryWhereContent.AppendFormat("\t\t\t\twhereStr += \" and {0}=@{0} \";", model.ColumnName);
                    queryWhereContent.Append("\t\t\t}\r\n");
                    queryWhereContent.AppendLine();

                    index++;
                }
            }

            string searchExtend = string.Empty;
            if (pagePost.ToString().Length > 0)
            {
                pagePost.Append(";");
                searchExtend = " + pageData";
            }

            if (searchContent.ToString() != string.Empty)
            {
                // 非空。添加查询按钮
                searchContent.Append("&nbsp;&nbsp;<input type=\"button\" id=\"btnsearch\" value=\"查询\" />");
            }

            // 构建Model类
            StringBuilder modelContent = new StringBuilder();
            index = 0;
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (index == 0)
                {
                    modelContent.Append("/// <summary>\r\n");
                }
                else
                {
                    modelContent.Append("\t\t/// <summary>\r\n");
                }

                modelContent.AppendFormat("\t\t/// {0}\r\n", item.Comment);
                modelContent.Append("\t\t/// </summary>\r\n");
                modelContent.AppendFormat("\t\tprivate {2} {0} = {1};\r\n", field, Tool.ToDefaultValue(item.DBType), Tool.ToClassType(item.DBType));
                modelContent.AppendLine();

                modelContent.Append("\t\t/// <summary>\r\n");
                modelContent.AppendFormat("\t\t/// {0}\r\n", item.Comment);
                modelContent.Append("\t\t/// </summary>\r\n");
                modelContent.AppendFormat("\t\tpublic {1} {0}\r\n", attribute, Tool.ToClassType(item.DBType));
                modelContent.Append("\t\t{\r\n");
                modelContent.AppendFormat("\t\t\tget {{ return this.{0}; }}\r\n", field);
                modelContent.AppendFormat("\t\t\tset {{ this.{0} = value; }}\r\n", field);
                modelContent.Append("\t\t}");
                if (tempModel.TableList.Count != index + 1)
                {
                    modelContent.AppendLine();
                    modelContent.AppendLine();
                }

                index++;
            }

            //  生成connectionfactory
            // 构建DAL类
            // 构建update数据
            StringBuilder updateContent = new StringBuilder("string updateSql = \"update top(1)");
            updateContent.AppendFormat(" {0} set ", tempModel.TableName);
            StringBuilder updateParamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            index = 0;
            StringBuilder addContent = new StringBuilder();
            StringBuilder valueContent = new StringBuilder(") values (");
            StringBuilder deleteKey = new StringBuilder();
            StringBuilder deleteContent = new StringBuilder();
            deleteContent.AppendFormat("string deleteSql = \"delete from {0} ", tempModel.TableName);
            StringBuilder deleteParamContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            StringBuilder addparamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            addContent.AppendFormat("string insertSql = \"insert {0}(", tempModel.TableName);
            var addlist = (from f in tempModel.TableList where f.IsMark == 0 select f).ToList();
            foreach (var item in addlist)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (index == 0)
                {
                    addContent.Append(item.ColumnName);
                    valueContent.Append("@" + item.ColumnName);

                    updateContent.AppendFormat("{0}=@{0}", item.ColumnName);
                }
                else
                {
                    addContent.Append(" ," + item.ColumnName);
                    valueContent.Append(" ,@" + item.ColumnName);
                    updateContent.AppendFormat(",{0}=@{0}", item.ColumnName);
                }

                addparamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), attribute);
                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), attribute);

                index++;
            }

            StringBuilder deleteStr = new StringBuilder();
            index = 0;
            var keylist = (from f in tempModel.TableList where f.IsMark == 1 select f).ToList();
            if (keylist.Count > 0)
            {
                updateContent.Append(" where 1=1 ");
                deleteContent.Append(" where 1=1 ");
                foreach (var item in keylist)
                {
                    string field = item.ColumnName.ToFirstLower();
                    string attribute = item.ColumnName;

                    // 如果两个一样了。。。
                    if (field == attribute)
                    {
                        attribute = attribute.ToFirstUpper();
                    }

                    updateContent.AppendFormat(" and {0}=@{0}", item.ColumnName);
                    updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), attribute);

                    deleteContent.AppendFormat(" and {0}=@{0}", item.ColumnName);
                    deleteParamContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), field);

                    if (index == 0)
                    {
                        deleteKey.AppendFormat("{0} {1}", Tool.ToClassType(item.DBType), field);
                    }
                    else
                    {
                        deleteKey.AppendFormat(", {0} {1}", Tool.ToClassType(item.DBType), field);
                    }

                    deleteStr.AppendFormat("{0} idData = {1};", Tool.ToClassType(item.DBType), Tool.ToStringToType("id", item.DBType));

                    index++;
                }
            }

            deleteContent.Append("\";");
            addContent.Append(valueContent.ToString() + ");\";");
            updateContent.Append("\";");

            var queryHead = string.Format("(int page, int pageSize{0})", queryParamsContent.ToString());
            var queryCountHead = string.Format("({0})", (queryParamsContent.ToString().Length > 0 ? queryParamsContent.ToString().Substring(1, queryParamsContent.ToString().Length - 1) : queryParamsContent.ToString()));
            var assignContent = new StringBuilder();
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (item.IsMark == 0)
                {
                    assignContent.AppendFormat("\t\t\t\t\t\tmodel.{0} = sqldr[\"{1}\"] == DBNull.Value ? {2} : {3};\r\n", attribute, item.ColumnName, Tool.ToDefaultValue(item.DBType), Tool.ToDefaultDBValue(item.DBType, item.ColumnName));
                }
                else
                {
                    assignContent.AppendFormat("model.{0} = sqldr[\"{1}\"] == DBNull.Value ? {2} : {3};\r\n", attribute, item.ColumnName, Tool.ToDefaultValue(item.DBType), Tool.ToDefaultDBValue(item.DBType, item.ColumnName));
                }
            }

            // 构建aspx.cs类
            StringBuilder addAndUpdteStr = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            index = 0;
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (index == 0)
                {
                    addAndUpdteStr.AppendFormat("string {0} = HttpUtility.UrlDecode(Request[\"{0}\"]);\r\n", field);
                    if (item.IsMainKey == 1)
                    {
                        createModel.AppendFormat("if (!string.IsNullOrEmpty({0}))\r\n", field);
                        createModel.Append("\t\t\t{\r\n");
                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, Tool.ToStringToType(field, item.DBType));
                        createModel.Append("\t\t\t}\r\n");
                    }
                    else
                    {
                        createModel.AppendFormat("model.{0} = {1};\r\n", attribute, Tool.ToStringToType(field, item.DBType));
                    }
                }
                else
                {
                    addAndUpdteStr.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"{0}\"]);\r\n", field);
                    createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, Tool.ToStringToType(field, item.DBType));
                }

                index++;
            }

            // 开始生成文件夹和替换内容
            string dir = tempModel.TargetDir;
            string modelPath = Path.Combine(dir, "Model");
            string dalPath = Path.Combine(dir, "DAL");
            string uiPath = Path.Combine(dir, "Web" + tempModel.DbName);

            string[] dirs = new string[] { modelPath, dalPath, uiPath };
            foreach (var item in dirs)
            {
                if (!Directory.Exists(item))
                {
                    Directory.CreateDirectory(item);
                }
            }

            // 模板文件所在位置
            string temppath = tempModel.SourceDir;
            string tempModelPath = Path.Combine(temppath, "model.txt");
            string tempDalPath = Path.Combine(temppath, "dal.txt");
            string tempFactoryPath = Path.Combine(temppath, "factory.txt");
            string tempAspxPath = Path.Combine(temppath, "aspx.txt");
            string tempAspxCsPath = Path.Combine(temppath, "aspx.cs.txt");
            string tempAspxDesignerCsPath = Path.Combine(temppath, "aspx.designer.cs.txt");
            string tempsqlhelperPath = Path.Combine(temppath, "sqlhelper.cs.txt");
            string tempwebconfig = Path.Combine(temppath, "web.config.txt");
            string tempassemblyinfo = Path.Combine(temppath, "assemblyinfo.cs.txt");

            // Model/Model.cs
            string oldNameSpace = nameSpace;
            if (tempModel.IsDenpendDLL)
            {
                nameSpace = "Model";
            }

            string resultModelPath = Path.Combine(modelPath, modelName + ".cs");
            string modelTxt = File.ReadAllText(tempModelPath, Encoding.UTF8);
            string resultModelTxt = modelTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{ModelName}}", modelName).Replace("{{ModelContent}}", modelContent.ToString());
            File.WriteAllText(resultModelPath, resultModelTxt, Encoding.UTF8);

            if (tempModel.IsDenpendDLL)
            {
                nameSpace = "DAL";
            }

            // DAL/ModelDAL.cs
            string resultDalPath = Path.Combine(dalPath, dalName + ".cs");
            string resultFactoryPath = Path.Combine(dalPath, "ConnectionFactory.cs");
            string dalTxt = File.ReadAllText(tempDalPath, Encoding.UTF8);
            string resultDalTxt = dalTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{DALName}}", dalName).Replace("{{ClassName}}", tempModel.TableName.ToFirstUpper()).Replace("{{ModelName}}", modelName).Replace("{{AddContent}}", addContent.ToString()).Replace("{{AddParamsContent}}", addparamsContent.ToString()).Replace("{{DataBase}}", tempModel.DbName).Replace("{{UpdateContent}}", updateContent.ToString()).Replace("{{UpdateParamsContent}}", updateParamsContent.ToString()).Replace("{{QueryHead}}", queryHead).Replace("{{QueryCountHead}}", queryCountHead).Replace("{{QueryWhere}}", queryWhereContent.ToString()).Replace("{{TableName}}", tempModel.TableName).Replace("{{AssignContent}}", assignContent.ToString()).Replace("{{QueryData}}", queryData.ToString()).Replace("{{QueryParamsContent}}", queryParamsContent.ToString()).Replace("{{keyId}}", keyId).Replace("{{DeleteKey}}", deleteKey.ToString()).Replace("{{DeleteContent}}", deleteContent.ToString()).Replace("{{DeleteParamsContent}}", deleteParamContent.ToString());
            File.WriteAllText(resultDalPath, resultDalTxt, Encoding.UTF8);

            // DAL/ConnectionFactory.cs
            string factoryTxt = File.ReadAllText(tempFactoryPath, Encoding.UTF8);
            string resultFactoryTxt = factoryTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{DataBase}}", tempModel.DbName).Replace("{{ConnectionString}}", tempModel.ConnectionStr);
            File.WriteAllText(resultFactoryPath, resultFactoryTxt, Encoding.UTF8);

            nameSpace = oldNameSpace;
            // UI/XXXManager.aspx
            string resultAspxPath = Path.Combine(uiPath, pageName + ".aspx");
            string resultAspxDesignerPath = Path.Combine(uiPath, pageName + ".aspx.designer.cs");
            string resultAspxCsPath = Path.Combine(uiPath, pageName + ".aspx.cs");

            string aspxTxt = File.ReadAllText(tempAspxPath, Encoding.UTF8);
            string resultAspxTxt = aspxTxt.Replace("{{PageName}}", pageName).Replace("{{NameSpace}}", nameSpace).Replace("{{Title}}", title).Replace("{{SearchContent}}", searchContent.ToString()).Replace("{{ThInfo}}", thinfo.ToString()).Replace("{{FormHeight}}", formHeight.ToString()).Replace("{{FormInfo}}", formInfo.ToString()).Replace("{{SetValue}}", setValue.ToString()).Replace("{{CreateParams}}", createParams.ToString()).Replace("{{POSTDATA}}", postData.ToString()).Replace("{{KeyId}}", KeyId).Replace("{{keyId}}", keyId).Replace("{{Condition}}", coditionContent.ToString()).Replace("{{PagePost}}", pagePost.ToString()).Replace("{{TableWidth}}", tableWidth.ToString()).Replace("{{SearchExtend}}", searchExtend);
            File.WriteAllText(resultAspxPath, resultAspxTxt, Encoding.UTF8);

            // UI/XXXManager.aspx.designer.cs
            string aspxdesignerTxt = File.ReadAllText(tempAspxDesignerCsPath, Encoding.UTF8);
            string resultAspxDesignerTxt = aspxdesignerTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{ClassName}}", pageName);
            File.WriteAllText(resultAspxDesignerPath, resultAspxDesignerTxt, Encoding.UTF8);

            if (tempModel.IsDenpendDLL)
            {
                // 写入 web.config配置文件
                File.WriteAllText(Path.Combine(uiPath, "Web.config"), File.ReadAllText(tempwebconfig, Encoding.UTF8), Encoding.UTF8);
            }
            else
            {
                // 写入 web.config配置文件
                File.WriteAllText(Path.Combine(dir, "Web.config"), File.ReadAllText(tempwebconfig, Encoding.UTF8), Encoding.UTF8);
            }

            // UI/XXXManager.aspx.cs
            string aspxCsTxt = File.ReadAllText(tempAspxCsPath, Encoding.UTF8);
            string resultAspxCsTxt = aspxCsTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{PageName}}", pageName).Replace("{{AddAndUpdteStr}}", addAndUpdteStr.ToString()).Replace("{{ModelName}}", modelName).Replace("{{CreateModel}}", createModel.ToString()).Replace("{{DALName}}", dalName).Replace("{{keyId}}", keyId.ToFirstLower()).Replace("{{ClassName}}", tempModel.TableName.ToFirstUpper()).Replace("{{QueryData}}", queryData.ToString()).Replace("{{QueryListCount}}", queryListCountParams.ToString()).Replace("{{QueryList}}", queryListParams.ToString()).Replace("{{DeleteStr}}", deleteStr.ToString());
            File.WriteAllText(resultAspxCsPath, resultAspxCsTxt, Encoding.UTF8);

            // 复制sqlhelper 
            string resultSqlHelperPath = Path.Combine(dalPath, "SqlHelper.cs");
            string resultSqlHelperTxt = File.ReadAllText(tempsqlhelperPath, Encoding.UTF8);
            File.WriteAllText(resultSqlHelperPath, resultSqlHelperTxt.Replace("{{NameSpace}}", nameSpace), Encoding.UTF8);

            // 复制 properties/assemblyinfo.cs文件
            if (tempModel.IsDenpendDLL)
            {
                List<string> listdir = new List<string>() { modelPath, dalPath, uiPath };
                foreach (var fdir in listdir)
                {
                    // 独立的需要创建三个dll
                    var ffolder = Path.Combine(fdir, "Properties");
                    if (!Directory.Exists(ffolder))
                    {
                        Directory.CreateDirectory(ffolder);
                    }

                    // 写入文件到folder
                    string resultAssemblyInfoPath = Path.Combine(ffolder, "AssemblyInfo.cs");
                    string resultAssemblyInfoTxt = File.ReadAllText(tempassemblyinfo, Encoding.UTF8);
                    File.WriteAllText(resultAssemblyInfoPath, resultAssemblyInfoTxt.Replace("{{Title}}", fdir.Substring(fdir.LastIndexOf("\\") + 1, fdir.Length - fdir.LastIndexOf("\\") - 1)).Replace("{{Year}}", DateTime.Now.Year.ToString()).Replace("{{Guid}}", Guid.NewGuid().ToString()), Encoding.UTF8);
                }
            }
            else
            {
                var ffolder = Path.Combine(dir, "Properties");
                if (!Directory.Exists(ffolder))
                {
                    Directory.CreateDirectory(ffolder);
                }

                // 写入文件到folder
                string resultAssemblyInfoPath = Path.Combine(ffolder, "AssemblyInfo.cs");
                string resultAssemblyInfoTxt = File.ReadAllText(tempassemblyinfo, Encoding.UTF8);
                File.WriteAllText(resultAssemblyInfoPath, resultAssemblyInfoTxt.Replace("{{Title}}", nameSpace).Replace("{{Year}}", DateTime.Now.Year.ToString()).Replace("{{Guid}}", Guid.NewGuid().ToString()), Encoding.UTF8);
            }
        }

        //TODO:拆分一下，分成UI，Model和DAL三部分，先。

        /// <summary>
        /// 创建Model类
        /// </summary>
        /// <param name="tempModel">模板对象</param>
        public void CreateModel(TempModel tempModel)
        {
            string modelName = tempModel.TableName.ToFirstUpper();
            string nameSpace = tempModel.NameSpace.Trim();

            // 构建Model类
            StringBuilder modelContent = new StringBuilder();
            int index = 0;
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (index == 0)
                {
                    modelContent.Append("/// <summary>\r\n");
                }
                else
                {
                    modelContent.Append("\t\t/// <summary>\r\n");
                }

                modelContent.AppendFormat("\t\t/// {0}\r\n", item.Comment);
                modelContent.Append("\t\t/// </summary>\r\n");
                modelContent.AppendFormat("\t\tprivate {2} {0} = {1};\r\n", field, Tool.ToDefaultValue(item.DBType), Tool.ToClassType(item.DBType));
                modelContent.AppendLine();

                modelContent.Append("\t\t/// <summary>\r\n");
                modelContent.AppendFormat("\t\t/// {0}\r\n", item.Comment);
                modelContent.Append("\t\t/// </summary>\r\n");
                modelContent.AppendFormat("\t\tpublic {1} {0}\r\n", attribute, Tool.ToClassType(item.DBType));
                modelContent.Append("\t\t{\r\n");
                modelContent.AppendFormat("\t\t\tget {{ return this.{0}; }}\r\n", field);
                modelContent.AppendFormat("\t\t\tset {{ this.{0} = value; }}\r\n", field);
                modelContent.Append("\t\t}");
                if (tempModel.TableList.Count != index + 1)
                {
                    modelContent.AppendLine();
                    modelContent.AppendLine();
                }

                index++;
            }

            // Model/Model.cs
            if (tempModel.IsDenpendDLL)
            {
                nameSpace = "Model";
            }

            string temppath = tempModel.SourceDir;
            string tempModelPath = Path.Combine(temppath, "model.txt");
            string dir = tempModel.TargetDir;
            string modelPath = Path.Combine(dir, "Model");

            string[] dirs = new string[] { modelPath };
            foreach (var item in dirs)
            {
                if (!Directory.Exists(item))
                {
                    Directory.CreateDirectory(item);
                }
            }

            string resultModelPath = Path.Combine(modelPath, modelName + ".cs");
            string modelTxt = File.ReadAllText(tempModelPath, Encoding.UTF8);
            string resultModelTxt = modelTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{ModelName}}", modelName).Replace("{{ModelContent}}", modelContent.ToString());
            File.WriteAllText(resultModelPath, resultModelTxt, Encoding.UTF8);
        }

        /// <summary>
        /// 创建DAL类对象
        /// </summary>
        /// <param name="tempModel">模板类对象</param>
        public void CreateDAL(TempModel tempModel)
        {
            string[] searchArray = tempModel.SearchColumns.Trim().Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string title = tempModel.Title.Trim();
            // 读取表名，读取内容，读取CheckListBox构造页面,构造Model,构造DAL层
            string modelName = tempModel.TableName;
            string dalName = string.Format("{0}DAL", tempModel.TableName);

            string nameSpace = tempModel.NameSpace.Trim();

            // 小写开头的主键Id
            string keyId = string.Empty;

            // 大写开头的主键Id
            StringBuilder queryParamsContent = new StringBuilder();
            StringBuilder queryWhereContent = new StringBuilder();
            StringBuilder queryData = new StringBuilder();
            /*
             <label for="txtOrderId">订单号：</label><input type="text" id="txtOrderId" /> <input type="button" id="btnsearch" value="查询" />
             */
            int index = 0;
            foreach (var item in searchArray)
            {
                var model = tempModel.TableList.Find(p => p.ColumnName.ToLower() == item.ToLower());
                if (model != null)
                {
                    string field = model.ColumnName.ToFirstLower();
                    string attribute = model.ColumnName;

                    // 如果两个一样了。。。
                    if (field == attribute)
                    {
                        attribute = attribute.ToFirstUpper();
                    }

                    if (index == 0)
                    {
                        queryWhereContent.AppendFormat("if (!string.IsNullOrEmpty({0}))\r\n", field);
                    }
                    else
                    {
                        queryWhereContent.AppendFormat("\t\t\tif (!string.IsNullOrEmpty({0}))\r\n", field);
                    }

                    queryWhereContent.Append("\t\t\t{\r\n");
                    queryWhereContent.AppendFormat("\t\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", model.ColumnName, Tool.ToDBTypeString(model.DBType), field);
                    queryWhereContent.AppendFormat("\t\t\t\twhereStr += \" and {0}=@{0} \";", model.ColumnName);
                    queryWhereContent.Append("\t\t\t}\r\n");
                    queryWhereContent.AppendLine();

                    index++;
                }
            }

            //  生成connectionfactory
            // 构建DAL类
            // 构建update数据
            StringBuilder updateContent = new StringBuilder("string updateSql = \"update top(1)");
            updateContent.AppendFormat(" {0} set ", tempModel.TableName);
            StringBuilder updateParamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            index = 0;
            StringBuilder addContent = new StringBuilder();
            StringBuilder valueContent = new StringBuilder(") values (");
            StringBuilder deleteKey = new StringBuilder();
            StringBuilder deleteContent = new StringBuilder();
            deleteContent.AppendFormat("string deleteSql = \"delete from {0} ", tempModel.TableName);
            StringBuilder deleteParamContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            StringBuilder addparamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            addContent.AppendFormat("string insertSql = \"insert {0}(", tempModel.TableName);
            var addlist = (from f in tempModel.TableList where f.IsMark == 0 select f).ToList();
            foreach (var item in addlist)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (index == 0)
                {
                    addContent.Append(item.ColumnName);
                    valueContent.Append("@" + item.ColumnName);

                    updateContent.AppendFormat("{0}=@{0}", item.ColumnName);
                }
                else
                {
                    addContent.Append(" ," + item.ColumnName);
                    valueContent.Append(" ,@" + item.ColumnName);
                    updateContent.AppendFormat(",{0}=@{0}", item.ColumnName);
                }

                addparamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), attribute);
                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), attribute);

                index++;
            }

            StringBuilder deleteStr = new StringBuilder();
            index = 0;
            var keylist = (from f in tempModel.TableList where f.IsMark == 1 select f).ToList();
            if (keylist.Count > 0)
            {
                updateContent.Append(" where 1=1 ");
                deleteContent.Append(" where 1=1 ");
                foreach (var item in keylist)
                {
                    string field = item.ColumnName.ToFirstLower();
                    string attribute = item.ColumnName;

                    // 如果两个一样了。。。
                    if (field == attribute)
                    {
                        attribute = attribute.ToFirstUpper();
                    }

                    keyId = field;

                    updateContent.AppendFormat(" and {0}=@{0}", item.ColumnName);
                    updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), attribute);

                    deleteContent.AppendFormat(" and {0}=@{0}", item.ColumnName);
                    deleteParamContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), field);

                    if (index == 0)
                    {
                        deleteKey.AppendFormat("{0} {1}", Tool.ToClassType(item.DBType), field);
                    }
                    else
                    {
                        deleteKey.AppendFormat(", {0} {1}", Tool.ToClassType(item.DBType), field);
                    }

                    deleteStr.AppendFormat("{0} idData = {1};", Tool.ToClassType(item.DBType), Tool.ToStringToType("id", item.DBType));

                    index++;
                }
            }

            deleteContent.Append("\";");
            addContent.Append(valueContent.ToString() + ");\";");
            updateContent.Append("\";");

            var queryHead = string.Format("(int page, int pageSize{0})", queryParamsContent.ToString());
            var queryCountHead = string.Format("({0})", (queryParamsContent.ToString().Length > 0 ? queryParamsContent.ToString().Substring(1, queryParamsContent.ToString().Length - 1) : queryParamsContent.ToString()));
            var assignContent = new StringBuilder();
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (item.IsMark == 0)
                {
                    assignContent.AppendFormat("\t\t\t\t\t\tmodel.{0} = sqldr[\"{1}\"] == DBNull.Value ? {2} : {3};\r\n", attribute, item.ColumnName, Tool.ToDefaultValue(item.DBType), Tool.ToDefaultDBValue(item.DBType, item.ColumnName));
                }
                else
                {
                    assignContent.AppendFormat("model.{0} = sqldr[\"{1}\"] == DBNull.Value ? {2} : {3};\r\n", attribute, item.ColumnName, Tool.ToDefaultValue(item.DBType), Tool.ToDefaultDBValue(item.DBType, item.ColumnName));
                }
            }

            // 开始生成文件夹和替换内容
            string dir = tempModel.TargetDir;
            string modelPath = Path.Combine(dir, "Model");
            string dalPath = Path.Combine(dir, "DAL");

            string[] dirs = new string[] { dalPath };
            foreach (var item in dirs)
            {
                if (!Directory.Exists(item))
                {
                    Directory.CreateDirectory(item);
                }
            }

            // 模板文件所在位置
            string temppath = tempModel.SourceDir;
            string tempModelPath = Path.Combine(temppath, "model.txt");
            string tempDalPath = Path.Combine(temppath, "dal.txt");
            string tempFactoryPath = Path.Combine(temppath, "factory.txt");
            string tempsqlhelperPath = Path.Combine(temppath, "sqlhelper.cs.txt");

            if (tempModel.IsDenpendDLL)
            {
                nameSpace = "DAL";
            }

            // DAL/ModelDAL.cs
            string resultDalPath = Path.Combine(dalPath, dalName + ".cs");
            string resultFactoryPath = Path.Combine(dalPath, "ConnectionFactory.cs");
            string dalTxt = File.ReadAllText(tempDalPath, Encoding.UTF8);
            string resultDalTxt = dalTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{DALName}}", dalName).Replace("{{ClassName}}", tempModel.TableName.ToFirstUpper()).Replace("{{ModelName}}", modelName).Replace("{{AddContent}}", addContent.ToString()).Replace("{{AddParamsContent}}", addparamsContent.ToString()).Replace("{{DataBase}}", tempModel.DbName).Replace("{{UpdateContent}}", updateContent.ToString()).Replace("{{UpdateParamsContent}}", updateParamsContent.ToString()).Replace("{{QueryHead}}", queryHead).Replace("{{QueryCountHead}}", queryCountHead).Replace("{{QueryWhere}}", queryWhereContent.ToString()).Replace("{{TableName}}", tempModel.TableName).Replace("{{AssignContent}}", assignContent.ToString()).Replace("{{QueryData}}", queryData.ToString()).Replace("{{QueryParamsContent}}", queryParamsContent.ToString()).Replace("{{keyId}}", keyId).Replace("{{DeleteKey}}", deleteKey.ToString()).Replace("{{DeleteContent}}", deleteContent.ToString()).Replace("{{DeleteParamsContent}}", deleteParamContent.ToString());
            File.WriteAllText(resultDalPath, resultDalTxt, Encoding.UTF8);

            // DAL/ConnectionFactory.cs
            string factoryTxt = File.ReadAllText(tempFactoryPath, Encoding.UTF8);
            string resultFactoryTxt = factoryTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{DataBase}}", tempModel.DbName).Replace("{{ConnectionString}}", tempModel.ConnectionStr);
            File.WriteAllText(resultFactoryPath, resultFactoryTxt, Encoding.UTF8);

            // 复制sqlhelper 
            string resultSqlHelperPath = Path.Combine(dalPath, "SqlHelper.cs");
            string resultSqlHelperTxt = File.ReadAllText(tempsqlhelperPath, Encoding.UTF8);
            File.WriteAllText(resultSqlHelperPath, resultSqlHelperTxt.Replace("{{NameSpace}}", nameSpace), Encoding.UTF8);
        }

        /// <summary>
        /// 创建UI关联对象
        /// </summary>
        /// <param name="tempModel">模板类对象</param>
        public void CreateUI(TempModel tempModel)
        {
            string[] searchArray = tempModel.SearchColumns.Trim().Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string title = tempModel.Title.Trim();
            // 读取表名，读取内容，读取CheckListBox构造页面,构造Model,构造DAL层
            string pageName = string.Format("{0}Manager", tempModel.TableName);
            string modelName = tempModel.TableName;
            string dalName = string.Format("{0}DAL", tempModel.TableName);

            string nameSpace = tempModel.NameSpace.Trim();
            int tableWidth = tempModel.TableList.Count * 100 + 50;
            int formHeight = tempModel.TableList.Count * 35 + 50;

            // 小写开头的主键Id
            string keyId = string.Empty;

            // 大写开头的主键Id
            string KeyId = string.Empty;
            StringBuilder thinfo = new StringBuilder();
            StringBuilder formInfo = new StringBuilder();
            StringBuilder setValue = new StringBuilder();
            StringBuilder createParams = new StringBuilder();
            StringBuilder postData = new StringBuilder("\t\t\tvar postData = ");
            int index = 0;
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                // 初始化table显示数据
                if (item.IsMainKey == 1)
                {
                    thinfo.AppendFormat("<th data-options=\"field:'{0}'\" style=\"display: none;\">{1}</th>\r\n", attribute, item.Comment);
                }
                else
                {
                    thinfo.AppendFormat("<th data-options=\"field:'{0}',width:80,align:'center'\">{1}</th>\r\n", attribute, item.Comment);
                }

                // 初始化form显示数据
                if (item.IsMainKey == 1)
                {
                    keyId = item.ColumnName;
                    KeyId = item.ColumnName.ToFirstUpper();
                    formInfo.AppendFormat(@"<input type=""hidden"" id=""{0}"" value="""" />", field);
                }
                else
                {
                    // &#12288; 占一个中文字符
                    formInfo.AppendFormat(@"<div class=""fitem"">
                <label for=""{0}"">
                    {1}:</label>
                <input class=""easyui-validatebox"" type=""text"" id=""{0}"" name=""{0}""
                    data-options=""required:true"" />
            </div>", field, item.Comment.PadLeft(4, "&#12288;"));
                }

                setValue.AppendFormat("\t\t\t\t$(\"#{0}\").val(row.{1});\r\n", field, attribute);
                createParams.AppendFormat("\t\t\tvar {0} = $(\"#{0}\").val();\r\n", field);
                if (index == 0)
                {
                    postData.AppendFormat("\"{0}=\" + encodeURI({0}) ", field);
                }
                else
                {
                    postData.AppendFormat(" + \"&{0}=\" + encodeURI({0})", field);
                }

                index++;
            }

            postData.Append(";");

            StringBuilder searchContent = new StringBuilder();
            StringBuilder coditionContent = new StringBuilder();
            StringBuilder pagePost = new StringBuilder();
            StringBuilder queryParamsContent = new StringBuilder();
            StringBuilder queryWhereContent = new StringBuilder();
            StringBuilder queryData = new StringBuilder();
            StringBuilder queryListParams = new StringBuilder();
            StringBuilder queryListCountParams = new StringBuilder();
            /*
             <label for="txtOrderId">订单号：</label><input type="text" id="txtOrderId" /> <input type="button" id="btnsearch" value="查询" />
             */
            index = 0;
            foreach (var item in searchArray)
            {
                var model = tempModel.TableList.Find(p => p.ColumnName.ToLower() == item.ToLower());
                if (model != null)
                {
                    string field = model.ColumnName.ToFirstLower();
                    string attribute = model.ColumnName;

                    // 如果两个一样了。。。
                    if (field == attribute)
                    {
                        attribute = attribute.ToFirstUpper();
                    }

                    // 找到一个
                    searchContent.AppendFormat("&nbsp;&nbsp;<label for=\"txt{0}\">{1}：</label><input type=\"text\" id=\"txt{0}\" />", attribute, model.Comment);
                    coditionContent.AppendFormat("\t\t\tvar txt{0} = $(\"#txt{0}\").val();\r\n", attribute);

                    // var pagePost = "&xxx=" + xxx + "&yyy=" +yyy;
                    if (index == 0)
                    {
                        pagePost.AppendFormat("\t\t\tvar pageData = \"&{0}=\" + txt{0}", attribute);
                    }
                    else
                    {
                        pagePost.AppendFormat("+ \"&{0}=\" + txt{0}", attribute);
                    }

                    queryParamsContent.AppendFormat(", {0} {1}", Tool.ToClassType(model.DBType), field);
                    if (index == 0)
                    {
                        queryData.AppendFormat("string {0} = HttpUtility.UrlDecode(Request[\"{0}\"]);\r\n", field);
                    }
                    else
                    {
                        queryData.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"{0}\"]);\r\n", field);
                    }

                    queryData.AppendFormat("\t\t\t{0} {1}Data = {2};\r\n", Tool.ToClassType(model.DBType), field, Tool.ToStringToType(field, model.DBType));
                    if (index == 0)
                    {
                        queryListParams.AppendFormat(", {0}Data", field);
                        queryListCountParams.AppendFormat("{0}Data", field);
                    }
                    else
                    {
                        queryListParams.AppendFormat(", {0}Data", field);
                        queryListCountParams.AppendFormat(", {0}Data", field);
                    }

                    /*
                     if (!string.IsNullOrEmpty(orderId))
                    {
                        listParams.Add(new SqlParameter("@OrderId", SqlDbType.VarChar) { Value = orderId });
                        whereStr += " and OrderId=@OrderId ";
                    }
                     */

                    if (index == 0)
                    {
                        queryWhereContent.AppendFormat("if (!string.IsNullOrEmpty({0}))\r\n", field);
                    }
                    else
                    {
                        queryWhereContent.AppendFormat("\t\t\tif (!string.IsNullOrEmpty({0}))\r\n", field);
                    }

                    queryWhereContent.Append("\t\t\t{\r\n");
                    queryWhereContent.AppendFormat("\t\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", model.ColumnName, Tool.ToDBTypeString(model.DBType), field);
                    queryWhereContent.AppendFormat("\t\t\t\twhereStr += \" and {0}=@{0} \";", model.ColumnName);
                    queryWhereContent.Append("\t\t\t}\r\n");
                    queryWhereContent.AppendLine();

                    index++;
                }
            }

            string searchExtend = string.Empty;
            if (pagePost.ToString().Length > 0)
            {
                pagePost.Append(";");
                searchExtend = " + pageData";
            }

            if (searchContent.ToString() != string.Empty)
            {
                // 非空。添加查询按钮
                searchContent.Append("&nbsp;&nbsp;<input type=\"button\" id=\"btnsearch\" value=\"查询\" />");
            }

            // 构建Model类
            StringBuilder modelContent = new StringBuilder();
            index = 0;
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (index == 0)
                {
                    modelContent.Append("/// <summary>\r\n");
                }
                else
                {
                    modelContent.Append("\t\t/// <summary>\r\n");
                }

                modelContent.AppendFormat("\t\t/// {0}\r\n", item.Comment);
                modelContent.Append("\t\t/// </summary>\r\n");
                modelContent.AppendFormat("\t\tprivate {2} {0} = {1};\r\n", field, Tool.ToDefaultValue(item.DBType), Tool.ToClassType(item.DBType));
                modelContent.AppendLine();

                modelContent.Append("\t\t/// <summary>\r\n");
                modelContent.AppendFormat("\t\t/// {0}\r\n", item.Comment);
                modelContent.Append("\t\t/// </summary>\r\n");
                modelContent.AppendFormat("\t\tpublic {1} {0}\r\n", attribute, Tool.ToClassType(item.DBType));
                modelContent.Append("\t\t{\r\n");
                modelContent.AppendFormat("\t\t\tget {{ return this.{0}; }}\r\n", field);
                modelContent.AppendFormat("\t\t\tset {{ this.{0} = value; }}\r\n", field);
                modelContent.Append("\t\t}");
                if (tempModel.TableList.Count != index + 1)
                {
                    modelContent.AppendLine();
                    modelContent.AppendLine();
                }

                index++;
            }

            //  生成connectionfactory
            // 构建DAL类
            // 构建update数据
            StringBuilder updateContent = new StringBuilder("string updateSql = \"update top(1)");
            updateContent.AppendFormat(" {0} set ", tempModel.TableName);
            StringBuilder updateParamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");

            index = 0;
            StringBuilder addContent = new StringBuilder();
            StringBuilder valueContent = new StringBuilder(") values (");
            StringBuilder deleteKey = new StringBuilder();
            StringBuilder deleteContent = new StringBuilder();
            deleteContent.AppendFormat("string deleteSql = \"delete from {0} ", tempModel.TableName);
            StringBuilder deleteParamContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            StringBuilder addparamsContent = new StringBuilder("List<SqlParameter> listParams = new List<SqlParameter>();\r\n");
            addContent.AppendFormat("string insertSql = \"insert {0}(", tempModel.TableName);
            var addlist = (from f in tempModel.TableList where f.IsMark == 0 select f).ToList();
            foreach (var item in addlist)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (index == 0)
                {
                    addContent.Append(item.ColumnName);
                    valueContent.Append("@" + item.ColumnName);

                    updateContent.AppendFormat("{0}=@{0}", item.ColumnName);
                }
                else
                {
                    addContent.Append(" ," + item.ColumnName);
                    valueContent.Append(" ,@" + item.ColumnName);
                    updateContent.AppendFormat(",{0}=@{0}", item.ColumnName);
                }

                addparamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), attribute);
                updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), attribute);

                index++;
            }

            StringBuilder deleteStr = new StringBuilder();
            index = 0;
            var keylist = (from f in tempModel.TableList where f.IsMark == 1 select f).ToList();
            if (keylist.Count > 0)
            {
                updateContent.Append(" where 1=1 ");
                deleteContent.Append(" where 1=1 ");
                foreach (var item in keylist)
                {
                    string field = item.ColumnName.ToFirstLower();
                    string attribute = item.ColumnName;

                    // 如果两个一样了。。。
                    if (field == attribute)
                    {
                        attribute = attribute.ToFirstUpper();
                    }

                    updateContent.AppendFormat(" and {0}=@{0}", item.ColumnName);
                    updateParamsContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = model.{2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), attribute);

                    deleteContent.AppendFormat(" and {0}=@{0}", item.ColumnName);
                    deleteParamContent.AppendFormat("\t\t\tlistParams.Add(new SqlParameter(\"@{0}\", {1}) {{ Value = {2} }});\r\n", item.ColumnName, Tool.ToDBTypeString(item.DBType), field);

                    if (index == 0)
                    {
                        deleteKey.AppendFormat("{0} {1}", Tool.ToClassType(item.DBType), field);
                    }
                    else
                    {
                        deleteKey.AppendFormat(", {0} {1}", Tool.ToClassType(item.DBType), field);
                    }

                    deleteStr.AppendFormat("{0} idData = {1};", Tool.ToClassType(item.DBType), Tool.ToStringToType("id", item.DBType));

                    index++;
                }
            }

            deleteContent.Append("\";");
            addContent.Append(valueContent.ToString() + ");\";");
            updateContent.Append("\";");

            var queryHead = string.Format("(int page, int pageSize{0})", queryParamsContent.ToString());
            var queryCountHead = string.Format("({0})", (queryParamsContent.ToString().Length > 0 ? queryParamsContent.ToString().Substring(1, queryParamsContent.ToString().Length - 1) : queryParamsContent.ToString()));
            var assignContent = new StringBuilder();
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (item.IsMark == 0)
                {
                    assignContent.AppendFormat("\t\t\t\t\t\tmodel.{0} = sqldr[\"{1}\"] == DBNull.Value ? {2} : {3};\r\n", attribute, item.ColumnName, Tool.ToDefaultValue(item.DBType), Tool.ToDefaultDBValue(item.DBType, item.ColumnName));
                }
                else
                {
                    assignContent.AppendFormat("model.{0} = sqldr[\"{1}\"] == DBNull.Value ? {2} : {3};\r\n", attribute, item.ColumnName, Tool.ToDefaultValue(item.DBType), Tool.ToDefaultDBValue(item.DBType, item.ColumnName));
                }
            }

            // 构建aspx.cs类
            StringBuilder addAndUpdteStr = new StringBuilder();
            StringBuilder createModel = new StringBuilder();
            index = 0;
            foreach (var item in tempModel.TableList)
            {
                string field = item.ColumnName.ToFirstLower();
                string attribute = item.ColumnName;

                // 如果两个一样了。。。
                if (field == attribute)
                {
                    attribute = attribute.ToFirstUpper();
                }

                if (index == 0)
                {
                    addAndUpdteStr.AppendFormat("string {0} = HttpUtility.UrlDecode(Request[\"{0}\"]);\r\n", field);
                    if (item.IsMainKey == 1)
                    {
                        createModel.AppendFormat("if (!string.IsNullOrEmpty({0}))\r\n", field);
                        createModel.Append("\t\t\t{\r\n");
                        createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, Tool.ToStringToType(field, item.DBType));
                        createModel.Append("\t\t\t}\r\n");
                    }
                    else
                    {
                        createModel.AppendFormat("model.{0} = {1};\r\n", attribute, Tool.ToStringToType(field, item.DBType));
                    }
                }
                else
                {
                    addAndUpdteStr.AppendFormat("\t\t\tstring {0} = HttpUtility.UrlDecode(Request[\"{0}\"]);\r\n", field);
                    createModel.AppendFormat("\t\t\tmodel.{0} = {1};\r\n", attribute, Tool.ToStringToType(field, item.DBType));
                }

                index++;
            }

            // 开始生成文件夹和替换内容
            string dir = tempModel.TargetDir;
            string modelPath = Path.Combine(dir, "Model");
            string dalPath = Path.Combine(dir, "DAL");
            string uiPath = Path.Combine(dir, "Web" + tempModel.DbName);

            string[] dirs = new string[] { modelPath, dalPath, uiPath };
            foreach (var item in dirs)
            {
                if (!Directory.Exists(item))
                {
                    Directory.CreateDirectory(item);
                }
            }

            // 模板文件所在位置
            string temppath = tempModel.SourceDir;
            string tempModelPath = Path.Combine(temppath, "model.txt");
            string tempDalPath = Path.Combine(temppath, "dal.txt");
            string tempFactoryPath = Path.Combine(temppath, "factory.txt");
            string tempAspxPath = Path.Combine(temppath, "aspx.txt");
            string tempAspxCsPath = Path.Combine(temppath, "aspx.cs.txt");
            string tempAspxDesignerCsPath = Path.Combine(temppath, "aspx.designer.cs.txt");
            string tempsqlhelperPath = Path.Combine(temppath, "sqlhelper.cs.txt");
            string tempwebconfig = Path.Combine(temppath, "web.config.txt");
            string tempassemblyinfo = Path.Combine(temppath, "assemblyinfo.cs.txt");

            // Model/Model.cs
            string oldNameSpace = nameSpace;
            if (tempModel.IsDenpendDLL)
            {
                nameSpace = "Model";
            }

            string resultModelPath = Path.Combine(modelPath, modelName + ".cs");
            string modelTxt = File.ReadAllText(tempModelPath, Encoding.UTF8);
            string resultModelTxt = modelTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{ModelName}}", modelName).Replace("{{ModelContent}}", modelContent.ToString());
            File.WriteAllText(resultModelPath, resultModelTxt, Encoding.UTF8);

            if (tempModel.IsDenpendDLL)
            {
                nameSpace = "DAL";
            }

            // DAL/ModelDAL.cs
            string resultDalPath = Path.Combine(dalPath, dalName + ".cs");
            string resultFactoryPath = Path.Combine(dalPath, "ConnectionFactory.cs");
            string dalTxt = File.ReadAllText(tempDalPath, Encoding.UTF8);
            string resultDalTxt = dalTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{DALName}}", dalName).Replace("{{ClassName}}", tempModel.TableName.ToFirstUpper()).Replace("{{ModelName}}", modelName).Replace("{{AddContent}}", addContent.ToString()).Replace("{{AddParamsContent}}", addparamsContent.ToString()).Replace("{{DataBase}}", tempModel.DbName).Replace("{{UpdateContent}}", updateContent.ToString()).Replace("{{UpdateParamsContent}}", updateParamsContent.ToString()).Replace("{{QueryHead}}", queryHead).Replace("{{QueryCountHead}}", queryCountHead).Replace("{{QueryWhere}}", queryWhereContent.ToString()).Replace("{{TableName}}", tempModel.TableName).Replace("{{AssignContent}}", assignContent.ToString()).Replace("{{QueryData}}", queryData.ToString()).Replace("{{QueryParamsContent}}", queryParamsContent.ToString()).Replace("{{keyId}}", keyId).Replace("{{DeleteKey}}", deleteKey.ToString()).Replace("{{DeleteContent}}", deleteContent.ToString()).Replace("{{DeleteParamsContent}}", deleteParamContent.ToString());
            File.WriteAllText(resultDalPath, resultDalTxt, Encoding.UTF8);

            // DAL/ConnectionFactory.cs
            string factoryTxt = File.ReadAllText(tempFactoryPath, Encoding.UTF8);
            string resultFactoryTxt = factoryTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{DataBase}}", tempModel.DbName).Replace("{{ConnectionString}}", tempModel.ConnectionStr);
            File.WriteAllText(resultFactoryPath, resultFactoryTxt, Encoding.UTF8);

            nameSpace = oldNameSpace;
            // UI/XXXManager.aspx
            string resultAspxPath = Path.Combine(uiPath, pageName + ".aspx");
            string resultAspxDesignerPath = Path.Combine(uiPath, pageName + ".aspx.designer.cs");
            string resultAspxCsPath = Path.Combine(uiPath, pageName + ".aspx.cs");

            string aspxTxt = File.ReadAllText(tempAspxPath, Encoding.UTF8);
            string resultAspxTxt = aspxTxt.Replace("{{PageName}}", pageName).Replace("{{NameSpace}}", nameSpace).Replace("{{Title}}", title).Replace("{{SearchContent}}", searchContent.ToString()).Replace("{{ThInfo}}", thinfo.ToString()).Replace("{{FormHeight}}", formHeight.ToString()).Replace("{{FormInfo}}", formInfo.ToString()).Replace("{{SetValue}}", setValue.ToString()).Replace("{{CreateParams}}", createParams.ToString()).Replace("{{POSTDATA}}", postData.ToString()).Replace("{{KeyId}}", KeyId).Replace("{{keyId}}", keyId).Replace("{{Condition}}", coditionContent.ToString()).Replace("{{PagePost}}", pagePost.ToString()).Replace("{{TableWidth}}", tableWidth.ToString()).Replace("{{SearchExtend}}", searchExtend);
            File.WriteAllText(resultAspxPath, resultAspxTxt, Encoding.UTF8);

            // UI/XXXManager.aspx.designer.cs
            string aspxdesignerTxt = File.ReadAllText(tempAspxDesignerCsPath, Encoding.UTF8);
            string resultAspxDesignerTxt = aspxdesignerTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{ClassName}}", pageName);
            File.WriteAllText(resultAspxDesignerPath, resultAspxDesignerTxt, Encoding.UTF8);

            if (tempModel.IsDenpendDLL)
            {
                // 写入 web.config配置文件
                File.WriteAllText(Path.Combine(uiPath, "Web.config"), File.ReadAllText(tempwebconfig, Encoding.UTF8), Encoding.UTF8);
            }
            else
            {
                // 写入 web.config配置文件
                File.WriteAllText(Path.Combine(dir, "Web.config"), File.ReadAllText(tempwebconfig, Encoding.UTF8), Encoding.UTF8);
            }

            // UI/XXXManager.aspx.cs
            string aspxCsTxt = File.ReadAllText(tempAspxCsPath, Encoding.UTF8);
            string resultAspxCsTxt = aspxCsTxt.Replace("{{NameSpace}}", nameSpace).Replace("{{PageName}}", pageName).Replace("{{AddAndUpdteStr}}", addAndUpdteStr.ToString()).Replace("{{ModelName}}", modelName).Replace("{{CreateModel}}", createModel.ToString()).Replace("{{DALName}}", dalName).Replace("{{keyId}}", keyId.ToFirstLower()).Replace("{{ClassName}}", tempModel.TableName.ToFirstUpper()).Replace("{{QueryData}}", queryData.ToString()).Replace("{{QueryListCount}}", queryListCountParams.ToString()).Replace("{{QueryList}}", queryListParams.ToString()).Replace("{{DeleteStr}}", deleteStr.ToString());
            File.WriteAllText(resultAspxCsPath, resultAspxCsTxt, Encoding.UTF8);

            // 复制sqlhelper 
            string resultSqlHelperPath = Path.Combine(dalPath, "SqlHelper.cs");
            string resultSqlHelperTxt = File.ReadAllText(tempsqlhelperPath, Encoding.UTF8);
            File.WriteAllText(resultSqlHelperPath, resultSqlHelperTxt.Replace("{{NameSpace}}", nameSpace), Encoding.UTF8);

            // 复制 properties/assemblyinfo.cs文件
            if (tempModel.IsDenpendDLL)
            {
                List<string> listdir = new List<string>() { modelPath, dalPath, uiPath };
                foreach (var fdir in listdir)
                {
                    // 独立的需要创建三个dll
                    var ffolder = Path.Combine(fdir, "Properties");
                    if (!Directory.Exists(ffolder))
                    {
                        Directory.CreateDirectory(ffolder);
                    }

                    // 写入文件到folder
                    string resultAssemblyInfoPath = Path.Combine(ffolder, "AssemblyInfo.cs");
                    string resultAssemblyInfoTxt = File.ReadAllText(tempassemblyinfo, Encoding.UTF8);
                    File.WriteAllText(resultAssemblyInfoPath, resultAssemblyInfoTxt.Replace("{{Title}}", fdir.Substring(fdir.LastIndexOf("\\") + 1, fdir.Length - fdir.LastIndexOf("\\") - 1)).Replace("{{Year}}", DateTime.Now.Year.ToString()).Replace("{{Guid}}", Guid.NewGuid().ToString()), Encoding.UTF8);
                }
            }
            else
            {
                var ffolder = Path.Combine(dir, "Properties");
                if (!Directory.Exists(ffolder))
                {
                    Directory.CreateDirectory(ffolder);
                }

                // 写入文件到folder
                string resultAssemblyInfoPath = Path.Combine(ffolder, "AssemblyInfo.cs");
                string resultAssemblyInfoTxt = File.ReadAllText(tempassemblyinfo, Encoding.UTF8);
                File.WriteAllText(resultAssemblyInfoPath, resultAssemblyInfoTxt.Replace("{{Title}}", nameSpace).Replace("{{Year}}", DateTime.Now.Year.ToString()).Replace("{{Guid}}", Guid.NewGuid().ToString()), Encoding.UTF8);
            }
        }

        public void CreateAssembly(TempModel tempModel)
        {
            string dir = tempModel.TargetDir;
            string modelPath = Path.Combine(dir, "Model");
            string dalPath = Path.Combine(dir, "DAL");
            string uiPath = Path.Combine(dir, "Web" + tempModel.DbName);

            string temppath = tempModel.SourceDir;
            string tempassemblyinfo = Path.Combine(temppath, "assemblyinfo.cs.txt");
            string nameSpace = tempModel.NameSpace.Trim();
            if (tempModel.IsDenpendDLL)
            {
                List<string> listdir = new List<string>() { modelPath, dalPath, uiPath };
                foreach (var fdir in listdir)
                {
                    // 独立的需要创建三个dll
                    var ffolder = Path.Combine(fdir, "Properties");
                    if (!Directory.Exists(ffolder))
                    {
                        Directory.CreateDirectory(ffolder);
                    }

                    // 写入文件到folder
                    string resultAssemblyInfoPath = Path.Combine(ffolder, "AssemblyInfo.cs");
                    string resultAssemblyInfoTxt = File.ReadAllText(tempassemblyinfo, Encoding.UTF8);
                    File.WriteAllText(resultAssemblyInfoPath, resultAssemblyInfoTxt.Replace("{{Title}}", fdir.Substring(fdir.LastIndexOf("\\") + 1, fdir.Length - fdir.LastIndexOf("\\") - 1)).Replace("{{Year}}", DateTime.Now.Year.ToString()).Replace("{{Guid}}", Guid.NewGuid().ToString()), Encoding.UTF8);
                }
            }
            else
            {
                var ffolder = Path.Combine(dir, "Properties");
                if (!Directory.Exists(ffolder))
                {
                    Directory.CreateDirectory(ffolder);
                }

                // 写入文件到folder
                string resultAssemblyInfoPath = Path.Combine(ffolder, "AssemblyInfo.cs");
                string resultAssemblyInfoTxt = File.ReadAllText(tempassemblyinfo, Encoding.UTF8);
                File.WriteAllText(resultAssemblyInfoPath, resultAssemblyInfoTxt.Replace("{{Title}}", nameSpace).Replace("{{Year}}", DateTime.Now.Year.ToString()).Replace("{{Guid}}", Guid.NewGuid().ToString()), Encoding.UTF8);
            }
        }
    }
}
