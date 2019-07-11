using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool
{
    public class SearchInfo
    {
        private InputInfo inputInfo = new InputInfo();

        public InputInfo InputInfo
        {
            get { return inputInfo; }
            set { inputInfo = value; }
        }

        private OutputInfo outInfo = new OutputInfo();

        public OutputInfo OutInfo
        {
            get { return outInfo; }
            set { outInfo = value; }
        }
    }

    public class InputInfo
    {
        private string arg1 = string.Empty;

        public string Arg1
        {
            get { return arg1; }
            set { arg1 = value; }
        }

        private string arg2 = string.Empty;

        public string Arg2
        {
            get { return arg2; }
            set { arg2 = value; }
        }

        private string arg3 = string.Empty;

        public string Arg3
        {
            get { return arg3; }
            set { arg3 = value; }
        }

        private string arg4 = string.Empty;

        public string Arg4
        {
            get { return arg4; }
            set { arg4 = value; }
        }

        private string arg5 = string.Empty;

        public string Arg5
        {
            get { return arg5; }
            set { arg5 = value; }
        }

        private string arg6 = string.Empty;

        public string Arg6
        {
            get { return arg6; }
            set { arg6 = value; }
        }
    }

    public class OutputInfo
    {
        public int OutType = 0;

        private string outString = string.Empty;

        public string OutString
        {
            get { return outString; }
            set { outString = value; }
        }
    }
}
