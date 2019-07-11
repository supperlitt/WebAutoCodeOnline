using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    public class BootstrapHelper
    {
        /// <summary>
        /// 创建Model类
        /// </summary>
        /// <param name="model">模板对象</param>
        public string CreateModel(BootstrapModel model)
        {
            return BootstrapModelHelper.GetClassString(model);
        }

        public string CreateASPX(BootstrapModel model)
        {
            StringBuilder aspxContent = new StringBuilder();
            aspxContent.Append(BootstrapAspxHelper.CreatePageHead(model.NameSpace, model.TableName.ToFirstUpper() + "Manager"));
            aspxContent.Append(BootstrapAspxHelper.CreateHeader(model.Title));
            aspxContent.Append(BootstrapAspxHelper.CreateBodyHead(model.Title));
            aspxContent.Append(BootstrapAspxHelper.CreateSearchContent(model));
            aspxContent.Append(BootstrapAspxHelper.CreateCmdToolBar(model));
            aspxContent.Append(BootstrapAspxHelper.CreateDataGrid(model));
            aspxContent.Append(BootstrapAspxHelper.CreateDialog(model));
            aspxContent.Append(BootstrapAspxHelper.CreateNotifyMsg());
            aspxContent.Append(BootstrapAspxHelper.CreateJsDateFormat());
            aspxContent.Append(BootstrapAspxHelper.CreateJsOperation(model));
            aspxContent.Append(BootstrapAspxHelper.CreateJsLoad(model));
            aspxContent.Append(BootstrapAspxHelper.CreateBottomContent());

            return aspxContent.ToString();
        }

        public string CreateASPXCS(BootstrapModel model)
        {
            StringBuilder aspxcsContent = new StringBuilder();
            aspxcsContent.Append(BootstrapAspxCsHelper.CreateCSHead(model.NameSpace, model.TableName.ToFirstUpper()));
            aspxcsContent.Append(BootstrapAspxCsHelper.CreatePageLoad(model));
            aspxcsContent.Append(BootstrapAspxCsHelper.CreateLoadData(model));
            aspxcsContent.Append(BootstrapAspxCsHelper.CreateAddData(model));
            aspxcsContent.Append(BootstrapAspxCsHelper.CreateEditData(model));
            aspxcsContent.Append(BootstrapAspxCsHelper.CreateBatEditData(model));
            aspxcsContent.Append(BootstrapAspxCsHelper.CreateDeleteData(model));
            aspxcsContent.Append(BootstrapAspxCsHelper.CreateDownAndDownAll(model));
            aspxcsContent.Append(BootstrapAspxCsHelper.CreateBottom());

            return aspxcsContent.ToString();
        }

        public string CreateDAL(BootstrapModel model)
        {
            StringBuilder dalContent = new StringBuilder();
            dalContent.Append(BootstrapDALHelper.CreateDALHeader(model.NameSpace, model.TableName.ToFirstUpper()));
            dalContent.Append(BootstrapDALHelper.CreateAddMethod(model));
            dalContent.Append(BootstrapDALHelper.CreateEditMethod(model));
            dalContent.Append(BootstrapDALHelper.CreateBatEditMethod(model));
            dalContent.Append(BootstrapDALHelper.CreateDeleteMethod(model));
            dalContent.Append(BootstrapDALHelper.CreateQueryListMethod(model));
            dalContent.Append(BootstrapDALHelper.CreateGetAllAndPart(model));

            dalContent.Append(BootstrapDALHelper.CreateBottom());

            return dalContent.ToString();
        }

        public string CreateFactory(BootstrapModel model)
        {
            StringBuilder facContent = new StringBuilder();
            facContent.Append(BootstrapFactoryHelper.CreateFactory(model));

            return facContent.ToString();
        }
    }
}
