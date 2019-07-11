using System;
using System.Collections.Generic;
using System.Text;
namespace WebAutoCodeOnline
{
    public class LeaveMsg
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
        /// 昵称
        /// </summary>
        private string nickName = string.Empty;

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName
        {
            get { return this.nickName; }
            set { this.nickName = value; }
        }

        /// <summary>
        /// Email
        /// </summary>
        private string email = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        /// <summary>
        /// Msg
        /// </summary>
        private string msg = string.Empty;

        /// <summary>
        /// Msg
        /// </summary>
        public string Msg
        {
            get { return this.msg; }
            set { this.msg = value; }
        }

        /// <summary>
        /// IP
        /// </summary>
        private string iP = string.Empty;

        /// <summary>
        /// IP
        /// </summary>
        public string IP
        {
            get { return this.iP; }
            set { this.iP = value; }
        }

        /// <summary>
        /// LeaveTime
        /// </summary>
        private DateTime leaveTime = DateTime.Parse("1970-1-1");

        /// <summary>
        /// LeaveTime
        /// </summary>
        public DateTime LeaveTime
        {
            get { return this.leaveTime; }
            set { this.leaveTime = value; }
        }

        /// <summary>
        /// LeaveTime
        /// </summary>
        public string LeaveTimeStr
        {
            get { return this.leaveTime.ToString("yyyy-MM-dd HH:mm"); }
        }
    }
}
