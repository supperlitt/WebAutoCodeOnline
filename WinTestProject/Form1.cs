using Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinTestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //using (SqlConnection sqlcn = new SqlConnection())
            //{
            //    sqlcn.Open();
            //    SqlCommand sqlcm = new SqlCommand();
            //    sqlcm.Connection = sqlcn;
            //    sqlcm.CommandType = CommandType.Text;
            //    sqlcm.CommandText = "";
            //    sqlcm.ExecuteNonQuery();
            //}
        }

        private void btnZipTest_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(this.txtDir.Text);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var file in files)
            {
                string fileName = file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 1);
                dic.Add(fileName, File.ReadAllText(file, Encoding.UTF8));
            }

            byte[] result = ZipHelper.Zip(dic);

            File.WriteAllBytes(Path.Combine(this.txtDir.Text, "tmp.zip"), result);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var basekey = HexUtil.Read16Byte("40448af5c5c145eaa79056453d47e4b6");
            var key = HexUtil.Read16Byte("40448af5c5c145eaa79056453d47e4b6");
            var url = HexUtil.Read16Byte("3053020100044c304a0201000204b35c25a802030f4881020463bc8cb602045b01e7cd0425617570696d675f643735633966306264313632383937655f313532363835313533323835340204010800020201000400");
            AESHelper.SetKey(basekey);

            byte[] iv = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            string result = AESHelper.AESDecrypt(Convert.ToBase64String(url), Encoding.UTF8.GetString(iv));
        }

    }

    /// <summary>
    /// 标题
    /// </summary>
    public class TestInfo
    {
        /// <summary>
        /// 测试Id
        /// </summary>
        private int testId = 0;

        /// <summary>
        /// 测试Id
        /// </summary>
        public int TestId
        {
            get { return this.testId; }
            set { this.testId = value; }
        }

        /// <summary>
        /// 测试名称
        /// </summary>
        private string testName = string.Empty;

        /// <summary>
        /// 测试名称
        /// </summary>
        public string TestName
        {
            get { return this.testName; }
            set { this.testName = value; }
        }

        /// <summary>
        /// 测试密码
        /// </summary>
        private string testPwd = string.Empty;

        /// <summary>
        /// 测试密码
        /// </summary>
        public string TestPwd
        {
            get { return this.testPwd; }
            set { this.testPwd = value; }
        }

        /// <summary>
        /// 测试金额
        /// </summary>
        private decimal testMemory = 0m;

        /// <summary>
        /// 测试金额
        /// </summary>
        public decimal TestMemory
        {
            get { return this.testMemory; }
            set { this.testMemory = value; }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        private DateTime addDate = DateTime.Parse("1970-1-1");

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate
        {
            get { return this.addDate; }
            set { this.addDate = value; }
        }
    }
}
