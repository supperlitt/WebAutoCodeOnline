using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB.Code
{
    public class CommentTool
    {
        /// <summary>
        /// 创建一个注释，从当前位置开始，回车换行结束
        /// </summary>
        /// <param name="commentStr"></param>
        /// <param name="tabCount">tab的个数</param>
        /// <returns></returns>
        public static string CreateComment(string commentStr, int tabCount)
        {
            string tabStr = string.Empty;
            for (int i = 0; i < tabCount; i++)
            {
                tabStr += "\t";
            }

            StringBuilder content = new StringBuilder();
            content.AppendLine(tabStr + "/// <summary>");
            content.AppendLine(tabStr + "/// " + commentStr);
            content.AppendLine(tabStr + "/// </summary>");

            return content.ToString();
        }
    }
}
