using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZarahDB_Library;
using ZarahDB_Library.Helpers;
using ZarahDB_Library.Types;

namespace ZarahDB_WebAPI.Controllers.Tests
{
    [TestClass]
    public class TableControllerTests
    {
        [TestMethod]
        public void CsvPutTest()
        {
        }

        [TestMethod]
        public void CsvFilePutTest()
        {
        }

        [TestMethod]
        public void CsvFolderPutTest()
        {
        }

        [TestMethod]
        public void DeleteTableTest()
        {
        }

        [TestMethod]
        public void ListTableTest()
        {
            const string testInstanceName = "zdb_APITests";
            const string testTableName = "TestTable";

            Debug.WriteLine("Creating an instance of the Instance API controller.");
            var tableController = new TableController();

            Debug.WriteLine($"Setting test instance to: {testInstanceName}.");

            var resultExistsTable = tableController.ExistsTable(testInstanceName, testTableName);
            if (resultExistsTable.Value == "True")
            {
                Debug.WriteLine($"{testTableName} table already exists in the {testInstanceName} instance, so we will detete it.");
                tableController.DeleteTable(testInstanceName, testTableName);
            }
            else
            {
                Debug.WriteLine($"{testTableName} table does not exist in the {testInstanceName} instance.");
            }

            //Set the Max Depth for the table (assures the table exists)
            StatusMessageValue resultMaxDepthTable = tableController.MaxDepth(testInstanceName, testTableName, 7);
            Debug.WriteLine("Set Max Depth, which will create an empty table.");
            Assert.AreEqual(resultMaxDepthTable.Status, "200");

            resultExistsTable = tableController.ExistsTable(testInstanceName, testTableName);
            if (resultExistsTable.Value == "True")
            {
                Debug.WriteLine($"{testTableName} table now exists in the {testInstanceName} as expected.");
            }
            else
            {
                Assert.Fail($"{testTableName} table does not exist in the {testInstanceName} instance even after attempting to create it.");
            }

            Debug.WriteLine($"List of tables in the {testInstanceName} instance:");
            var resultListTable = tableController.ListTable(testInstanceName);
            foreach (var table in resultListTable.List)
            {
                Debug.WriteLine(table);
            }
        }

        [TestMethod]
        public void ExistsTableTest()
        {
        }

        [TestMethod]
        public void LockTableTest()
        {
        }

        [TestMethod]
        public void UnlockTableTest()
        {
        }
    }
}