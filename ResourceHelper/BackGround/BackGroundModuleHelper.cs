using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceHelper
{
    public class BackGroundModuleHelper
    {
        private static List<ModuleInfo> dataList = null;

        static BackGroundModuleHelper()
        {
            dataList = new List<ModuleInfo>();
            dataList.Add(new ModuleInfo() { ModuleName = "登录1", AdapterType = 1, Desc = "不包含记住密码" });
            dataList.Add(new ModuleInfo() { ModuleName = "登录2", AdapterType = 1, Desc = "包含记住密码" });
        }

        public static List<ModuleInfo> GetList()
        {
            return dataList;
        }
    }
}
