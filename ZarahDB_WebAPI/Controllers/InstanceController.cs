// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 04-09-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-09-2016
// ***********************************************************************
// <copyright file="InstanceController.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Configuration;
using System.IO;
using System.Linq;
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
    /// Class InstanceController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class InstanceController : ApiController
    {
        /// <summary>
        /// Deletes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        /// <response code="200">OK - May contain an HTML response if IIS rejects the call.</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <response code="597">Network timeout error (Persistent Lock for more than {timeoutSeconds} seconds.)</response>
        /// <remarks>Deletes the specified instance. Any backup contained in the instance is also deleted. Be sure to take a copy of the backup before
        /// you delete the instance. A locked instance must be unlocked before it can be deleted.</remarks>
        [AcceptVerbs("DELETE")]
        [Route("Instance")]
        public StatusMessageValue DeleteInstance(
            [FromUri] string instance,
            [FromUri] int? timeoutSeconds = null)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                statusMessageValue = ZarahDB.DeleteInstance(new Uri(instance), timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            return statusMessageValue;
        }

        /// <summary>
        /// Gets a list of instances names.
        /// </summary>
        /// <returns>StatusList.</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <remarks>Gets a list of instances names. Only permitted instances are shown.</remarks>
        [AcceptVerbs("GET")]
        [Route("Instance")]
        public StatusList GetInstance()
        {
            var statusList = new StatusList();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusList)) return statusList;

            //Call the method
            try
            {
                var allowedInstances = ConfigurationManager.AppSettings["AllowedInstances"];
                var instancesRootFolder = ConfigurationManager.AppSettings["InstancesRootFolder"];

                statusList = ZarahDB.ListInstance(instancesRootFolder);

                if (allowedInstances != "*")
                {
                    var instancesAllowed = allowedInstances.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    statusList.List =  instancesAllowed.ToList().OrderBy(inst => inst).ToList();
                    statusList.Value = statusList.List.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                statusList = StatusHelper.SetStatusList(ex.HResult.ToString(), ex.Message, "");
            }

            return statusList;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>StatusMessageValue</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <remarks>Creates a new empty instance. An instance represents the highest level of data collection, and is physically 
        /// the file system folder where all tables (sub-folder) will be created and maintained. Backups are done at the instance
        /// level, as are restores.</remarks>
        [AcceptVerbs("POST")]
        [Route("Instance")]
        public StatusMessageValue CreateInstance(
            string instance)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            var instanceName = instance;
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                var oldValue = ConfigurationManager.AppSettings["AllowedInstances"];

                statusMessageValue = ZarahDB.CreateInstance(new Uri(instance));

                if (oldValue != "*")
                {
                    if (oldValue == instanceName)
                    {
                        return statusMessageValue;
                    }
                    var found = false;
                    if (oldValue.Contains(','))
                    {
                        var instanceList = oldValue.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);
                        found = instanceList.Any(allowedInstance => allowedInstance == instanceName);
                    }
                    if (statusMessageValue.Status == "200")
                    {
                        if (!found)
                        {
                            var newValue = $"{oldValue},{instanceName}";
                            ConfigurationManager.AppSettings["AllowedInstances"] = newValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            return statusMessageValue;
        }

        /// <summary>
        /// Creates a backup of the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusMessageValue</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <response code="597">Network timeout error (Persistent Lock for more than {timeoutSeconds} seconds.)</response>
        /// <remarks>Creates a .zip file containing the full contents of an instance. Only one backup can exist in the instance at a time, so
        /// before creating a new backup, download or move the existing backup file. The return value contains the filename of the
        /// backup file. The backup file will exist in the root of the instance, which is configured in the web.config file as the
        /// InstancesRootFolder in the settings section of the configuration section. The instance is locked during the backup.</remarks>
        [AcceptVerbs("POST")]
        [Route("Instance/Backup")]
        public StatusMessageValue BackupInstance(
            [FromUri] string instance,
            [FromUri] int? timeoutSeconds = null)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                statusMessageValue = ZarahDB.Backup(new Uri(instance), timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            //Adjust the response to return just the backup name and not the path
            statusMessageValue.Value = Path.GetFileName(statusMessageValue.Value);

            return statusMessageValue;
        }

        /// <summary>
        /// Sets the default maximum folder depth for the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="maxDepth">The maximum depth.</param>
        /// <returns>StatusMessageValue</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <remarks>Sets the default maximum folder depth for the instance. As row files are scattered, they will never scatter
        /// further than the number of sub-folders given from the top of the table. By default, the  maximum folder depth for the instance is 5.
        /// Once any data is written to a table, the maximum folder depth for the table is set, based on the default for the instance. Changing 
        /// the instance default only affects new tables, before any data is written to them. It does not affect tables which have ever contained data
        /// or where the maximum folder depth for the table has ever been set.
        /// </remarks>
        [AcceptVerbs("POST")]
        [Route("Instance/MaxDepth")]
        public StatusMessageValue MaxDepthInstance(
            [FromUri] string instance,
            [FromUri] string table = "",
            [FromUri] int maxDepth = 5)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;
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
        /// Restores the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="backupFile">The backup file.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusMessageValue</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <response code="597">Network timeout error (Persistent Lock for more than {timeoutSeconds} seconds.)</response>
        /// <remarks>Restores the files of an instance based on the contents of a backup .zip file. The restore will overwrite any data
        /// already existing in the instance. Rows are replaced, but rows which do not exist in the backup are unaffected.
        /// To avoid this, typically the instance is Deleted prior to a restore. This assures that the instance will exactly match
        /// the backup image once the restore is complete. The instance is locked during the restore.</remarks>
        [AcceptVerbs("PUT")]
        [Route("Instance/Restore")]
        public StatusMessageValue RestoreInstance(
            [FromUri] string instance,
            [FromUri] string backupFile,
            [FromUri] int? timeoutSeconds = null)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Update backup file to a physical path based on InstancesRootFolder
            var backupFileLocalPath = Path.Combine(instance, backupFile);

            //Call the method
            try
            {
                statusMessageValue = ZarahDB.Restore(new Uri(instance), new Uri(backupFileLocalPath), timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            //Adjust the response to return just the backup name and not the path
            statusMessageValue.Value = Path.GetFileName(statusMessageValue.Value);

            return statusMessageValue;
        }

        /// <summary>
        /// Locks the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <remarks>Locks the instance. A locked instance is read-only. Use Instance/Unlock to restore read-write access.</remarks>
        [AcceptVerbs("PUT")]
        [Route("Instance/Lock")]
        public StatusMessageValue LockInstance(
            [FromUri] string instance)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    instance = null;
                }
                string localPath = null;
                if (instance != null)
                {
                    try
                    {
                        localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
                    }
                    catch
                    {
                        localPath = instance;
                    }
                }
                ZarahDB.LockInstance(localPath == null ? null : new Uri(localPath));
                statusMessageValue = StatusHelper.SetStatusMessageValue(ZarahDB_Library.Enums.StatusCode.OK);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            return statusMessageValue;
        }
        /// <summary>
        /// Gets the status of an instance lock.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>StatusMessageValue.</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <remarks>Locks the instance. A locked instance is read-only. Use Instance/Unlock to restore read-write access.</remarks>
        [AcceptVerbs("GET")]
        [Route("Instance/Lock")]
        public StatusMessageValue GetInstanceLock(
            [FromUri] string instance)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    instance = null;
                }
                string localPath = null;
                if (instance != null)
                {
                    try
                    {
                        localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
                    }
                    catch
                    {
                        localPath = instance;
                    }
                }

                statusMessageValue = ZarahDB.GetInstanceLock(localPath == null ? null : new Uri(localPath));
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            return statusMessageValue;
        }

        /// <summary>
        /// Unlocks the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <remarks>Unlocks the instance. An unlocked instance is read-write. Use Instance/Lock to set an instance to read-only access.</remarks>
        [AcceptVerbs("PUT")]
        [Route("Instance/Unlock")]
        public StatusMessageValue UnlockInstance(
            [FromUri] string instance)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    instance = null;
                }
                string localPath = null;
                if (instance != null)
                {
                    try
                    {
                        localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
                    }
                    catch
                    {
                        localPath = instance;
                    }
                }
                ZarahDB.UnlockInstance(localPath == null ? null : new Uri(localPath));
                statusMessageValue = StatusHelper.SetStatusMessageValue(ZarahDB_Library.Enums.StatusCode.OK);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            return statusMessageValue;
        }

        /// <summary>
        /// Test for the existence of a single instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Authorization has been denied for this request.</response>
        /// <response code="403">Forbidden. Instance disallowed by configuration. (HTTP Response Code is 200, Response Body Status is 403)</response>
        /// <response code="520">Exception. The message will contain the exception message.</response>
        /// <remarks>Test for the existence of a single instance. Only authorized instances are tested.</remarks>
        [AcceptVerbs("GET")]
        [Route("Instance/Exists")]
        public StatusMessageValue ExistsInstance(
            [FromUri] string instance)
        {
            var statusMessageValue = new StatusMessageValue();

            //Check security to be sure this method is allowed to execute
            if (!SecurityHelper.MethodAllowed(MethodBase.GetCurrentMethod().Name, ref statusMessageValue)) return statusMessageValue;
            if (!SecurityHelper.InstanceAllowed(instance, ref statusMessageValue)) return statusMessageValue;

            //Update instance to physical path based on InstancesRootFolder
            instance = WebHelper.GetInstancePath(instance);

            //Call the method
            try
            {
                var result = ZarahDB.Exists(new Uri(instance));
                statusMessageValue = StatusHelper.SetStatusMessageValue(ZarahDB_Library.Enums.StatusCode.OK);
                statusMessageValue.Value = result ? "True" : "False";
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }

            return statusMessageValue;
        }

    }
}
