namespace WinGenerateCodeDB.Child
{
    partial class VMSettingFrm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDeleteColValue = new System.Windows.Forms.TextBox();
            this.txtDeleteColName = new System.Windows.Forms.TextBox();
            this.rbtnDelete1 = new System.Windows.Forms.RadioButton();
            this.rbtnDelete2 = new System.Windows.Forms.RadioButton();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtQueryExcept = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtEditExcept = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtAddExcept = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtAll = new System.Windows.Forms.TextBox();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEdit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAddCheck = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAdd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAddCheck = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(662, 405);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.btnSaveAll);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(654, 379);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = " 通用设置 ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtDeleteColValue);
            this.groupBox4.Controls.Add(this.txtDeleteColName);
            this.groupBox4.Controls.Add(this.rbtnDelete1);
            this.groupBox4.Controls.Add(this.rbtnDelete2);
            this.groupBox4.Location = new System.Drawing.Point(339, 202);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(300, 110);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "删除选项(找不到列明，执行物理生成)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(245, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "值";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(142, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "列明";
            // 
            // txtDeleteColValue
            // 
            this.txtDeleteColValue.Location = new System.Drawing.Point(231, 70);
            this.txtDeleteColValue.Name = "txtDeleteColValue";
            this.txtDeleteColValue.Size = new System.Drawing.Size(48, 21);
            this.txtDeleteColValue.TabIndex = 5;
            // 
            // txtDeleteColName
            // 
            this.txtDeleteColName.Location = new System.Drawing.Point(102, 70);
            this.txtDeleteColName.Name = "txtDeleteColName";
            this.txtDeleteColName.Size = new System.Drawing.Size(113, 21);
            this.txtDeleteColName.TabIndex = 4;
            // 
            // rbtnDelete1
            // 
            this.rbtnDelete1.AutoSize = true;
            this.rbtnDelete1.Checked = true;
            this.rbtnDelete1.Location = new System.Drawing.Point(15, 33);
            this.rbtnDelete1.Name = "rbtnDelete1";
            this.rbtnDelete1.Size = new System.Drawing.Size(71, 16);
            this.rbtnDelete1.TabIndex = 3;
            this.rbtnDelete1.TabStop = true;
            this.rbtnDelete1.Text = "物理删除";
            this.rbtnDelete1.UseVisualStyleBackColor = true;
            // 
            // rbtnDelete2
            // 
            this.rbtnDelete2.AutoSize = true;
            this.rbtnDelete2.Location = new System.Drawing.Point(15, 71);
            this.rbtnDelete2.Name = "rbtnDelete2";
            this.rbtnDelete2.Size = new System.Drawing.Size(71, 16);
            this.rbtnDelete2.TabIndex = 3;
            this.rbtnDelete2.Text = "逻辑删除";
            this.rbtnDelete2.UseVisualStyleBackColor = true;
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveAll.Location = new System.Drawing.Point(547, 330);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(92, 38);
            this.btnSaveAll.TabIndex = 2;
            this.btnSaveAll.Text = "保存设置";
            this.btnSaveAll.UseVisualStyleBackColor = true;
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtQueryExcept);
            this.groupBox3.Location = new System.Drawing.Point(7, 202);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(304, 166);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "查询排除列（一行一个）";
            // 
            // txtQueryExcept
            // 
            this.txtQueryExcept.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQueryExcept.Location = new System.Drawing.Point(19, 20);
            this.txtQueryExcept.Multiline = true;
            this.txtQueryExcept.Name = "txtQueryExcept";
            this.txtQueryExcept.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQueryExcept.Size = new System.Drawing.Size(264, 131);
            this.txtQueryExcept.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtEditExcept);
            this.groupBox2.Location = new System.Drawing.Point(335, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 166);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "编辑排除列（一行一个）";
            // 
            // txtEditExcept
            // 
            this.txtEditExcept.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEditExcept.Location = new System.Drawing.Point(19, 20);
            this.txtEditExcept.Multiline = true;
            this.txtEditExcept.Name = "txtEditExcept";
            this.txtEditExcept.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEditExcept.Size = new System.Drawing.Size(264, 131);
            this.txtEditExcept.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAddExcept);
            this.groupBox1.Location = new System.Drawing.Point(7, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 166);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "添加排除列（一行一个）";
            // 
            // txtAddExcept
            // 
            this.txtAddExcept.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddExcept.Location = new System.Drawing.Point(19, 20);
            this.txtAddExcept.Multiline = true;
            this.txtAddExcept.Name = "txtAddExcept";
            this.txtAddExcept.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAddExcept.Size = new System.Drawing.Size(264, 131);
            this.txtAddExcept.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkAddCheck);
            this.tabPage2.Controls.Add(this.txtAll);
            this.tabPage2.Controls.Add(this.txtQuery);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.txtEdit);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtAdd);
            this.tabPage2.Controls.Add(this.txtAddCheck);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(654, 379);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "参考模板（仅开始使用，启动时已隐藏）";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtAll
            // 
            this.txtAll.Location = new System.Drawing.Point(7, 10);
            this.txtAll.Multiline = true;
            this.txtAll.Name = "txtAll";
            this.txtAll.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAll.Size = new System.Drawing.Size(277, 352);
            this.txtAll.TabIndex = 3;
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(347, 281);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQuery.Size = new System.Drawing.Size(292, 83);
            this.txtQuery.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(301, 282);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "查询";
            // 
            // txtEdit
            // 
            this.txtEdit.Location = new System.Drawing.Point(347, 189);
            this.txtEdit.Multiline = true;
            this.txtEdit.Name = "txtEdit";
            this.txtEdit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEdit.Size = new System.Drawing.Size(292, 88);
            this.txtEdit.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(301, 188);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "编辑";
            // 
            // txtAddCheck
            // 
            this.txtAddCheck.Location = new System.Drawing.Point(347, 33);
            this.txtAddCheck.Multiline = true;
            this.txtAddCheck.Name = "txtAddCheck";
            this.txtAddCheck.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAddCheck.Size = new System.Drawing.Size(292, 64);
            this.txtAddCheck.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(301, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "添加";
            // 
            // txtAdd
            // 
            this.txtAdd.Location = new System.Drawing.Point(347, 101);
            this.txtAdd.Multiline = true;
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAdd.Size = new System.Drawing.Size(292, 84);
            this.txtAdd.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(301, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 36);
            this.label6.TabIndex = 1;
            this.label6.Text = "添加\r\n唯一\r\n判断";
            // 
            // chkAddCheck
            // 
            this.chkAddCheck.AutoSize = true;
            this.chkAddCheck.Location = new System.Drawing.Point(347, 12);
            this.chkAddCheck.Name = "chkAddCheck";
            this.chkAddCheck.Size = new System.Drawing.Size(96, 16);
            this.chkAddCheck.TabIndex = 4;
            this.chkAddCheck.Text = "添加重复校验";
            this.chkAddCheck.UseVisualStyleBackColor = true;
            // 
            // VMSettingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 408);
            this.Controls.Add(this.tabControl1);
            this.Name = "VMSettingFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View Model 设置";
            this.Load += new System.EventHandler(this.VMSettingFrm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtAddExcept;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtEditExcept;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtQueryExcept;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddCheck;
        private System.Windows.Forms.TextBox txtEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAll;
        private System.Windows.Forms.RadioButton rbtnDelete1;
        private System.Windows.Forms.RadioButton rbtnDelete2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtDeleteColName;
        private System.Windows.Forms.TextBox txtDeleteColValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkAddCheck;
    }
}