using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAutoCodeOnline
{
    /// <summary>
    /// Location 的摘要说明
    /// </summary>
    public class Location : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            try
            {
                string type = context.Request.QueryString["type"];
                string data = context.Request.Form["data"];
                data = HelpEncrypt.Decode(data, "supperlitt");
                string result = string.Empty;
                switch (type)
                {
                    case "upload":
                        string[] dataArray = data.Split(new char[] { '#' }, StringSplitOptions.None);
                        string phone = dataArray[0];
                        string pwd = dataArray[1];
                        string lat = dataArray[2];
                        string lng = dataArray[3];
                        string imei = dataArray[4];
                        string content = dataArray[5];

                        result = Tool.UpdateLocation(phone, pwd, lat, lng, imei, content);
                        result = HelpEncrypt.Encode(result, "supperlitt");

                        break;
                    case "download":
                        string[] dArray = data.Split(new char[] { '#' }, StringSplitOptions.None);
                        string phoneNum = dArray[0];
                        string phonePwd = dArray[1];
                        result = Tool.QueryLocation(phoneNum, phonePwd);
                        if (!string.IsNullOrEmpty(result))
                        {
                            result = HelpEncrypt.Encode(result, "supperlitt");
                        }

                        break;
                }

                // 返回空，失败，1-返回1，已经存在，返回其他
                context.Response.Write(result);
            }
            catch (Exception ex)
            {
                // context.Response.Write(ex.Message);
                // 吃掉异常了
            }

            context.Response.Flush();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}