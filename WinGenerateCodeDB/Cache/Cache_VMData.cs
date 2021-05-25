using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace WinGenerateCodeDB
{
    /// <summary>
    /// 保存View Model 的设置
    /// </summary>
    public class Cache_VMData
    {
        private static JavaScriptSerializer js = new JavaScriptSerializer();
        private static List<VMDataInfo> dataList = new List<VMDataInfo>();
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache_vm.txt");

        public static void SaveData(List<VMDataInfo> list)
        {
            foreach (var item in list)
            {
                if (item.type == 0)
                {
                    var model = dataList.Find(p => p.db_name == item.db_name && p.type == 0);
                    if (model != null)
                    {
                        model = item;
                    }
                    else
                    {
                        dataList.Add(item);
                    }
                }
                else if (item.type == 1)
                {
                    var model = dataList.Find(p => p.db_name == item.db_name && p.type == 1 && p.table_name == item.table_name);
                    if (model != null)
                    {
                        model = item;
                    }
                    else
                    {
                        dataList.Add(item);
                    }
                }
            }

            // 保存dataList到文件
            StringBuilder content = new StringBuilder();
            foreach (var item in dataList)
            {
                content.AppendLine(js.Serialize(item));
            }

            File.WriteAllText(path, content.ToString(), Encoding.UTF8);
        }

        public static List<VMDataInfo> LoadData()
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path, Encoding.UTF8);
                foreach (var line in lines)
                {
                    try
                    {
                        var info = js.Deserialize<VMDataInfo>(line);
                        if (info != null)
                        {
                            dataList.Add(info);
                        }
                    }
                    catch (Exception e) { }
                }
            }

            return dataList;
        }

        public static List<SqlColumnInfo> GetVMList(string table_name, VMType vmType, List<SqlColumnInfo> current)
        {
            List<SqlColumnInfo> result = new List<SqlColumnInfo>();
            var tbInfo = dataList.Find(p => p.db_name == Cache_Next.GetDbName() && p.table_name == table_name);
            if (tbInfo == null || (vmType == VMType.Add && tbInfo.add_list.Count == 0)
                || (vmType == VMType.Edit && tbInfo.edit_list.Count == 0)
                || (vmType == VMType.Query && tbInfo.query_list.Count == 0))
            {
                var info = dataList.Find(p => p.db_name == Cache_Next.GetDbName() && p.type == 0);
                if (info == null)
                {
                    return current;
                }
                else
                {
                    if (vmType == VMType.Add)
                    {
                        return current.FindAll(p => !info.add_list.Contains(p.Name));
                    }
                    else if (vmType == VMType.Edit)
                    {
                        return current.FindAll(p => !info.edit_list.Contains(p.Name));
                    }
                    else
                    {
                        return current.FindAll(p => !info.query_list.Contains(p.Name));
                    }
                }
            }
            else
            {
                if (vmType == VMType.Add)
                {
                    return current.FindAll(p => tbInfo.add_list.Contains(p.Name));
                }
                else if (vmType == VMType.Edit)
                {
                    return current.FindAll(p => tbInfo.edit_list.Contains(p.Name));
                }
                else
                {
                    return current.FindAll(p => tbInfo.query_list.Contains(p.Name));
                }
            }
        }

        public static SqlColumnInfo GetDeleteColumnNameAndValue(string table_name, List<SqlColumnInfo> current)
        {
            List<SqlColumnInfo> result = new List<SqlColumnInfo>();
            var tbInfo = dataList.Find(p => p.db_name == Cache_Next.GetDbName() && p.type == 0);
            if (tbInfo != null)
            {
                if (tbInfo.extendInfo != null && tbInfo.extendInfo.type == 0 && tbInfo.extendInfo.v1 == 1)
                {
                    var item = current.Find(p => p.Name == tbInfo.extendInfo.name);
                    if (item != null)
                    {
                        SqlColumnInfo col = new SqlColumnInfo()
                        {
                            Name = item.Name,
                            DefaultValue = tbInfo.extendInfo.value
                        };

                        return col;
                    }
                }
            }

            return null;
        }

        public static List<SqlColumnInfo> GetAddCheckList(string table_name, List<SqlColumnInfo> current)
        {
            List<SqlColumnInfo> result = new List<SqlColumnInfo>();
            var tbInfo = dataList.Find(p => p.db_name == Cache_Next.GetDbName() && p.table_name == table_name);
            if (tbInfo == null || tbInfo.add_check_list.Count == 0)
            {
                return new List<SqlColumnInfo>();
            }
            else
            {
                var equalList = tbInfo.add_check_list.FindAll(p => p.Contains("="));
                var paramsList = tbInfo.add_check_list.FindAll(p => !p.Contains("="));
                var list = current.FindAll(p => paramsList.Contains(p.Name));
                List<SqlColumnInfo> equalSqlList = new List<SqlColumnInfo>();
                foreach (var item in equalList)
                {
                    // string[] key = item.Split(new char[] { '=' });
                }

                return new List<SqlColumnInfo>();
            }
        }
    }

    public enum VMType
    {
        Add = 0,
        Edit = 1,
        Query = 2
    }

    public class VMDataInfo
    {
        public string db_name { get; set; }

        /// <summary>
        /// 设置类型
        /// 0-通用设置
        /// 1-单表设置
        /// </summary>
        public int type { get; set; }

        public string table_name { get; set; }

        public List<string> add_list { get; set; }

        public List<string> edit_list { get; set; }

        public List<string> query_list { get; set; }

        public List<string> add_check_list { get; set; }

        public VMDataExtendInfo extendInfo { get; set; }

        public VMDataInfo()
        {
            add_list = new List<string>();
            edit_list = new List<string>();
            query_list = new List<string>();
        }
    }

    public class VMDataExtendInfo
    {
        /// <summary>
        /// 扩展类型
        /// 0-删除的字段
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 【删除字段】
        /// 0-物理删除
        /// 1-逻辑删除
        /// </summary>
        public int v1 { get; set; }

        public string name { get; set; }

        public string value { get; set; }
    }
}
