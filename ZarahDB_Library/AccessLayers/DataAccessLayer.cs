// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 07-05-2015
//
// Last Modified By : Mike.Reed
// Last Modified On : 08-27-2015
// ***********************************************************************
// <copyright file="DataAccessLayer.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2015 Benchmark Solutions LLC
// </copyright>
// <summary>
// This is the DAL (Data Access Layer)
// This class contains all methods which access or modify the database instance.
// This class is exposed internally, with no public methods.
// This class is aware of the FAL (File Access Layer) but not the file system itself.
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ZarahDB_Library.Enums;
using ZarahDB_Library.Helpers;
using ZarahDB_Library.Models;
using ZarahDB_Library.Types;

namespace ZarahDB_Library.AccessLayers
{
    //Copyright 2015 Benchmark Solutions LLC
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

    internal static class DataAccessLayer
    {
        #region Create

        public static StatusMessageValue CreateInstance(Uri instance)
        {
            var requestedTicks = StringHelper.NowTicks();

            var startTicks = StringHelper.NowTicks();

            var result = FileAccessLayer.CreateInstance(instance);

            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return result;
        }

        #endregion

        #region List

        public static StatusList ListInstance(string instancesRootFolder)
        {
            var requestedTicks = StringHelper.NowTicks();
            var startTicks = StringHelper.NowTicks();

            if (string.IsNullOrWhiteSpace(instancesRootFolder))
            {
                instancesRootFolder = Directory.GetParent(ZarahDBModel.GetInstance().AbsolutePath).ToString();
            }

            StatusList statusList;
            try
            {
                statusList = FileAccessLayer.ListInstance(instancesRootFolder);
            }
            catch (Exception ex)
            {
                statusList = StatusHelper.SetStatusList(ex.HResult.ToString(), ex.Message, "");
            }

            statusList.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return statusList;
        }

        public static StatusList ListTable(Uri instance, int timeoutSeconds)
        {
            var requestedTicks = StringHelper.NowTicks();
            var startTicks = StringHelper.NowTicks();

            StatusList statusList;
            try
            {
                statusList = FileAccessLayer.ListTable(instance, timeoutSeconds);
            }
            catch (Exception ex)
            {
                statusList = StatusHelper.SetStatusList(ex.HResult.ToString(), ex.Message, "");
            }

            statusList.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return statusList;
        }

        #endregion

        #region Backup

        /// <summary>
        ///     Backups the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusValue</returns>
        internal static StatusMessageValue Backup(Uri instance, int timeoutSeconds)
        {
            var requestedTicks = StringHelper.NowTicks();

            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(timeoutSeconds);
            while (FileAccessLayer.InstanceLocked(instance))
            {
                if (DateTime.Now > endTime)
                {
                    var statusValue = new StatusMessageValue
                    {
                        Status = "597",
                        Message = $"Network timeout error (Persistant Lock for more than {timeoutSeconds} seconds.)",
                        Value = "",
                        Statistics = StatusHelper.FinalizeStats(requestedTicks, "")
                    };
                    return statusValue;
                }
                Thread.Sleep(100);
            }

            var startTicks = StringHelper.NowTicks();

            FileAccessLayer.LockInstance(instance);
            var result = FileAccessLayer.Backup(instance);
            FileAccessLayer.UnlockInstance(instance);

            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return result;
        }

        /// <summary>
        ///     Restores the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="backup">The backup.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusValue</returns>
        internal static StatusMessageValue Restore(Uri instance, Uri backup, int timeoutSeconds)
        {
            var requestedTicks = StringHelper.NowTicks();

            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(timeoutSeconds);
            while (FileAccessLayer.InstanceLocked(instance))
            {
                if (DateTime.Now > endTime)
                {
                    var statusValue = new StatusMessageValue
                    {
                        Status = "597",
                        Value = $"Network timeout error (Persistant Lock for more than {timeoutSeconds} seconds.)",
                        Statistics = StatusHelper.FinalizeStats("", requestedTicks)
                    };
                    return statusValue;
                }
                Thread.Sleep(100);
            }

            var startTicks = StringHelper.NowTicks();

            FileAccessLayer.LockInstance(instance);
            StatusMessageValue result;
            try
            {
                result = FileAccessLayer.Restore(instance, backup);
            }
            catch (Exception ex)
            {
                result = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message, "");
            }
            FileAccessLayer.UnlockInstance(instance);

            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return result;
        }

        #endregion

        #region CSV

        public static StatusMessageValue CsvPut(string csvData, List<string> columns, string keyColumn,
            string fieldSeparator, string encloser, string lineTerminator, string commentLineStarter, Uri instance,
            string table, int timeoutSeconds)
        {
            var requestedTicks = StringHelper.NowTicks();

            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(timeoutSeconds);
            while (FileAccessLayer.InstanceLocked(instance))
            {
                if (DateTime.Now > endTime)
                {
                    var statusValue = new StatusMessageValue
                    {
                        Status = "597",
                        Value = $"Network timeout error (Persistant Lock for more than {timeoutSeconds} seconds.)",
                        Statistics = StatusHelper.FinalizeStats("", requestedTicks)
                    };
                    return statusValue;
                }
                Thread.Sleep(100);
            }

            var startTicks = StringHelper.NowTicks();

            var result = FileAccessLayer.CsvPut(csvData, columns, keyColumn, fieldSeparator, encloser, lineTerminator,
                commentLineStarter, instance, table, timeoutSeconds);

            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);
            return result;
        }

        public static StatusMessageValue CsvFilePut(Uri csvFile, List<string> columns, string keyColumn,
            string fieldSeparator, string encloser, string lineTerminator, string commentLineStarter, Uri instance,
            string table, int timeoutSeconds)
        {
            var requestedTicks = StringHelper.NowTicks();

            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(timeoutSeconds);
            while (FileAccessLayer.InstanceLocked(instance))
            {
                if (DateTime.Now > endTime)
                {
                    var statusValue = new StatusMessageValue
                    {
                        Status = "597",
                        Value = $"Network timeout error (Persistant Lock for more than {timeoutSeconds} seconds.)",
                        Statistics = StatusHelper.FinalizeStats("", requestedTicks)
                    };
                    return statusValue;
                }
                Thread.Sleep(100);
            }

            var startTicks = StringHelper.NowTicks();

            var result = FileAccessLayer.CsvFilePut(csvFile, columns, keyColumn, fieldSeparator, encloser,
                lineTerminator,
                commentLineStarter, instance, table, timeoutSeconds);

            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);
            return result;
        }

        public static bool CsvFileInsert(Uri csvFile, List<string> columns, string keyColumn, string fieldSeparator,
            string encloser, string lineTerminator, string commentLineStarter, Uri instance, string table)
        {
            return FileAccessLayer.CsvFileInsert(csvFile, columns, keyColumn, fieldSeparator, encloser, lineTerminator,
                commentLineStarter, instance, table);
        }

        #endregion

        #region Delete

        /// <summary>
        ///     Deletes the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        internal static StatusMessageValue DeleteInstance(Uri instance, int timeoutSeconds)
        {
            var requestedTicks = StringHelper.NowTicks();

            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(timeoutSeconds);
            while (FileAccessLayer.InstanceLocked(instance))
            {
                if (DateTime.Now > endTime)
                {
                    var statusValue = new StatusMessageValue
                    {
                        Status = "597",
                        Message = $"Network timeout error (Persistant Lock for more than {timeoutSeconds} seconds.)",
                        Value = "",
                        Statistics = StatusHelper.FinalizeStats(requestedTicks, "")
                    };
                    return statusValue;
                }
                Thread.Sleep(100);
            }

            var startTicks = StringHelper.NowTicks();

            FileAccessLayer.LockInstance(instance);
            var result = FileAccessLayer.DeleteInstance(instance);
            FileAccessLayer.UnlockInstance(instance);

            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return result;
        }

        /// <summary>
        ///     Deletes the table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        internal static StatusMessageValue DeleteTable(Uri instance, string table, int timeoutSeconds)
        {
            var requestedTicks = StringHelper.NowTicks();

            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(timeoutSeconds);
            while (FileAccessLayer.InstanceLocked(instance))
            {
                if (DateTime.Now > endTime)
                {
                    var statusValue = new StatusMessageValue
                    {
                        Status = "597",
                        Message = $"Network timeout error (Persistant Lock for more than {timeoutSeconds} seconds.)",
                        Value = "",
                        Statistics = StatusHelper.FinalizeStats(requestedTicks, "")
                    };
                    return statusValue;
                }
                Thread.Sleep(100);
            }

            var startTicks = StringHelper.NowTicks();

            FileAccessLayer.LockInstance(instance);
            var result = FileAccessLayer.DeleteTable(instance, table);
            FileAccessLayer.UnlockInstance(instance);

            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return result;


        }

        #endregion

        #region Exists

        /// <summary>
        ///     Tests for the existence of specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        internal static bool Exists(Uri instance)
        {
            return FileAccessLayer.Exists(instance);
        }

        /// <summary>
        ///     Tests for the existence of specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        internal static bool Exists(Uri instance, string table)
        {
            return FileAccessLayer.Exists(instance, table);
        }

        /// <summary>
        ///     Tests for the existence of specified instance.
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
        internal static bool Exists(Uri instance, string table, string key, bool? checkExactMatch = true)
        {
            return FileAccessLayer.Exists(instance, table, key, checkExactMatch);
        }

        /// <summary>
        ///     Tests for the existence of specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="checkExactMatch">
        ///     Key collisions are possible. if set to <c>true</c> the file is checked to contain the
        ///     exact key if there is any chance of collision. Set this to <c>false</c> for the fastest execution when key
        ///     collisions are known not to exist.
        /// </param>
        /// <returns>List of keys, from the KeyList that exist.</returns>
        internal static KeyList Exists(Uri instance, string table, KeyList keyList, bool? checkExactMatch = true)
        {
            return FileAccessLayer.Exists(instance, table, keyList, checkExactMatch);
        }

        #endregion

        #region Get

        /// <summary>
        ///     Gets the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="column">The column.</param>
        /// <returns>StatusValue</returns>
        internal static StatusMessageValue Get(Uri instance, string table, string key, string column)
        {
            var requestedTicks = StringHelper.NowTicks();
            var startTicks = requestedTicks;

            var statusKeyColumnValues = Get(instance, table, key);

            if (!statusKeyColumnValues.ColumnValues.Any())
            {
                //TODO: Mike: Work: Set status with helper
                var statusValue = new StatusMessageValue
                {
                    Status = "200",
                    Message = "OK",
                    Value = null,
                    Statistics = StatusHelper.FinalizeStats("", requestedTicks)
                };
                return statusValue;
            }

            string value = null;
            foreach (var columnValue in statusKeyColumnValues.ColumnValues)
            {
                if (!string.Equals(columnValue.Key, column, StringComparison.InvariantCultureIgnoreCase)) continue;

                value = columnValue.Value;
                break;
            }

            //TODO: Mike: Work: Set status with helper
            var result = new StatusMessageValue
            {
                Status = "200",
                Message = "OK",
                Value = value,
                Statistics = StatusHelper.FinalizeStats(startTicks, requestedTicks)
            };

            return result;
        }

        internal static StatusKeyColumnValue GetValue(Uri instance, string table, string key, string column)
        {
            var requestedTicks = StringHelper.NowTicks();
            var startTicks = requestedTicks;

            var statusKeyColumnValue = new StatusKeyColumnValue
            {
                Key = key,
                Column = column,
                Status = "0"
            };

            var keyColumnValueses = FileAccessLayer.GetAllKeyColumnValues(instance, table, key);

            var keyColumnValues =
                (from record in keyColumnValueses where record.Key == key select record).FirstOrDefault();

            if (keyColumnValues == null)
            {
                return statusKeyColumnValue;
            }

            string value = null;
            string previousValue = null;
            var updated = DateTime.MinValue.Ticks;
            foreach (var columnValue in keyColumnValues.ColumnValues)
            {
                if (!string.Equals(columnValue.Column, column, StringComparison.InvariantCultureIgnoreCase)) continue;

                value = columnValue.Value;
                previousValue = columnValue.PreviousValue;
                updated = Convert.ToInt64(columnValue.Updated ?? "0");
                break;
            }

            //TODO: Mike: Work: Set status with helper
            var result = new StatusKeyColumnValue
            {
                Status = "200",
                Message = "OK",
                Key = key,
                Column = column,
                Value = value,
                PreviousValue = previousValue,
                Updated = updated,
                Statistics = StatusHelper.FinalizeStats(startTicks, requestedTicks)
            };

            return result;
        }

        /// <summary>
        ///     Gets the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="columnSet">The column set.</param>
        /// <returns>StatusKeyColumnValues</returns>
        internal static StatusKeyColumnValues Get(Uri instance, string table, string key, List<string> columnSet)
        {
            var statusKeyColumnValues = Get(instance, table, key);

            var newColumnValues = new Dictionary<string, string>();
            foreach (var columnValue in statusKeyColumnValues.ColumnValues)
            {
                if (columnSet.Contains(columnValue.Key) || columnSet.Contains("*"))
                {
                    newColumnValues[columnValue.Key] = columnValue.Value;
                }
            }
            statusKeyColumnValues.ColumnValues = newColumnValues;
            statusKeyColumnValues.Status = "200";

            return statusKeyColumnValues;
        }

        /// <summary>
        ///     Gets the specified status transaction.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        internal static StatusTransaction Get(StatusTransaction statusTransaction, string table, string key)
        {
            //If the key was copied over to the transaction, use that data
            var statusKeyColumnValues = Get(new Uri(statusTransaction.TransactionIndex), table, key);

            //If not, use the instance data
            if (!statusKeyColumnValues.ColumnValues.Any())
            {
                statusKeyColumnValues = Get(statusTransaction.Instance, table, key);
            }

            //Whichever key was used, pull the column and values and add them to the results
            KeyColumnValues existingKey = null;
            foreach (var columnValue in statusKeyColumnValues.ColumnValues)
            {
                existingKey =
                    (from record in statusTransaction.Results where record.Key == key select record).FirstOrDefault();
                var existing = true;
                if (existingKey == null)
                {
                    existing = false;
                    existingKey = new KeyColumnValues {Key = key};
                }
                var newColumnValue = new ColumnValue
                {
                    Column = columnValue.Key,
                    Value = columnValue.Value
                };
                existingKey.ColumnValues.Add(newColumnValue);
                if (!existing)
                {
                    statusTransaction.Results.Add(existingKey);
                }
            }
            StatusHelper.SetTransactionStatus(statusTransaction, existingKey, StatusCode.OK, StatusCode.OK);

            return statusTransaction;
        }

        /// <summary>
        ///     Gets the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <returns>StatusKeyColumnValues</returns>
        internal static StatusKeyColumnValues Get(Uri instance, string table, string key)
        {
            var requestedTicks = StringHelper.NowTicks();
            const int timeoutSeconds = 30; //TODO: This needs to be passed in

            var statusKeyColumnValues = new StatusKeyColumnValues
            {
                Key = key,
                Status = "0"
            };

            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(timeoutSeconds);
            while (FileAccessLayer.InstanceLocked(instance))
            {
                if (DateTime.Now > endTime)
                {
                    statusKeyColumnValues = new StatusKeyColumnValues
                    {
                        Status = "597",
                        Message = $"Network timeout error (Persistant Lock for more than {timeoutSeconds} seconds.)",
                        Value = "",
                        Statistics = StatusHelper.FinalizeStats(requestedTicks, "")
                    };
                    return statusKeyColumnValues;
                }
                Thread.Sleep(100);
            }

            var startTicks = StringHelper.NowTicks();

            var keyColumnValueses = FileAccessLayer.GetAllKeyColumnValues(instance, table, key);

            var keyColumnValues =
                (from record in keyColumnValueses where record.Key == key select record).FirstOrDefault();

            if (keyColumnValues == null)
            {
                return statusKeyColumnValues;
            }

            foreach (var columnValues in keyColumnValues.ColumnValues)
            {
                try
                {
                    statusKeyColumnValues.ColumnValues.Add(columnValues.Column, columnValues.Value);
                    statusKeyColumnValues.Status = "200";
                }
                catch (Exception)
                {
                    //Intentionally Left Blank
                }
            }

            statusKeyColumnValues.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return statusKeyColumnValues;
        }

        #endregion

        #region Locks

        /// <summary>
        ///     Locks the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        internal static void LockInstance(Uri instance)
        {
            FileAccessLayer.LockInstance(instance);
        }

        /// <summary>
        ///     Unlocks the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        internal static void UnlockInstance(Uri instance)
        {
            FileAccessLayer.UnlockInstance(instance);
        }

        /// <summary>
        ///     Gets the instance lock status.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>StatusMessageValue.</returns>
        public static StatusMessageValue GetInstanceLock(Uri instance)
        {
            var requestedTicks = StringHelper.NowTicks();
            var startTicks = requestedTicks;

            var instanceLocked = FileAccessLayer.GetInstanceLock(instance);

            var result = StatusHelper.SetStatusMessageValue(StatusCode.OK, instanceLocked.ToString());
            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return result;
        }

        /// <summary>
        ///     Locks a table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        internal static void LockTable(Uri instance, string table)
        {
            FileAccessLayer.LockTable(instance, table);
        }

        /// <summary>
        ///     Unlocks a table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        internal static void UnlockTable(Uri instance, string table)
        {
            FileAccessLayer.UnlockTable(instance, table);
        }

        #endregion

        #region MaxDepth

        internal static StatusMessageValue GetMaxDepth(Uri instance, string table)
        {
            var requestedTicks = StringHelper.NowTicks();
            var startTicks = requestedTicks;

            var maxDepth = FileAccessLayer.GetMaxDepth(instance, table);
            var result = StatusHelper.SetStatusMessageValue(StatusCode.OK, maxDepth.ToString());
            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return result;
        }

        internal static StatusMessageValue SetMaxDepth(Uri instance, string table, int maxDepth)
        {
            var requestedTicks = StringHelper.NowTicks();
            var startTicks = requestedTicks;

            FileAccessLayer.SetMaxDepth(instance, table, maxDepth);

            var currentMaxDepth = FileAccessLayer.GetMaxDepth(instance, table);
            var result = StatusHelper.SetStatusMessageValue(StatusCode.OK, currentMaxDepth.ToString());
            result.Statistics = StatusHelper.FinalizeStats(requestedTicks, startTicks);

            return result;
        }

        #endregion

        #region Put

        /// <summary>
        ///     Puts the specified status transaction.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        internal static StatusMessageValue Put(StatusTransaction statusTransaction, string table, string key,
            string column,
            string value,
            int timeoutSeconds)
        {
            //Lock the instance key and copy it to the transaction key
            FileAccessLayer.TransactionLockKey(statusTransaction, table, key);

            //Update the transaction key
            var result = Put(new Uri(statusTransaction.TransactionIndex), table, key, column, value, timeoutSeconds);

            //Add the results to the transaction
            //TODO: Mike: Work: Add result to transaction so we can track progress and have a history

            return result;
        }

        internal static StatusMessageValue Put(Uri instance, string table, KeyColumnValues keyColumnValues,
            int timeoutSeconds)
        {
            var requestedTicks = StringHelper.NowTicks();
            var startTicks = requestedTicks;

            var result = FileAccessLayer.Put(instance, table, keyColumnValues, timeoutSeconds);

            result.Statistics = StatusHelper.FinalizeStats(startTicks, requestedTicks);

            return result;
        }

        /// <summary>
        ///     Puts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusValue</returns>
        internal static StatusMessageValue Put(Uri instance, string table, string key, string column, string value,
            int timeoutSeconds)
        {
            return FileAccessLayer.Put(instance, table, key, column, value, timeoutSeconds);
        }

        #endregion

        #region Script

        /// <summary>
        ///     Runs an ad hoc script.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="script">The script.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>StatusTransaction</returns>
        internal static StatusTransaction Script(Uri instance, string script,
            Dictionary<string, string> variables = null)
        {
            //Create the new transaction
            var newStatusTransaction = new StatusTransaction
            {
                Instance = instance,
                Script = script
            };
            StatusHelper.SetStartTicks(newStatusTransaction);
            if (variables != null)
            {
                newStatusTransaction.Variables = variables;
            }

            try
            {
                //Create the base transaction before stating processing.
                newStatusTransaction = FileAccessLayer.BeginTransaction(newStatusTransaction);

                //Process the script within the transaction.
                newStatusTransaction = ProcessTransaction(newStatusTransaction);
            }
            catch (Exception ex)
            {
                StatusHelper.SetTransactionStatus(newStatusTransaction, StatusCode.Internal_Server_Error, ex.Message);
            }

            //Assume script did any CommitTransaction() calls prior to ending. 
            //Any uncommitted changes are rolled back.
            //If everything was committed, this will just clean up the Transaction folder.
            newStatusTransaction = FileAccessLayer.RollbackTransaction(newStatusTransaction);

            //The transactions were recorded with full details and result sets, and are returned.
            StatusHelper.SetEndTicks(newStatusTransaction);
            return newStatusTransaction;
        }

        internal static bool PutScript(Uri instance, string scriptName, string script)
        {
            return FileAccessLayer.PutScript(instance, scriptName, script);
        }

        public static string GetScript(Uri instance, string scriptName)
        {
            return FileAccessLayer.GetScript(instance, scriptName);
        }

        public static StatusTransaction ExecuteScript(Uri instance, string scriptName,
            Dictionary<string, string> variables)
        {
            var newStatusTransaction = new StatusTransaction();
            if (string.IsNullOrEmpty(instance.AbsolutePath))
            {
                return newStatusTransaction;
            }
            if (string.IsNullOrEmpty(scriptName))
            {
                return newStatusTransaction;
            }

            var script = FileAccessLayer.GetScript(instance, scriptName);
            newStatusTransaction = Script(instance, script);

            return newStatusTransaction;
        }

        #endregion

        #region Transaction

        /// <summary>
        ///     Processes the transaction.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <returns>StatusTransaction</returns>
        private static StatusTransaction ProcessTransaction(StatusTransaction statusTransaction)
        {
            var script = statusTransaction.Script;
            if (string.IsNullOrEmpty(script))
            {
                StatusHelper.SetTransactionStatus(statusTransaction, StatusCode.Internal_Server_Error,
                    "Script is null or empty.");
                return statusTransaction;
            }

            script = StringHelper.ReplaceEx(script, "\r", "\n");

            var inCommand = false;
            for (var cursor = 0; cursor < script.Length; cursor++)
            {
                //Find start of next line and enter it
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                //Not sure why ReSharper gets this wrong. It is used, it is important and it works as expected
                if (!inCommand)
                {
                    if (string.IsNullOrWhiteSpace(script.Substring(cursor, 1)))
                    {
                        continue;
                    }
                    if (script.Substring(cursor, 2) == "//")
                    {
                        cursor = script.IndexOf("\n", cursor, StringComparison.Ordinal);
                        if (cursor == 0)
                        {
                            break; //Comment is last line of script
                        }
                        continue;
                    }
                    // ReSharper disable once RedundantAssignment
                    inCommand = true;
                }

                //Parse the command
                var operatorWithOperands = ParseOperatorWithOperands(script, ref cursor);
                inCommand = false;

                //Replace variables
                for (var index = 0; index < operatorWithOperands.Operands.Count; index++)
                {
                    if (!statusTransaction.Variables.ContainsKey(operatorWithOperands.Operands[index])) continue;

                    string value;
                    statusTransaction.Variables.TryGetValue(operatorWithOperands.Operands[index], out value);
                    if (index == 0 && operatorWithOperands.Operator.ToLowerInvariant().Equals("set")) continue;
                    operatorWithOperands.Operands[index] = value;
                }

                statusTransaction.Command = ConstructCommand(operatorWithOperands);

                //Process the command
                var isSyntaxError = ProcessCommand(statusTransaction, operatorWithOperands);

                //Make sure it processed successfully, if so, handle the next command
                if (!isSyntaxError) continue;

                //Syntax error - unrecognized command
                StatusHelper.SetTransactionStatus(statusTransaction, StatusCode.Internal_Server_Error,
                    "Syntax Error - Unrecognized Command");
                break;
            }

            //Return the results of the script
            return statusTransaction;
        }

        private static string ConstructCommand(OperatorWithOperands operatorWithOperands)
        {
            var command = $"{operatorWithOperands.Operator}(";
            for (var i = 0; i < operatorWithOperands.Operands.Count; i++)
            {
                if (i == 0)
                {
                    command = $"{command}{operatorWithOperands.Operands[i]}";
                    continue;
                }
                command = $"{command}, {operatorWithOperands.Operands[i]}";
            }
            command = $"{command});";
            return command;
        }

        /// <summary>
        ///     Parses the operator with operands.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="cursor">The cursor.</param>
        /// <returns>OperatorWithOperands</returns>
        private static OperatorWithOperands ParseOperatorWithOperands(string script, ref int cursor)
        {
            var newOperatorWithOperands = new OperatorWithOperands();

            var openParenthesis = script.IndexOf('(', cursor);
            newOperatorWithOperands.Operator = script.Substring(cursor, openParenthesis - cursor);
            cursor = openParenthesis + 1;
            var inOperand = false;
            var quoteCharacter = "";
            var inQuotes = false;
            var newOperand = "";
            for (var i = cursor; i < script.Length; i++)
            {
                var subString1 = script.Substring(i, 1);
                var subString2 = script.Substring(i, 2);
                if (!inOperand)
                {
                    quoteCharacter = "";
                    if (string.IsNullOrWhiteSpace(subString1))
                    {
                        continue;
                    }
                    inOperand = true;
                    if (subString1.Equals("'"))
                    {
                        quoteCharacter = "'";
                        inQuotes = true;
                        continue;
                    }
                    if (subString1.Equals("\""))
                    {
                        quoteCharacter = "\"";
                        inQuotes = true;
                        continue;
                    }
                }
                if (inQuotes)
                {
                    if (subString2.Equals(quoteCharacter + ","))
                    {
                        cursor = i + 1;
                        inQuotes = false;
                        inOperand = false;
                        newOperatorWithOperands.Operands.Add(newOperand);
                        newOperand = "";
                        continue;
                    }
                    if (subString2.Equals(quoteCharacter + ")"))
                    {
                        cursor = i + 1;
                        inQuotes = false;
                        inOperand = false;
                        newOperatorWithOperands.Operands.Add(newOperand);
                        newOperand = "";
                        continue;
                    }
                    newOperand = newOperand + subString1;
                }
                else
                {
                    if (subString2.Trim().Equals(","))
                    {
                        cursor = i + 1;
                        inOperand = false;
                        if (!string.IsNullOrEmpty(newOperand))
                        {
                            newOperatorWithOperands.Operands.Add(newOperand);
                        }
                        newOperand = "";
                        continue;
                    }
                    if (subString2.Equals(");"))
                    {
                        cursor = i + 2;
                        if (!string.IsNullOrEmpty(newOperand))
                        {
                            newOperatorWithOperands.Operands.Add(newOperand);
                        }
                        break;
                    }
                    newOperand = newOperand + subString1;
                }
            }

            return newOperatorWithOperands;
        }

        /// <summary>
        ///     Processes the command.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="operatorWithOperands">The operator with operands.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        private static bool ProcessCommand(StatusTransaction statusTransaction,
            OperatorWithOperands operatorWithOperands)
        {
            var isSyntaxError = false;
            switch (operatorWithOperands.Operator.ToLower())
            {
                case "put":
                    switch (operatorWithOperands.Operands.Count)
                    {
                        case 5:
                            Put(statusTransaction, operatorWithOperands.Operands[0],
                                operatorWithOperands.Operands[1], operatorWithOperands.Operands[2],
                                operatorWithOperands.Operands[3], Convert.ToInt16(operatorWithOperands.Operands[4]));
                            StatusHelper.SetTransactionStatus(statusTransaction, StatusCode.OK, StatusCode.OK);
                            break;
                        default:
                            isSyntaxError = true;
                            break;
                    }
                    break;
                case "get":
                    switch (operatorWithOperands.Operands.Count)
                    {
                        case 2:
                            Get(statusTransaction, operatorWithOperands.Operands[0], operatorWithOperands.Operands[1]);
                            break;
                        default:
                            isSyntaxError = true;
                            break;
                    }
                    break;
                case "committransaction":
                    switch (operatorWithOperands.Operands.Count)
                    {
                        case 1:
                            CommitTransaction(statusTransaction);
                            StatusHelper.SetTransactionStatus(statusTransaction, StatusCode.OK, StatusCode.OK);
                            break;
                        default:
                            isSyntaxError = true;
                            break;
                    }
                    break;
                case "backup":
                    switch (operatorWithOperands.Operands.Count)
                    {
                        case 1:
                            var result = Backup(statusTransaction.Instance,
                                Convert.ToInt16(operatorWithOperands.Operands[0]));
                            StatusHelper.SetTransactionStatus(statusTransaction, result.Status, result.Value);
                            break;
                        default:
                            isSyntaxError = true;
                            break;
                    }
                    break;
                case "deleteinstance":
                    switch (operatorWithOperands.Operands.Count)
                    {
                        case 1:
                            var result = DeleteInstance(statusTransaction.Instance,
                                Convert.ToInt16(operatorWithOperands.Operands[0]));
                            StatusHelper.SetTransactionStatus(statusTransaction, result.Status, result.Message);
                            break;
                        default:
                            isSyntaxError = true;
                            break;
                    }
                    break;
                case "set":
                    switch (operatorWithOperands.Operands.Count)
                    {
                        case 2:
                            statusTransaction.Variables[operatorWithOperands.Operands[0]] =
                                operatorWithOperands.Operands[1];
                            StatusHelper.SetTransactionStatus(statusTransaction, StatusCode.OK, StatusCode.OK);
                            break;
                        default:
                            isSyntaxError = true;
                            break;
                    }
                    break;
                default:
                    isSyntaxError = true;
                    break;
            }
            return isSyntaxError;
        }

        /// <summary>
        ///     Commits the transaction.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        private static StatusTransaction CommitTransaction(StatusTransaction statusTransaction)
        {
            return FileAccessLayer.CommitTransaction(statusTransaction);
        }

        #endregion
    }
}