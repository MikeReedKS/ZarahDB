// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 09-10-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-10-2016
// ***********************************************************************
// <copyright file="KeyController.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
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
    ///     Class KeyController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class KeyController : ApiController
    {
        /// <summary>
        ///     Deletes a list of keys for the specified table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="keyList">The keys.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        [AcceptVerbs("DELETE")]
        [Route("Keys")]
        public bool DeleteKeys(
            [FromUri] string instance,
            [FromUri] string table,
            [FromBody] KeyList keyList,
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
                //TODO: Add a delete keys method
                result = ZarahDB.DeleteTable(localPath == null ? null : new Uri(localPath), table, timeoutSeconds);
            }
            catch
            {
                return false;
            }

            return result;
        }

        /// <summary>
        ///     Test for the existence of a single key.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="checkExactMatch">
        ///     Key collisions are possible. if set to <c>true</c> the file is checked to contain the
        ///     exact key if there is any chance of collision. Set this to <c>false</c> for the fastest execution when key
        ///     collisions are known not to exist.
        /// </param>
        /// <returns><c>true</c> if the key exists, <c>false</c> if it does not.</returns>
        /// <remarks>
        ///     One of the most powerful features of ZarahDB is the ability to load up huge numbers of samples and then see if they
        ///     exist.
        ///     Exists uses the speed of the disk subsystem to simply ask if a single file exists. This is extremely fast, even
        ///     with huge numbers
        ///     of records to compare. Does user X exist? Does word X exist? When those types of questions are needed, then using a
        ///     ZarahDB may be
        ///     your best option.
        /// </remarks>
        [AcceptVerbs("GET")]
        [Route("Key/Exists")]
        public StatusMessageValue ExistsKey(
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] string key,
            [FromUri] bool? checkExactMatch = true)
        {
            var statusMessageValue = new StatusMessageValue();

            var statistics = new Statistics();
            StatusHelper.SetStartTicks(statistics);

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue))
                return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                var result = ZarahDB.Exists(new Uri(instance), table, key, checkExactMatch);
                statusMessageValue = StatusHelper.SetStatusMessageValue(ZarahDB_Library.Enums.StatusCode.OK);
                statusMessageValue.Value = result ? "True" : "False";
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            statistics = StatusHelper.FinalizeStats(statistics.RequestedTicks.ToString(),
                statistics.StartTicks.ToString());
            statusMessageValue.Statistics = statistics;
            return statusMessageValue;
        }

        /// <summary>
        ///     Test for the existence of a list of keys, returning a the list of keys which exist.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="keyList">The keys.</param>
        /// <param name="checkExactMatch">
        ///     Key collisions are possible. if set to <c>true</c> the file is checked to contain the
        ///     exact key if there is any chance of collision. Set this to <c>false</c> for the fastest execution when key
        ///     collisions are known not to exist.
        /// </param>
        /// <returns>Keys</returns>
        /// <remarks>
        ///     Commonly used to validate a list of keys. Because no data is fetched, it can check a large number of
        ///     keys very quickly. Populate a table with a large number of names and pass in a list of names and this
        ///     will return all the names that already exist in the table.
        /// </remarks>
        [AcceptVerbs("GET")]
        [Route("Keys/Exists")]
        public KeyList ExistsKeys(
            [FromUri] string instance,
            [FromUri] string table,
            [FromBody] KeyList keyList,
            [FromUri] bool? checkExactMatch = true)
        {
            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name)) return new KeyList();
            if (!SecurityHelper.InstanceAllowed(instance)) return new KeyList();

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            var result = new KeyList();
            try
            {
                result = ZarahDB.Exists(new Uri(instance), table, keyList, checkExactMatch);
            }
            catch (Exception)
            {
                //Intentionally left Blank - On any exception, hide the exception and return an empty list
            }

            return result;
        }

        /// <summary>
        ///     Deletes the specified key.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        [AcceptVerbs("DELETE")]
        [Route("Key")]
        public bool DeleteKey(
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] string key,
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
                //TODO: Add a delete key method
                result = ZarahDB.DeleteTable(localPath == null ? null : new Uri(localPath), table, timeoutSeconds);
            }
            catch
            {
                return false;
            }

            return result;
        }

        /// <summary>
        ///     Gets all the values associated with a single key.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <returns>StatusKeyColumnValues</returns>
        [AcceptVerbs("GET")]
        [Route("Key")]
        public StatusKeyColumnValues GetKey(
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] string key)
        {
            var statusKeyColumnValues = new StatusKeyColumnValues();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusKeyColumnValues))
                return statusKeyColumnValues;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusKeyColumnValues)) return statusKeyColumnValues;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                statusKeyColumnValues = ZarahDB.Get(instance == null ? null : new Uri(instance), table, key);
            }
            catch (Exception ex)
            {
                statusKeyColumnValues.Status = ex.HResult.ToString();
                statusKeyColumnValues.Message = ex.Message;
                //TODO: Mike: Work: Create status helper for this
                //StatusHelper. .SetStatusKeyColumnValuesStatus(statusKeyColumnValues, ex.HResult.ToString(), ex.Message);
            }

            return statusKeyColumnValues;
        }

        /// <summary>
        ///     Gets all values associated with a list of keys.
        /// </summary>
        /// <param name="getKeysDto">The getColumnsDto.</param>
        /// <returns>StatusMessageValue</returns>
        [AcceptVerbs("GET")]
        [Route("Keys")]
        public StatusKeysColumnValues GetKeys(
            [FromBody] GetKeysDto getKeysDto)
        {
            //TODO: Migrate to SecurityHelper
            if (getKeysDto.Instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                getKeysDto.Instance = null;
            }
            string localPath = null;
            if (getKeysDto.Instance != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, getKeysDto.Instance);
            }
            if (getKeysDto.Table.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                getKeysDto.Table = null;
            }

            var statusKeysColumnValues = new StatusKeysColumnValues();
            try
            {
                statusKeysColumnValues = ZarahDB.Get(localPath == null ? null : new Uri(localPath), getKeysDto.Table,
                    getKeysDto.Keys);
            }
            catch (Exception ex)
            {
                statusKeysColumnValues.Status = ex.HResult.ToString();
                statusKeysColumnValues.Message = ex.Message;
                //TODO: Mike: Work: Create status helper for this
                //StatusHelper. .SetStatusKeyColumnValuesStatus(statusMessageValue, ex.HResult.ToString(), ex.Message);
            }

            return statusKeysColumnValues;
        }
    }
}