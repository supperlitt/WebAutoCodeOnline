using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WinGenerateCodeDB.Cache;
using WinGenerateCodeDB.Child;
using WinGenerateCodeDB.Code;
using WinGenerateCodeDB.Model;

namespace WinGenerateCodeDB
{
    public partial class MainFrm_New : Form
    {
        private IConnect connect = null;
        private string server = string.Empty;
        private string name = string.Empty;
        private string pwd = string.Empty;
        private int port = 0;
        private string dbname = string.Empty;
        private string tablename = string.Empty;

        private List<TabPage> cache_tabPageList = new List<TabPage>();
        private int currentTabIndex = 0;

        /// <summary>
        /// 是否是导入，如果是导入操作，就不需要清理某些列的内容了。
        /// </summary>
        private bool isImport = false;

        public MainFrm_New()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            InitListViewColumns();
            InitDefaultSetting();
            foreach (TabPage tab in this.tabControl1.TabPages)
            {
                cache_tabPageList.Add(tab);
            }

            do
            {
                this.tabControl1.TabPages.RemoveAt(1);
            } while (this.tabControl1.TabPages.Count > 1);
        }

        private void InitListViewColumns()
        {
            this.lstDBs.FullRowSelect = true;
            this.lstDBs.HideSelection = false;
            this.lstDBs.Columns.Add("名称", 250, HorizontalAlignment.Left);

            this.lstTables.FullRowSelect = true;
            this.lstTables.HideSelection = false;
            this.lstTables.MultiSelect = true;
            this.lstTables.Columns.Add("名称", 300, HorizontalAlignment.Left);

            this.lstColumns.FullRowSelect = true;
            this.lstColumns.Columns.Add("名称", 200, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("主键", 46, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("自增", 46, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("类型", 80, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("长度", 46, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("允许空", 60, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("默认值", 100, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("注释", 120, HorizontalAlignment.Left);
        }

        private void InitDefaultSetting()
        {
            this.txtNameSpace.Text = StaticVariable.Default_NameSpace;
            this.txtModelSuffix.Text = StaticVariable.Default_Model_Suffix;
            this.txtUISuffix.Text = StaticVariable.Default_UI_Suffix;
            this.txtDalSuffix.Text = StaticVariable.Default_DAL_Suffix;
        }

        private void SetDbList(ListView lst, List<string> dbList)
        {
            this.Invoke(new Action<ListView>(p =>
            {
                p.BeginUpdate();
                p.Items.Clear();
                foreach (var db in dbList)
                {
                    ListViewItem item = new ListViewItem(db);
                    p.Items.Add(item);
                }

                p.EndUpdate();
            }), lst);
        }

        private void SetColumnsList(List<SqlColumnInfo> list)
        {
            this.Invoke(new Action<ListView>(p =>
            {
                p.BeginUpdate();
                p.Items.Clear();
                foreach (var columnInfo in list)
                {
                    ListViewItem item = new ListViewItem(columnInfo.Name);
                    item.SubItems.AddRange(new string[] 
                    { 
                        columnInfo.IsMainKey ? "√" : "", 
                        columnInfo.IsAutoIncrement ? "√" : "" ,
                        columnInfo.DbType,
                        columnInfo.Length,
                        columnInfo.IsAllowNull ? "√" : "",
                        columnInfo.DefaultValue,
                        columnInfo.Comment
                    });

                    p.Items.Add(item);
                }

                p.EndUpdate();
            }), this.lstColumns);
        }

        #region 选择数据库

        private void btnConnect_Click(object sender, EventArgs e)
        {
            int dbType = 0;
            if (rbtnMySql.Checked)
            {
                connect = new MySqlConnectHelper();
            }
            else if (rbtnMsSql.Checked)
            {
                dbType = 1;
                connect = new MsSqlConnectHelper();
            }
            else if (rbtnSqlite.Checked)
            {
                dbType = 2;
                connect = null;
            }

            try
            {
                this.server = this.txtServer.Text;
                this.name = this.txtName.Text;
                this.pwd = this.txtPwd.Text;
                this.port = int.Parse(this.txtPort.Text);
                var list = connect.GetDbList(this.server, this.name, this.pwd, port);

                SetDbList(lstDBs, list);
                PageCache.SetServer(this.server, this.name, this.pwd, this.port);
                PageCache.SetDbType(dbType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstDBs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (connect != null && lstDBs.SelectedItems.Count > 0)
            {
                this.dbname = (lstDBs.SelectedItems[0] as ListViewItem).Text;
                var list = connect.GetTableList(this.server, this.name, this.pwd, port, dbname);

                SetDbList(lstTables, list);
                PageCache.SetDatabase(this.dbname);
            }
        }

        private void lstTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (connect != null && lstTables.SelectedItems.Count > 0)
            {
                this.tablename = (lstTables.SelectedItems[0] as ListViewItem).Text;
                var list = connect.GetColumnsList(server, name, pwd, port, dbname, tablename);
                SetColumnsList(list);

                PageCache.SetTable(this.tablename);
                PageCache.SetColumnList(list, isImport);
            }
        }

        private void rbtnMySql_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPort.Enabled = true;
            RadioButton rbtn = (sender as RadioButton);
            if (rbtn.Text == "MySql")
            {
                this.txtPort.Text = StaticVariable.MySql_Default_Port;
            }
            else if (rbtn.Text == "MsSql")
            {
                this.txtPort.Text = StaticVariable.MsSql_Default_Port;
            }
            else
            {
                this.txtPort.Text = "0";
                this.txtPort.Enabled = false;
            }
        }

        #endregion

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action<TabControl>(p =>
            {
                p.TabPages.RemoveAt(0);
                p.TabPages.AddRange(new TabPage[] { cache_tabPageList[0] });
            }), this.tabControl1);
        }

        private void btnNextMulTable_Click(object sender, EventArgs e)
        {
            List<string> tableList = new List<string>();
            if (connect != null && lstTables.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lstTables.SelectedItems)
                {
                    tableList.Add(item.Text);
                }
            }

            if (tableList.Count > 0)
            {
                AspNetCoreHelper.InitDbName(PageCache.DatabaseName);
                AspNetCoreHelper.InitTables(tableList);
                foreach (var tbName in tableList)
                {
                    var list = connect.GetColumnsList(server, name, pwd, port, dbname, tbName);
                    AspNetCoreHelper.AddColumnList(tbName, list);
                }

                this.Invoke(new Action<TabControl>(p =>
                {
                    int mulIndex = 2;
                    p.TabPages.RemoveAt(0);
                    p.TabPages.AddRange(new TabPage[] { cache_tabPageList[mulIndex] });
                }), this.tabControl1);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            List<string> tableList = new List<string>();
            if (connect != null && lstTables.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lstTables.SelectedItems)
                {
                    tableList.Add(item.Text);
                }
            }

            if (tableList.Count > 0)
            {
                AspNetHelper.InitDbName(PageCache.DatabaseName);
                AspNetHelper.InitTables(tableList);
                foreach (var tbName in tableList)
                {
                    var list = connect.GetColumnsList(server, name, pwd, port, dbname, tbName);
                    AspNetHelper.AddColumnList(tbName, list);
                }

                this.Invoke(new Action<TabControl>(p =>
                {
                    int mulIndex = 1;
                    p.TabPages.RemoveAt(0);
                    p.TabPages.AddRange(new TabPage[] { cache_tabPageList[mulIndex] });
                }), this.tabControl1);
            }
        }

        private void btnNoSave_Click(object sender, EventArgs e)
        {
            // 初始化设置成默认
        }

        #region asp.net

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            PageCache.SetUIType(this.rbtnEasyUI.Checked ? 0 : 1);
            PageCache.SetDbTool(this.rbtn_Sql.Checked ? 0 : this.rbtn_ORM_Dapper.Checked ? 1 : this.rbtn_NHibernate.Checked ? 2 : 3);
            PageCache.SetWebType(this.rbtn_web_asp_net.Checked ? 0 : this.rbtn_web_mvc.Checked ? 1 : 2);
            PageCache.SetModelType(this.rbtn_attr_attr.Checked ? 0 : this.rbtn_attr_lowStart.Checked ? 1 : 2);
            PageCache.SetParamValue(this.txtNameSpace.Text, this.txtModelSuffix.Text, this.txtDalSuffix.Text, this.txtUISuffix.Text);

            Button btn = sender as Button;
            string action = btn.Text;
            switch (action)
            {
                case "Model":
                    AspNet_CreateModel();
                    break;
                case "Aspx":
                    AspNet_CreateAspx();
                    break;
                case "Aspx.cs":
                    AspNet_CreateAspxcs();
                    break;
            }

            if (btn.Text == "Model")
            {
            }
            else if (btn.Text == "Aspx")
            {
            }
            else if (btn.Text == "Aspx.cs")
            {
            }
            else if (btn.Text == "DAL")
            {
                #region DAL
                int uitype = PageCache.UIType;
                int dbtype = PageCache.DbType;
                int dbtool = PageCache.DbTool;
                if (dbtool == 0)
                {
                    if (uitype == 0)
                    {
                        // easyui
                        if (dbtype == 0)
                        {
                            // mysql
                            string modelStr = DALHelper_EasyUI_MySql.CreateDAL();
                        }
                        else if (dbtype == 1)
                        {
                            // mssql
                            string modelStr = DALHelper_EasyUI_MsSql.CreateDAL();
                        }
                    }
                    else if (uitype == 1)
                    {
                        // bootstarp
                        if (dbtype == 0)
                        {
                            // mysql
                            string modelStr = DALHelper_Bootstrap_MySql.CreateDAL();
                        }
                        else if (dbtype == 1)
                        {
                            // mssql
                            string modelStr = DALHelper_Bootstrap_MsSql.CreateDAL();
                        }
                    }
                }
                else if (dbtool == 1)
                {
                    string modelStr = DALHelper_Dapper.CreateDAL();
                }
                #endregion
            }
            else if (btn.Text == "Factory")
            {
                #region Factory
                int dbtype = PageCache.DbType;
                int dbtool = PageCache.DbTool;
                if (dbtool == 0)
                {
                    if (dbtype == 0)
                    {
                        // mysql
                        string modelStr = FactoryHelper_MySql.CreateFactory();
                    }
                    else if (dbtype == 1)
                    {
                        // mysql
                        string modelStr = FactoryHelper_MsSql.CreateFactory();
                    }
                }
                else if (dbtool == 1)
                {
                    if (dbtype == 0)
                    {
                        // mysql
                        string modelStr = FactoryHelper_Dapper_MySql.CreateFactory();
                    }
                    else if (dbtype == 1)
                    {
                        // mysql
                        string modelStr = FactoryHelper_Dapper_MsSql.CreateFactory();
                    }
                }
                #endregion
            }
            else if (btn.Text == "Config")
            {
                #region Config
                int uitype = PageCache.UIType;
                int dbtype = PageCache.DbType;
                if (uitype == 0)
                {
                    // easyui
                    if (dbtype == 0)
                    {
                        // mysql
                        string modelStr = ConfigHelper.GetConnectStringConfig();
                    }
                }
                else if (uitype == 1)
                {
                    // bootstarp
                    if (dbtype == 0)
                    {
                        // mysql
                        string modelStr = ConfigHelper.GetConnectStringConfig();
                    }
                }
                #endregion
            }
            else if (btn.Text == "View")
            {
                #region View
                int webType = PageCache.WebType;
                int uiType = PageCache.UIType;
                if (webType == 1)
                {
                    if (uiType == 0)
                    {
                        string viewStr = MvcViewHelper_EasyUI.CreateView();
                    }
                    else if (uiType == 1)
                    {
                        string viewStr = MvcViewHelper_Bootstrap.CreateView();
                    }

                }
                #endregion
            }
            else if (btn.Text == "Controller")
            {
                #region Controller
                string controllerStr = MvcControllerHelper.CreateController();
                #endregion
            }
        }

        private void btmExport_Click(object sender, EventArgs e)
        {
            // 打包当前数据到zip并保存
            if (tbAspNet.TabPages.Count > 0)
            {
                Dictionary<string, string> fileDic = new Dictionary<string, string>();
                foreach (TabPage item in tbAspNet.TabPages)
                {
                    string key = item.Text;
                    string text = (item.Controls[0] as TextBox).Text;

                    if (!fileDic.ContainsKey(key + ".cs"))
                    {
                        fileDic.Add(key + ".cs", text);
                    }
                }

                if (fileDic.Count > 0)
                {
                    var data = ZipHelper.Zip(fileDic);

                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "压缩文件|*.zip";
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string fileName = dialog.FileName;
                        if (!fileName.EndsWith(".zip"))
                        {
                            fileName += ".zip";
                        }

                        File.WriteAllBytes(fileName, data);
                    }
                }
            }
        }

        private void AspNet_CreateModel()
        {
            Dictionary<string, string> result = AspNetHelper.CreateModel(this.txtNameSpace.Text, this.txtModelSuffix.Text, this.chkQueryModel.Checked, this.chkIsSplit.Checked);

            FillInTab(result, tbAspNet);
        }

        private void AspNet_CreateAspx()
        {
            int action = (int)(action_type.add | action_type.edit | action_type.query_all);
            Dictionary<string, string> result = AspNetHelper.CreateAspx(this.txtNameSpace.Text, this.txtModelSuffix.Text, action);

            FillInTab(result, tbAspNet);
        }

        private void AspNet_CreateAspxcs()
        {
            int action = (int)(action_type.add | action_type.edit | action_type.query_all);
            Dictionary<string, string> result = AspNetHelper.CreateAspxcs(this.txtNameSpace.Text, this.txtModelSuffix.Text, this.txtDalSuffix.Text, action);

            FillInTab(result, tbAspNet);
        }

        #endregion

        #region asp.net core

        private void btnCore_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Text)
            {
                case "Model":
                    Core_CreateModel();
                    break;
                case "ApiController":
                    Core_CreateApiController();
                    break;
                case "DAL":
                    Core_CreateDAL();
                    break;
                case "Other":
                    Core_CreateOther();
                    break;
                case "CopyClass":
                    Core_CreateCopyClass();
                    break;
                case "CopyFullClass":
                    Core_CreateCopyFullClass();
                    break;
                case "SaveFile":
                    Core_SaveFile();
                    break;
            }
        }

        private void Core_CreateModel()
        {
            Dictionary<string, string> result = AspNetCoreHelper.CreateModel(this.txtCoreNameSpace.Text, this.txtCoreModelSuffix.Text, this.chkQueryModel.Checked, this.chkIsSplit.Checked);

            FillInTab(result, tbAspNetCore);
        }

        private void Core_CreateApiController()
        {
            Dictionary<string, string> result = AspNetCoreHelper.CreateApiController(this.txtCoreNameSpace.Text, this.txtCoreDalSuffix.Text, this.txtCoreModelSuffix.Text);

            FillInTab(result, tbAspNetCore);
        }

        private void Core_CreateDAL()
        {
            Dictionary<string, string> result = AspNetCoreHelper.CreateDAL(this.txtCoreNameSpace.Text, this.txtCoreDalSuffix.Text, this.txtCoreModelSuffix.Text);

            FillInTab(result, tbAspNetCore);
        }

        private void Core_CreateOther()
        {
            Dictionary<string, string> result = AspNetCoreHelper.CreateOther(this.txtCoreNameSpace.Text);

            FillInTab(result, tbAspNetCore);
        }

        private void Core_CreateCopyClass()
        {
            if (this.tbAspNetCore.TabPages.Count > 0)
            {
                string text = (this.tbAspNetCore.TabPages[this.tbAspNetCore.SelectedIndex].Controls[0] as TextBox).Text;

                // 只拷贝类代码
                Regex regex = new Regex(@"\{\s+(?<text>[\s\S]+)\s+\}\s{0,}$");
                string data = regex.Match(text).Groups["text"].Value;

                // 处理 text
                Clipboard.SetText(data);
            }
        }

        private void Core_CreateCopyFullClass()
        {
            if (this.tbAspNetCore.TabPages.Count > 0)
            {
                Clipboard.SetText((this.tbAspNetCore.TabPages[this.tbAspNetCore.SelectedIndex].Controls[0] as TextBox).Text);
            }
        }

        private void Core_SaveFile()
        {
            // 打包当前数据到zip并保存
            if (tbAspNetCore.TabPages.Count > 0)
            {
                Dictionary<string, string> fileDic = new Dictionary<string, string>();
                foreach (TabPage item in tbAspNetCore.TabPages)
                {
                    string key = item.Text;
                    string text = (item.Controls[0] as TextBox).Text;

                    if (!fileDic.ContainsKey(key + ".cs"))
                    {
                        fileDic.Add(key + ".cs", text);
                    }
                }

                if (fileDic.Count > 0)
                {
                    var data = ZipHelper.Zip(fileDic);

                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "压缩文件|*.zip";
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string fileName = dialog.FileName;
                        if (!fileName.EndsWith(".zip"))
                        {
                            fileName += ".zip";
                        }

                        File.WriteAllBytes(fileName, data);
                    }
                }
            }
        }
        #endregion

        private void FillInTab(Dictionary<string, string> result, TabControl tabControl)
        {
            tabControl.TabPages.Clear();
            foreach (var item in result)
            {
                TextBox textBox = new TextBox();
                textBox.Location = new System.Drawing.Point(6, 6);
                textBox.Multiline = true;
                textBox.Size = new System.Drawing.Size(1022, 462);
                textBox.TabIndex = 0;
                textBox.ScrollBars = ScrollBars.Vertical;
                textBox.Text = item.Value;

                TabPage tb = new TabPage();
                tb.Text = item.Key;
                tb.Controls.Add(textBox);

                tabControl.TabPages.Add(tb);
            }
        }

        private void txtResult_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null)
                return;
            if (e.KeyChar == (char)1)       // Ctrl-A 相当于输入了AscII=1的控制字符
            {
                textBox.SelectAll();
                e.Handled = true;      // 不再发出“噔”的声音
            }
        }
    }
}