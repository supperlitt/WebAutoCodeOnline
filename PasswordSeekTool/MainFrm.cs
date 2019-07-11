using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PasswordSeekTool
{
    public partial class MainFrm : Form
    {
        private SearchInfo searchInfo = new SearchInfo();

        public MainFrm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchInfo = new SearchInfo();
            searchInfo.InputInfo.Arg1 = this.txtArg1.Text.Trim();
            searchInfo.InputInfo.Arg2 = this.txtArg2.Text.Trim();
            searchInfo.InputInfo.Arg3 = this.txtArg3.Text.Trim();
            searchInfo.InputInfo.Arg4 = this.txtArg4.Text.Trim();
            searchInfo.InputInfo.Arg5 = this.txtArg5.Text.Trim();
            searchInfo.InputInfo.Arg6 = this.txtArg6.Text.Trim();

            searchInfo.OutInfo.OutType = rbtnAll.Checked ? 0 : 1;
            searchInfo.OutInfo.OutString = this.txtOutArg1.Text.Trim();

            Thread t = new Thread(SearchExecute);
            t.IsBackground = true;
            t.Start();
        }

        private void SearchExecute()
        {
            while (true)
            {
                // 进行加密处理

            }
        }
    }
}
