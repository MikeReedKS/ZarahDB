using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZarahDB_Library.Types
{
    public partial class ZarahDBTest
    {
        [TestMethod]
        public void ListInstance()
        {
            GetRootFolder();

            Console.WriteLine($"Instance List:");
            var statusList = ZarahDB.ListInstance(null);
            foreach (var item in statusList.List)
            {
                Console.WriteLine(item);
            }

            Assert.AreEqual(statusList.Status, "200", statusList.Status);
        }

        //[TestMethod]
        //public void CreateInstance()
        //{
        //    var rootFolder = GetRootFolder();
        //    var newInstance

        //    Console.WriteLine($"Instance List:");
        //    var statusList = ZarahDB.CreateInstance();
        //    foreach (var item in statusList.List)
        //    {
        //        Console.WriteLine(item);
        //    }

        //    Assert.AreEqual(statusList.Status, "200", statusList.Status);
        //}

        //[TestMethod]
        //public void ListInstance()
        //{
        //    GetRootFolder();

        //    Console.WriteLine($"Instance List:");
        //    var statusList = ZarahDB.ListInstance(null);
        //    foreach (var item in statusList.List)
        //    {
        //        Console.WriteLine(item);
        //    }

        //    Assert.AreEqual(statusList.Status, "200", statusList.Status);
        //}

        private Uri GetRootFolder()
        {
            var rootFolder = ZarahDB.GetInstance();
            Console.WriteLine($"Default Instance:{rootFolder}");
            rootFolder = new Uri(Directory.GetParent(rootFolder.AbsolutePath).ToString());
            Console.WriteLine($"Root Folder:{rootFolder}");
            Console.WriteLine("");
            return rootFolder;
        }
    }
}
