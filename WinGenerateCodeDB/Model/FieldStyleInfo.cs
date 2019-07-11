using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public class FieldStyleInfo
    {
        [DefaultValue("默认")]
        public string FieldName
        {
            get;
            set;
        }

        [DefaultValue("")]
        public int FormatType
        {
            get;
            set;
        }

        [DefaultValue("")]
        public string FormatStr
        {
            get;
            set;
        }
    }
}
