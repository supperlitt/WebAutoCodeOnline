using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordSeekTool
{
    /// <summary>
    /// DES加密/解密类。
    /// Copyright (C) weikebuluo
    /// </summary>
    public class EncryptHelper
    {
        public static string GetMD5(string md5, Encoding encode)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] value, hash;
            value = encode.GetBytes(md5);
            hash = md.ComputeHash(value);
            md.Clear();
            string temp = "";
            for (int i = 0, len = hash.Length; i < len; i++)
            {
                temp += hash[i].ToString("X").PadLeft(2, '0');
            }

            return temp;
        }

        public static string GetSHA1(string strSource, Encoding encode)
        {
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bytResult = sha.ComputeHash(encode.GetBytes(strSource));
            //转换成字符串，32位  
            string strResult = BitConverter.ToString(bytResult);
            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉  
            strResult = strResult.Replace("-", "");

            return strResult;
        }
    }
}
