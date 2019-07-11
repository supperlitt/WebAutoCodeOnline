//***************************************************************************************
// Autor:Tecky
// Date:2008-06-03
// Desc:xxtea 算法的C#加密实现
// e-mail:likui318@163.com
//
//
//***************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class XXTEA_CSDN
    {
        public static string Encrypt(string source, string key)
        {
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            //UTF8==>BASE64==>XXTEA==>BASE64
            byte[] bytData = encoder.GetBytes(base64Encode(source));
            byte[] bytKey = encoder.GetBytes(key);
            if (bytData.Length == 0)
            {
                return "";
            }

            return System.Convert.ToBase64String(ToByteArray(Encrypt(ToUInt32Array(bytData, true), ToUInt32Array(bytKey, false)), false));
        }

        public static string Decrypt(string source, string key, bool isbase64 = false)
        {
            if (source.Length == 0)
            {
                return "";
            }
            // reverse
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            byte[] bytData = System.Convert.FromBase64String(source);
            byte[] bytKey = encoder.GetBytes(key);

            if (isbase64)
            {
                return base64Decode(encoder.GetString(ToByteArray(Decrypt(ToUInt32Array(bytData, false), ToUInt32Array(bytKey, false)), true)));
            }
            else
            {
                return base64Decode(encoder.GetString(ToByteArray(Decrypt(ToUInt32Array(bytData, false), ToUInt32Array(bytKey, false)), true)));
            }
        }

        private static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum = 0, e;
            Int32 p, q = 6 + 52 / (n + 1);
            while (q-- > 0)
            {
                sum = unchecked(sum + delta);
                e = sum >> 2 & 3;
                for (p = 0; p < n; p++)
                {
                    y = v[p + 1];
                    z = unchecked(v[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                y = v[0];
                z = unchecked(v[n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
            }
            return v;
        }

        private static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum, e;
            Int32 p, q = 6 + 52 / (n + 1);
            sum = unchecked((UInt32)(q * delta));
            while (sum != 0)
            {
                e = sum >> 2 & 3;
                for (p = n; p > 0; p--)
                {
                    z = v[p - 1];
                    y = unchecked(v[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                z = v[n];
                y = unchecked(v[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                sum = unchecked(sum - delta);
            }
            return v;
        }

        private static UInt32[] ToUInt32Array(Byte[] Data, Boolean IncludeLength)
        {
            Int32 n = (((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1));
            UInt32[] Result;
            if (IncludeLength)
            {
                Result = new UInt32[n + 1];
                Result[n] = (UInt32)Data.Length;
            }
            else
            {
                Result = new UInt32[n];
            }
            n = Data.Length;
            for (Int32 i = 0; i < n; i++)
            {
                Result[i >> 2] |= (UInt32)Data[i] << ((i & 3) << 3);
            }
            return Result;
        }

        private static Byte[] ToByteArray(UInt32[] Data, Boolean IncludeLength)
        {
            Int32 n;
            if (IncludeLength)
            {
                n = (Int32)Data[Data.Length - 1];
            }
            else
            {
                n = Data.Length << 2;
            }
            Byte[] Result = new Byte[n];
            for (Int32 i = 0; i < n; i++)
            {
                Result[i] = (Byte)(Data[i >> 2] >> ((i & 3) << 3));
            }
            return Result;
        }

        private static string base64Decode(string data)
        {
            try
            {
                byte[] todecode_byte = Convert.FromBase64String(data);
                return Encoding.UTF8.GetString(todecode_byte);
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        private static string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }
    }
}
