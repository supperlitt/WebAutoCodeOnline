using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Common
{
    public class RSAHelper
    {
        /// <summary> 
        /// 生成公钥,私钥对
        /// </summary> 
        public static string[] GenerateKeys()
        {
            string[] sKeys = new String[2];
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            sKeys[0] = rsa.ToXmlString(true);//私钥
            sKeys[1] = rsa.ToXmlString(false);//公钥
            return sKeys;
        }

        /// <summary> 
        /// RSA 加密
        /// </summary> 
        /// <param name="sSource" >明文</param> 
        /// <param name="sPublicKey" >公钥</param> 
        public static string EncryptString(string sSource, string sPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string plaintext = sSource;
            rsa.FromXmlString(sPublicKey);
            byte[] cipherbytes;
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), false);

            StringBuilder sbString = new StringBuilder();
            for (int i = 0; i < cipherbytes.Length; i++)
            {
                sbString.Append(cipherbytes[i] + ",");
            }

            return sbString.ToString();
        }

        /// <summary> 
        /// RSA 解密
        /// </summary> 
        /// <param name="sSource">密文</param> 
        /// <param name="sPrivateKey">私钥</param> 
        public static string DecryptString(String sSource, string sPrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(sPrivateKey);
            byte[] byteEn = rsa.Encrypt(Encoding.UTF8.GetBytes("a"), false);
            string[] sBytes = sSource.Split(',');

            for (int j = 0; j < sBytes.Length; j++)
            {
                if (sBytes[j] != "")
                {
                    byteEn[j] = Byte.Parse(sBytes[j]);
                }
            }

            byte[] plaintbytes = rsa.Decrypt(byteEn, false);

            return Encoding.UTF8.GetString(plaintbytes);
        }
    }
}
