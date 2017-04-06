// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="ZarahDBModel.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Reflection;
using ZarahDB_Library.Enums;

namespace ZarahDB_Library.Models
{
    //Copyright 2015-2016 Benchmark Solutions LLC
    //Originally created by Mike Reed

    //Licensed under the Apache License, Version 2.0 (the "License");
    //you may not use this file except in compliance with the License.
    //You may obtain a copy of the License at

    //    http://www.apache.org/licenses/LICENSE-2.0

    //Unless required by applicable law or agreed to in writing, software
    //distributed under the License is distributed on an "AS IS" BASIS,
    //WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    //See the License for the specific language governing permissions and
    //limitations under the License.

    /// <summary>
    ///     Class ZarahDBModel.
    /// </summary>
    internal static class ZarahDBModel
    {
        #region Default Instance

        /// <summary>
        ///     The instance
        /// </summary>
        internal static Uri Instance = GetDefaultInstance();

        /// <summary>
        ///     Gets the default instance.
        /// </summary>
        /// <returns>Uri.</returns>
        private static Uri GetDefaultInstance()
        {
            Uri instance;
            try
            {
                instance =
                    new Uri(
                        Path.Combine(
                            Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ??
                            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "zdb"));
            }
            catch (Exception)
            {
                instance = null;
            }
            return instance;
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <returns>Uri.</returns>
        internal static Uri GetInstance()
        {
            return Instance;
        }

        /// <summary>
        ///     Puts the instance.
        /// </summary>
        internal static void PutInstance()
        {
            PutInstance("", null);
        }

        /// <summary>
        ///     Puts the instance.
        /// </summary>
        /// <param name="instanceName">Name of the instance.</param>
        internal static void PutInstance(string instanceName)
        {
            PutInstance(instanceName, null);
        }

        /// <summary>
        ///     Puts the instance.
        /// </summary>
        /// <param name="instanceName">Name of the instance.</param>
        /// <param name="rootFolder">The root folder.</param>
        internal static void PutInstance(string instanceName, InstanceLocation? rootFolder)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
            {
                instanceName = "zdb";
            }

            switch (rootFolder)
            {
                case InstanceLocation.CommonApplicationData:
                    //AppData for all users
                    var instancePathCommonApplicationData = Environment.SpecialFolder.CommonApplicationData;
                    Instance =
                        new Uri(Path.Combine(Environment.GetFolderPath(instancePathCommonApplicationData), instanceName));
                    break;
                case InstanceLocation.ApplicationData:
                    //AppData for the current user
                    Instance =
                        new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            instanceName));
                    break;
                case InstanceLocation.DesktopDirectory:
                    //Desktop of current user
                    Instance =
                        new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                            instanceName));
                    break;
                case InstanceLocation.Location:
                    //Location of assembly as specified if dynamic, falls back to ApplicationData if null
                    Instance =
                        new Uri(
                            Path.Combine(
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), instanceName));
                    break;
                case InstanceLocation.BaseDirectory:
                    //Base directory that the assembly resolver uses to probe for assemblies
                    Instance = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, instanceName));
                    break;
                case InstanceLocation.Codebase:
                    //Location of assembly
                    Instance =
                        new Uri(
                            Path.Combine(
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ??
                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), instanceName));
                    break;
                default: //Default is Codebase
                    Instance =
                        new Uri(
                            Path.Combine(
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) ??
                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), instanceName));
                    break;
            }
        }

        /// <summary>
        ///     Puts the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        internal static void PutInstance(Uri instance)
        {
            Instance = instance;
        }

        #endregion

        #region Default Table

        /// <summary>
        ///     The table
        /// </summary>
        internal static string Table;

        /// <summary>
        ///     Gets the table.
        /// </summary>
        /// <returns>System.String.</returns>
        internal static string GetTable()
        {
            return Table;
        }

        /// <summary>
        ///     Puts the table.
        /// </summary>
        internal static void PutTable()
        {
            Table = "default_table";
        }

        /// <summary>
        ///     Puts the table.
        /// </summary>
        /// <param name="table">The table.</param>
        internal static void PutTable(string table)
        {
            Table = table;
        }

        #endregion

        #region Default TimeoutSeconds

        /// <summary>
        ///     The timeout seconds
        /// </summary>
        internal static int TimeoutSeconds = 30;

        /// <summary>
        ///     Gets the timeout seconds.
        /// </summary>
        /// <returns>System.Int32.</returns>
        internal static int GetTimeoutSeconds()
        {
            return TimeoutSeconds;
        }

        /// <summary>
        ///     Puts the timeout seconds.
        /// </summary>
        internal static void PutTimeoutSeconds()
        {
            TimeoutSeconds = 30;
        }

        /// <summary>
        ///     Puts the timeout seconds.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        internal static void PutTimeoutSeconds(int timeoutSeconds)
        {
            TimeoutSeconds = timeoutSeconds;
        }

        #endregion
    }
}