using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinAutoCode
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string start = ConfigManager.GetDefaultStartFrm();
            var frm = FrmManager.GetFrm(start);
            Application.Run(frm);
        }
    }
}
