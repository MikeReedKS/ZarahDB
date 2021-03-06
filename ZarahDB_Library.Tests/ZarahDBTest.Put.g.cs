using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ZarahDB_Library.Tests
{
    public partial class ZarahDBTest
    {
        [TestMethod]
        public void PutValidTestKey()
        {
            var s = Put(null, null, "Test Key", "Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutValidTableTestKey()
        {
            var s = Put(null, "Test Table", "Test Key", "Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutTwoValidTestKeys()
        {
            var s = Put(null, null, "Test Key", "Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
            s = Put(null, null, "Test Key", "Test Column 2", "Test Value 2");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutFourValidTestKeys()
        {
            var s = Put(null, "Test Table", "Test Key", "Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
            s = Put(null, "Test Table", "Test Key", "Second Test Column", "Test Value 2");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
            s = Put(null, "Test Table", "Test Key", "Third Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
            s = Put(null, "Test Table", "Test Key", "Test Column", "Updated Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutJsonValue()
        {
            var testList = new List<string> {@"|,/n\n>}{[]}", "2nd Entry", "3nd Entry"};
            var json = JsonConvert.SerializeObject(testList);
            json = "{ " + $"\"Values\":{json}}}";
            var nestedTestList = new List<string> {json, "2nd outer value", "3rd outer value"};
            var jsonValue = JsonConvert.SerializeObject(nestedTestList);
            var s = Put(null, "Test Table", "Test Key", "Test Column", jsonValue);
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutAllNullsButWithEmptyKey()
        {
            var s = Put(null, null, "", null, null);
            if (s.Status != "500")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutKeyWithBunchOfBackspacesAndACoupleOfZs()
        {
            var s = Put(null, null, "\b\b\bZ\b\b\b\bZ", "Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutAllNulls()
        {
            var s = Put(null, null, null, null, null);
            if (s.Status != "500")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutEmptyKey()
        {
            var s = Put(null, null, "", "Test Column", "Test Value");
            if (s.Status == "200")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutZeroKey()
        {
            var s = Put(null, null, "\0", "Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutExtendedCharacters()
        {
            var uri = new Uri(@"C:\Ā_zdb_test_instance");
            var s = Put(uri, "©Test Table", "®Test Key", "¡TestColumn", "ÖTest Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
        }

        [TestMethod]
        public void PutMultipleIllegalCharacterKey()
        {
            var s = Put(null, null, "\0\0  \0\0\0E", "        ", null);
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }
        }
    }
}