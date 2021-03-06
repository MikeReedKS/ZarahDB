using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//TODO: Mike: Work: Pass back and accept as input the ticks associated with data, for offline locks. Don't update a column if the ticks < ticks on the update.

namespace ZarahDB_Library.Tests
{
    public partial class ZarahDBTest
    {
        [TestMethod]
        public void ExistsValidTestKey()
        {
            var s = Put(null, null, "Test Key", "Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }

            s = Put(null, null, "Test Key", "Second Test Column", "2");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }

            s = Put(null, null, "Test Key", "Another Test Column", "Three");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }

            s = Put(null, null, "Test Key", "Test Column", "Updated Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }

            var result = Exists(null, null, "Test Key");
            if (!result)
            {
                Assert.Fail(result.ToString());
            }
        }

        [TestMethod]
        public void ExistsInvalidTestKey()
        {
            var result = Exists(null, null, "Test Key That Does Not Exist - No test should make a key like this");
            if (result)
            {
                Assert.Fail(result.ToString());
            }
        }

        [TestMethod]
        public void ExistsBlankTable()
        {
            var b = Exists(null, "", null);
            Assert.AreEqual(false, b);
        }

        [TestMethod]
        public void ExistsEmptyKey()
        {
            var b = Exists(null, null, "");
            Assert.AreEqual(false, b);
        }

        [TestMethod]
        public void ExistsExtendedCharacterKey()
        {
            // ReSharper disable StringLiteralTypo
            var b = Exists(null, null,
                "脇䄣考㿤℣⇣℣℣考℣℣ⅅ考␦舃℣怣考聂耀耣℣℣℣䁈℣℣℣∄耄℣者∣䅄⅃聃㿣≈脣℣䀃∣⠃℣H℣⅃℣℣⇣℣≃ꀣ℣聃聃℣⊃⅃⅃⅃℣℣℣℣﻿");
            // ReSharper restore StringLiteralTypo
            Assert.AreEqual(false, b);
        }

        [TestMethod]
        public void ExistsValidInstance()
        {
            var uri = new Uri(@"c:\Ā_zdb_test_instance");
            var s = Put(uri, "Test Table", "Test Key", "Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }

            var b = Exists(uri);
            Assert.AreEqual(true, b);
        }

        [TestMethod]
        public void ExistsInvalidInstance()
        {
            var uri = new Uri(@"c:\Ā1234-DoesNotExitAndNeverShouldExist");
            var b = Exists(uri);
            Assert.AreEqual(false, b);
        }

        [TestMethod]
        public void ExistsValidTable()
        {
            var s = Put(null, "Test Table", "Test Key", "Test Column", "Test Value");
            if (s.Status != "200")
            {
                Assert.Fail(s.Value);
            }

            var b = Exists(null, "Test Table");
            Assert.AreEqual(true, b);
        }

        [TestMethod]
        public void ExistsInvalidTable()
        {
            var uri = new Uri(@"c:\Ā1234-DoesNotExitAndNeverShouldExist");
            var b = Exists(uri);
            Assert.AreEqual(false, b);
        }

        [TestMethod]
        public void BackupNull()
        {
            var statusValue = Backup(null, null);
            if (statusValue.Status != "200")
            {
                Assert.Fail(statusValue.Status);
            }
        }

        //[TestMethod]
        //public void CsvInsert()
        //{
        //    var instance = new Uri(@"C:\zdb\FacebookUsers_Insert");
        //    const string table = "FacebookUsers";
        //    SetMaxDepth(instance, table, 14);
        //    var maxDepth = Convert.ToInt32(GetMaxDepth(instance, table).Value);
        //    if (maxDepth != 14)
        //    {
        //        Assert.Fail("Max depth was not set correctly.");
        //    }

        //    //var csvFolder = new Uri(@"D:\FacebookUsers\1and1");
        //    var csvFolder = new Uri(@"D:\tfs\Source\ZarahDB\ZarahDB_Library.Tests01\CsvData");
        //    var columns = new List<string>
        //    {
        //        "First Name",
        //        "Sex",
        //        "FacebookId",
        //        "Last Name",
        //        "Page URL",
        //        "Language Code",
        //        "Full Name",
        //        "Facebook Username"
        //    };
        //    const string keyColumn = "FacebookId";
        //    const string fieldSeparator = "~*|*~";
        //    const string encloser = "";
        //    const string lineTerminator = "~**||**~";
        //    const string commentLineStarter = "The remote server returned an error:";
        //    var result = CsvInsert(csvFolder, columns, keyColumn, fieldSeparator, encloser, lineTerminator,
        //        commentLineStarter, instance, table);
        //    if (!result)
        //    {
        //        Assert.Fail("Failed");
        //    }
        //}

        //[TestMethod]
        //public void CsvFilePut1()
        //{
        //    var instance = new Uri(@"C:\zdb\FacebookUsers_FilePut");
        //    const string table = "FacebookUsers";
        //    SetMaxDepth(instance, table, 14);
        //    var maxDepth = GetMaxDepth(instance, table);
        //    if (maxDepth != 14)
        //    {
        //        Assert.Fail("Max depth was not set correctly.");
        //    }

        //    var csvFile = new Uri(@"D:\tfs\Source\ZarahDB\ZarahDB_Library.Tests01\CsvData\FacebookUsers 100001545003000 to 100001545003999.txt");
        //    var columns = new List<string>() { "First Name", "Sex", "FacebookId", "Last Name", "Page URL", "Language Code", "Full Name", "Facebook Username" };
        //    const string keyColumn = "FacebookId";
        //    const string fieldSeparator = "~*|*~";
        //    const string encloser = "";
        //    const string lineTerminator = "~**||**~";
        //    const string commentLineStarter = "The remote server returned an error:";
        //    const int timeoutSeconds = 30;
        //    var statusTransaction = CsvFilePut(csvFile, columns, keyColumn, fieldSeparator, encloser, lineTerminator, commentLineStarter, instance, table, timeoutSeconds);
        //    if (statusTransaction.Status != "200")
        //    {
        //        Assert.Fail(statusTransaction.Status);
        //    }
        //}

        //[TestMethod]
        //public void CsvFolderPut1()
        //{
        //    var instance = new Uri( @"C:\zdb\FacebookUsers_FolderPut");
        //    const string table = "FacebookUsers";
        //    SetMaxDepth(instance, table, 14);
        //    var maxDepth = GetMaxDepth(instance, table);
        //    if (maxDepth != 14)
        //    {
        //        Assert.Fail("Max depth was not set correctly.");
        //    }

        //    //var csvFolder = new Uri(@"D:\FacebookUsers\1and1");
        //    var csvFolder = new Uri(@"D:\tfs\Source\ZarahDB\ZarahDB_Library.Tests01\CsvData");
        //    var headers = new List<string> { "First Name", "Sex", "FacebookId", "Last Name", "Page URL", "Language Code", "Full Name", "Facebook Username" };
        //    const string keyColumn = "FacebookId";
        //    const string fieldSeparator = "~*|*~";
        //    const string encloser = "";
        //    const string lineTerminator = "~**||**~";
        //    const string commentLineStarter = "The remote server returned an error:";
        //    const int timeoutSeconds = 30;
        //    var statusTransaction = CsvFolderPut(csvFolder, headers, keyColumn, fieldSeparator, encloser, lineTerminator, commentLineStarter, instance, table, timeoutSeconds);
        //    if (statusTransaction.Status != "200")
        //    {
        //        Assert.Fail(statusTransaction.Status);
        //    }
        //}

        [TestMethod]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void TransactionSimple()
        {
            var script = @"
//Set the timeout value
Set(@TimeoutSeconds, 30);

//Backup
// How the instance was prior to running this script
Backup(@TimeoutSeconds);

//Delete
// Clean out any existing data so that we get a known result
//DeleteInstance(@TimeoutSeconds);

//Set the table and the key
Set(@Table, 'TransactionTest_Table_1');
Set(@Key, 'TransactionTest_Key 0');

//Put 10 values in one key
Put(@Table, @Key, 'Column 0', '0', @TimeoutSeconds);
Put(@Table, @Key, 'Column 1', '1', @TimeoutSeconds);
Put(@Table, @Key, 'Column 2', '2', @TimeoutSeconds);
Put(@Table, @Key, 'Column 3', '3', @TimeoutSeconds);
Put(@Table, @Key, 'Column 4', '4', @TimeoutSeconds);
Put(@Table, @Key, 'Column 5', '5', @TimeoutSeconds);
Put(@Table, @Key, 'Column 6', '6', @TimeoutSeconds);
Put(@Table, @Key, 'Column 7', '7', @TimeoutSeconds);
Put(@Table, @Key, 'Column 8', '8', @TimeoutSeconds);
Put(@Table, @Key, 'Column 9', '9', @TimeoutSeconds);
Put(@Table, @Key, 'Column A', 'A', @TimeoutSeconds);

//Delete Column A
//Delete(@Table, @Key, 'Column A', @TimeoutSeconds);

//Commit the above values
CommitTransaction(@TimeoutSeconds);

//Set the table and the column
Set(@Table, 'TransactionTest_Table_2');
Set(@Column, 'Value');

//Put 8 values in 8 different keys
// Using a fixed column name emulates simple Key/Value pairs
Put(@Table, 'A', @Column, '0', @TimeoutSeconds);
Put(@Table, 'BC', @Column, '1', @TimeoutSeconds);
Put(@Table, 'DEF', @Column, '2', @TimeoutSeconds);
Put(@Table, 'GHIJ', @Column, '3', @TimeoutSeconds);
Put(@Table, 'KLMNO', @Column, '4', @TimeoutSeconds);
Put(@Table, 'PQRSTU', @Column, '5', @TimeoutSeconds);
Put(@Table, 'VWXYZ12', @Column, '6', @TimeoutSeconds);
Put(@Table, '34567890', @Column, '7', @TimeoutSeconds);

//Commit the above values
CommitTransaction(@TimeoutSeconds);

//Put information about a few people
Set(@Table, 'Email');

Set(@Key, 'Mike.Reed@BenchmarkSolutionsLLC.net');
Put(@Table, @Key, 'Name', 'Mike Reed', @TimeoutSeconds);
Put(@Table, @Key, 'Company', 'Benchmark Solutions LLC', @TimeoutSeconds);
Put(@Table, @Key, 'Title', 'Sr. Developer', @TimeoutSeconds);

Set(@Key, 'Mike.Reed@ZarahDB.com');
Put(@Table, @Key, 'Name', 'Mike Reed', @TimeoutSeconds);
Put(@Table, @Key, 'Company', 'Benchmark Solutions LLC', @TimeoutSeconds);
Put(@Table, @Key, 'Title', 'Sr. Developer', @TimeoutSeconds);

Set(@Key, 'Mark@HisFictitiousCompany.com');
Put(@Table, @Key, 'Name', 'Mark O'ffill', @TimeoutSeconds);
Put(@Table, @Key, 'Company', 'His Fictitious Company LLC', @TimeoutSeconds);

Set(@Key, 'MLT@MuttonLettuceAndTomato.Sandwich');
Put(@Table, @Key, 'Name', 'Michael Thomas', @TimeoutSeconds);
Put(@Table, @Key, 'Company', 'Mutton Lettuce & Tomato Corp.', @TimeoutSeconds);
Put(@Table, @Key, 'Title', 'Primary Architect', @TimeoutSeconds);

Set(@Key, 'kkeezer@OMGLikeIdGiveYouMyRealEmail.net');
Put(@Table, @Key, 'Name', 'Kyle Keezer', @TimeoutSeconds);
Put(@Table, @Key, 'Title', 'Network Systems Administrator', @TimeoutSeconds);

Set(@Key, 'NotMyRealEmail@MobileMustangs.com');
Put(@Table, @Key, 'Name', 'Shawn Tillary', @TimeoutSeconds);
Put(@Table, @Key, 'Company', 'Mobile Mustangs', @TimeoutSeconds);
Put(@Table, @Key, 'Title', 'Owner', @TimeoutSeconds);

//Do a backup mid-transaction
// Should not contain uncommitted items in the instance data relating to this transaction
Backup(@TimeoutSeconds);

//Commit the above values
CommitTransaction(@TimeoutSeconds);

//Do a backup following the transaction
// Should contain all data relating to this transaction
Backup(@TimeoutSeconds);

//Get data from the Email table
Get(@Table, 'Mike.Reed@ZarahDB.com');
Get(@Table, 'MLT@MuttonLettuceAndTomato.Sandwich');
Get(@Table, 'kkeezer@OMGLikeIdGiveYouMyRealEmail.net');
";
            //TODO: Mike: Work: Added Delete command to script, caused a syntax error but still came back 200/OK.
            var instance = new Uri(@"C:\TransactionTest_Instance");
            var statusTransaction = Transaction(instance, script);
            if (statusTransaction.Status != "200")
            {
                Assert.Fail(statusTransaction.Status);
            }
        }
    }
}