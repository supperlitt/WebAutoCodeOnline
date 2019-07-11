using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public class TemplateInfo
    {
        private List<TemplateChildInfo> childList = new List<TemplateChildInfo>();

        public List<TemplateChildInfo> ChildList
        {
            get { return this.childList; }
            set { this.childList = value; }
        }

        public string TypeStr
        {
            get
            {
                return string.Join(",", (from f in childList select f.Type).ToArray());
            }
        }

        public string Group { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }
    }

    public class TemplateChildInfo
    {
        private List<string> filePath = new List<string>();

        public List<string> FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }

        public string Type { get; set; }
    }
}
