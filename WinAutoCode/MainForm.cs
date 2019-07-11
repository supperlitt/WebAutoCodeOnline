using CodeHelper;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Common;

namespace WinAutoCode
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab && this.txtName.Focused)
            {
                this.txtName.AppendText("\t");
                return true;
            }
            else
            {
                return base.ProcessDialogKey(keyData);
            }
        }

        #region 控件事件

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.cmbEncodeType.SelectedIndex = 0;
            this.lblEmail.Text = "作者邮箱：wojiaotanghao@qq.com\r\n\r\n当前版本：1.16.0111.1\r\n\r\n官方网站：http://www.51debug.com";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            NormalModel model = new NormalModel();
            model.DbName = this.txtDBName.Text.Trim();
            model.SearchColumnsStr = this.txtSearchColumnName.Text.Trim();
            model.TableStr = this.txtName.Text.Trim();
            model.NameSpace = this.txtNameSpace.Text.Trim();
            model.DbType = rbtnMSSQL.Checked ? 0 : 1;

            UIHelper.InitNormalCode(model);
            CreateClass(model);
            if (model.DbType == 0)
            {
                SqlCreateSqlHelper sqlHelper = new SqlCreateSqlHelper(model);
                SetContent(this.txtAddCode, sqlHelper.CreateInsertMethod);
                SetContent(this.txtDeleteCode, sqlHelper.CreateDeleteMethod);
                SetContent(this.txtBatDeleteCode, sqlHelper.CreateBatDeleteMethod);
                SetContent(this.txtUpdateCode, sqlHelper.CreateUpdateMethod);
                SetContent(this.txtSearchAllCode, sqlHelper.CreateSelectByPageAndSizeAddCount);
                SetContent(this.txtSearchPageCode, sqlHelper.CreateSelectByPageAndSize);
                SetContent(this.txtFactoryCode, sqlHelper.CreateConnectionFactory);
                SetContent(this.txtHelperCode, SourceHelper.GetResource, "sqlhelper.txt");
                SetContent(this.txtAll, new List<Func<string>>() { sqlHelper.CreateInsertMethod, 
                sqlHelper.CreateDeleteMethod, 
                sqlHelper.CreateBatDeleteMethod, 
                sqlHelper.CreateUpdateMethod, 
                sqlHelper.CreateSelectByPageAndSizeAddCount, 
                sqlHelper.CreateSelectByPageAndSize });
            }
            else if (model.DbType == 1)
            {
                MySqlCreateMySqlHelper sqlHelper = new MySqlCreateMySqlHelper(model);
                SetContent(this.txtAddCode, sqlHelper.CreateInsertMethod);
                SetContent(this.txtDeleteCode, sqlHelper.CreateDeleteMethod);
                SetContent(this.txtBatDeleteCode, sqlHelper.CreateBatDeleteMethod);
                SetContent(this.txtUpdateCode, sqlHelper.CreateUpdateMethod);
                SetContent(this.txtSearchAllCode, sqlHelper.CreateSelectByPageAndSizeAddCount);
                SetContent(this.txtSearchPageCode, sqlHelper.CreateSelectByPageAndSize);
                SetContent(this.txtFactoryCode, sqlHelper.CreateConnectionFactory);
                SetContent(this.txtHelperCode, SourceHelper.GetResource, "mysqlhelper.txt");
                SetContent(this.txtAll, new List<Func<string>>() { sqlHelper.CreateInsertMethod, 
                sqlHelper.CreateDeleteMethod, 
                sqlHelper.CreateBatDeleteMethod, 
                sqlHelper.CreateUpdateMethod, 
                sqlHelper.CreateSelectByPageAndSizeAddCount, 
                sqlHelper.CreateSelectByPageAndSize });
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                (sender as TextBox).SelectAll();
            }
        }

        /// <summary>
        /// 编码解码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCodeCmd(object sender, EventArgs e)
        {
            string text = (sender as Button).Text;
            switch (text)
            {
                case "base64编码":
                    SetContent(this.txtEncodeValue, new Func<string, string, string>(EncodingHelper.EncodeBase64), this.txtEncodeKey.Text.Trim(), this.cmbEncodeType.SelectedItem.ToString());
                    break;
                case "base64解码":
                    SetContent(this.txtEncodeValue, new Func<string, string, string>(EncodingHelper.DecodeBase64), this.txtEncodeKey.Text.Trim(), this.cmbEncodeType.SelectedItem.ToString());
                    break;
                case "URL编码":
                    SetContent(this.txtEncodeValue, new Func<string, string>(EncodingHelper.UrlEncode), this.txtEncodeKey.Text.Trim());
                    break;
                case "URL解码":
                    SetContent(this.txtEncodeValue, new Func<string, string>(EncodingHelper.UrlDecode), this.txtEncodeKey.Text.Trim());
                    break;
                case @"\u编码":
                    SetContent(this.txtEncodeValue, new Func<string, string, string>(EncodingHelper.BackslashUEncode), this.txtEncodeKey.Text.Trim(), this.cmbEncodeType.SelectedItem.ToString());
                    break;
                case @"\u解码":
                    SetContent(this.txtEncodeValue, new Func<string, string, string>(EncodingHelper.BackslashUDecode), this.txtEncodeKey.Text.Trim(), this.cmbEncodeType.SelectedItem.ToString());
                    break;
                case "%f编码":
                    SetContent(this.txtEncodeValue, new Func<string, string, string>(EncodingHelper.PercentFEncode), this.txtEncodeKey.Text.Trim(), this.cmbEncodeType.SelectedItem.ToString());
                    break;
                case "%f解码":
                    SetContent(this.txtEncodeValue, new Func<string, string, string>(EncodingHelper.PercentFDecode), this.txtEncodeKey.Text.Trim(), this.cmbEncodeType.SelectedItem.ToString());
                    break;
            }
        }

        /// <summary>
        /// 加密，解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDES_Click(object sender, EventArgs e)
        {
            string btnName = (sender as Button).Text;
            switch (btnName)
            {
                case "DES加密":
                    SetContent(this.txtValue, new Func<string, string, string>(EncryptHelper.Encrypt), this.txtKey.Text.Trim(), this.txtParam.Text.Trim());
                    break;
                case "DES解密":
                    SetContent(this.txtValue, new Func<string, string, string>(EncryptHelper.Decrypt), this.txtKey.Text.Trim(), this.txtParam.Text.Trim());
                    break;
                case "RSA加密":
                    {
                        SetContent(this.txtValue, new Func<string, string, string>(RSANewHelper.Encrypt), this.txtKey.Text.Trim(), this.txtPublicKey.Text.Trim());
                    }
                    break;
                case "RSA解密":
                    {
                        SetContent(this.txtValue, new Func<string, string, string>(RSANewHelper.Decrypt), this.txtKey.Text.Trim(), this.txtPrivateKey.Text.Trim());
                    }
                    break;
                case "生成公钥私钥":
                    string[] keys = RSAHelper.GenerateKeys();
                    this.txtPrivateKey.Text = keys[0];
                    this.txtPublicKey.Text = keys[1];
                    break;
                case "MD5":
                    SetContent(this.txtValue, new Func<string, string>(EncryptHelper.GetMD5), this.txtKey.Text.Trim());
                    break;
                case "SHA1":
                    SetContent(this.txtValue, new Func<string, string>(EncryptHelper.GetSHA1), this.txtKey.Text.Trim());
                    break;
                case "随机AES密钥":
                    SetContent(this.txtParam, new Func<int, string>(AESHelper.GetIv), 16);
                    break;
                case "AES加密":
                    SetContent(this.txtValue, new Func<string, string, string>(AESHelper.AESEncrypt), this.txtKey.Text.Trim(), this.txtParam.Text.Trim());
                    break;
                case "AES解密":
                    SetContent(this.txtValue, new Func<string, string, string>(AESHelper.AESDecrypt), this.txtKey.Text.Trim(), this.txtParam.Text.Trim());
                    break;
            }
        }

        private void rbtnMySQL_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnMySQL.Checked)
            {
                this.txtName.Text = @"--标题
create table TestInfo
(
    TestId int primary key auto_increment, --测试Id
    TestName varchar(20) not null, -- 测试名称
    TestPwd varchar(50) not null, -- 测试密码
    TestMemory decimal not null, -- 测试金额
    AddDate datetime not null -- 添加时间
)";
            }
            else
            {
                this.txtName.Text = @"--标题
create table TestInfo
(
    TestId int primary key identity(1,1), --测试Id
    TestName varchar(20) not null, -- 测试名称
    TestPwd varchar(50) not null, -- 测试密码
    TestMemory decimal not null, -- 测试金额
    AddDate datetime not null -- 添加时间
)";
            }
        }

        private void menuItemEasyUI_Click(object sender, EventArgs e)
        {
            var frm = FrmManager.GetFrm("EasyUIAutoFrm");
            this.Hide();
            frm.Show();
            frm.Location = this.Location;
        }
        #endregion

        #region 私有方法
        private void CreateClass(NormalModel model)
        {
            string result = string.Empty;
            try
            {
                // 创建类代码
                TableHelper tableHelper = new TableHelper(true, true, true);
                result = tableHelper.GetClassString(model);
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
    }
}
