using System;
using System.Collections.Generic;
using System.Text;
namespace Test
{
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

        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddDateStr
        {
            get { return this.addDate.ToString("yyyy-MM-dd HH:mm"); }
        }
    }
}
