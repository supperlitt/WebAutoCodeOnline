using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAutoEasyUI
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 当前连接字符串
        /// </summary>
        private string currentConnectionStr = string.Empty;

        private string currentTableName = string.Empty;

        private string currentDbName = string.Empty;

        private List<TableInfo> currentTableList = new List<TableInfo>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnTestConnect_Click(object sender, EventArgs e)
        {
            this.currentConnectionStr = string.Format("server={0};database=master;uid={1};pwd={2};", this.txtServer.Text, this.txtUid.Text, this.txtPwd.Text);

            var list = DBAccess.GetALLDB(currentConnectionStr);

            this.cmbBox.DataSource = list;
        }

        private void cmbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbBox.SelectedIndex > 0)
            {
                this.currentDbName = this.cmbBox.SelectedItem.ToString();
                this.currentConnectionStr = string.Format("server={0};database={3};uid={1};pwd={2};", this.txtServer.Text, this.txtUid.Text, this.txtPwd.Text, this.currentDbName);

                var list = DBAccess.GetAllTables(currentConnectionStr);

                this.lstBox.DataSource = list;
            }
        }

        private void lstBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstBox.SelectedItem != null)
            {
                this.currentTableName = this.lstBox.SelectedItem.ToString();
                this.currentTableList = DBAccess.GetAllTableInfo(this.currentTableName, this.currentConnectionStr);

                this.dgView.DataSource = this.currentTableList;
                this.chkListDataGrid.Items.Clear();
                this.chkListDataGrid.Items.AddRange((from f in this.currentTableList select f.ColumnName).ToArray());
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.dgView.AutoGenerateColumns = false;
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtDir.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string searchColumn = this.txtSearchColumns.Text.Trim();
            string title = this.txtTitle.Text.Trim();
            string nameSpace = this.txtNameSpace.Text.Trim();
            string dir = this.txtDir.Text.Trim();
            string temppath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template");
            bool isindenpend = this.chkIndependDLL.Checked;

            var tempInfo = new TempModel();
            tempInfo.SearchColumns = searchColumn;
            tempInfo.Title = title;
            tempInfo.NameSpace = nameSpace;
            tempInfo.SourceDir = temppath;
            tempInfo.TargetDir = dir;
            tempInfo.TableName = currentTableName;
            tempInfo.DbName = currentDbName;
            tempInfo.ConnectionStr = currentConnectionStr;

            var list = this.dgView.DataSource as List<TableInfo>;
            // 更新表格的内容
            foreach (var item in currentTableList)
            {
                var model = list.Find(p => p.ColumnName == item.ColumnName);
                if (model != null)
                {
                    item.Comment = model.Comment;
                }
            }

            tempInfo.TableList = currentTableList;
            tempInfo.IsDenpendDLL = isindenpend;

            IHelper helper = new EasyUIHelper();
            helper.ReleaseFile(tempInfo);

            MessageBox.Show("生成成功！");
        }

        private void chkDataGridAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkListDataGrid.Items.Count; i++)
            {
                this.chkListDataGrid.SetItemChecked(i, this.chkDataGridAll.Checked);
            }
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            // 检测目录，创建csproject文件。
            string dir = this.txtDir.Text;
            bool isindenpend = this.chkIndependDLL.Checked;
            string modelDir = Path.Combine(dir, "Model");
            string dalDir = Path.Combine(dir, "DAL");
            string uiDir = Path.Combine(dir, "Web" + currentDbName);

            string webDLLName = txtNameSpace.Text;
            ProjectHelper helper = new ProjectHelper();
            if (isindenpend)
            {
                string modelGuid = Guid.NewGuid().ToString();
                string dalGuid = Guid.NewGuid().ToString();
                string webGuid = Guid.NewGuid().ToString();

                List<ReferInfo> referList = new List<ReferInfo>();
                List<string> modellist = Directory.GetFiles(modelDir, "*.cs").ToList();
                List<string> dallist = Directory.GetFiles(dalDir, "*.cs").ToList();
                List<string> uilist = Directory.GetFiles(uiDir, "*.aspx", SearchOption.AllDirectories).ToList();
                List<string> cslist = (from f in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories).ToList()
                                       where !f.EndsWith(".aspx.cs") && !f.EndsWith(".aspx.designer.cs")
                                       select f).ToList();

                uilist = (from f in uilist
                          select f.Substring(uiDir.Length + 1)).ToList();

                cslist = (from f in cslist
                          select f.Substring(uiDir.Length + 1)).ToList();

                modellist = (from f in modellist
                             select f.Substring(modelDir.Length + 1)).ToList();

                dallist = (from f in dallist
                           select f.Substring(dir.Length + "Model".Length + 1 + 1, f.Length - dir.Length - 1)).ToList();

                // 独立dll. DAL,Model,Web+DbName
                helper.CreateDLLProject(modelDir, "Model", "Model", modelGuid, modellist, referList);
                BuildProject(Path.Combine(modelDir, "Model.csproj"));
                referList.Add(new ReferInfo()
                {
                    Name = "Model",
                    Path = @"..\Model\Model.csproj",
                    Guid = modelGuid
                });

                helper.CreateDLLProject(dalDir, "DAL", "DAL", dalGuid, dallist, referList);
                BuildProject(Path.Combine(dalDir, "DAL.csproj"));
                referList.Add(new ReferInfo()
                {
                    Name = "DAL",
                    Path = @"..\DAL\DAL.csproj",
                    Guid = dalGuid
                });

                helper.CreateWebSiteProject(uiDir, webDLLName, webDLLName, webGuid, uilist, cslist, uilist, referList);
                BuildProject(Path.Combine(uiDir, webDLLName + ".csproj"));
            }
            else
            {
                string webGuid = Guid.NewGuid().ToString();

                List<string> uilist = Directory.GetFiles(uiDir, "*.aspx", SearchOption.AllDirectories).ToList();
                List<string> cslist = (from f in Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories).ToList()
                                       where !f.EndsWith(".aspx.cs") && !f.EndsWith(".aspx.designer.cs")
                                       select f).ToList();

                // 按照非独立dll处理
                uilist = (from f in uilist
                          select "Web" + currentDbName + "\\" + f.Substring(uiDir.Length + 1, f.Length - uiDir.Length - 1)).ToList();

                cslist = (from f in cslist
                          select f.Substring(dir.Length + 1, f.Length - dir.Length - 1)).ToList();

                // 独立dll. DAL,Model,Web+DbName
                helper.CreateWebSiteProject(dir, webDLLName, webDLLName, webGuid, uilist, cslist, uilist, null);
                BuildProject(Path.Combine(dir, webDLLName + ".csproj"));
            }

            // 完成编译了。。
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            // 拷贝文件，部署网站
            string webSiteDesc = this.txtWebSiteName.Text.Trim();
            string webSiteIP = this.txtWebSiteIP.Text.Trim();
            string webSitePort = this.txtWebSitePort.Text.Trim();
            string webSitePath = this.txtWebSitePath.Text.Trim();

            // 是否允许覆盖
            bool allowAdd = this.chkAdd.Checked;

            if (new List<string>() { webSiteDesc, webSiteIP, webSitePort, webSitePath }.TrueForAll(p => !string.IsNullOrEmpty(p)))
            {
                IIS7 iis = new IIS7();
                if (!iis.IsContainsSite(webSiteDesc))
                {
                    iis.AddSite(webSiteIP, webSitePort, webSiteDesc, webSitePath);

                    // 拷贝dll和文件和easyui文件到目录下
                }
                else
                {
                    // 直接拷贝文件过去
                    MessageBox.Show("站点已经存在，需要手动删除后方可进行操作");
                }
            }
        }

        private void BuildProject(string parameters)
        {
            Process p = new Process();
            p.StartInfo.FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe";
            p.StartInfo.Arguments = "\"" + parameters + "\"";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;//重定向
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;//无窗口
            p.Start();

            // 设置进程使用的CPU
            p.BeginErrorReadLine();//开始输出流的读取
            p.WaitForExit();//等待，直到处理完毕
            string content = p.StandardOutput.ReadToEnd();
            p.Close();
            p.Dispose();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtWebSitePath.Text = dialog.SelectedPath;
            }
        }

        private void btnJustCreate_Click(object sender, EventArgs e)
        {

        }
    }
}