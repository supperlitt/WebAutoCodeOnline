using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinGenerateCodeDB.Child
{
    public partial class FieldExtendFrm : Form
    {
        private List<string> columnNameList = new List<string>();
        public bool IsAddSuccess = false;
        public ExtendAttributeInfo ExtendInfo = new ExtendAttributeInfo();

        public FieldExtendFrm()
        {
            InitializeComponent();
        }

        public FieldExtendFrm(List<string> columnNameList, string filedName, string fieldDbType, string remark)
        {
            InitializeComponent();
            this.columnNameList = columnNameList;
            this.lblFieldName.Text = filedName;
            this.lblFieldName.Tag = fieldDbType;
            this.txtNewAttributeName.Text = filedName;
            this.txtComment.Text = remark;
        }

        public FieldExtendFrm(List<string> columnNameList, string filedName, ExtendAttributeInfo extendInfo)
        {
            InitializeComponent();
            this.columnNameList = columnNameList;
            this.lblFieldName.Text = extendInfo.DependColumn;
            this.lblFieldName.Tag = extendInfo.DependColumnType;
            this.txtNewAttributeName.Text = filedName;
            this.txtComment.Text = extendInfo.Comment;
            this.cmbType.SelectedItem = ExtendInfo.AttributeType;
            this.ExtendInfo = extendInfo;

            this.Text = "属性扩展窗口 - 编辑";
            this.btnAdd.Text = "保存";

            this.tabControl1.SelectedIndex = extendInfo.FormatType;
            if (extendInfo.FormatType == 0)
            {
                this.txt1.Text = extendInfo.FormatStr;
            }
            else if (extendInfo.FormatType == 1)
            {
                this.txt2.Text = extendInfo.FormatStr;
            }
            else if (extendInfo.FormatType == 2)
            {
                this.txt3.Text = extendInfo.FormatStr.Replace("\n", "\r\n");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ExtendInfo.NewAttName = this.txtNewAttributeName.Text;
            ExtendInfo.Comment = this.txtComment.Text;
            ExtendInfo.AttributeType = this.cmbType.SelectedItem.ToString();
            ExtendInfo.DependColumn = this.lblFieldName.Text;
            ExtendInfo.DependColumnType = this.lblFieldName.Tag as string;
            if (columnNameList.Contains(ExtendInfo.NewAttName))
            {
                MessageBox.Show("属性名称，已经存在列，或扩展属性中!");

                return;
            }

            ExtendInfo.FormatType = this.tabControl1.SelectedIndex;
            if (this.tabControl1.SelectedIndex == 0)
            {
                ExtendInfo.FormatStr = this.txt1.Text;
            }
            else if (this.tabControl1.SelectedIndex == 1)
            {
                ExtendInfo.FormatStr = this.txt2.Text;
            }
            else if (this.tabControl1.SelectedIndex == 2)
            {
                ExtendInfo.FormatStr = this.txt3.Text;
            }

            this.IsAddSuccess = true;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FieldExtendFrm_Load(object sender, EventArgs e)
        {
            this.cmbType.SelectedIndex = 0;
        }
    }
}
