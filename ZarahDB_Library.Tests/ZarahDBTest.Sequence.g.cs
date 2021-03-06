using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZarahDB_Library.Enums;

namespace ZarahDB_Library.Tests
{
    public partial class ZarahDBTest
    {
        [TestMethod]
        public void SequenceChangeValue()
        {
            //Set instance and table for test
            const Uri instance = null;
            const string table = "Test";

            //Write single value
            const string key = "Test Key";
            const string column = "Test Column";
            var value = "Test Value 1";
            var statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Get single value
            statusValue = Get(instance, table, key, column);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Status);
            }
            if (statusValue.Value != value)
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            value = "Test Value 2";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Get single value
            statusValue = Get(instance, table, key, column);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Status);
            }
            if (statusValue.Value != value)
            {
                Assert.Fail(statusValue.Value);
            }
        }

        [TestMethod]
        public void SequenceGetAllColumnsForKey()
        {
            //Set instance and table for test
            const Uri instance = null;
            const string table = "Test Table";

            //Write single value
            const string key = "Test Key";
            var column = "Test Column 1";
            var value = "Test Value 1";
            var statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            column = "Test Column 2";
            value = "Test Value 2";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            column = "Test Column 3";
            value = "Test Value 3";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            column = "Test Column 4";
            value = "Test Value 4";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Get all columns for the key
            var columnList = new List<string> {"*"};
            var statusKeyColumnValues = Get(null, table, key, columnList);
            if (statusKeyColumnValues.Status != "200")
            {
                Assert.Fail(statusKeyColumnValues.Status);
            }
        }

        [TestMethod]
        public void SequenceZeroCharacterKey()
        {
            //Set instance and table for test
            const Uri instance = null;
            const string table = "Test Table";

            //Write single value
            const string key = "\0";
            var column = "Test Column 1";
            var value = "Test Value 1";
            var statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            column = "Test Column 2";
            value = "Test Value 2";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            column = "Test Column 3";
            value = "Test Value 3";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            column = "Test Column 4";
            value = "Test Value 4";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Get all columns for the key
            var columnList = new List<string> {"*"};
            var statusKeyColumnValues = Get(null, table, key, columnList);
            if (statusKeyColumnValues.Status != "200")
            {
                Assert.Fail(statusKeyColumnValues.Status);
            }
        }

        [TestMethod]
        public void SequenceGetSelectedColumnsForKey()
        {
            //Set instance and table for test
            const Uri instance = null;
            const string table = "Test Table";

            //Write single value
            const string key = "Test Key";
            var column = "Test Column 1";
            var value = "Test Value 1";
            var statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            column = "Test Column 2";
            value = "Test Value 2";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            column = "Test Column 3";
            value = "Test Value 3";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Write a new single value
            column = "Test Column 4";
            value = "Test Value 4";
            statusValue = Put(instance, table, key, column, value);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Value);
            }

            //Get all columns for the key
            var columnList = new List<string> {"*"};
            var statusKeyColumnValues = Get(null, table, key, columnList);
            if (statusKeyColumnValues.Status != "200")
            {
                Assert.Fail(statusKeyColumnValues.Status);
            }

            //Get selected columns for the key
            columnList = new List<string> {"Test Column 1", "Test Column 3", "Test Column 5"};
            statusKeyColumnValues = Get(null, table, key, columnList);
            if (statusKeyColumnValues.Status != "200")
            {
                Assert.Fail(statusKeyColumnValues.Status);
            }
        }

        [TestMethod]
        public void SequenceCreateTestDeleteInstance()
        {
            //Set instance and table for test
            var instance = new Uri(@"C:\zdb_root\test_instance");
            const string table = "Test Table";

            //Write single value
            const string key = "Test Key";
            var column = "Test Column 1";
            var value = "Test Value 1";
            var statusMessageValue = Put(instance, table, key, column, value);
            if (statusMessageValue.Status != "200")
            {
                Assert.Fail(statusMessageValue.Value);
            }

            //Perform a backup
            statusMessageValue = Backup(instance, null);
            if (statusMessageValue.Status != "200")
            {
                Assert.Fail(statusMessageValue.Value);
            }

            //Delete the instance
            var result = DeleteInstance(instance, null);
            if (result.Status != "200")
            {
                Assert.Fail(result.ToString());
            }
        }

        [TestMethod]
        public void SequenceCreateTestDeleteTable()
        {
        }

        [TestMethod]
        public void SequenceRootFolderNull()
        {
            const string table = "Test Table";
            const string key = "Test Key";
            const string column = "Test Column";
            string value = $"Test Value {Guid.NewGuid()}";
            Console.WriteLine(value);

            ZarahDB.PutInstance();
            var defaultInstancePath = ZarahDB.GetInstance();
            Console.WriteLine($"null {defaultInstancePath.AbsolutePath}");
            var statusMessageValue = ZarahDB.Put(null, table, key, column, value);
            if (statusMessageValue.Status != "200") Assert.Fail(statusMessageValue.Status);
            statusMessageValue = ZarahDB.Get(null, table, key, column);
            Assert.AreEqual(statusMessageValue.Value, value, statusMessageValue.Value);
        }

        [TestMethod]
        public void SequenceRootFolderApplicationData()
        {
            const string table = "Test Table";
            const string key = "Test Key";
            const string column = "Test Column";
            string value = $"Test Value {Guid.NewGuid()}";
            Console.WriteLine(value);

            ZarahDB.PutInstance(null, InstanceLocation.ApplicationData);
            var defaultInstancePath = ZarahDB.GetInstance();
            Console.WriteLine($"ApplicationData {defaultInstancePath.AbsolutePath}");
            var statusMessageValue = ZarahDB.Put(null, table, key, column, value);
            if (statusMessageValue.Status != "200") Assert.Fail(statusMessageValue.Status);
            statusMessageValue = ZarahDB.Get(null, table, key, column);
            Assert.AreEqual(statusMessageValue.Value, value, statusMessageValue.Value);
        }

        [TestMethod]
        public void SequenceRootFolderBaseDirectory()
        {
            const string table = "Test Table";
            const string key = "Test Key";
            const string column = "Test Column";
            string value = $"Test Value {Guid.NewGuid()}";
            Console.WriteLine(value);

            ZarahDB.PutInstance(null, InstanceLocation.BaseDirectory);
            var defaultInstancePath = ZarahDB.GetInstance();
            Console.WriteLine($"BaseDirectory {defaultInstancePath.AbsolutePath}");
            var statusMessageValue = ZarahDB.Put(null, table, key, column, value);
            if (statusMessageValue.Status != "200") Assert.Fail(statusMessageValue.Status);
            statusMessageValue = ZarahDB.Get(null, table, key, column);
            Assert.AreEqual(statusMessageValue.Value, value, statusMessageValue.Value);
        }

        //[TestMethod]
        //public void SequenceRootFolderCommonApplicationData()
        //{
        //    const string table = "Test Table";
        //    const string key = "Test Key";
        //    const string column = "Test Column";
        //    string value = $"Test Value {Guid.NewGuid()}";
        //    Console.WriteLine(value);

        //    ZarahDB.PutInstance(null, InstanceLocation.CommonApplicationData);
        //    var defaultInstancePath = ZarahDB.GetInstance();
        //    Console.WriteLine($"CommonApplicationData {defaultInstancePath.AbsolutePath}");
        //    var statusMessageValue = ZarahDB.Put(null, table, key, column, value);
        //    if (statusMessageValue.Status != "200") Assert.Fail(statusMessageValue.Status);
        //    statusMessageValue = ZarahDB.Get(null, table, key, column);
        //    Assert.AreEqual(statusMessageValue.Value, value, statusMessageValue.Value);
        //}

        [TestMethod]
        public void SequenceRootFolderDesktopDirectory()
        {
            const string table = "Test Table";
            const string key = "Test Key";
            const string column = "Test Column";
            string value = $"Test Value {Guid.NewGuid()}";
            Console.WriteLine(value);

            ZarahDB.PutInstance(null, InstanceLocation.DesktopDirectory);
            var defaultInstancePath = ZarahDB.GetInstance();
            Console.WriteLine($"DesktopDirectory {defaultInstancePath.AbsolutePath}");
            var statusMessageValue = ZarahDB.Put(null, table, key, column, value);
            if (statusMessageValue.Status != "200") Assert.Fail(statusMessageValue.Status);
            statusMessageValue = ZarahDB.Get(null, table, key, column);
            Assert.AreEqual(statusMessageValue.Value, value, statusMessageValue.Value);
        }

        [TestMethod]
        public void SequenceRootFolderLocation()
        {
            const string table = "Test Table";
            const string key = "Test Key";
            const string column = "Test Column";
            string value = $"Test Value {Guid.NewGuid()}";
            Console.WriteLine(value);

            ZarahDB.PutInstance(null, InstanceLocation.Location);
            var defaultInstancePath = ZarahDB.GetInstance();
            Console.WriteLine($"Location {defaultInstancePath.AbsolutePath}");
            var statusMessageValue = ZarahDB.Put(null, table, key, column, value);
            if (statusMessageValue.Status != "200") Assert.Fail(statusMessageValue.Status);
            statusMessageValue = ZarahDB.Get(null, table, key, column);
            Assert.AreEqual(statusMessageValue.Value, value, statusMessageValue.Value);
        }

        [TestMethod]
        public void SequenceRootFolderCodebase()
        {
            const string table = "Test Table";
            const string key = "Test Key";
            const string column = "Test Column";
            string value = $"Test Value {Guid.NewGuid()}";
            Console.WriteLine(value);

            ZarahDB.PutInstance(null, InstanceLocation.Codebase);
            var defaultInstancePath = ZarahDB.GetInstance();
            Console.WriteLine($"Codebase {defaultInstancePath.AbsolutePath}");
            var statusMessageValue = ZarahDB.Put(null, table, key, column, value);
            if (statusMessageValue.Status != "200") Assert.Fail(statusMessageValue.Status);
            statusMessageValue = ZarahDB.Get(null, table, key, column);
            Assert.AreEqual(statusMessageValue.Value, value, statusMessageValue.Value);
        }
    }
}