using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinAutoCode
{
    /// <summary>
    /// 窗体管理对象
    /// </summary>
    public class FrmManager
    {
        private static List<Form> frmList = new List<Form>();

        // 添加窗口
        public static void AddFrm(Form frm)
        {
            frmList.Add(frm);
        }

        /// <summary>
        /// 得到窗口对象
        /// </summary>
        /// <param name="frmKey"></param>
        /// <returns></returns>
        public static Form GetFrm(string frmKey)
        {
            var frm = frmList.Find(p => p.Name == frmKey);
            if (frm == null)
            {
                frm = CreateNewFrm(frmKey);
                frmList.Add(frm);
            }
            else
            {
                frm.Visible = true;
                frm.WindowState = FormWindowState.Normal;
            }

            return frm;
        }

        /// <summary>
        /// 移除窗口。返回剩余窗口数
        /// </summary>
        /// <param name="frmKey"></param>
        /// <returns></returns>
        public static int GetFrmVisiableCount()
        {
            return frmList.FindAll(p => p.Visible == true).Count;
        }

        private static Form CreateNewFrm(string frmKey)
        {
            Form frm = null;
            switch (frmKey)
            {
                case "MainForm":
                    frm = new MainForm();
                    frm.FormClosing += frm_FormClosing;
                    break;
                case "EasyUIAutoFrm":
                    frm = new EasyUIAutoFrm();
                    frm.FormClosing += frm_FormClosing;
                    break;
                default:
                    frm = new MainForm();
                    frm.FormClosing += frm_FormClosing;
                    break;
            }

            return frm;
        }

        private static void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var frm = sender as Form;
            frm.Hide();
            int count = GetFrmVisiableCount();
            if (count == 0)
            {
                // 得到最后的窗口，写入启动项目
                ConfigManager.SetDefaultStartFrm(frm.Name);

                Application.ExitThread();
                Application.Exit();

                return;
            }

            e.Cancel = true;
        }
    }
}
