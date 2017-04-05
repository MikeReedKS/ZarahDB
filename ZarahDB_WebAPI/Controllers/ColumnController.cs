// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 09-10-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-10-2016
// ***********************************************************************
// <copyright file="ColumnController.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Web;
using System.Web.Http;

namespace ZarahDB_WebAPI.Controllers
{
    /// <summary>
    /// Class ColumnController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ColumnController : ApiController
    {
        /// <summary>
        /// Deletes an index for the specified column.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="column">The column.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        [AcceptVerbs("DELETE")]
        [Route("Index")]
        public bool DeleteIndex(
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] string column,
            [FromUri] int? timeoutSeconds = null)
        {
            //TODO: Migrate to SecurityHelper
            if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                instance = null;
            }
            string localPath = null;
            if (instance != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
            }

            bool result;
            try
            {
                //TODO: Add a delete index method
                result = false;
            }
            catch
            {
                return false;
            }

            return result;
        }
        
        /// <summary>
        /// Creates an index for the specified column.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="column">The column.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        [AcceptVerbs("POST")]
        [Route("Index")]
        public bool CreateIndex(
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] string column,
            [FromUri] int? timeoutSeconds = null)
        {
            //TODO: Migrate to SecurityHelper
            if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                instance = null;
            }
            string localPath = null;
            if (instance != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
            }

            bool result;
            try
            {
                //TODO: Add a create index method
                result = false;
            }
            catch
            {
                return false;
            }

            return result;
        }
    }
}
