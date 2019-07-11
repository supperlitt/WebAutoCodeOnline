using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceHelper
{
    public interface IAspx
    {
        string Create_Aspx(ModuleInfo module);

        string Create_Aspx_Cs(ModuleInfo module);

        string Create_Model_Cs(ModuleInfo module);

        string Create_DAL_Cs(ModuleInfo module);
    }
}
