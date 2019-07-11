using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebAutoCodeOnline
{
    public class ProjectFileCache
    {
        private static List<ProjectTreeInfo> dataList = new List<ProjectTreeInfo>();
        private static object lockObj = new object();

        public static ProjectTreeInfo GetByKey(string name)
        {
            lock (lockObj)
            {
                var item = dataList.Find(p => p.Name == name);
                if (item == null)
                {
                    var list = ProjectCache.GetDataList();
                    item = new ProjectTreeInfo();
                    item.Name = name;

                    var model = list.Find(p => p.ProjectTypeName == name);
                    if (model != null)
                    {
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", model.DirName);
                        TreeManager treeManager = new TreeManager();
                        var treeList = treeManager.GetDataList(path, "0", null);
                        item.TreeList = treeList;
                        item.SingleTreeList = treeManager.GetSingleList();
                        dataList.Add(item);

                        return item;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return item;
                }
            }
        }
    }

    public class ProjectTreeInfo
    {
        public string Name { get; set; }

        public List<TreeInfo> TreeList { get; set; }

        private List<SingleTree> singleTreeList = new List<SingleTree>();
        public List<SingleTree> SingleTreeList
        {
            get { return this.singleTreeList; }
            set { this.singleTreeList = value; }
        }
    }

    public class TreeInfo
    {
        public string id { get; set; }

        public string text { get; set; }

        public string parentid { get; set; }

        private List<TreeInfo> childrenField = new List<TreeInfo>();
        public List<TreeInfo> children
        {
            get { return childrenField; }
            set { this.childrenField = value; }
        }
    }

    public class SingleTree
    {
        public string id { get; set; }
        public string path { get; set; }
    }

    public class TreeManager
    {
        private List<SingleTree> singleTreeList = new List<SingleTree>();

        private List<TreeInfo> dataList = new List<TreeInfo>();

        private int aId = 0;

        public List<TreeInfo> GetDataList(string dirPath, string parentId, TreeInfo parentTree)
        {
            string[] files = Directory.GetFiles(dirPath);
            string[] dirs = Directory.GetDirectories(dirPath);
            foreach (var dir in dirs)
            {
                TreeInfo tree = new TreeInfo();
                DirectoryInfo d = new DirectoryInfo(dir);

                aId++;
                tree.id = aId.ToString();
                tree.parentid = parentId;
                tree.text = d.Name;
                dataList.Add(tree);
                this.GetDataList(dir, aId.ToString(), tree);
            }

            foreach (var file in files)
            {
                FileInfo f = new FileInfo(file);
                TreeInfo tree = new TreeInfo();

                aId++;
                tree.id = aId.ToString();
                tree.parentid = parentId;
                string name = f.Name.Remove(f.Name.Length - 4, 4);
                tree.text = name;
                singleTreeList.Add(new SingleTree() { id = aId.ToString(), path = f.FullName });
                if (parentTree != null)
                {
                    parentTree.children.Add(tree);
                }
                else
                {
                    dataList.Add(tree);
                }
            }

            return dataList;
        }

        public List<SingleTree> GetSingleList()
        {
            return singleTreeList;
        }
    }
}