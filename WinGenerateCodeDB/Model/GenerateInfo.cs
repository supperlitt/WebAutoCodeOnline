using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public class GenerateInfo
    {
        public string FileName { get; set; }

        public string ShartName
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    return FileName.Substring(FileName.LastIndexOf("\\") + 1, FileName.Length - FileName.LastIndexOf("\\") - 1);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string ShartName_NotTxt
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    return FileName.Substring(FileName.LastIndexOf("\\") + 1, FileName.Length - FileName.LastIndexOf("\\") - 1 - 4);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string FileDir { get; set; }

        public byte[] FileData { get; set; }
    }
}
