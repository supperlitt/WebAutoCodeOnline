using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.Administration;
using System.DirectoryServices;
using System.Security.AccessControl;

namespace WinAutoEasyUI
{
    public class IIS7
    {
        public void AddSite(string ip, string port, string name, string path)
        {
            ServerManager iisManager = new ServerManager();
            var site = iisManager.Sites.Add(name, "http", GetBindInfomation(ip, port), path);
            iisManager.CommitChanges();
            if (!IsAppPoolName(name))
            {
                CreateAppPool(name);
            }

            AssignVDirToAppPool(name);
        }

        /// <summary>
        /// 是否包含站点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsContainsSite(string name)
        {
            DirectoryEntry Services = new DirectoryEntry("IIS://localhost/W3SVC");
            foreach (DirectoryEntry server in Services.Children)
            {
                if (server.SchemaClassName == "IIsWebServer")
                {
                    if (server.Properties["ServerComment"].Value.ToString() == name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static void AssignVDirToAppPool(string siteName)
        {
            try
            {
                ServerManager serverManager = new ServerManager();
                Site site = serverManager.Sites[siteName];
                site.Applications[0].ApplicationPoolName = siteName;
                serverManager.CommitChanges();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 判断程序池是否存在
        /// </summary>
        /// <param name="AppPoolName">程序池名称</param>
        /// <returns>true存在 false不存在</returns>
        private bool IsAppPoolName(string AppPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            foreach (DirectoryEntry getdir in appPools.Children)
            {
                if (getdir.Name.Equals(AppPoolName))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 删除指定程序池
        /// </summary>
        /// <param name="AppPoolName">程序池名称</param>
        /// <returns>true删除成功 false删除失败</returns>
        private bool DeleteAppPool(string AppPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            foreach (DirectoryEntry getdir in appPools.Children)
            {
                if (getdir.Name.Equals(AppPoolName))
                {
                    try
                    {
                        getdir.DeleteTree();
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 创建应用程序池
        /// </summary>
        /// <param name="AppPoolName"></param>
        /// <returns></returns>
        private bool CreateAppPool(string AppPoolName)
        {
            try
            {
                if (!IsAppPoolName(AppPoolName))
                {
                    DirectoryEntry newpool;
                    DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                    newpool = appPools.Children.Add(AppPoolName, "IIsApplicationPool");
                    newpool.CommitChanges();
                }

                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限
        /// </summary>
        /// <param name="FileAdd">文件夹路径</param>
        public void SetFileRole(string dir)
        {
            string FileAdd = dir;
            FileAdd = FileAdd.Remove(FileAdd.LastIndexOf('\\'), 1);
            DirectorySecurity fSec = new DirectorySecurity();
            fSec.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            System.IO.Directory.SetAccessControl(FileAdd, fSec);
        }

        private string GetBindInfomation(string ip, string port)
        {
            return string.Format("{0}:{1}:", ip, port);
        }
    }
}
