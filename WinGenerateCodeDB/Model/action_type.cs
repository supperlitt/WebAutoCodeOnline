using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinGenerateCodeDB
{
    public enum action_type
    {
        none = 0,
        add = 1,
        bat_add = 2,
        edit = 4,
        //bat_edit = 8, 移除掉 批量编辑
        delete = 16,
        bat_delete = 32,
        real_delete = 64,
        bat_real_delete = 128,
        query_info = 256,
        query_list = 512,
        query_all = 1024,
        export_select = 2048,
        export_all = 4096
    }
}
