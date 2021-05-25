using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinGenerateCodeDB
{
    public class TabControlNF : System.Windows.Forms.TabControl
    {
        public TabControlNF()
        {
            InitializeComponent();
            TabSet();
        }

        /// <summary>
        /// 设定控件绘制模式
        /// </summary>
        private void TabSet()
        {
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.Alignment = TabAlignment.Left;
            this.SizeMode = TabSizeMode.Fixed;
            this.Multiline = true;
            this.ItemSize = new Size(25, 210);
        }

        /// <summary>
        /// 重绘控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabLeft_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font("微软雅黑", 10.0f);
            SolidBrush brush = new SolidBrush(Color.Black);
            RectangleF tRectangleF = GetTabRect(e.Index);
            StringFormat sf = new StringFormat();//封装文本布局信息 
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Near;
            g.DrawString(this.Controls[e.Index].Text, font, brush, tRectangleF, sf);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabLeft_DrawItem);
            this.ResumeLayout(false);
        }
    }
}
