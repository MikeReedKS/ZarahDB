using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ZarahDB_Library.Tests
{
    public partial class ZarahDBTest
    {
        [TestMethod]
        public void GetNull()
        {
            var statusKeyColumnValues = Get(null, null, "​");
            if (statusKeyColumnValues.Status != "0")
            {
                Assert.Fail(statusKeyColumnValues.Status);
            }
        }

        [TestMethod]
        public void GetTestKey()
        {
            var statusKeyColumnValues = Get(null, null, "Test Key");
            if (statusKeyColumnValues.Status != "200")
            {
                Assert.Fail(statusKeyColumnValues.Status);
            }
        }

        [TestMethod]
        public void GetTestKeyColumnListStar()
        {
            var statusKeyColumnValues = Get(null, "Test Table", "Test Key", new List<string> {"*"});
            if (statusKeyColumnValues.Status != "200")
            {
                Assert.Fail(statusKeyColumnValues.Status);
            }
        }

        [TestMethod]
        public void GetJsonValue()
        {
            var testList = new List<string> {@"|,/n\n>}{[]}", "2nd Entry", "3nd Entry"};
            var json = JsonConvert.SerializeObject(testList);
            json = "{ " + $"\"Values\":{json}}}";
            var nestedTestList = new List<string> {json, "2nd outer value", "3rd outer value"};
            var jsonValue = JsonConvert.SerializeObject(nestedTestList);

            //Put the value to read in the DB
            var resultPut = Put(null, "Test Table", "Test Key", "Test Column", jsonValue);
            if (resultPut.Status != "200")
            {
                Assert.Fail(resultPut.Value);
            }

            //Read the JSON back from the DB
            var resultGet = Get(null, "Test Table", "Test Key");
            if (resultGet.Status != "200")
            {
                Assert.Fail(resultGet.Message);
            }

            if (!resultGet.ColumnValues.Any())
            {
                Assert.Fail("No value returned.");
            }
            if (!resultGet.ColumnValues["Test Column"].Equals(jsonValue))
            {
                Assert.Fail("Value retrieved did not match original value.");
            }
        }
    }
}