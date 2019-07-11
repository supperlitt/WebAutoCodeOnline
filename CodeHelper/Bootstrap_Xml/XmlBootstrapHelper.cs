using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    public class XmlBootstrapHelper
    {
        public string CreateDAL(BootstrapModel model)
        {
            StringBuilder dalContent = new StringBuilder();
            dalContent.Append(BootstrapMySqlDALHelper.CreateDALHeader(model.NameSpace, model.TableName.ToFirstUpper()));
            dalContent.Append(BootstrapMySqlDALHelper.CreateAddMethod(model));
            dalContent.Append(BootstrapMySqlDALHelper.CreateEditMethod(model));
            dalContent.Append(BootstrapMySqlDALHelper.CreateBatEditMethod(model));
            dalContent.Append(BootstrapMySqlDALHelper.CreateDeleteMethod(model));
            dalContent.Append(BootstrapMySqlDALHelper.CreateQueryListMethod(model));
            dalContent.Append(BootstrapMySqlDALHelper.CreateGetAllAndPart(model));

            dalContent.Append(BootstrapMySqlDALHelper.CreateBottom());

            return dalContent.ToString();
        }

        public string CreateFactory(BootstrapModel model)
        {
            StringBuilder facContent = new StringBuilder();
            facContent.Append(BootstrapMySqlFactoryHelper.CreateFactory(model));

            return facContent.ToString();
        }
    }
}
