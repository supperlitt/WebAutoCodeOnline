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
    public partial class VMSettingFrm : Form
    {
        public VMSettingFrm()
        {
            InitializeComponent();
        }

        private void VMSettingFrm_Load(object sender, EventArgs e)
        {
            // 移除模板（仅供观看）
            this.tabControl1.TabPages.RemoveAt(1);
            var tbList = Cache_Next.GetTableList();
            foreach (var item in tbList)
            {
                TabPage tb = CreateTabPage(Cache_Next.GetColumnList(item));
                tb.Text = item;
                this.tabControl1.TabPages.Add(tb);
            }

            LoadSetting();
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            List<VMDataInfo> list = new List<VMDataInfo>();
            int index = 0;
            foreach (TabPage tb in this.tabControl1.TabPages)
            {
                if (index == 0)
                {
                    var commonVM = new VMDataInfo() { db_name = Cache_Next.GetDbName(), type = 0 };
                    commonVM.add_list.AddRange(this.txtAddExcept.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                    commonVM.edit_list.AddRange(this.txtEditExcept.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                    commonVM.query_list.AddRange(this.txtQueryExcept.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

                    commonVM.extendInfo = new VMDataExtendInfo();
                    commonVM.extendInfo.type = 0;
                    commonVM.extendInfo.v1 = this.rbtnDelete1.Checked ? 0 : 1;
                    commonVM.extendInfo.name = this.txtDeleteColName.Text;
                    commonVM.extendInfo.value = this.txtDeleteColValue.Text;

                    list.Add(commonVM);
                }
                else
                {
                    var tableVM = new VMDataInfo() { db_name = Cache_Next.GetDbName(), type = 1, table_name = tb.Text };
                    tableVM.add_list.AddRange((tb.Controls[1] as TextBox).Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                    tableVM.edit_list.AddRange((tb.Controls[2] as TextBox).Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                    tableVM.query_list.AddRange((tb.Controls[3] as TextBox).Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

                    list.Add(tableVM);
                }

                index++;
            }

            Cache_VMData.SaveData(list);
        }

        private void LoadSetting()
        {
            var list = Cache_VMData.LoadData();
            int index = 0;
            foreach (TabPage tb in this.tabControl1.TabPages)
            {
                if (index == 0)
                {
                    var model = list.Find(p => p.db_name == Cache_Next.GetDbName() && p.type == 0);
                    if (model != null)
                    {
                        this.txtAddExcept.Text = string.Join("\r\n", model.add_list.ToArray());
                        this.txtEditExcept.Text = string.Join("\r\n", model.edit_list.ToArray());
                        this.txtQueryExcept.Text = string.Join("\r\n", model.query_list.ToArray());
                        if (model.extendInfo != null)
                        {
                            if (model.extendInfo.type == 0)
                            {
                                this.rbtnDelete2.Checked = model.extendInfo.v1 == 1;
                                this.txtDeleteColName.Text = model.extendInfo.name ?? "";
                                this.txtDeleteColValue.Text = model.extendInfo.value ?? "";
                            }
                        }
                    }
                }
                else
                {
                    var model = list.Find(p => p.db_name == Cache_Next.GetDbName() && p.type == 1 && p.table_name == tb.Text);
                    if (model != null)
                    {
                        (tb.Controls[1] as TextBox).Text = string.Join("\r\n", model.add_list.ToArray());
                        (tb.Controls[2] as TextBox).Text = string.Join("\r\n", model.edit_list.ToArray());
                        (tb.Controls[3] as TextBox).Text = string.Join("\r\n", model.query_list.ToArray());
                    }
                }

                index++;
            }
        }

        private TabPage CreateTabPage(List<SqlColumnInfo> colList)
        {
            TabPage tab = new TabPage();
            tab.Width = 654;
            tab.Height = 379;

            TextBox txtAll = new TextBox();
            txtAll.Location = new System.Drawing.Point(7, 10);
            txtAll.Multiline = true;
            txtAll.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtAll.Size = new System.Drawing.Size(277, 352);
            txtAll.Text = string.Join("\r\n", (from f in colList select f.Name).ToArray());

            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            lbl1.Location = new System.Drawing.Point(301, 13);
            lbl1.Size = new System.Drawing.Size(29, 12);
            lbl1.Text = "添加";

            TextBox txtAdd = new TextBox();
            txtAdd.Location = new System.Drawing.Point(347, 13);
            txtAdd.Multiline = true;
            txtAdd.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtAdd.Size = new System.Drawing.Size(292, 114);

            Label lbl2 = new Label();
            lbl2.AutoSize = true;
            lbl2.Location = new System.Drawing.Point(301, 136);
            lbl2.Size = new System.Drawing.Size(29, 12);
            lbl2.Text = "编辑";

            TextBox txtEdit = new TextBox();
            txtEdit.Location = new System.Drawing.Point(347, 133);
            txtEdit.Multiline = true;
            txtEdit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtEdit.Size = new System.Drawing.Size(292, 114);

            Label lbl3 = new Label();
            lbl3.AutoSize = true;
            lbl3.Location = new System.Drawing.Point(301, 254);
            lbl3.Size = new System.Drawing.Size(29, 12);
            lbl3.Text = "查询";

            TextBox txtQuery = new TextBox();
            txtQuery.Location = new System.Drawing.Point(347, 251);
            txtQuery.Multiline = true;
            txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtQuery.Size = new System.Drawing.Size(292, 114);

            tab.Controls.Add(txtAll);
            tab.Controls.Add(txtAdd);
            tab.Controls.Add(txtEdit);
            tab.Controls.Add(txtQuery);

            tab.Controls.Add(lbl1);
            tab.Controls.Add(lbl2);
            tab.Controls.Add(lbl3);

            return tab;
        }
    }
}
