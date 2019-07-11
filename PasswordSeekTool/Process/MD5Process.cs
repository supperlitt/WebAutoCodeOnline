using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Process
{
    public class MD5Process : IProcess
    {
        public byte[] Run(Encoding encode, params string[] args)
        {
            return encode.GetBytes(EncryptHelper.GetMD5(args[0], encode));
        }
    }
}
