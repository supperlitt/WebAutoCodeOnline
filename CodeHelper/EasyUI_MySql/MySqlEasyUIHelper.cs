using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    public class MySqlEasyUIHelper
    {
        public string CreateDAL(EasyUIModel model)
        {
            StringBuilder dalContent = new StringBuilder();
            dalContent.Append(EasyUIMySqlDALHelper.CreateDALHeader(model.NameSpace, model.TableName.ToFirstUpper()));
            dalContent.Append(EasyUIMySqlDALHelper.CreateAddMethod(model));
            dalContent.Append(EasyUIMySqlDALHelper.CreateEditMethod(model));
            dalContent.Append(EasyUIMySqlDALHelper.CreateBatEditMethod(model));
            dalContent.Append(EasyUIMySqlDALHelper.CreateDeleteMethod(model));
            dalContent.Append(EasyUIMySqlDALHelper.CreateQueryListMethod(model));
            dalContent.Append(EasyUIMySqlDALHelper.CreateGetAllAndPart(model));

            dalContent.Append(EasyUIMySqlDALHelper.CreateBottom());

            return dalContent.ToString();
        }

        public string CreateFactory(EasyUIModel model)
        {
            StringBuilder facContent = new StringBuilder();
            facContent.Append(EasyUIMySqlFactoryHelper.CreateFactory(model));

            return facContent.ToString();
        }
    }
}
