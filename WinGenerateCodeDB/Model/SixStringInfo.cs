using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB.Model
{
    public class SixStringInfo
    {
        public string AttrName { get; set; }

        public string ColName { get; set; }

        public string Remark { get; set; }

        public string DbType { get; set; }

        public string AttrType { get; set; }

        private FieldStyleInfo style = new FieldStyleInfo();

        public FieldStyleInfo Style
        {
            get { return this.style; }
            set { this.style = value; }
        }
    }
}
