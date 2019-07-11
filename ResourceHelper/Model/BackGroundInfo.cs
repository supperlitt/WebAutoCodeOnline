using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceHelper
{
    public class BackGroundInfo
    {
        private List<ModuleInfo> moduleList = new List<ModuleInfo>();

        public List<ModuleInfo> ModuleList
        {
            get { return this.moduleList; }
            set { this.moduleList = value; }
        }


    }
}
