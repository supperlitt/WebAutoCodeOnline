using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    /// <summary>
    /// helper接口
    /// </summary>
    public interface IHelper
    {
        /// <summary>
        /// 通过字符串生成类
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string GetClassString(NormalModel model);
    }
}
