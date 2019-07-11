using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinGenerateCodeDB
{
    public static class ExtendTool
    {
        /// <summary>
        /// ListView选中下一个
        /// </summary>
        /// <param name="lst"></param>
        public static void Selecte_Next(this ListView lst)
        {
            lst.Invoke(new Action<ListView>(p =>
            {
                if (p.SelectedItems.Count > 0)
                {
                    if (p.Items.Count > p.SelectedItems[0].Index + 1)
                    {
                        p.Items[p.SelectedItems[0].Index + 1].Selected = true;
                        p.SelectedItems[0].Selected = false;
                    }
                }
            }), lst);
        }
    }
}
