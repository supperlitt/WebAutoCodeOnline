using System;
using System.Collections.Generic;
using System.Text;
namespace WebAutoCodeOnline
{
    public class Article
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
        /// Title
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Content
        /// </summary>
        private string content = string.Empty;

        /// <summary>
        /// Content
        /// </summary>
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Tags
        /// </summary>
        private string tags = string.Empty;

        /// <summary>
        /// Tags
        /// </summary>
        public string Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        /// <summary>
        /// 分组，使用的二进制交集
        /// </summary>
        private int groupIds = 0;

        /// <summary>
        /// 分组，使用的二进制交集
        /// </summary>
        public int GroupIds
        {
            get { return this.groupIds; }
            set { this.groupIds = value; }
        }

        /// <summary>
        /// PublishTime
        /// </summary>
        private DateTime publishTime = DateTime.Parse("1970-1-1");

        /// <summary>
        /// PublishTime
        /// </summary>
        public DateTime PublishTime
        {
            get { return this.publishTime; }
            set { this.publishTime = value; }
        }

        /// <summary>
        /// PublishTime
        /// </summary>
        public string PublishTimeStr
        {
            get { return this.publishTime.ToString("yyyy-MM-dd HH:mm"); }
        }

        /// <summary>
        /// LastChangeTime
        /// </summary>
        private DateTime lastChangeTime = DateTime.Parse("1970-1-1");

        /// <summary>
        /// LastChangeTime
        /// </summary>
        public DateTime LastChangeTime
        {
            get { return this.lastChangeTime; }
            set { this.lastChangeTime = value; }
        }

        /// <summary>
        /// LastChangeTime
        /// </summary>
        public string LastChangeTimeStr
        {
            get { return this.lastChangeTime.ToString("yyyy-MM-dd HH:mm"); }
        }

        /// <summary>
        /// 是否显示，
        /// </summary>
        private int isShow = 0;

        /// <summary>
        /// 是否显示，
        /// </summary>
        public int IsShow
        {
            get { return this.isShow; }
            set { this.isShow = value; }
        }

        /// <summary>
        /// 是否置顶
        /// </summary>
        private int isTop = 0;

        /// <summary>
        /// 是否置顶
        /// </summary>
        public int IsTop
        {
            get { return this.isTop; }
            set { this.isTop = value; }
        }
    }
}
