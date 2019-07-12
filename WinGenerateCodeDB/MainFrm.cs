using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WinGenerateCodeDB.Cache;
using WinGenerateCodeDB.Child;
using WinGenerateCodeDB.Code;
using WinGenerateCodeDB.Model;

namespace WinGenerateCodeDB
{
    public partial class MainFrm : Form
    {
        private IConnect connect = null;
        private string server = string.Empty;
        private string name = string.Empty;
        private string pwd = string.Empty;
        private int port = 0;
        private string dbname = string.Empty;
        private string tablename = string.Empty;

        private List<TabPage> tabPageList = new List<TabPage>();
        private int currentTabIndex = 0;

        /// <summary>
        /// 是否是导入，如果是导入操作，就不需要清理某些列的内容了。
        /// </summary>
        private bool isImport = false;

        public MainFrm()
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
                tabPageList.Add(tab);
            }

            this.tabControl1.TabPages.RemoveAt(1);
            this.tabControl1.TabPages.RemoveAt(1);
            this.tabControl1.TabPages.RemoveAt(1);
        }

        private void InitListViewColumns()
        {
            this.lstDBs.FullRowSelect = true;
            this.lstDBs.HideSelection = false;
            this.lstDBs.Columns.Add("名称", 250, HorizontalAlignment.Left);

            this.lstTables.FullRowSelect = true;
            this.lstTables.HideSelection = false;
            this.lstTables.Columns.Add("名称", 260, HorizontalAlignment.Left);

            this.lstColumns.FullRowSelect = true;
            this.lstColumns.Columns.Add("名称", 200, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("主键", 46, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("自增", 46, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("类型", 80, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("长度", 46, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("允许空", 60, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("默认值", 100, HorizontalAlignment.Left);
            this.lstColumns.Columns.Add("注释", 120, HorizontalAlignment.Left);

            this.lstFiles.FullRowSelect = true;
            this.lstFiles.Columns.Add("文件名", 300, HorizontalAlignment.Left);

            this.lstFields.FullRowSelect = true;
            this.lstFields.MultiSelect = true;
            this.lstFields.HideSelection = false;
            this.lstFields.Columns.Add("列名", 160, HorizontalAlignment.Left);
            this.lstFields.Columns.Add("备注", 115, HorizontalAlignment.Left);

            this.lstExtendAttribute.FullRowSelect = true;
            this.lstExtendAttribute.MultiSelect = true;
            this.lstExtendAttribute.HideSelection = false;
            this.lstExtendAttribute.Columns.Add("列名", 120, HorizontalAlignment.Left);
            this.lstExtendAttribute.Columns.Add("备注", 100, HorizontalAlignment.Left);
            this.lstExtendAttribute.Columns.Add("类型", 65, HorizontalAlignment.Left);

            this.lstSelectedFields.FullRowSelect = true;
            this.lstSelectedFields.HideSelection = false;
            this.lstSelectedFields.Columns.Add("属性名", 120, HorizontalAlignment.Left);
            this.lstSelectedFields.Columns.Add("列名", 80, HorizontalAlignment.Left);
            this.lstSelectedFields.Columns.Add("标题", 80, HorizontalAlignment.Left);
            this.lstSelectedFields.Columns.Add("样式", 75, HorizontalAlignment.Left);

            this.lstCmdList.FullRowSelect = true;
            this.lstCmdList.HideSelection = false;
            this.lstCmdList.Columns.Add("操作", 70, HorizontalAlignment.Left);
            this.lstCmdList.Columns.Add("列", 145, HorizontalAlignment.Left);
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

                this.lstFields.Items.Clear();
                this.lstExtendAttribute.Items.Clear();
                foreach (var model in list)
                {
                    ListViewItem item = new ListViewItem(model.Name);
                    item.Tag = model;
                    item.SubItems.AddRange(new string[] { model.Comment });
                    this.lstFields.Items.Add(item);
                }
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

        #region 功能选择

        private void btnSelectedField_Click(object sender, EventArgs e)
        {
            if (this.lstFields.SelectedItems.Count > 0)
            {
                var list = new List<SixStringInfo>();
                foreach (ListViewItem item in this.lstFields.SelectedItems)
                {
                    list.Add(new SixStringInfo() { AttrName = item.Text, ColName = item.Text, Remark = item.SubItems[1].Text, AttrType = (item.Tag as SqlColumnInfo).DbType, DbType = (item.Tag as SqlColumnInfo).DbType });
                }

                List<SixStringInfo> selectedFields = new List<SixStringInfo>();
                foreach (ListViewItem item in this.lstSelectedFields.Items)
                {
                    var info = item.Tag as SixStringInfo;
                    selectedFields.Add(info);
                }

                foreach (var field in list)
                {
                    var model = selectedFields.Find(p => p.AttrName == field.AttrName);
                    if (model == null)
                    {
                        selectedFields.Add(field);
                        ListViewItem item = new ListViewItem(field.AttrName);
                        item.Tag = field;
                        item.SubItems.AddRange(new string[] { field.ColName, field.Remark, field.Style.FieldName });
                        this.lstSelectedFields.Items.Add(item);
                    }
                }
            }
        }

        private void btnUnSelectedField_Click(object sender, EventArgs e)
        {
            if (this.lstSelectedFields.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in this.lstSelectedFields.SelectedItems)
                {
                    this.lstSelectedFields.Items.Remove(item);
                }
            }
        }

        private void btnSetTitle_Click(object sender, EventArgs e)
        {
            if (this.lstSelectedFields.SelectedItems.Count > 0)
            {
                this.lstSelectedFields.SelectedItems[0].SubItems[2].Text = this.txtTitle.Text;
                ((WinGenerateCodeDB.Model.SixStringInfo)(this.lstSelectedFields.SelectedItems[0].Tag)).Remark = this.txtTitle.Text;
                if (this.chkJumpNext.Checked)
                {
                    this.lstSelectedFields.Selecte_Next();
                }
            }
        }

        private void btnAddStyle_Click(object sender, EventArgs e)
        {
            if (this.lstSelectedFields.SelectedItems.Count > 0)
            {
                ListViewItem item = this.lstSelectedFields.SelectedItems[0];
                item.SubItems[3].Text = this.tabControl2.SelectedTab.Text;
                FieldStyleInfo styleInfo = new FieldStyleInfo();
                styleInfo.FieldName = item.Text;
                styleInfo.FormatType = this.tabControl2.SelectedIndex;
                if (styleInfo.FormatType == 0)
                {
                    styleInfo.FormatStr = this.txtLimitLength.Text;
                }
                else if (styleInfo.FormatType == 0)
                {
                    styleInfo.FormatStr = this.txtDateFormat.Text;
                }
                else if (styleInfo.FormatType == 0)
                {
                    styleInfo.FormatStr = this.txtDefaultSelectValue.Text + "$$$" + this.txtSelectData.Text;
                }

                var tagInfo = item.Tag as SixStringInfo;
                tagInfo.Style = styleInfo;
                item.Tag = tagInfo;

                if (this.chkJumpNext_Style.Checked &&
                    this.lstSelectedFields.Items.Count > this.lstSelectedFields.SelectedItems[0].Index + 1)
                {
                    this.lstSelectedFields.SelectedItems[0].Selected = false;
                    this.lstSelectedFields.Items[this.lstSelectedFields.SelectedItems[0].Index + 1].Selected = true;
                }
            }
        }

        private void btnCmd_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            CmdInfo info = new CmdInfo();
            info.AttrList = new List<ClassAttributeInfo>();

            List<string> columnList = new List<string>();
            foreach (ListViewItem item in this.lstSelectedFields.Items)
            {
                var tagInfo = item.Tag as SixStringInfo;

                columnList.Add(item.Text);
                ClassAttributeInfo attrInfo = new ClassAttributeInfo();
                attrInfo.AttrName = tagInfo.AttrName;
                attrInfo.ColName = tagInfo.ColName;
                attrInfo.TitleName = tagInfo.Remark;
                attrInfo.AttrType = tagInfo.AttrType;
                attrInfo.DbType = tagInfo.DbType;
                attrInfo.Style = tagInfo.Style;

                info.AttrList.Add(attrInfo);
            }

            ListViewItem cmdItem = new ListViewItem(btn.Text);
            cmdItem.SubItems.AddRange(new string[] { string.Join(",", columnList.ToArray()) });
            cmdItem.Tag = Guid.NewGuid().ToString("N");
            this.lstCmdList.Items.Add(cmdItem);

            info.CmdName = cmdItem.Text;
            info.Guid = cmdItem.Tag as string;
            PageCache.AddCmd(info);
        }

        private void btnAddExtendAttribute_Click(object sender, EventArgs e)
        {
            List<string> columnNameList = new List<string>();
            foreach (ListViewItem item in this.lstFields.Items)
            {
                columnNameList.Add(item.Text);
            }

            foreach (ListViewItem item in this.lstExtendAttribute.Items)
            {
                columnNameList.Add(item.Text);
            }

            if (this.lstFields.SelectedItems.Count > 0)
            {
                string fieldsStr = this.lstFields.SelectedItems[0].Text;
                string fieldDbType = (this.lstFields.SelectedItems[0].Tag as SqlColumnInfo).DbType;
                string commentStr = this.lstFields.SelectedItems[0].SubItems[1].Text;
                FieldExtendFrm frm = new FieldExtendFrm(columnNameList, fieldsStr, fieldDbType, commentStr);
                frm.ShowDialog();
                if (frm.IsAddSuccess)
                {
                    ListViewItem item = new ListViewItem(frm.ExtendInfo.NewAttName);
                    item.Tag = frm.ExtendInfo;
                    item.SubItems.AddRange(new string[] { frm.ExtendInfo.Comment, frm.ExtendInfo.AttributeType });
                    this.lstExtendAttribute.Items.Add(item);

                    PageCache.AddAttribute(frm.ExtendInfo);
                }
            }
        }

        private void btnChangeExtendAttribute_Click(object sender, EventArgs e)
        {
            List<string> columnNameList = new List<string>();
            foreach (ListViewItem item in this.lstFields.Items)
            {
                columnNameList.Add(item.Text);
            }

            foreach (ListViewItem item in this.lstExtendAttribute.Items)
            {
                columnNameList.Add(item.Text);
            }

            if (this.lstExtendAttribute.SelectedItems.Count > 0)
            {
                ListViewItem editItem = this.lstExtendAttribute.SelectedItems[0];
                string oldAttrName = editItem.Text;
                columnNameList.Remove(oldAttrName);

                FieldExtendFrm frm = new FieldExtendFrm(columnNameList, editItem.Text, editItem.Tag as ExtendAttributeInfo);
                frm.ShowDialog();
                if (frm.IsAddSuccess)
                {
                    this.lstExtendAttribute.SelectedItems[0].Tag = frm.ExtendInfo;
                    editItem.Text = frm.ExtendInfo.NewAttName;
                    editItem.SubItems[1].Text = frm.ExtendInfo.Comment;
                    PageCache.ChangeExtendInfo(oldAttrName, frm.ExtendInfo);
                }
            }
        }

        private void btnRemoveExtendAttribute_Click(object sender, EventArgs e)
        {
            if (this.lstExtendAttribute.SelectedItems.Count > 0)
            {
                for (int i = 0; i < this.lstExtendAttribute.SelectedItems.Count; )
                {
                    PageCache.RemoveAttribute(this.lstExtendAttribute.SelectedItems[0].Text);
                    this.lstExtendAttribute.SelectedItems[0].Remove();
                }
            }
        }

        private void btnRemoveCmd_Click(object sender, EventArgs e)
        {
            if (this.lstCmdList.SelectedItems.Count > 0)
            {
                for (int i = 0; i < this.lstCmdList.SelectedItems.Count; )
                {
                    PageCache.RemoveCmd(this.lstCmdList.SelectedItems[0].Tag as string);
                    this.lstCmdList.SelectedItems[0].Remove();
                }
            }
        }

        private void btnSelectedField2_Click(object sender, EventArgs e)
        {
            if (this.lstExtendAttribute.SelectedItems.Count > 0)
            {
                var list = new List<SixStringInfo>();
                foreach (ListViewItem item in this.lstExtendAttribute.SelectedItems)
                {
                    var extendInfo = item.Tag as ExtendAttributeInfo;
                    list.Add(new SixStringInfo() { AttrName = item.Text, ColName = extendInfo.DependColumn, Remark = item.SubItems[1].Text, DbType = extendInfo.DependColumnType, AttrType = "varchar" });
                }

                List<SixStringInfo> selectedFields = new List<SixStringInfo>();
                foreach (ListViewItem item in this.lstSelectedFields.Items)
                {
                    var tagInfo = item.Tag as SixStringInfo;
                    selectedFields.Add(tagInfo);
                }

                foreach (var field in list)
                {
                    var model = selectedFields.Find(p => p.AttrName == field.AttrName);
                    if (model == null)
                    {
                        selectedFields.Add(field);
                        ListViewItem item = new ListViewItem(field.AttrName);
                        item.Tag = field;
                        item.SubItems.AddRange(new string[] { field.ColName, field.Remark, field.Style.FieldName });
                        this.lstSelectedFields.Items.Add(item);
                    }
                }
            }
        }

        private void btnSetExtendComment_Click(object sender, EventArgs e)
        {
            string comment = this.txtExtendComment.Text;
            if (this.lstExtendAttribute.SelectedItems.Count > 0)
            {
                this.lstExtendAttribute.SelectedItems[0].SubItems[1].Text = comment;
                this.lstExtendAttribute.Selecte_Next();
            }
        }

        private void btnSetFieldComment_Click(object sender, EventArgs e)
        {
            string comment = this.txtFieldComment.Text;
            if (this.lstFields.SelectedItems.Count > 0)
            {
                this.lstFields.SelectedItems[0].SubItems[1].Text = comment;
                this.lstFields.Selecte_Next();
            }
        }
        #endregion

        #region 结果
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Text == "Model")
            {
                #region Model
                int modelType = PageCache.ModelStyle;
                if (modelType == 0)
                {
                    string modelStr = ModelHelper_Default.CreateModel();
                    this.txtResult.Text = modelStr;
                }
                else if (modelType == 1)
                {
                    string modelStr = ModelHelper_DefaultAttribute.CreateModel();
                    this.txtResult.Text = modelStr;
                }
                else
                {
                    string modelStr = ModelHelper_DefaultLowerField.CreateModel();
                    this.txtResult.Text = modelStr;
                }
                #endregion
            }
            else if (btn.Text == "Aspx")
            {
                #region Aspx
                int uitype = PageCache.UIType;
                if (uitype == 0)
                {
                    // easyui
                    string modelStr = AspxHelper_EasyUI.CreateASPX();
                    this.txtResult.Text = modelStr;
                }
                else if (uitype == 1)
                {
                    // bootstarp
                    string modelStr = AspxHelper_Bootstrap.CreateASPX();
                    this.txtResult.Text = modelStr;
                }
                #endregion
            }
            else if (btn.Text == "Aspx.cs")
            {
                #region Aspx.cs
                int uitype = PageCache.UIType;
                if (uitype == 0)
                {
                    // easyui
                    string modelStr = AspxCsHelper_EasyUI.CreateASPXCS();

                    this.txtResult.Text = modelStr;
                }
                else if (uitype == 1)
                {
                    // bootstarp
                    string modelStr = AspxCsHelper_Bootstrap.CreateASPXCS();
                    this.txtResult.Text = modelStr;
                }
                #endregion
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

                            this.txtResult.Text = modelStr;
                        }
                        else if (dbtype == 1)
                        {
                            // mssql
                            string modelStr = DALHelper_EasyUI_MsSql.CreateDAL();

                            this.txtResult.Text = modelStr;
                        }
                    }
                    else if (uitype == 1)
                    {
                        // bootstarp
                        if (dbtype == 0)
                        {
                            // mysql
                            string modelStr = DALHelper_Bootstrap_MySql.CreateDAL();
                            this.txtResult.Text = modelStr;
                        }
                        else if (dbtype == 1)
                        {
                            // mssql
                            string modelStr = DALHelper_Bootstrap_MsSql.CreateDAL();
                            this.txtResult.Text = modelStr;
                        }
                    }
                }
                else if (dbtool == 1)
                {
                    string modelStr = DALHelper_Dapper.CreateDAL();

                    this.txtResult.Text = modelStr;
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

                        this.txtResult.Text = modelStr;
                    }
                    else if (dbtype == 1)
                    {
                        // mysql
                        string modelStr = FactoryHelper_MsSql.CreateFactory();
                        this.txtResult.Text = modelStr;
                    }
                }
                else if (dbtool == 1)
                {
                    if (dbtype == 0)
                    {
                        // mysql
                        string modelStr = FactoryHelper_Dapper_MySql.CreateFactory();

                        this.txtResult.Text = modelStr;
                    }
                    else if (dbtype == 1)
                    {
                        // mysql
                        string modelStr = FactoryHelper_Dapper_MsSql.CreateFactory();
                        this.txtResult.Text = modelStr;
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

                        this.txtResult.Text = modelStr;
                    }
                }
                else if (uitype == 1)
                {
                    // bootstarp
                    if (dbtype == 0)
                    {
                        // mysql
                        string modelStr = ConfigHelper.GetConnectStringConfig();

                        this.txtResult.Text = modelStr;
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
                        this.txtResult.Text = viewStr;
                    }
                    else if (uiType == 1)
                    {
                        string viewStr = MvcViewHelper_Bootstrap.CreateView();
                        this.txtResult.Text = viewStr;
                    }

                }
                #endregion
            }
            else if (btn.Text == "Controller")
            {
                #region Controller
                string controllerStr = MvcControllerHelper.CreateController();

                this.txtResult.Text = controllerStr;
                #endregion
            }
            else if (btn.Text == "Test")
            {
                #region Test

                #endregion
            }
        }

        private void btmExport_Click(object sender, EventArgs e)
        {
            if (this.lstFiles.CheckedItems.Count > 0)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件保存路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string foldPath = dialog.SelectedPath;
                    foreach (ListViewItem item in this.lstFiles.CheckedItems)
                    {
                        GenerateInfo generateInfo = item.Tag as GenerateInfo;
                        string newName = generateInfo.ShartName_NotTxt;
                        foreach (var dicItem in GolableSetting.GetDic())
                        {
                            newName = newName.Replace(dicItem.Key, dicItem.Value);
                        }

                        File.WriteAllBytes(Path.Combine(foldPath, newName), generateInfo.FileData);
                    }
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lstFiles.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        #endregion

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action<TabControl>(p =>
            {
                if (currentTabIndex < (this.tabPageList.Count - 1))
                {
                    currentTabIndex++;
                    p.TabPages.RemoveAt(0);
                    p.TabPages.AddRange(new TabPage[] { tabPageList[currentTabIndex] });
                    if (currentTabIndex == 3)
                    {
                        PageCache.SetUIType(this.rbtnEasyUI.Checked ? 0 : 1);
                        PageCache.SetDbTool(this.rbtn_Sql.Checked ? 0 : this.rbtn_ORM_Dapper.Checked ? 1 : this.rbtn_NHibernate.Checked ? 2 : 3);
                        PageCache.SetParamValue(this.txtNameSpace.Text, this.txtModelSuffix.Text, this.txtDalSuffix.Text, this.txtUISuffix.Text);
                        PageCache.SetWebType(this.rbtn_web_asp_net.Checked ? 0 : this.rbtn_web_mvc.Checked ? 1 : 2);
                    }
                }
            }), this.tabControl1);
        }

        private void btnBefore_Click(object sender, EventArgs e)
        {
            if (currentTabIndex > 0)
            {
                currentTabIndex--;
                this.tabControl1.TabPages.RemoveAt(0);
                this.tabControl1.TabPages.AddRange(new TabPage[] { tabPageList[currentTabIndex] });
            }
        }

        private void btnSaveProgress_Click(object sender, EventArgs e)
        {
            // 当前选中库，选中表，当前添加的属性，设置的功能，和设置的选项值
            string xml = PageCache.ToXml();
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "自动化文件|*.code";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = dialog.FileName;
                if (!fileName.EndsWith(".code"))
                {
                    fileName += ".code";
                }

                File.WriteAllText(fileName, xml, Encoding.UTF8);
            }
        }

        private void btnImportProgress_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "自动化文件|*.code";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = dialog.FileName;
                string xml = File.ReadAllText(filePath, Encoding.UTF8);

                PageCache.ReadXml(xml);
                isImport = true;
                //AutoTestExecute();

                Thread autoTest = new Thread(AutoTestExecute);
                autoTest.IsBackground = true;
                autoTest.Start();
            }
        }

        private void AutoTestExecute()
        {
            try
            {
                int lowMinSeconds = 300;
                int longMinSeconds = 1000;
                this.tabControl1.TabIndex = 0;
                Thread.Sleep(lowMinSeconds);
                this.txtServer.Text = PageCache.server;
                this.txtName.Text = PageCache.name;
                this.txtPwd.Text = PageCache.pwd;
                this.txtPort.Text = PageCache.port.ToString();
                Thread.Sleep(lowMinSeconds);
                this.btnConnect_Click(this.btnConnect, new EventArgs());
                Thread.Sleep(lowMinSeconds);
                foreach (ListViewItem item in this.lstDBs.Items)
                {
                    if (item.Text == PageCache.DatabaseName)
                    {
                        item.Selected = true;
                        break;
                    }
                }

                Thread.Sleep(longMinSeconds);
                foreach (ListViewItem item in this.lstTables.Items)
                {
                    if (item.Text == PageCache.TableName)
                    {
                        item.Selected = true;
                        break;
                    }
                }

                Thread.Sleep(longMinSeconds);
                this.btnNext_Click(this.btnNext, new EventArgs());
                Thread.Sleep(lowMinSeconds);

                this.lstExtendAttribute.Items.Clear();
                foreach (var extendInfo in PageCache.GetExtendList())
                {
                    ListViewItem item = new ListViewItem(extendInfo.NewAttName);
                    item.Tag = extendInfo;
                    item.SubItems.AddRange(new string[] { extendInfo.Comment, extendInfo.AttributeType });
                    this.lstExtendAttribute.Items.Add(item);
                }

                Thread.Sleep(lowMinSeconds);
                this.lstCmdList.Items.Clear();
                foreach (var info in PageCache.GetCmdList())
                {
                    ListViewItem cmdItem = new ListViewItem(info.CmdName);
                    cmdItem.SubItems.AddRange(new string[] { string.Join(",", (from f in info.AttrList select f.AttrName).ToArray()) });
                    cmdItem.Tag = Guid.NewGuid().ToString("N");
                    this.lstCmdList.Items.Add(cmdItem);
                }

                Thread.Sleep(lowMinSeconds);
                this.btnNext_Click(this.btnNext, new EventArgs());

                rbtn_Sql.Checked = PageCache.DbTool == 0;
                rbtn_ORM_Dapper.Checked = PageCache.DbTool == 1;
                rbtn_NHibernate.Checked = PageCache.DbTool == 2;
                rbtn_EF.Checked = PageCache.DbTool == 3;

                rbtnEasyUI.Checked = PageCache.UIType == 0;
                rbtnBootstrap.Checked = PageCache.UIType == 1;
                rbtn_Layui.Checked = PageCache.UIType == 2;

                rbtn_web_asp_net.Checked = PageCache.WebType == 0;
                rbtn_web_mvc.Checked = PageCache.WebType == 1;
                rbtn_web_html.Checked = PageCache.WebType == 2;

                this.txtNameSpace.Text = PageCache.NameSpaceStr;
                this.txtModelSuffix.Text = PageCache.ModelSuffix;
                this.txtDalSuffix.Text = PageCache.DALSuffix;
                this.txtUISuffix.Text = PageCache.UISuffix;

                MessageBox.Show("导入成功.");
            }
            finally
            {
                isImport = false;
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