using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace WebAutoCodeOnline
{
    public class ProjectCache
    {
        private static List<ProjectTypeInfo> dataCache = new List<ProjectTypeInfo>();

        private static DateTime overTime = DateTime.MinValue;

        public static List<ProjectTypeInfo> GetDataList()
        {
            if (overTime.AddHours(2) > DateTime.Now)
            {
                return dataCache;
            }
            else
            {
                MySqlDAL.ProjectTypeInfoDAL dal = new MySqlDAL.ProjectTypeInfoDAL();
                dataCache = dal.GetAllProjectTypeList(false);
                overTime = DateTime.Now;
                return dataCache;
            }
        }
    }
}
