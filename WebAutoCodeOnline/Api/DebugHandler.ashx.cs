using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAutoCodeOnline
{
    /// <summary>
    /// DebugHandler 的摘要说明
    /// </summary>
    public class DebugHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string path = context.Request.FilePath;
            Regex regex = new Regex(@"\/(?<key>[\s\S]+)\.debug");
            string key = regex.Match(path).Groups["key"].Value;
            if (!string.IsNullOrEmpty(key))
            {
                bool ismatched = false;
                string result = string.Empty;
                switch (key.ToLower())
                {
                    case "md5":
                        {
                            ismatched = true;
                            string input = context.Request["key"] ?? "";
                            result = EncryptHelper.GetMD5(input);
                        }
                        break;
                    case "sha1":
                        {
                            ismatched = true;
                            string input = context.Request["key"] ?? "";
                            result = EncryptHelper.GetSHA1(input);
                        }
                        break;
                }

                if (ismatched)
                {
                    string jsonp = context.Request["jsonp"];
                    if (jsonp == null)
                    {
                        context.Response.Write(result);
                    }
                    else if (jsonp == "?")
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.Write("jsonp" + JsTool.GetIntFromTime() + "('" + result + "')");
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.Write(jsonp + "('" + result + "')");
                    }
                }
            }

            context.Response.Flush();
            context.Response.End();
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