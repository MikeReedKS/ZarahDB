// ***********************************************************************
// Assembly         : ZarahDB_WebAPITests
// Author           : Mike.Reed
// Created          : 03-26-2017
//
// Last Modified By : Mike.Reed
// Last Modified On : 03-26-2017
// ***********************************************************************
// <copyright file="InstanceControllerTests.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZarahDB_WebAPI.Controllers.Tests
{
    /// <summary>
    ///     Class InstanceControllerTests.
    /// </summary>
    [TestClass]
    public class InstanceControllerTests
    {
        /// <summary>
        ///     Bases the tests instance.
        /// </summary>
        [TestMethod]
        public void GeneralTest_Instance()
        {
            Debug.WriteLine("Creating an instance of the Instance API controller.");
            var instanceController = new InstanceController();

            var testInstanceName = "zdb_APITests";
            Debug.WriteLine($"Setting test instance to: {testInstanceName}.");

            Debug.WriteLine("Getting list of allowed instances which already exist.");
            var resultGetInstance = instanceController.GetInstance();
            Assert.AreEqual(resultGetInstance.Status, "200", "Failed to GetInstance.");
            Debug.WriteLine("Instances:");
            foreach (var instance in resultGetInstance.List)
            {
                Debug.WriteLine($"{instance}.");
            }

            Debug.WriteLine("See if the test instance exists, if so, delete it.");
            var resultExistsInstance = instanceController.ExistsInstance(testInstanceName);
            Assert.AreEqual(resultExistsInstance.Status, "200", "Failed to GetInstance.");
            if (resultExistsInstance.Value == "True")
            {
                Debug.WriteLine("Assuse the instance isn't locked.");
                var resultUnlockInstanceBeforeDelete = instanceController.UnlockInstance(testInstanceName);
                Assert.AreEqual(resultUnlockInstanceBeforeDelete.Status, "200", "Failed to UnlockInstance.");

                Debug.WriteLine($"Delete instance: {testInstanceName}.");
                var resultDeleteInstance = instanceController.DeleteInstance(testInstanceName);
                Assert.AreEqual(resultDeleteInstance.Status, "200", "Failed to DeleteInstance.");
            }

            Debug.WriteLine($"Creating Instance {testInstanceName}.");
            var resultCreateInstance = instanceController.CreateInstance(testInstanceName);
            Assert.AreEqual(resultCreateInstance.Status, "200", "Failed to CreateInstance.");

            Debug.WriteLine("Locking instance.");
            var resultLockInstance = instanceController.LockInstance(testInstanceName);
            Assert.AreEqual(resultLockInstance.Status, "200", "Failed to LockInstance.");

            Debug.WriteLine("Checking instance lock status.");
            var resultGetInstanceLock = instanceController.GetInstanceLock(testInstanceName);
            Assert.AreEqual(resultGetInstanceLock.Status, "200", "Failed to GetInstanceLock.");
            Assert.AreEqual(resultGetInstanceLock.Value, "True", "Failed to LockInstance.");

            var resultMaxDepthInstance = instanceController.MaxDepthInstance(testInstanceName, "", 7);
            Assert.AreEqual(resultMaxDepthInstance.Status, "200", "Failed to MaxDepthInstance.");

            var resultUnlockInstance = instanceController.UnlockInstance(testInstanceName);
            Assert.AreEqual(resultUnlockInstance.Status, "200", "Failed to UnlockInstance.");

            var resultBackupInstance = instanceController.BackupInstance(testInstanceName);
            Assert.AreEqual(resultBackupInstance.Status, "200", "Failed to BackupInstance.");

            var resultRestoreInstance = instanceController.RestoreInstance(testInstanceName, resultBackupInstance.Value);
            Assert.AreEqual(resultRestoreInstance.Status, "200", "Failed to RestoreInstance.");
        }
    }
}