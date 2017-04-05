using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZarahDB_WebAPI.Controllers.Tests
{
    [TestClass()]
    public class TableControllerTests
    {
        [TestMethod()]
        public void CsvPutTest()
        {

        }

        [TestMethod()]
        public void CsvFilePutTest()
        {

        }

        [TestMethod()]
        public void CsvFolderPutTest()
        {

        }

        [TestMethod()]
        public void DeleteTableTest()
        {

        }

        [TestMethod()]
        public void ListTableTest()
        {
            Debug.WriteLine("Creating an instance of the Instance API controller.");
            var tableController = new TableController();

            var testInstanceName = "zdb_APITests";
            Debug.WriteLine($"Setting test instance to: {testInstanceName}.");

            var resultListTable = tableController.ListTable(testInstanceName);

        }

        [TestMethod()]
        public void ExistsTableTest()
        {

        }

        [TestMethod()]
        public void LockTableTest()
        {

        }

        [TestMethod()]
        public void UnlockTableTest()
        {

        }
    }
}