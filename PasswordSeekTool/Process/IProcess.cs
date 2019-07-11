using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool
{
    public interface IProcess
    {
        byte[] Run(Encoding encode, params string[] args);
    }
}
