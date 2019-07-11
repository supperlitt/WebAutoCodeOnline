using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class EncryptJs
    {
        /// <summary>
        /// 下列站点都是用此方式解密
        /// cmf.cmpay.com
        /// </summary>
        /// <returns></returns>
        private static string Get_XXTea_Js()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            System.IO.StreamReader txtStream = new System.IO.StreamReader(asm.GetManifestResourceStream("Common.EncryptJs.xxtea.js"));

            return txtStream.ReadToEnd();
        }

        /// <summary>
        /// 下列站点都是用此方式加密
        /// cmpay
        /// sina
        /// 
        /// </summary>
        /// <returns></returns>
        private static string Get_Cmpay_Rsa_Js()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            System.IO.StreamReader txtStream = new System.IO.StreamReader(asm.GetManifestResourceStream("Common.EncryptJs.cmpay_rsa.js"));

            return txtStream.ReadToEnd();
        }

        /// <summary>
        /// 下列站点都是用此方式加密
        /// qq
        /// </summary>
        /// <returns></returns>
        private static string Get_QQ_GetPwd_Js()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            System.IO.StreamReader txtStream = new System.IO.StreamReader(asm.GetManifestResourceStream("Common.EncryptJs.qq_getpwd.js"));

            return txtStream.ReadToEnd();
        }

        /// <summary>
        /// 下列站点都是用此方式加密
        /// qq
        /// </summary>
        /// <returns></returns>
        private static string Get_QQ_GetSalt_Js()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            System.IO.StreamReader txtStream = new System.IO.StreamReader(asm.GetManifestResourceStream("Common.EncryptJs.qq_getsalt.js"));

            return txtStream.ReadToEnd();
        }

        /// <summary>
        /// 下列站点都是用此方式加密
        /// qq
        /// </summary>
        /// <returns></returns>
        private static string Get_58_RSAUtils_Js()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            System.IO.StreamReader txtStream = new System.IO.StreamReader(asm.GetManifestResourceStream("Common.EncryptJs.58_RSAUtils.js"));

            return txtStream.ReadToEnd();
        }

        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="sExpression">参数体</param>
        /// <param name="sCode">JavaScript代码的字符串</param>
        /// <returns></returns>
        private static string ExecuteScript(string sExpression, string sCode)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JScript";
            scriptControl.AddCode(sCode);
            try
            {
                string str = scriptControl.Eval(sExpression).ToString();
                return str;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return null;
        }

        public static string XXTea_Encrypt(string text, string key)
        {
            string js = Get_XXTea_Js();

            string function = string.Format("encryptToBase64('{0}','{1}')", text, key);
            return ExecuteScript(function, js);
        }

        public static string XXTea_Dencrypt(string text, string key)
        {
            string js = Get_XXTea_Js();

            string function = string.Format("decryptFromBase64('{0}','{1}')", text, key);
            return ExecuteScript(function, js);
        }
    }
}
