namespace WinTestProject
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnZipTest = new System.Windows.Forms.Button();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnZipTest
            // 
            this.btnZipTest.Location = new System.Drawing.Point(288, 12);
            this.btnZipTest.Name = "btnZipTest";
            this.btnZipTest.Size = new System.Drawing.Size(75, 23);
            this.btnZipTest.TabIndex = 0;
            this.btnZipTest.Text = "ZipTest";
            this.btnZipTest.UseVisualStyleBackColor = true;
            this.btnZipTest.Click += new System.EventHandler(this.btnZipTest_Click);
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(13, 13);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(254, 21);
            this.txtDir.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 262);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.btnZipTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnZipTest;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.Button button1;
    }
}

