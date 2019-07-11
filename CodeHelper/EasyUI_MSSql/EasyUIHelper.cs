using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeHelper
{
    public class EasyUIHelper
    {
        /// <summary>
        /// 创建Model类
        /// </summary>
        /// <param name="model">模板对象</param>
        public string CreateModel(EasyUIModel model)
        {
            return EasyUIModelHelper.GetClassString(model);
        }

        public string CreateASPX(EasyUIModel model)
        {
            StringBuilder aspxContent = new StringBuilder();
            aspxContent.Append(EasyUIAspxHelper.CreatePageHead(model.NameSpace, model.TableName.ToFirstUpper() + "Manager"));
            aspxContent.Append(EasyUIAspxHelper.CreateHeader(model.Title));
            aspxContent.Append(EasyUIAspxHelper.CreateBodyHead(model.Title));
            aspxContent.Append(EasyUIAspxHelper.CreateSearchContent(model));
            aspxContent.Append(EasyUIAspxHelper.CreateCmdToolBar(model));
            aspxContent.Append(EasyUIAspxHelper.CreateDataGrid(model));
            aspxContent.Append(EasyUIAspxHelper.CreateDialog(model));
            aspxContent.Append(EasyUIAspxHelper.CreateNotifyMsg());
            aspxContent.Append(EasyUIAspxHelper.CreateJsDateFormat());
            aspxContent.Append(EasyUIAspxHelper.CreateJsOperation(model));
            aspxContent.Append(EasyUIAspxHelper.CreateJsLoad(model));
            aspxContent.Append(EasyUIAspxHelper.CreateBottomContent());

            return aspxContent.ToString();
        }

        public string CreateASPXCS(EasyUIModel model)
        {
            StringBuilder aspxcsContent = new StringBuilder();
            aspxcsContent.Append(EasyUIAspxCsHelper.CreateCSHead(model.NameSpace, model.TableName.ToFirstUpper()));
            aspxcsContent.Append(EasyUIAspxCsHelper.CreatePageLoad(model));
            aspxcsContent.Append(EasyUIAspxCsHelper.CreateLoadData(model));
            aspxcsContent.Append(EasyUIAspxCsHelper.CreateAddData(model));
            aspxcsContent.Append(EasyUIAspxCsHelper.CreateEditData(model));
            aspxcsContent.Append(EasyUIAspxCsHelper.CreateBatEditData(model));
            aspxcsContent.Append(EasyUIAspxCsHelper.CreateDeleteData(model));
            aspxcsContent.Append(EasyUIAspxCsHelper.CreateDownAndDownAll(model));
            aspxcsContent.Append(EasyUIAspxCsHelper.CreateBottom());

            return aspxcsContent.ToString();
        }

        public string CreateDAL(EasyUIModel model)
        {
            StringBuilder dalContent = new StringBuilder();
            dalContent.Append(EasyUIDALHelper.CreateDALHeader(model.NameSpace, model.TableName.ToFirstUpper()));
            dalContent.Append(EasyUIDALHelper.CreateAddMethod(model));
            dalContent.Append(EasyUIDALHelper.CreateEditMethod(model));
            dalContent.Append(EasyUIDALHelper.CreateBatEditMethod(model));
            dalContent.Append(EasyUIDALHelper.CreateDeleteMethod(model));
            dalContent.Append(EasyUIDALHelper.CreateQueryListMethod(model));
            dalContent.Append(EasyUIDALHelper.CreateGetAllAndPart(model));

            dalContent.Append(EasyUIDALHelper.CreateBottom());

            return dalContent.ToString();
        }

        public string CreateFactory(EasyUIModel model)
        {
            StringBuilder facContent = new StringBuilder();
            facContent.Append(EasyUIFactoryHelper.CreateFactory(model));

            return facContent.ToString();
        }
    }
}
