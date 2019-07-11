namespace WinAutoCode
{
    partial class EasyUIAutoFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EasyUIAutoFrm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.类型TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEasyUI = new System.Windows.Forms.ToolStripMenuItem();
            this.tabHead = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkExport = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBatEdit = new System.Windows.Forms.TextBox();
            this.chkBatDel = new System.Windows.Forms.CheckBox();
            this.chkBatEdit = new System.Windows.Forms.CheckBox();
            this.chkEdit = new System.Windows.Forms.CheckBox();
            this.chkDel = new System.Windows.Forms.CheckBox();
            this.chkAdd = new System.Windows.Forms.CheckBox();
            this.rbtnMySQL = new System.Windows.Forms.RadioButton();
            this.rbtnMSSQL = new System.Windows.Forms.RadioButton();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnExportCode = new System.Windows.Forms.Button();
            this.btnCreateBootStrap = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtEditColumnName = new System.Windows.Forms.TextBox();
            this.txtAddColumnName = new System.Windows.Forms.TextBox();
            this.txtSearchColumnName = new System.Windows.Forms.TextBox();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabContentOne = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtClassCode = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.txtAspxCode = new System.Windows.Forms.TextBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.txtAspxCsCode = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtDALCode = new System.Windows.Forms.TextBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.txtFactoryCode = new System.Windows.Forms.TextBox();
            this.tabPage14 = new System.Windows.Forms.TabPage();
            this.txtSqlHelper = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.tabHead.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabContentOne.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage14.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.类型TToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(979, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 类型TToolStripMenuItem
            // 
            this.类型TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemNormal,
            this.menuItemEasyUI});
            this.类型TToolStripMenuItem.Name = "类型TToolStripMenuItem";
            this.类型TToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.类型TToolStripMenuItem.Text = "类型(&T)";
            // 
            // menuItemNormal
            // 
            this.menuItemNormal.Name = "menuItemNormal";
            this.menuItemNormal.Size = new System.Drawing.Size(154, 22);
            this.menuItemNormal.Text = "普通代码版(&N)";
            this.menuItemNormal.Click += new System.EventHandler(this.menuItemNormal_Click);
            // 
            // menuItemEasyUI
            // 
            this.menuItemEasyUI.Checked = true;
            this.menuItemEasyUI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemEasyUI.Name = "menuItemEasyUI";
            this.menuItemEasyUI.Size = new System.Drawing.Size(154, 22);
            this.menuItemEasyUI.Text = "EasyUI版(&E)";
            // 
            // tabHead
            // 
            this.tabHead.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabHead.Controls.Add(this.tabPage1);
            this.tabHead.Location = new System.Drawing.Point(0, 28);
            this.tabHead.Name = "tabHead";
            this.tabHead.SelectedIndex = 0;
            this.tabHead.Size = new System.Drawing.Size(979, 805);
            this.tabHead.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkExport);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.txtBatEdit);
            this.tabPage1.Controls.Add(this.chkBatDel);
            this.tabPage1.Controls.Add(this.chkBatEdit);
            this.tabPage1.Controls.Add(this.chkEdit);
            this.tabPage1.Controls.Add(this.chkDel);
            this.tabPage1.Controls.Add(this.chkAdd);
            this.tabPage1.Controls.Add(this.rbtnMySQL);
            this.tabPage1.Controls.Add(this.rbtnMSSQL);
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.btnExportCode);
            this.tabPage1.Controls.Add(this.btnCreateBootStrap);
            this.tabPage1.Controls.Add(this.btnCreate);
            this.tabPage1.Controls.Add(this.txtEditColumnName);
            this.tabPage1.Controls.Add(this.txtAddColumnName);
            this.tabPage1.Controls.Add(this.txtSearchColumnName);
            this.tabPage1.Controls.Add(this.txtDBName);
            this.tabPage1.Controls.Add(this.txtNameSpace);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtName);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tabContentOne);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(971, 779);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = " 表转代码 ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkExport
            // 
            this.chkExport.AutoSize = true;
            this.chkExport.Checked = true;
            this.chkExport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExport.Location = new System.Drawing.Point(814, 257);
            this.chkExport.Name = "chkExport";
            this.chkExport.Size = new System.Drawing.Size(48, 16);
            this.chkExport.TabIndex = 12;
            this.chkExport.Text = "导出";
            this.chkExport.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(651, 257);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "批量操作列：";
            // 
            // txtBatEdit
            // 
            this.txtBatEdit.Location = new System.Drawing.Point(651, 282);
            this.txtBatEdit.Name = "txtBatEdit";
            this.txtBatEdit.Size = new System.Drawing.Size(242, 21);
            this.txtBatEdit.TabIndex = 10;
            this.txtBatEdit.Text = "TestPwd,TestMemory";
            // 
            // chkBatDel
            // 
            this.chkBatDel.AutoSize = true;
            this.chkBatDel.Location = new System.Drawing.Point(814, 222);
            this.chkBatDel.Name = "chkBatDel";
            this.chkBatDel.Size = new System.Drawing.Size(72, 16);
            this.chkBatDel.TabIndex = 9;
            this.chkBatDel.Text = "批量删除";
            this.chkBatDel.UseVisualStyleBackColor = true;
            // 
            // chkBatEdit
            // 
            this.chkBatEdit.AutoSize = true;
            this.chkBatEdit.Checked = true;
            this.chkBatEdit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBatEdit.Location = new System.Drawing.Point(735, 256);
            this.chkBatEdit.Name = "chkBatEdit";
            this.chkBatEdit.Size = new System.Drawing.Size(72, 16);
            this.chkBatEdit.TabIndex = 9;
            this.chkBatEdit.Text = "批量编辑";
            this.chkBatEdit.UseVisualStyleBackColor = true;
            // 
            // chkEdit
            // 
            this.chkEdit.AutoSize = true;
            this.chkEdit.Checked = true;
            this.chkEdit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEdit.Location = new System.Drawing.Point(759, 223);
            this.chkEdit.Name = "chkEdit";
            this.chkEdit.Size = new System.Drawing.Size(48, 16);
            this.chkEdit.TabIndex = 8;
            this.chkEdit.Text = "编辑";
            this.chkEdit.UseVisualStyleBackColor = true;
            // 
            // chkDel
            // 
            this.chkDel.AutoSize = true;
            this.chkDel.Checked = true;
            this.chkDel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDel.Location = new System.Drawing.Point(705, 223);
            this.chkDel.Name = "chkDel";
            this.chkDel.Size = new System.Drawing.Size(48, 16);
            this.chkDel.TabIndex = 8;
            this.chkDel.Text = "删除";
            this.chkDel.UseVisualStyleBackColor = true;
            // 
            // chkAdd
            // 
            this.chkAdd.AutoSize = true;
            this.chkAdd.Checked = true;
            this.chkAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdd.Location = new System.Drawing.Point(651, 223);
            this.chkAdd.Name = "chkAdd";
            this.chkAdd.Size = new System.Drawing.Size(48, 16);
            this.chkAdd.TabIndex = 8;
            this.chkAdd.Text = "新增";
            this.chkAdd.UseVisualStyleBackColor = true;
            // 
            // rbtnMySQL
            // 
            this.rbtnMySQL.AutoSize = true;
            this.rbtnMySQL.Location = new System.Drawing.Point(202, 4);
            this.rbtnMySQL.Name = "rbtnMySQL";
            this.rbtnMySQL.Size = new System.Drawing.Size(53, 16);
            this.rbtnMySQL.TabIndex = 2;
            this.rbtnMySQL.Text = "MySQL";
            this.rbtnMySQL.UseVisualStyleBackColor = true;
            // 
            // rbtnMSSQL
            // 
            this.rbtnMSSQL.AutoSize = true;
            this.rbtnMSSQL.Checked = true;
            this.rbtnMSSQL.Location = new System.Drawing.Point(133, 4);
            this.rbtnMSSQL.Name = "rbtnMSSQL";
            this.rbtnMSSQL.Size = new System.Drawing.Size(53, 16);
            this.rbtnMSSQL.TabIndex = 1;
            this.rbtnMSSQL.TabStop = true;
            this.rbtnMSSQL.Text = "MSSQL";
            this.rbtnMSSQL.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(82, 7);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(29, 12);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "示例";
            // 
            // btnExportCode
            // 
            this.btnExportCode.Location = new System.Drawing.Point(861, 309);
            this.btnExportCode.Name = "btnExportCode";
            this.btnExportCode.Size = new System.Drawing.Size(93, 23);
            this.btnExportCode.TabIndex = 7;
            this.btnExportCode.Text = "导出代码到Zip";
            this.btnExportCode.UseVisualStyleBackColor = true;
            this.btnExportCode.Click += new System.EventHandler(this.btnExportCode_Click);
            // 
            // btnCreateBootStrap
            // 
            this.btnCreateBootStrap.Location = new System.Drawing.Point(741, 309);
            this.btnCreateBootStrap.Name = "btnCreateBootStrap";
            this.btnCreateBootStrap.Size = new System.Drawing.Size(111, 23);
            this.btnCreateBootStrap.TabIndex = 7;
            this.btnCreateBootStrap.Text = "生成BootStrap";
            this.btnCreateBootStrap.UseVisualStyleBackColor = true;
            this.btnCreateBootStrap.Click += new System.EventHandler(this.btnCreateBootStrap_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(651, 309);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "生成代码";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // txtEditColumnName
            // 
            this.txtEditColumnName.Location = new System.Drawing.Point(84, 311);
            this.txtEditColumnName.Name = "txtEditColumnName";
            this.txtEditColumnName.Size = new System.Drawing.Size(548, 21);
            this.txtEditColumnName.TabIndex = 6;
            this.txtEditColumnName.Text = "TestName,TestPwd";
            // 
            // txtAddColumnName
            // 
            this.txtAddColumnName.Location = new System.Drawing.Point(84, 255);
            this.txtAddColumnName.Name = "txtAddColumnName";
            this.txtAddColumnName.Size = new System.Drawing.Size(548, 21);
            this.txtAddColumnName.TabIndex = 6;
            this.txtAddColumnName.Text = "TestName,TestPwd,TestMemory";
            // 
            // txtSearchColumnName
            // 
            this.txtSearchColumnName.Location = new System.Drawing.Point(84, 282);
            this.txtSearchColumnName.Name = "txtSearchColumnName";
            this.txtSearchColumnName.Size = new System.Drawing.Size(548, 21);
            this.txtSearchColumnName.TabIndex = 6;
            this.txtSearchColumnName.Text = "TestName";
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(403, 220);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(229, 21);
            this.txtDBName.TabIndex = 5;
            this.txtDBName.Text = "TestDB";
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(84, 220);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(229, 21);
            this.txtNameSpace.TabIndex = 4;
            this.txtNameSpace.Text = "Test";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 258);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "添加列：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 314);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "编辑列：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 285);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "查询列：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(331, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "数据库名：";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.Location = new System.Drawing.Point(13, 23);
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtName.Size = new System.Drawing.Size(949, 191);
            this.txtName.TabIndex = 3;
            this.txtName.Text = resources.GetString("txtName.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "命名空间：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "创表语句：";
            // 
            // tabContentOne
            // 
            this.tabContentOne.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabContentOne.Controls.Add(this.tabPage3);
            this.tabContentOne.Controls.Add(this.tabPage6);
            this.tabContentOne.Controls.Add(this.tabPage7);
            this.tabContentOne.Controls.Add(this.tabPage4);
            this.tabContentOne.Controls.Add(this.tabPage8);
            this.tabContentOne.Controls.Add(this.tabPage14);
            this.tabContentOne.Controls.Add(this.tabPage2);
            this.tabContentOne.Location = new System.Drawing.Point(4, 338);
            this.tabContentOne.Name = "tabContentOne";
            this.tabContentOne.SelectedIndex = 0;
            this.tabContentOne.Size = new System.Drawing.Size(960, 435);
            this.tabContentOne.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtClassCode);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(952, 409);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Model代码";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtClassCode
            // 
            this.txtClassCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClassCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtClassCode.Location = new System.Drawing.Point(7, 7);
            this.txtClassCode.Multiline = true;
            this.txtClassCode.Name = "txtClassCode";
            this.txtClassCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtClassCode.Size = new System.Drawing.Size(939, 396);
            this.txtClassCode.TabIndex = 0;
            this.txtClassCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.txtAspxCode);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(952, 409);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "ASPX层";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // txtAspxCode
            // 
            this.txtAspxCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAspxCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAspxCode.Location = new System.Drawing.Point(6, 6);
            this.txtAspxCode.Multiline = true;
            this.txtAspxCode.Name = "txtAspxCode";
            this.txtAspxCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAspxCode.Size = new System.Drawing.Size(939, 400);
            this.txtAspxCode.TabIndex = 2;
            this.txtAspxCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.txtAspxCsCode);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(952, 409);
            this.tabPage7.TabIndex = 4;
            this.tabPage7.Text = "ASPX.CS层";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // txtAspxCsCode
            // 
            this.txtAspxCsCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAspxCsCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAspxCsCode.Location = new System.Drawing.Point(6, 6);
            this.txtAspxCsCode.Multiline = true;
            this.txtAspxCsCode.Name = "txtAspxCsCode";
            this.txtAspxCsCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAspxCsCode.Size = new System.Drawing.Size(939, 400);
            this.txtAspxCsCode.TabIndex = 2;
            this.txtAspxCsCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtDALCode);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(952, 409);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "DAL代码";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtDALCode
            // 
            this.txtDALCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDALCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDALCode.Location = new System.Drawing.Point(6, 6);
            this.txtDALCode.Multiline = true;
            this.txtDALCode.Name = "txtDALCode";
            this.txtDALCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDALCode.Size = new System.Drawing.Size(939, 397);
            this.txtDALCode.TabIndex = 1;
            this.txtDALCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.txtFactoryCode);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(952, 409);
            this.tabPage8.TabIndex = 5;
            this.tabPage8.Text = "数据库工厂层";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // txtFactoryCode
            // 
            this.txtFactoryCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFactoryCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFactoryCode.Location = new System.Drawing.Point(6, 6);
            this.txtFactoryCode.Multiline = true;
            this.txtFactoryCode.Name = "txtFactoryCode";
            this.txtFactoryCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFactoryCode.Size = new System.Drawing.Size(939, 400);
            this.txtFactoryCode.TabIndex = 2;
            this.txtFactoryCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // tabPage14
            // 
            this.tabPage14.Controls.Add(this.txtSqlHelper);
            this.tabPage14.Location = new System.Drawing.Point(4, 22);
            this.tabPage14.Name = "tabPage14";
            this.tabPage14.Size = new System.Drawing.Size(952, 409);
            this.tabPage14.TabIndex = 10;
            this.tabPage14.Text = "辅助类：Helper";
            this.tabPage14.UseVisualStyleBackColor = true;
            // 
            // txtSqlHelper
            // 
            this.txtSqlHelper.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlHelper.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSqlHelper.Location = new System.Drawing.Point(6, 4);
            this.txtSqlHelper.Multiline = true;
            this.txtSqlHelper.Name = "txtSqlHelper";
            this.txtSqlHelper.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSqlHelper.Size = new System.Drawing.Size(939, 400);
            this.txtSqlHelper.TabIndex = 4;
            this.txtSqlHelper.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(952, 409);
            this.tabPage2.TabIndex = 11;
            this.tabPage2.Text = "EasyUI相关";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // EasyUIAutoFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 838);
            this.Controls.Add(this.tabHead);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EasyUIAutoFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动生成EasyUI站点";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabHead.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabContentOne.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.tabPage14.ResumeLayout(false);
            this.tabPage14.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 类型TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemNormal;
        private System.Windows.Forms.ToolStripMenuItem menuItemEasyUI;
        private System.Windows.Forms.TabControl tabHead;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBatEdit;
        private System.Windows.Forms.CheckBox chkBatDel;
        private System.Windows.Forms.CheckBox chkBatEdit;
        private System.Windows.Forms.CheckBox chkDel;
        private System.Windows.Forms.RadioButton rbtnMySQL;
        private System.Windows.Forms.RadioButton rbtnMSSQL;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button btnExportCode;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.TextBox txtEditColumnName;
        private System.Windows.Forms.TextBox txtAddColumnName;
        private System.Windows.Forms.TextBox txtSearchColumnName;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabContentOne;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtClassCode;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox txtDALCode;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TextBox txtAspxCode;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TextBox txtAspxCsCode;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TextBox txtFactoryCode;
        private System.Windows.Forms.TabPage tabPage14;
        private System.Windows.Forms.TextBox txtSqlHelper;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkEdit;
        private System.Windows.Forms.CheckBox chkAdd;
        private System.Windows.Forms.CheckBox chkExport;
        private System.Windows.Forms.Button btnCreateBootStrap;
    }
}