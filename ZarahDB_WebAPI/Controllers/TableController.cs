// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 09-10-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-16-2016
// ***********************************************************************
// <copyright file="TableController.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Http;
using ZarahDB_Library;
using ZarahDB_Library.Helpers;
using ZarahDB_Library.Types;
using ZarahDB_WebAPI.Helpers;

namespace ZarahDB_WebAPI.Controllers
{
    /// <summary>
    ///     Class TableController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TableController : ApiController
    {
        /// <summary>
        ///     Import CSV data directly (data is passed in the call, no file required).
        /// </summary>
        /// <param name="csvData">The CSV data.</param>
        /// <param name="timeoutSeconds">The timeout seconds. (optional) Defaults to 30 seconds.</param>
        /// <returns>StatusTransaction</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">
        ///     Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status
        ///     is 403)
        /// </response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <response code="597">Network timeout error (Persistent Lock for more than {timeoutSeconds} seconds.)</response>
        /// <remarks>
        ///     Imports CSV data directly. This allows the client to post CSV data directly without the need to upload a CSV file.
        ///     The import bypasses the transaction system to allow the fastest possible addition of new data, but may result in
        ///     concurrency challenges.
        ///     It is advised that the instance be locked prior to issuing this request if there is a chance that other clients may
        ///     perform
        ///     other operations during the import. Any existing key/column/values may be overwritten with the CSV data. Existing
        ///     data that
        ///     is not overwritten with new data is untouched.
        /// </remarks>
        [AcceptVerbs("PUT")]
        [Route("Table/CSV")]
        public StatusMessageValue CsvPut(
            [FromBody] CsvDataDto csvData,
            [FromUri] int? timeoutSeconds = null)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue))
                return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(csvData.Instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            csvData.Instance = WebHelper.GetInstancePath(csvData.Instance);

            //Call the method
            try
            {
                statusMessageValue = ZarahDB.CsvPut(csvData.CSVData, csvData.Columns, csvData.KeyColumn,
                    csvData.FieldSeparator, csvData.Encloser, csvData.LineTerminator,
                    csvData.CommentLineStarter, new Uri(csvData.Instance), csvData.Table, timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ZarahDB_Library.Enums.StatusCode.Exception,
                    ex.Message);
            }

            //Adjust the response to return just the backup name and not the path
            statusMessageValue.Value = Path.GetFileName(statusMessageValue.Value);

            return statusMessageValue;
        }

        /// <summary>
        ///     Sets the default maximum folder depth for the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="maxDepth">The maximum depth.</param>
        /// <returns>StatusMessageValue</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">
        ///     Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status
        ///     is 403)
        /// </response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <remarks>
        ///     Sets the default maximum folder depth for the instance. As row files are scattered, they will never scatter
        ///     further than the number of sub-folders given from the top of the table. By default, the  maximum folder depth for
        ///     the instance is 5.
        ///     Once any data is written to a table, the maximum folder depth for the table is set, based on the default for the
        ///     instance. Changing
        ///     the instance default only affects new tables, before any data is written to them. It does not affect tables which
        ///     have ever contained data
        ///     or where the maximum folder depth for the table has ever been set.
        /// </remarks>
        [AcceptVerbs("POST")]
        [Route("Instance/MaxDepth")]
        public StatusMessageValue MaxDepth(
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] int maxDepth = 5)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue))
                return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                statusMessageValue = ZarahDB.SetMaxDepth(new Uri(instance), table, maxDepth);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            return statusMessageValue;
        }
        
        /// <summary>
                 ///     Import a single CSV file directly.
                 /// </summary>
                 /// <param name="csvFile">The CSV file.</param>
                 /// <param name="columns">The columns.</param>
                 /// <param name="keyColumn">The key column.</param>
                 /// <param name="fieldSeparator">The field separator.</param>
                 /// <param name="encloser">The encloser.</param>
                 /// <param name="lineTerminator">The line terminator.</param>
                 /// <param name="commentLineStarter">The comment line starter.</param>
                 /// <param name="instance">The instance.</param>
                 /// <param name="table">The table.</param>
                 /// <param name="timeoutSeconds">The timeout seconds.</param>
                 /// <returns>StatusTransaction</returns>
                 /// <remarks>
                 ///     Import a single CSV file.
                 ///     The import bypasses the transaction system to allow the fastest possible addition of new data, but may result in
                 ///     concurrency challenges.
                 ///     It is advised that the instance be locked prior to issuing this request if there is a chance that other clients may
                 ///     perform
                 ///     other operations during the import. Any existing key/column/values may be overwritten with the CSV data. Existing
                 ///     data that
                 ///     is not overwritten with new data is untouched.
                 /// </remarks>
        [AcceptVerbs("PUT")]
        [Route("Table/CSV/File")]
        public StatusMessageValue CsvFilePut(
            [FromUri] string csvFile,
            [FromBody] List<string> columns,
            [FromUri] string keyColumn,
            [FromUri] string fieldSeparator,
            [FromUri] string encloser,
            [FromUri] string lineTerminator,
            [FromUri] string commentLineStarter,
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] int? timeoutSeconds = null)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue))
                return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            if (csvFile.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                csvFile = null;
            }
            string localPath = null;
            if (csvFile != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, csvFile);
            }

            //Call the method
            try
            {
                statusMessageValue = ZarahDB.CsvFilePut(localPath == null ? null : new Uri(localPath), columns,
                    keyColumn, fieldSeparator, encloser, lineTerminator,
                    commentLineStarter, localPath == null ? null : new Uri(localPath), table, timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ZarahDB_Library.Enums.StatusCode.Exception,
                    ex.Message);
            }

            //Adjust the response to return just the backup name and not the path
            statusMessageValue.Value = Path.GetFileName(statusMessageValue.Value);

            return statusMessageValue;
        }

        /// <summary>
        ///     Import all the CSV files in a single folder.
        /// </summary>
        /// <param name="csvFolder">The CSV folder.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="keyColumn">The key column.</param>
        /// <param name="fieldSeparator">The field separator.</param>
        /// <param name="encloser">The encloser.</param>
        /// <param name="lineTerminator">The line terminator.</param>
        /// <param name="commentLineStarter">The comment line starter.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusTransaction</returns>
        [AcceptVerbs("PUT")]
        [Route("Table/CSV/Folder")]
        public StatusMessageValue CsvFolderPut(
            [FromUri] string csvFolder,
            [FromBody] List<string> columns,
            [FromUri] string keyColumn,
            [FromUri] string fieldSeparator,
            [FromUri] string encloser,
            [FromUri] string lineTerminator,
            [FromUri] string commentLineStarter,
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] int? timeoutSeconds = null)
        {
            //TODO: Migrate to SecurityHelper
            if (csvFolder.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                csvFolder = null;
            }
            string csvLocalPath = null;
            if (csvFolder != null)
            {
                csvLocalPath = Path.Combine(HttpRuntime.AppDomainAppPath, csvFolder);
            }

            if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                instance = null;
            }
            string localPath = null;
            if (instance != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
            }

            StatusMessageValue statusMessageValue;
            try
            {
                statusMessageValue = ZarahDB.CsvFolderPut(csvLocalPath == null ? null : new Uri(csvLocalPath), columns,
                    keyColumn, fieldSeparator, encloser, lineTerminator,
                    commentLineStarter, localPath == null ? null : new Uri(localPath), table, timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ZarahDB_Library.Enums.StatusCode.Exception,
                    ex.Message);
            }

            return statusMessageValue;
        }

        /// <summary>
        /// Deletes the specified table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        [AcceptVerbs("DELETE")]
        [Route("Table")]
        public StatusMessageValue DeleteTable(
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] int? timeoutSeconds = null)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                instance = null;
            }

            //Update instance to physical path based on InstancesRootFolder
            var localPath = WebHelper.GetInstancePath(instance);

            try
            {
                statusMessageValue = ZarahDB.DeleteTable(localPath == null ? null : new Uri(localPath), table, timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");

            }

            return statusMessageValue;
        }

        /// <summary>
        ///     Lists the tables in the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>StatusList.</returns>
        [AcceptVerbs("GET")]
        [Route("Table")]
        public StatusList ListTable(
            [FromUri] string instance)
        {
            var statusList = new StatusList();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusList)) return statusList;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusList)) return statusList;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                statusList = ZarahDB.ListTable(new Uri(instance));
            }
            catch (Exception ex)
            {
                statusList = StatusHelper.SetStatusList(ex.HResult.ToString(), ex.Message, "");
            }

            return statusList;
        }

        /// <summary>
        ///     Test for the existence of a single table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        [AcceptVerbs("GET")]
        [Route("Table/Exists")]
        public StatusMessageValue ExistsTable(
            [FromUri] string instance,
            [FromUri] string table)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue))
                return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                var result = ZarahDB.Exists(new Uri(instance), table);
                statusMessageValue = StatusHelper.SetStatusMessageValue(ZarahDB_Library.Enums.StatusCode.OK);
                statusMessageValue.Value = result ? "True" : "False";
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            return statusMessageValue;
        }

        /// <summary>
        ///     Locks a table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        [AcceptVerbs("PUT")]
        [Route("Table/Lock")]
        public void LockTable(
            [FromUri] string instance, [FromUri] string table)
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

            ZarahDB.LockTable(localPath == null ? null : new Uri(localPath), table);
        }

        /// <summary>
        ///     Unlocks a table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        [AcceptVerbs("PUT")]
        [Route("Table/Unlock")]
        public void UnlockTable(
            [FromUri] string instance, [FromUri] string table)
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

            ZarahDB.UnlockTable(localPath == null ? null : new Uri(localPath), table);
        }
    }
}