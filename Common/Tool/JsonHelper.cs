using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.JScript;

namespace Common
{
    /// <summary>
    /// Json字符串zhuanh
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 是否添加get set
        /// </summary>
        private bool isAddGetSet = false;

        /// <summary>
        /// 数据集合，临时
        /// </summary>
        private List<AutoClass> dataList = new List<AutoClass>();

        public JsonHelper()
        {
        }

        public JsonHelper(bool isAddGetSet)
        {
            this.isAddGetSet = isAddGetSet;
        }

        /// <summary>
        /// 获取类的字符串形式
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public string GetClassString(string jsonStr)
        {
            Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
            var m = Microsoft.JScript.Eval.JScriptEvaluate("(" + jsonStr + ")", ve);

            int index = 0;
            var result = GetDicType((JSObject)m, ref index);

            StringBuilder content = new StringBuilder();
            foreach (var item in dataList)
            {
                content.AppendFormat("\tpublic class {0}\r\n", item.CLassName);
                content.AppendLine("\t{");
                foreach (var model in item.Dic)
                {
                    if (isAddGetSet)
                    {
                        content.AppendFormat("\t\tpublic {0} {1}", model.Value, model.Key);
                        content.Append(" { get; set; }\r\n");
                    }
                    else
                    {
                        content.AppendFormat("\t\tpublic {0} {1};\r\n", model.Value, model.Key);
                    }

                    content.AppendLine();
                }

                content.AppendLine("\t}");
                content.AppendLine();
            }

            return content.ToString();
        }

        /// <summary>
        /// 获取类型的字符串表示
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetTypeString(Type type)
        {
            if (type == typeof(int))
            {
                return "int";
            }
            else if (type == typeof(bool))
            {
                return "bool";
            }
            else if (type == typeof(Int64))
            {
                return "long";
            }
            else if (type == typeof(string))
            {
                return "string";
            }
            else if (type == typeof(List<string>))
            {
                return "List<string>";
            }
            else if (type == typeof(List<int>))
            {
                return "List<int>";
            }
            else
            {
                return "string";
            }
        }

        /// <summary>
        /// 获取字典类型
        /// </summary>
        /// <returns></returns>
        private string GetDicType(JSObject jsObj, ref int index)
        {
            AutoClass classInfo = new AutoClass();

            var model = ((Microsoft.JScript.JSObject)(jsObj)).GetMembers(System.Reflection.BindingFlags.GetField);
            foreach (Microsoft.JScript.JSField item in model)
            {
                string name = item.Name;
                Type type = item.GetValue(item).GetType();
                if (type == typeof(ArrayObject))
                {
                    // 集合
                    string typeName = GetDicListType((ArrayObject)item.GetValue(item), ref index);
                    if (!string.IsNullOrEmpty(typeName))
                    {
                        classInfo.Dic.Add(name, typeName);
                    }
                }
                else if (type == typeof(JSObject))
                {
                    // 单个对象
                    string typeName = GetDicType((JSObject)item.GetValue(item), ref index);
                    if (!string.IsNullOrEmpty(typeName))
                    {
                        classInfo.Dic.Add(name, typeName);
                    }
                }
                else
                {
                    classInfo.Dic.Add(name, GetTypeString(type));
                }
            }

            index++;
            classInfo.CLassName = "Class" + index;
            dataList.Add(classInfo);
            return classInfo.CLassName;
        }

        /// <summary>
        /// 读取集合类型
        /// </summary>
        /// <param name="jsArray"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetDicListType(ArrayObject jsArray, ref int index)
        {
            string name = string.Empty;
            if ((int)jsArray.length > 0)
            {
                var item = jsArray[0];
                var type = item.GetType();
                if (type == typeof(JSObject))
                {
                    name = "List<" + GetDicType((JSObject)item, ref index) + ">";
                }
                else
                {
                    name = "List<" + GetTypeString(type) + ">";
                }
            }

            return name;
        }
    }

    public class AutoClass
    {
        public string CLassName { get; set; }

        private Dictionary<string, string> dic = new Dictionary<string, string>();

        public Dictionary<string, string> Dic
        {
            get
            {
                return this.dic;
            }
            set
            {
                this.dic = value;
            }
        }
    }
}
