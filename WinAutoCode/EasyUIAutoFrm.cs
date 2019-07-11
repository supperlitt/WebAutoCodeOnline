using CodeHelper;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinAutoCode
{
    public partial class EasyUIAutoFrm : Form
    {
        public EasyUIAutoFrm()
        {
            InitializeComponent();
        }

        private void menuItemNormal_Click(object sender, EventArgs e)
        {
            var frm = FrmManager.GetFrm("MainForm");
            this.Hide();
            frm.Show();
            frm.Location = this.Location;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            EasyUIModel model = new EasyUIModel();
            model.DbName = this.txtDBName.Text.Trim();
            model.AddColumnsStr = this.txtAddColumnName.Text.Trim();
            model.SearchColumnsStr = this.txtSearchColumnName.Text.Trim();
            model.EditColumnsStr = this.txtEditColumnName.Text.Trim();
            model.BatEditColumnsStr = this.txtBatEdit.Text.Trim();
            model.TableStr = this.txtName.Text.Trim();
            model.NameSpace = this.txtNameSpace.Text.Trim();
            model.DbType = rbtnMSSQL.Checked ? 0 : 1;
            model.IsDel = this.chkDel.Checked;
            model.IsBatDel = this.chkBatDel.Checked;
            model.IsBatEdit = this.chkBatEdit.Checked;
            model.IsAdd = this.chkAdd.Checked;
            model.IsEdit = this.chkEdit.Checked;
            model.IsExport = this.chkExport.Checked;

            // 初始化model对象的属性
            UIHelper.InitEasyUI(model);
            if (model.DbType == 0)
            {
                EasyUIHelper easyHelper = new EasyUIHelper();
                this.txtClassCode.Text = easyHelper.CreateModel(model);
                string aspxStr = easyHelper.CreateASPX(model);
                this.txtAspxCode.Text = aspxStr;
                this.txtAspxCsCode.Text = easyHelper.CreateASPXCS(model);
                this.txtDALCode.Text = easyHelper.CreateDAL(model);
                this.txtFactoryCode.Text = easyHelper.CreateFactory(model);
                this.txtSqlHelper.Text = SourceHelper.GetResource("sqlhelper.txt").Replace("命名空间", model.NameSpace);
            }
            else if (model.DbType == 1)
            {
                //CreateClass(temp);

                //MySqlCreateMySqlHelper sqlHelper = new MySqlCreateMySqlHelper(temp);
                //SetContent(this.txtAddCode, sqlHelper.CreateInsertMethod);
                //SetContent(this.txtDeleteCode, sqlHelper.CreateDeleteMethod);
                //SetContent(this.txtBatDeleteCode, sqlHelper.CreateBatDeleteMethod);
                //SetContent(this.txtUpdateCode, sqlHelper.CreateUpdateMethod);
                //SetContent(this.txtFactoryCode, sqlHelper.CreateConnectionFactory);
            }
        }

        private void btnExportCode_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Zip文件|*.zip";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = dialog.FileName;

                EasyUIModel model = new EasyUIModel();
                model.DbName = this.txtDBName.Text.Trim();
                model.AddColumnsStr = this.txtAddColumnName.Text.Trim();
                model.SearchColumnsStr = this.txtSearchColumnName.Text.Trim();
                model.EditColumnsStr = this.txtEditColumnName.Text.Trim();
                model.BatEditColumnsStr = this.txtBatEdit.Text.Trim();
                model.TableStr = this.txtName.Text.Trim();
                model.NameSpace = this.txtNameSpace.Text.Trim();
                model.DbType = rbtnMSSQL.Checked ? 0 : 1;
                model.IsDel = this.chkDel.Checked;
                model.IsBatDel = this.chkBatDel.Checked;
                model.IsBatEdit = this.chkBatEdit.Checked;
                model.IsAdd = this.chkAdd.Checked;
                model.IsEdit = this.chkEdit.Checked;
                model.IsExport = this.chkExport.Checked;

                // 初始化model对象的属性
                UIHelper.InitEasyUI(model);
                if (model.DbType == 0)
                {
                    EasyUIHelper easyHelper = new EasyUIHelper();
                    this.txtClassCode.Text = easyHelper.CreateModel(model);
                    string aspxStr = easyHelper.CreateASPX(model);
                    this.txtAspxCode.Text = aspxStr;
                    this.txtAspxCsCode.Text = easyHelper.CreateASPXCS(model);
                    this.txtDALCode.Text = easyHelper.CreateDAL(model);
                    this.txtFactoryCode.Text = easyHelper.CreateFactory(model);
                    this.txtSqlHelper.Text = SourceHelper.GetResource("sqlhelper.txt").Replace("命名空间", model.NameSpace);
                }
                else if (model.DbType == 1)
                {
                }

                // 构建数据，塞入zip包
                string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                string dir = Path.Combine(tempPath, model.TableName);
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir);
                }

                Directory.CreateDirectory(dir);

                string modelPath = Path.Combine(dir, model.TableName.ToFirstUpper() + ".cs");
                string aspxPath = Path.Combine(dir, model.TableName.ToFirstUpper() + "Manager.aspx");
                string aspxCsPath = Path.Combine(dir, model.TableName.ToFirstUpper() + "Manager.aspx.cs");
                string dalPath = Path.Combine(dir, model.TableName.ToFirstUpper() + "DAL.cs");
                string factoryPath = Path.Combine(dir, "ConnectionFactory.cs");
                string sqlHelperPath = Path.Combine(dir, "SqlHelper.cs");

                File.WriteAllText(modelPath, this.txtClassCode.Text);
                File.WriteAllText(aspxPath, this.txtAspxCode.Text);
                File.WriteAllText(aspxCsPath, this.txtAspxCsCode.Text);
                File.WriteAllText(dalPath, this.txtDALCode.Text);
                File.WriteAllText(factoryPath, this.txtFactoryCode.Text);
                File.WriteAllText(sqlHelperPath, this.txtSqlHelper.Text);

                string zipPath = Path.Combine(tempPath, Guid.NewGuid().ToString("N") + ".zip");
                ZipHelper.ZipDirectory(dir, zipPath);
                File.Copy(zipPath, filePath);
                File.Delete(zipPath);
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                (sender as TextBox).SelectAll();
            }
        }

        #region 私有方法
        private void CreateClass(NormalModel temp)
        {
            string result = string.Empty;
            try
            {
                // 创建类代码
                TableHelper tableHelper = new TableHelper(true, true, true);
                result = tableHelper.GetClassString(temp);
                this.txtClassCode.Text = result;
            }
            catch (Exception ex)
            {
                this.txtClassCode.Text = ex.ToString();
            }
        }

        private void SetContent(TextBox text, List<Func<string>> funcList)
        {
            StringBuilder content = new StringBuilder();
            try
            {
                foreach (var func in funcList)
                {
                    // 添加代码
                    string result = func();
                    content.AppendLine(result);
                }

                text.Text = content.ToString();
            }
            catch (Exception ex)
            {
                text.Text = ex.ToString();
            }
        }
        private void SetContent(TextBox text, Func<string> func)
        {
            string result = string.Empty;
            try
            {
                // 添加代码
                result = func();
                text.Text = result;
            }
            catch (Exception ex)
            {
                text.Text = ex.ToString();
            }
        }
        private void SetContent(TextBox text, Func<string, string> func, string arg)
        {
            string result = string.Empty;
            try
            {
                // 添加代码
                result = func(arg);
                text.Text = result;
            }
            catch (Exception ex)
            {
                text.Text = ex.ToString();
            }
        }
        private void SetContent(TextBox text, Func<int, string> func, int arg)
        {
            string result = string.Empty;
            try
            {
                // 添加代码
                result = func(arg);
                text.Text = result;
            }
            catch (Exception ex)
            {
                text.Text = ex.ToString();
            }
        }
        private void SetContent(TextBox text, Func<string, string, string> func, string arg1, string arg2)
        {
            string result = string.Empty;
            try
            {
                // 添加代码
                result = func(arg1, arg2);
                text.Text = result;
            }
            catch (Exception ex)
            {
                text.Text = ex.ToString();
            }
        }
        private void SetContent(TextBox text, Func<string, string, string, string> func, string arg1, string arg2, string arg3)
        {
            string result = string.Empty;
            try
            {
                // 添加代码
                result = func(arg1, arg2, arg3);
                text.Text = result;
            }
            catch (Exception ex)
            {
                text.Text = ex.ToString();
            }
        }
        #endregion

        private void btnCreateBootStrap_Click(object sender, EventArgs e)
        {
            BootstrapModel model = new BootstrapModel();
            model.DbName = this.txtDBName.Text.Trim();
            model.AddColumnsStr = this.txtAddColumnName.Text.Trim();
            model.SearchColumnsStr = this.txtSearchColumnName.Text.Trim();
            model.EditColumnsStr = this.txtEditColumnName.Text.Trim();
            model.BatEditColumnsStr = this.txtBatEdit.Text.Trim();
            model.TableStr = this.txtName.Text.Trim();
            model.NameSpace = this.txtNameSpace.Text.Trim();
            model.DbType = rbtnMSSQL.Checked ? 0 : 1;
            model.IsDel = this.chkDel.Checked;
            model.IsBatDel = this.chkBatDel.Checked;
            model.IsBatEdit = this.chkBatEdit.Checked;
            model.IsAdd = this.chkAdd.Checked;
            model.IsEdit = this.chkEdit.Checked;
            model.IsExport = this.chkExport.Checked;

            // 初始化model对象的属性
            UIHelper.InitBootstrap(model);
            if (model.DbType == 0)
            {
                BootstrapHelper bootStrapHelper = new BootstrapHelper();
                this.txtClassCode.Text = bootStrapHelper.CreateModel(model);
                string aspxStr = bootStrapHelper.CreateASPX(model);
                this.txtAspxCode.Text = aspxStr;
                this.txtAspxCsCode.Text = bootStrapHelper.CreateASPXCS(model);
                this.txtDALCode.Text = bootStrapHelper.CreateDAL(model);
                this.txtFactoryCode.Text = bootStrapHelper.CreateFactory(model);
                this.txtSqlHelper.Text = SourceHelper.GetResource("sqlhelper.txt").Replace("命名空间", model.NameSpace);
            }
            else if (model.DbType == 1)
            {
                BootstrapHelper bootStrapHelper = new BootstrapHelper();
                this.txtClassCode.Text = bootStrapHelper.CreateModel(model);
                string aspxStr = bootStrapHelper.CreateASPX(model);
                this.txtAspxCode.Text = aspxStr;
                this.txtAspxCsCode.Text = bootStrapHelper.CreateASPXCS(model);
                MySqlBootstrapHelper mysqlHelper = new MySqlBootstrapHelper();
                this.txtDALCode.Text = mysqlHelper.CreateDAL(model);
                this.txtFactoryCode.Text = mysqlHelper.CreateFactory(model);
                this.txtSqlHelper.Text = SourceHelper.GetResource("sqlhelper.txt").Replace("命名空间", model.NameSpace);
            }
        }
    }
}
