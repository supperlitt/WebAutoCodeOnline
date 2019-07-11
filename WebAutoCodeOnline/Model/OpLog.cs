using System;
using System.Collections.Generic;
using System.Text;
namespace WebAutoCodeOnline
{
    public class OpLog
    {
        /// <summary>
        /// Id
        /// </summary>
        private int id = 0;

        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// OpType
        /// </summary>
        private int opType = 0;

        /// <summary>
        /// OpType
        /// </summary>
        public int OpType
        {
            get { return this.opType; }
            set { this.opType = value; }
        }

        /// <summary>
        /// OpContent
        /// </summary>
        private string opContent = string.Empty;

        /// <summary>
        /// OpContent
        /// </summary>
        public string OpContent
        {
            get { return this.opContent; }
            set { this.opContent = value; }
        }

        /// <summary>
        /// OpIP
        /// </summary>
        private string opIP = string.Empty;

        /// <summary>
        /// OpIP
        /// </summary>
        public string OpIP
        {
            get { return this.opIP; }
            set { this.opIP = value; }
        }

        /// <summary>
        /// OpTime
        /// </summary>
        private DateTime opTime = DateTime.Parse("1970-1-1");

        /// <summary>
        /// OpTime
        /// </summary>
        public DateTime OpTime
        {
            get { return this.opTime; }
            set { this.opTime = value; }
        }

        /// <summary>
        /// OpTime
        /// </summary>
        public string OpTimeStr
        {
            get { return this.opTime.ToString("yyyy-MM-dd HH:mm"); }
        }
    }
}
