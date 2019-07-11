using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Process
{
    public class AESProcess : IProcess
    {
        public byte[] Run(Encoding encode, params string[] args)
        {
            byte[] result = null;
            if (args.Length == 2)
            {
                string key = args[0];
                string value = args[1];
                string iv = "0000000000000000";

                AESHelper.Key = key;
                string content = AESHelper.AESEncrypt(value, iv);

                return encode.GetBytes(content);
            }
            else if (args.Length == 3)
            {
                string key = args[0];
                string value = args[1];
                string iv = args[2];

                AESHelper.Key = key;
                string content = AESHelper.AESEncrypt(value, iv);

                return encode.GetBytes(content);
            }

            return result;
        }
    }
}
