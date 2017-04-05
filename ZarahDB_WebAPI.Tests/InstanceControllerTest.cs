// <copyright file="InstanceControllerTest.cs">Copyright ©  2015</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZarahDB_Library;
using ZarahDB_WebAPI.Controllers;

namespace ZarahDB_WebAPI.Controllers.Tests
{
    /// <summary>This class contains parameterized unit tests for InstanceController</summary>
    [PexClass(typeof(InstanceController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class InstanceControllerTest
    {
        /// <summary>Test stub for DeleteInstance(String, Nullable`1&lt;Int32&gt;)</summary>
        [PexMethod]
        public StatusMessageValue DeleteInstanceTest(
            [PexAssumeUnderTest]InstanceController target,
            string instance,
            int? timeoutSeconds
        )
        {
            StatusMessageValue result = target.DeleteInstance(instance, timeoutSeconds);
            Assert.AreEqual(result,0);
            return result;
            // TODO: add assertions to method InstanceControllerTest.DeleteInstanceTest(InstanceController, String, Nullable`1<Int32>)
        }
    }
}
