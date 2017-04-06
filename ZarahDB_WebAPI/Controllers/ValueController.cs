// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 09-10-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-10-2016
// ***********************************************************************
// <copyright file="ValueController.cs" company="Benchmark Solutions LLC">
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
    ///     Class ValueController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ValueController : ApiController
    {
        /// <summary>
        ///     Gets the value associated with a single column.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="column">The column.</param>
        /// <returns>StatusKeyColumnValues</returns>
        [AcceptVerbs("GET")]
        [Route("Value")]
        public StatusKeyColumnValue GetValue(
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] string key,
            [FromUri] string column)
        {
            var statusKeyColumnValue = new StatusKeyColumnValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusKeyColumnValue))
                return statusKeyColumnValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusKeyColumnValue)) return statusKeyColumnValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            if (table.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                table = null;
            }
            if (key.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                key = null;
            }

            //Call the method
            try
            {
                statusKeyColumnValue = ZarahDB.GetValue(instance == null ? null : new Uri(instance), table, key, column);
            }
            catch (Exception ex)
            {
                statusKeyColumnValue = StatusHelper.SetStatusKeyColumnValue(ex.HResult.ToString(), ex.Message, "");
            }
            return statusKeyColumnValue;
        }

        /// <summary>
        ///     Gets the values associated with a list of columns for a single key.
        /// </summary>
        /// <param name="getColumnsDto">The getColumnsDto.</param>
        /// <returns>StatusMessageValue</returns>
        [AcceptVerbs("GET")]
        [Route("Values")]
        public StatusKeyColumnValues GetValues(
            [FromBody] GetColumnsDto getColumnsDto)
        {
            //TODO: Migrate to SecurityHelper
            if (getColumnsDto.Instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                getColumnsDto.Instance = null;
            }
            string localPath = null;
            if (getColumnsDto.Instance != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, getColumnsDto.Instance);
            }
            if (getColumnsDto.Table.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                getColumnsDto.Table = null;
            }
            if (getColumnsDto.Key.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                getColumnsDto.Key = null;
            }

            var statusKeyColumnValues = new StatusKeyColumnValues();
            try
            {
                statusKeyColumnValues = ZarahDB.Get(localPath == null ? null : new Uri(localPath), getColumnsDto.Table,
                    getColumnsDto.Key, getColumnsDto.Columns);
            }
            catch (Exception ex)
            {
                statusKeyColumnValues.Status = ex.HResult.ToString();
                statusKeyColumnValues.Message = ex.Message;
                //TODO: Mike: Work: Create status helper for this
                //StatusHelper. .SetStatusKeyColumnValuesStatus(statusMessageValue, ex.HResult.ToString(), ex.Message);
            }

            return statusKeyColumnValues;
        }

        /// <summary>
        ///     Stores a single value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusValue</returns>
        [AcceptVerbs("POST")]
        [Route("Value")]
        public StatusMessageValue PutValue(
            [FromUri] string instance,
            [FromUri] string table,
            [FromUri] string key,
            [FromUri] string column,
            [FromUri] string value,
            [FromUri] int? timeoutSeconds = null)
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
                statusMessageValue = ZarahDB.Put(new Uri(instance), table, key, column, value, timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }
            return statusMessageValue;
        }

        /// <summary>
        ///     Stores any number of values across any number of keys.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="statusKeysColumnValues">The put values dto.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusValue</returns>
        [AcceptVerbs("POST")]
        [Route("Values")]
        public StatusMessageValue PutValues(
            [FromUri] string instance,
            [FromUri] string table,
            [FromBody] StatusKeysColumnValues statusKeysColumnValues,
            [FromUri] int? timeoutSeconds = null)
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
                statusMessageValue = ZarahDB.Put(new Uri(instance), table, statusKeysColumnValues.KeysColumnValues,
                    timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }
            return statusMessageValue;
        }
    }
}