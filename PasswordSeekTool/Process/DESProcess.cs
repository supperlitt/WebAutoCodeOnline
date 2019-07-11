using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Process
{
    public class DESProcess : IProcess
    {
        public byte[] Run(Encoding encode, params string[] args)
        {
            string content = DESHelper.Encrypt(encode, args[0], args[1], args[2]);

            return encode.GetBytes(content);
        }
    }
}
