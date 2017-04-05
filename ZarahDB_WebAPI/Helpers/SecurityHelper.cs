// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-18-2016
// ***********************************************************************
// <copyright file="SecurityHelper.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Configuration;
using System.Linq;
using ZarahDB_Library.Enums;
using ZarahDB_Library.Helpers;
using ZarahDB_Library.Types;

namespace ZarahDB_WebAPI.Helpers
{
    /// <summary>
    /// Class SecurityHelper.
    /// </summary>
    class SecurityHelper
    {
        /// <summary>
        /// Methods the allowed.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MethodAllowed(string methodName)
        {
            var methodAllowed = ConfigurationManager.AppSettings[methodName] ?? "false";
            return methodAllowed.Trim().ToLowerInvariant() == "true";
        }

        /// <summary>
        /// Methods the allowed.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="statusMessageValue">The status message value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MethodAllowed(string methodName, ref StatusMessageValue statusMessageValue)
        {
            var methodAllowed = ConfigurationManager.AppSettings[methodName] ?? "false";
            if (methodAllowed.Trim().ToLowerInvariant() == "true") return true;
            statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Forbidden);
            statusMessageValue.Value = "Method disabled by configuration.";
            if (statusMessageValue.Statistics == null)
            {
                statusMessageValue.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusMessageValue.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }

        /// <summary>
        /// Methods the allowed.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="statusKeyColumnValue">The status key value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MethodAllowed(string methodName, ref StatusKeyColumnValue statusKeyColumnValue)
        {
            var methodAllowed = ConfigurationManager.AppSettings[methodName] ?? "false";
            if (methodAllowed.Trim().ToLowerInvariant() == "true") return true;
            statusKeyColumnValue = StatusHelper.SetStatusKeyColumnValue(StatusCode.Forbidden);
            statusKeyColumnValue.Value = "Method disabled by configuration.";
            if (statusKeyColumnValue.Statistics == null)
            {
                statusKeyColumnValue.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusKeyColumnValue.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }

        /// <summary>
        /// Methods the allowed.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="statusList">The status list.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MethodAllowed(string methodName, ref StatusList statusList)
        {
            var methodAllowed = ConfigurationManager.AppSettings[methodName] ?? "false";
            if (methodAllowed.Trim().ToLowerInvariant() == "true") return true;
            statusList = StatusHelper.SetStatusList(StatusCode.Forbidden);
            statusList.Value = "Method disabled by configuration.";
            if (statusList.Statistics == null)
            {
                statusList.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusList.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }

        /// <summary>
        /// Methods the allowed.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="statusKeyColumnValues">The status key column values.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MethodAllowed(string methodName, ref StatusKeyColumnValues statusKeyColumnValues)
        {
            var methodAllowed = ConfigurationManager.AppSettings[methodName] ?? "false";
            if (methodAllowed.Trim().ToLowerInvariant() == "true") return true;
            statusKeyColumnValues = StatusHelper.SetStatusKeyColumnValues(StatusCode.Forbidden);
            statusKeyColumnValues.Value = "Method disabled by configuration.";
            if (statusKeyColumnValues.Statistics == null)
            {
                statusKeyColumnValues.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusKeyColumnValues.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }

        /// <summary>
        /// Methods the allowed.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MethodAllowed(string methodName, ref StatusTransaction statusTransaction)
        {
            var methodAllowed = ConfigurationManager.AppSettings[methodName] ?? "false";
            if (methodAllowed.Trim().ToLowerInvariant() == "true") return true;
            //TODO: Mike: Work: set the correct status to record the rejected method
            //statusTransaction = StatusHelper.SetStatusTransaction(StatusCode.Forbidden);
            //statusTransaction.Value = "Method disabled by configuration.";
            if (statusTransaction.Statistics == null)
            {
                statusTransaction.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusTransaction.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }

        /// <summary>
        /// Instances the allowed.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InstanceAllowed(string instance)
        {
            var instancesAllowedList = ConfigurationManager.AppSettings["AllowedInstances"] ?? "*";

            //Wildcard allows all instances
            if (instancesAllowedList.Equals("*"))
            {
                return true;
            }

            var instancesAllowed = instancesAllowedList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return instancesAllowed.Any() && instancesAllowed.Contains(instance);
        }

        /// <summary>
        /// Instances the allowed.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="statusMessageValue">The status message value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InstanceAllowed(string instance, ref StatusMessageValue statusMessageValue)
        {
            var instancesAllowedList = ConfigurationManager.AppSettings["AllowedInstances"] ?? "*";

            //Wildcard allows all instances
            if (instancesAllowedList.Equals("*"))
            {
                return true;
            }

            var instancesAllowed = instancesAllowedList.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (instancesAllowed.Any())
            {
                if (instancesAllowed.Contains(instance.ToLower())) return true;
            }

            statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Forbidden);
            statusMessageValue.Value = "Instance disallowed by configuration.";
            if (statusMessageValue.Statistics == null)
            {
                statusMessageValue.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusMessageValue.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }

        /// <summary>
        /// Instances the allowed.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="statusKeyColumnValue">The status key value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InstanceAllowed(string instance, ref StatusKeyColumnValue statusKeyColumnValue)
        {
            var instancesAllowedList = ConfigurationManager.AppSettings["AllowedInstances"] ?? "*";

            //Wildcard allows all instances
            if (instancesAllowedList.Equals("*"))
            {
                return true;
            }

            var instancesAllowed = instancesAllowedList.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (instancesAllowed.Any())
            {
                if (instancesAllowed.Contains(instance.ToLower())) return true;
            }

            statusKeyColumnValue = StatusHelper.SetStatusKeyColumnValue(StatusCode.Forbidden);
            statusKeyColumnValue.Value = "Instance disallowed by configuration.";
            if (statusKeyColumnValue.Statistics == null)
            {
                statusKeyColumnValue.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusKeyColumnValue.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }

        /// <summary>
        /// Instances the allowed.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="statusList">The status list.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InstanceAllowed(string instance, ref StatusList statusList)
        {
            var instancesAllowedList = ConfigurationManager.AppSettings["AllowedInstances"] ?? "*";

            //Wildcard allows all instances
            if (instancesAllowedList.Equals("*"))
            {
                return true;
            }

            var instancesAllowed = instancesAllowedList.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (instancesAllowed.Any())
            {
                if (instancesAllowed.Contains(instance.ToLower())) return true;
            }

            statusList = StatusHelper.SetStatusList(StatusCode.Forbidden);
            statusList.Value = "Instance disallowed by configuration.";
            if (statusList.Statistics == null)
            {
                statusList.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusList.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }

        /// <summary>
        /// Instances the allowed.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InstanceAllowed(string instance, ref StatusTransaction statusTransaction)
        {
            var instancesAllowedList = ConfigurationManager.AppSettings["AllowedInstances"] ?? "*";

            //Wildcard allows all instances
            if (instancesAllowedList.Equals("*"))
            {
                return true;
            }

            var instancesAllowed = instancesAllowedList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (instancesAllowed.Any())
            {
                if (instancesAllowed.Contains(instance)) return true;
            }

            statusTransaction = StatusHelper.SetStatusTransaction(StatusCode.Forbidden);
            statusTransaction.Value = "Instance disallowed by configuration.";
            if (statusTransaction.Statistics == null)
            {
                statusTransaction.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusTransaction.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }

        /// <summary>
        /// Instances the allowed.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="statusKeyColumnValues">The status key column values.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InstanceAllowed(string instance, ref StatusKeyColumnValues statusKeyColumnValues)
        {
            var instancesAllowedList = ConfigurationManager.AppSettings["AllowedInstances"] ?? "*";

            //Wildcard allows all instances
            if (instancesAllowedList.Equals("*"))
            {
                return true;
            }

            var instancesAllowed = instancesAllowedList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (instancesAllowed.Any())
            {
                if (instancesAllowed.Contains(instance)) return true;
            }

            //TODO: Mike: Work: Fix this!
            //statusKeyColumnValues = StatusHelper.SetStatusTransaction(StatusCode.Forbidden);
            statusKeyColumnValues.Value = "Instance disallowed by configuration.";
            if (statusKeyColumnValues.Statistics == null)
            {
                statusKeyColumnValues.Statistics = new Statistics();
            }
            var nowTicks = StringHelper.NowTicks();
            statusKeyColumnValues.Statistics = StatusHelper.FinalizeStats(nowTicks, nowTicks);
            return false;
        }
    }
}
