using System;
using System.Collections.Generic;
using System.Text;
namespace WebAutoCodeOnline
{
    public class UserInfo
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
        /// UserName
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        /// <summary>
        /// UserPwd
        /// </summary>
        private string userPwd = string.Empty;

        /// <summary>
        /// UserPwd
        /// </summary>
        public string UserPwd
        {
            get { return this.userPwd; }
            set { this.userPwd = value; }
        }

        /// <summary>
        /// LastLoginTime
        /// </summary>
        private DateTime lastLoginTime = DateTime.Parse("1970-1-1");

        /// <summary>
        /// LastLoginTime
        /// </summary>
        public DateTime LastLoginTime
        {
            get { return this.lastLoginTime; }
            set { this.lastLoginTime = value; }
        }

        /// <summary>
        /// LastLoginTime
        /// </summary>
        public string LastLoginTimeStr
        {
            get { return this.lastLoginTime.ToString("yyyy-MM-dd HH:mm"); }
        }
    }
}
