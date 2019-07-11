using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeHelper
{
    /// <summary>
    /// UI帮助类
    /// </summary>
    public class UIHelper
    {
        public static void InitNormalCode(NormalModel model)
        {
            if (model.DbType == 0)
            {
                Regex tableComRegex = new Regex(@"--\s{0,}(?<tablecom>[^\s]+)\s{0,}create\s+table\s+");
                Regex tableRegex = new Regex(@"create\s+table\s+(?<tableName>\w+)");
                Regex columnRegex = new Regex(@"\s{0,}(?<key>\w+)\s+(?<type>\w+)");
                Regex mainKeyRegex = new Regex(@"primary\s+key", RegexOptions.IgnoreCase);
                Regex commentRegex = new Regex(@"-{2,}\s{0,}(?<comment>[^\s]+)");
                model.TableName = tableRegex.Match(model.TableStr).Groups["tableName"].Value;
                model.Title = tableComRegex.Match(model.TableStr).Groups["tablecom"].Value;

                #region 加载列集合
                var columnList = new List<ColumnInfo>();
                string[] lines = model.TableStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    Match m = columnRegex.Match(line);
                    string key = m.Groups["key"].Value;
                    string value = m.Groups["type"].Value;
                    if (key == "" || value == "" || key.ToLower() == "create" || value.ToLower() == "table")
                    {
                        continue;
                    }

                    string comment = commentRegex.Match(line).Groups["comment"].Value;
                    if (string.IsNullOrEmpty(comment))
                    {
                        comment = key;
                    }

                    columnList.Add(new ColumnInfo()
                    {
                        ColumnName = key,
                        DBType = value,
                        IsAutoIncrement = line.ToLower().Contains("identity"),
                        IsMainKey = mainKeyRegex.IsMatch(line),
                        Comment = comment
                    });
                }

                model.ColumnList.AddRange(columnList.ToArray());
                string[] searchArray = model.SearchColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in searchArray)
                {
                    var m = model.ColumnList.Find(p => p.ColumnName == item);
                    if (m != null)
                    {
                        model.SearchColumns.Add(m);
                    }
                }
                #endregion
            }
            else if (model.DbType == 1)
            {
                Regex tableComRegex = new Regex(@"--\s{0,}(?<tablecom>[^\s]+)\s{0,}create\s+table\s+");
                Regex tableRegex = new Regex(@"create\s+table\s+(?<tableName>\w+)");
                Regex columnRegex = new Regex(@"\s{0,}(?<key>\w+)\s+(?<type>\w+)");
                Regex mainKeyRegex = new Regex(@"primary\s+key", RegexOptions.IgnoreCase);
                Regex commentRegex = new Regex(@"-{2,}\s{0,}(?<comment>[^\s]+)");
                model.TableName = tableRegex.Match(model.TableStr).Groups["tableName"].Value;
                model.Title = tableComRegex.Match(model.TableStr).Groups["tablecom"].Value;

                #region 加载列集合
                var columnList = new List<ColumnInfo>();
                string[] lines = model.TableStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    Match m = columnRegex.Match(line);
                    string key = m.Groups["key"].Value;
                    string value = m.Groups["type"].Value;
                    if (key == "" || value == "" || key.ToLower() == "create" || value.ToLower() == "table")
                    {
                        continue;
                    }

                    string comment = commentRegex.Match(line).Groups["comment"].Value;
                    if (string.IsNullOrEmpty(comment))
                    {
                        comment = key;
                    }

                    columnList.Add(new ColumnInfo()
                    {
                        ColumnName = key,
                        DBType = value,
                        IsAutoIncrement = line.ToLower().Contains("auto_increment"),
                        IsMainKey = mainKeyRegex.IsMatch(line),
                        Comment = comment
                    });
                }

                model.ColumnList.AddRange(columnList.ToArray());
                string[] searchArray = model.SearchColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in searchArray)
                {
                    var m = model.ColumnList.Find(p => p.ColumnName == item);
                    if (m != null)
                    {
                        model.SearchColumns.Add(m);
                    }
                }
                #endregion
            }
        }

        public static void InitEasyUI(EasyUIModel model)
        {
            if (model.DbType == 0)
            {
                // MS SQL
                Regex tableComRegex = new Regex(@"--\s{0,}(?<tablecom>[^\s]+)\s{0,}create\s+table\s+");
                Regex tableRegex = new Regex(@"create\s+table\s+(?<tableName>\w+)");
                Regex columnRegex = new Regex(@"\s{0,}(?<key>\w+)\s+(?<type>\w+)");
                Regex mainKeyRegex = new Regex(@"primary\s+key", RegexOptions.IgnoreCase);
                Regex commentRegex = new Regex(@"-{2,}\s{0,}(?<comment>[^\s]+)");
                model.TableName = tableRegex.Match(model.TableStr).Groups["tableName"].Value;
                model.Title = tableComRegex.Match(model.TableStr).Groups["tablecom"].Value;

                #region 加载列集合
                var columnList = new List<ColumnInfo>();
                string[] lines = model.TableStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    Match m = columnRegex.Match(line);
                    string key = m.Groups["key"].Value;
                    string value = m.Groups["type"].Value;
                    if (key == "" || value == "" || key.ToLower() == "create" || value.ToLower() == "table")
                    {
                        continue;
                    }

                    string comment = commentRegex.Match(line).Groups["comment"].Value;
                    if (string.IsNullOrEmpty(comment))
                    {
                        comment = key;
                    }

                    columnList.Add(new ColumnInfo()
                    {
                        ColumnName = key,
                        DBType = value,
                        IsAutoIncrement = line.ToLower().Contains("identity"),
                        IsMainKey = mainKeyRegex.IsMatch(line),
                        Comment = comment
                    });
                }

                model.ColumnList.AddRange(columnList.ToArray());
                string[] array = model.SearchColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in array)
                {
                    var m = model.ColumnList.Find(p => p.ColumnName == item);
                    if (m != null)
                    {
                        model.SearchColumns.Add(m);
                    }
                }
                #endregion

                #region 添加，编辑，批量编辑
                if (model.IsAdd)
                {
                    array = model.AddColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.AddColumns.Add(m);
                        }
                    }
                }

                if (model.IsEdit)
                {
                    array = model.EditColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.EditColumns.Add(m);
                        }
                    }
                }

                if (model.IsBatEdit)
                {
                    array = model.BatEditColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.BatEditColumns.Add(m);
                        }
                    }
                }
                #endregion
            }
            else if (model.DbType == 1)
            {
                // MySQL
                Regex tableComRegex = new Regex(@"--\s{0,}(?<tablecom>[^\s]+)\s{0,}create\s+table\s+");
                Regex tableRegex = new Regex(@"create\s+table\s+(?<tableName>\w+)");
                Regex columnRegex = new Regex(@"\s{0,}(?<key>\w+)\s+(?<type>\w+)");
                Regex mainKeyRegex = new Regex(@"primary\s+key", RegexOptions.IgnoreCase);
                Regex commentRegex = new Regex(@"-{2,}\s{0,}(?<comment>[^\s]+)");
                model.TableName = tableRegex.Match(model.TableStr).Groups["tableName"].Value;
                model.Title = tableComRegex.Match(model.TableStr).Groups["tablecom"].Value;

                #region 加载列集合
                var columnList = new List<ColumnInfo>();
                string[] lines = model.TableStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    Match m = columnRegex.Match(line);
                    string key = m.Groups["key"].Value;
                    string value = m.Groups["type"].Value;
                    if (key == "" || value == "" || key.ToLower() == "create" || value.ToLower() == "table")
                    {
                        continue;
                    }

                    string comment = commentRegex.Match(line).Groups["comment"].Value;
                    if (string.IsNullOrEmpty(comment))
                    {
                        comment = key;
                    }

                    columnList.Add(new ColumnInfo()
                    {
                        ColumnName = key,
                        DBType = value,
                        IsAutoIncrement = line.ToLower().Contains("auto_increment"),
                        IsMainKey = mainKeyRegex.IsMatch(line),
                        Comment = comment
                    });
                }

                model.ColumnList.AddRange(columnList.ToArray());
                string[] array = model.SearchColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in array)
                {
                    var m = model.ColumnList.Find(p => p.ColumnName == item);
                    if (m != null)
                    {
                        model.SearchColumns.Add(m);
                    }
                }
                #endregion

                #region 添加，编辑，批量编辑
                if (model.IsAdd)
                {
                    array = model.AddColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.AddColumns.Add(m);
                        }
                    }
                }

                if (model.IsEdit)
                {
                    array = model.EditColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.EditColumns.Add(m);
                        }
                    }
                }

                if (model.IsBatEdit)
                {
                    array = model.BatEditColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.BatEditColumns.Add(m);
                        }
                    }
                }
                #endregion
            }

            var col = model.ColumnList.Find(p => p.IsMainKey);
            if (col != null)
            {
                model.MainKeyIdStr = col.ColumnName;
                model.MainKeyIdDBType = col.DBType;
            }
        }

        public static void InitBootstrap(BootstrapModel model)
        {
            if (model.DbType == 0)
            {
                // MS SQL
                Regex tableComRegex = new Regex(@"--\s{0,}(?<tablecom>[^\s]+)\s{0,}create\s+table\s+");
                Regex tableRegex = new Regex(@"create\s+table\s+(?<tableName>\w+)");
                Regex columnRegex = new Regex(@"\s{0,}(?<key>\w+)\s+(?<type>\w+)");
                Regex mainKeyRegex = new Regex(@"primary\s+key", RegexOptions.IgnoreCase);
                Regex commentRegex = new Regex(@"-{2,}\s{0,}(?<comment>[^\s]+)");
                model.TableName = tableRegex.Match(model.TableStr).Groups["tableName"].Value;
                model.Title = tableComRegex.Match(model.TableStr).Groups["tablecom"].Value;

                #region 加载列集合
                var columnList = new List<ColumnInfo>();
                string[] lines = model.TableStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    Match m = columnRegex.Match(line);
                    string key = m.Groups["key"].Value;
                    string value = m.Groups["type"].Value;
                    if (key == "" || value == "" || key.ToLower() == "create" || value.ToLower() == "table")
                    {
                        continue;
                    }

                    string comment = commentRegex.Match(line).Groups["comment"].Value;
                    if (string.IsNullOrEmpty(comment))
                    {
                        comment = key;
                    }

                    columnList.Add(new ColumnInfo()
                    {
                        ColumnName = key,
                        DBType = value,
                        IsAutoIncrement = line.ToLower().Contains("identity"),
                        IsMainKey = mainKeyRegex.IsMatch(line),
                        Comment = comment
                    });
                }

                model.ColumnList.AddRange(columnList.ToArray());
                string[] array = model.SearchColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in array)
                {
                    var m = model.ColumnList.Find(p => p.ColumnName == item);
                    if (m != null)
                    {
                        model.SearchColumns.Add(m);
                    }
                }
                #endregion

                #region 添加，编辑，批量编辑
                if (model.IsAdd)
                {
                    array = model.AddColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.AddColumns.Add(m);
                        }
                    }
                }

                if (model.IsEdit)
                {
                    array = model.EditColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.EditColumns.Add(m);
                        }
                    }
                }

                if (model.IsBatEdit)
                {
                    array = model.BatEditColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.BatEditColumns.Add(m);
                        }
                    }
                }
                #endregion
            }
            else if (model.DbType == 1)
            {
                // MySQL
                Regex tableComRegex = new Regex(@"--\s{0,}(?<tablecom>[^\s]+)\s{0,}create\s+table\s+");
                Regex tableRegex = new Regex(@"create\s+table\s+(?<tableName>\w+)");
                Regex columnRegex = new Regex(@"\s{0,}(?<key>\w+)\s+(?<type>\w+)");
                Regex mainKeyRegex = new Regex(@"primary\s+key", RegexOptions.IgnoreCase);
                Regex commentRegex = new Regex(@"-{2,}\s{0,}(?<comment>[^\s]+)");
                model.TableName = tableRegex.Match(model.TableStr).Groups["tableName"].Value;
                model.Title = tableComRegex.Match(model.TableStr).Groups["tablecom"].Value;

                #region 加载列集合
                var columnList = new List<ColumnInfo>();
                string[] lines = model.TableStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    Match m = columnRegex.Match(line);
                    string key = m.Groups["key"].Value;
                    string value = m.Groups["type"].Value;
                    if (key == "" || value == "" || key.ToLower() == "create" || value.ToLower() == "table")
                    {
                        continue;
                    }

                    string comment = commentRegex.Match(line).Groups["comment"].Value;
                    if (string.IsNullOrEmpty(comment))
                    {
                        comment = key;
                    }

                    columnList.Add(new ColumnInfo()
                    {
                        ColumnName = key,
                        DBType = value,
                        IsAutoIncrement = line.ToLower().Contains("auto_increment"),
                        IsMainKey = mainKeyRegex.IsMatch(line),
                        Comment = comment
                    });
                }

                model.ColumnList.AddRange(columnList.ToArray());
                string[] array = model.SearchColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in array)
                {
                    var m = model.ColumnList.Find(p => p.ColumnName == item);
                    if (m != null)
                    {
                        model.SearchColumns.Add(m);
                    }
                }
                #endregion

                #region 添加，编辑，批量编辑
                if (model.IsAdd)
                {
                    array = model.AddColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.AddColumns.Add(m);
                        }
                    }
                }

                if (model.IsEdit)
                {
                    array = model.EditColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.EditColumns.Add(m);
                        }
                    }
                }

                if (model.IsBatEdit)
                {
                    array = model.BatEditColumnsStr.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        var m = model.ColumnList.Find(p => p.ColumnName == item);
                        if (m != null)
                        {
                            model.BatEditColumns.Add(m);
                        }
                    }
                }
                #endregion
            }

            var col = model.ColumnList.Find(p => p.IsMainKey);
            if (col != null)
            {
                model.MainKeyIdStr = col.ColumnName;
                model.MainKeyIdDBType = col.DBType;
            }
        }
    }
}