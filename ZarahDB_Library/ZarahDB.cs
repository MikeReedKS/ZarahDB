// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 07-04-2015
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-05-2017
// ***********************************************************************
// <copyright file="ZarahDB.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2015 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZarahDB_Library.AccessLayers;
using ZarahDB_Library.Enums;
using ZarahDB_Library.Helpers;
using ZarahDB_Library.Models;
using ZarahDB_Library.Types;

namespace ZarahDB_Library
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

    /// <summary>
    ///     Scattered Database Implementation Library.
    ///     This is the public class which exposes the functionality publicly.
    ///     This class is aware of the Model and the DAL (Data Access Layer) but not the FAL (File Access Layer).
    /// </summary>
    public static class ZarahDB
    {
        #region Commands

        #region Create

        public static StatusMessageValue CreateInstance(Uri instance)
        {
            return DataAccessLayer.CreateInstance(instance);
        }

        #endregion

        #region List

        public static StatusList ListInstance(string instancesRootFolder)
        {
            return DataAccessLayer.ListInstance(instancesRootFolder);
        }

        public static StatusList ListTable()
        {
            var instance = ZarahDBModel.GetInstance();
            var timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            return ListTable(instance, timeoutSeconds);
        }

        public static StatusList ListTable(Uri instance)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            var timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            return ListTable(instance, timeoutSeconds);
        }

        public static StatusList ListTable(Uri instance, int? timeoutSeconds)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }
            return DataAccessLayer.ListTable(instance, (int) timeoutSeconds);
        }

        #endregion

        #region Backup

        /// <summary>
        ///     Backups this instance.
        /// </summary>
        /// <returns>StatusValue</returns>
        public static StatusMessageValue Backup()
        {
            var instance = ZarahDBModel.GetInstance();
            var timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            return Backup(instance, timeoutSeconds);
        }

        /// <summary>
        ///     Backups the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>StatusValue</returns>
        public static StatusMessageValue Backup(Uri instance)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            var timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            return Backup(instance, timeoutSeconds);
        }

        /// <summary>
        ///     Backups the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusMessageValue Backup(Uri instance, int? timeoutSeconds)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }

            return DataAccessLayer.Backup(instance, (int) timeoutSeconds);
        }

        /// <summary>
        ///     Restores the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="backup">The backup.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusMessageValue Restore(Uri instance, Uri backup, int? timeoutSeconds)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }

            return DataAccessLayer.Restore(instance, backup, (int) timeoutSeconds);
        }

        #endregion

        #region Csv

        /// <summary>
        ///     Inserts records from a folder of CSV files.
        ///     This method does not support transactions. The values are written directly to increase speed.
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
        /// <returns><c>true</c> if successful, <c>false</c> otherwise</returns>
        public static bool CsvFolderInsert(Uri csvFolder, List<string> columns, string keyColumn, string fieldSeparator,
            string encloser, string lineTerminator, string commentLineStarter, Uri instance, string table)
        {
            var result = false;
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }

            try
            {
                var files = Directory.EnumerateFiles(csvFolder.AbsolutePath);

                //foreach (var csvFile in files)
                Parallel.ForEach(files, csvFile =>
                {
                    result = DataAccessLayer.CsvFileInsert(new Uri(csvFile), columns, keyColumn, fieldSeparator,
                        encloser,
                        lineTerminator, commentLineStarter, instance, table);
                }
                    );
            }
            catch
            {
                return false;
            }

            return result;
        }

        /// <summary>
        ///     Puts records from a folder of CSV files.
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
        public static StatusMessageValue CsvFolderPut(Uri csvFolder, List<string> columns, string keyColumn,
            string fieldSeparator, string encloser, string lineTerminator, string commentLineStarter,
            Uri instance, string table, int? timeoutSeconds)
        {
            var statusMessageValue = new StatusMessageValue();
            try
            {
                var files = Directory.EnumerateFiles(csvFolder.AbsolutePath);

                foreach (var csvFile in files)
                {
                    statusMessageValue = CsvFilePut(new Uri(csvFile), columns, keyColumn, fieldSeparator, encloser,
                        lineTerminator,
                        commentLineStarter, instance, table, timeoutSeconds);
                }
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(ex.HResult.ToString(), ex.Message,
                    csvFolder.AbsolutePath);
            }

            return statusMessageValue;
        }

        /// <summary>
        ///     Inserts records from CSV formatted data.
        /// </summary>
        /// <param name="csvData">The CSV data.</param>
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
        public static StatusMessageValue CsvPut(string csvData, List<string> columns, string keyColumn,
            string fieldSeparator,
            string encloser, string lineTerminator, string commentLineStarter, Uri instance, string table,
            int? timeoutSeconds)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }

            var result = DataAccessLayer.CsvPut(csvData, columns, keyColumn, fieldSeparator, encloser,
                lineTerminator, commentLineStarter, instance, table, (int) timeoutSeconds);

            return result;
        }

        /// <summary>
        ///     Inserts records from a single CSV file.
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
        public static StatusMessageValue CsvFilePut(Uri csvFile, List<string> columns, string keyColumn,
            string fieldSeparator,
            string encloser, string lineTerminator, string commentLineStarter, Uri instance, string table,
            int? timeoutSeconds)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }

            var result = DataAccessLayer.CsvFilePut(csvFile, columns, keyColumn, fieldSeparator, encloser,
                lineTerminator,
                commentLineStarter, instance, table, (int) timeoutSeconds);

            return result;
        }

        #endregion

        #region Delete

        //TODO: Mike: Work: Implement Delete!
        //public static string Delete(KeyColumnValues keyColumnValues)
        //{
        //    var instance = ZarahDBModel.GetInstance();
        //    var table = ZarahDBModel.GetTable();
        //    bool result;
        //    try
        //    {
        //        result = InstanceDataAccess.Delete(instance, table, keyColumnValues);
        //    }
        //    catch (Exception e)
        //    {
        //        return e.Message;
        //    }
        //    if (result)
        //    {
        //        return "200 OK";
        //    }
        //    return "500 Internal Server Error";
        //}

        //public static string Delete(KeyColumnValues keyColumnValues, string zarahDBTableKeyName)
        //{
        //    var zarahDB = ZarahDBModel.GetInstance();
        //    var zarahDBTable = ZarahDBModel.GetTable();
        //    bool result;
        //    try
        //    {
        //        result = InstanceDataAccess.Delete(zarahDB, zarahDBTable, keyColumnValues, zarahDBTableKeyName);
        //    }
        //    catch (Exception e)
        //    {
        //        return e.Message;
        //    }
        //    if (result)
        //    {
        //        return "200 OK";
        //    }
        //    return "500 Internal Server Error";
        //}

        /// <summary>
        ///     Deletes the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>System.Boolean</returns>
        public static StatusMessageValue DeleteInstance(Uri instance, int? timeoutSeconds)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }

            return DataAccessLayer.DeleteInstance(instance, (int) timeoutSeconds);
        }

        /// <summary>
        ///     Deletes the table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>System.Boolean</returns>
        public static StatusMessageValue DeleteTable(Uri instance, string table, int? timeoutSeconds)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }

            return DataAccessLayer.DeleteTable(instance, table, (int) timeoutSeconds);
        }

        #endregion

        #region Exists

        /// <summary>
        ///     Tests if the specified instance exists.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>System.Boolean</returns>
        public static bool Exists(Uri instance)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }

            try
            {
                return DataAccessLayer.Exists(instance);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Tests if the specified table exists.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <returns>System.Boolean</returns>
        public static bool Exists(Uri instance, string table)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }

            try
            {
                return DataAccessLayer.Exists(instance, table);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Tests if the specified key exists.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="checkExactMatch">
        ///     Key collisions are possible. if set to <c>true</c> the file is checked to contain the
        ///     exact key if there is any chance of collision. Set this to <c>false</c> for the fastest execution when key
        ///     collisions are known not to exist.
        /// </param>
        /// <returns>System.Boolean</returns>
        public static bool Exists(Uri instance, string table, string key, bool? checkExactMatch = true)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (key == null)
            {
                return false;
            }

            try
            {
                return DataAccessLayer.Exists(instance, table, key, checkExactMatch);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Tests if the specified list of keys exist.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="keyList">The list of keys.</param>
        /// <param name="checkExactMatch">
        ///     Key collisions are possible. if set to <c>true</c> the file is checked to contain the
        ///     exact key if there is any chance of collision. Set this to <c>false</c> for the fastest execution when key
        ///     collisions are known not to exist.
        /// </param>
        /// <returns>Keys</returns>
        public static KeyList Exists(Uri instance, string table, KeyList keyList, bool? checkExactMatch = true)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            var newKeys = new KeyList();
            if (keyList == null)
            {
                return newKeys;
            }

            try
            {
                newKeys = DataAccessLayer.Exists(instance, table, keyList, checkExactMatch);
            }
            catch (Exception)
            {
                return newKeys;
            }

            return newKeys;
        }

        #endregion

        #region Get

        /// <summary>
        ///     Gets this instance. Not Implemented.
        /// </summary>
        /// <returns>StatusValue</returns>
        public static StatusMessageValue Get()
        {
            var statusValue = new StatusMessageValue
            {
                Status = "501",
                Value = "Not Implemented"
            };
            return statusValue;
        }

        /// <summary>
        ///     Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>StatusKeyColumnValues</returns>
        public static StatusKeyColumnValues Get(string key)
        {
            return Get(null, null, key);
        }

        /// <summary>
        ///     Gets the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <returns>StatusKeyColumnValues</returns>
        public static StatusKeyColumnValues Get(string table, string key)
        {
            return Get(null, table, key);
        }

        /// <summary>
        ///     Gets the specified key.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <returns>StatusKeyColumnValues</returns>
        public static StatusKeyColumnValues Get(Uri instance, string table, string key)
        {
            StatusKeyColumnValues statusKeyColumnValues;

            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (key == null)
            {
                var columnValues = new Dictionary<string, string>();
                columnValues["Message"] = "Internal Server Error - Null Key";

                statusKeyColumnValues = new StatusKeyColumnValues
                {
                    Status = "500",
                    Key = null,
                    ColumnValues = columnValues
                };
                return statusKeyColumnValues;
            }

            try
            {
                statusKeyColumnValues = DataAccessLayer.Get(instance, table, key);
            }
            catch (Exception e)
            {
                var columnValues = new Dictionary<string, string>();
                columnValues["Message"] = e.Message;

                statusKeyColumnValues = new StatusKeyColumnValues
                {
                    Status = e.HResult.ToString(),
                    Key = key,
                    ColumnValues = columnValues
                };
                return statusKeyColumnValues;
            }

            return statusKeyColumnValues;
        }

        /// <summary>
        ///     Gets the specified list of keys.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>StatusKeysColumnValues</returns>
        public static StatusKeysColumnValues Get(Uri instance, string table, List<string> keys)
        {
            var statusKeysColumnValues = new StatusKeysColumnValues();

            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }

            foreach (var key in keys)
            {
                if (key == null) continue;
                try
                {
                    var statusKeyColumnValues = DataAccessLayer.Get(instance, table, key);
                    var newKeyColumnValues = new KeyColumnValues {Key = key};
                    foreach (var columnValue in statusKeyColumnValues.ColumnValues)
                    {
                        var newColumnValue = new ColumnValue
                        {
                            Column = columnValue.Key,
                            Value = columnValue.Value
                        };
                        newKeyColumnValues.ColumnValues.Add(newColumnValue);
                    }
                    statusKeysColumnValues.KeysColumnValues.Add(newKeyColumnValues);
                }
                catch (Exception e)
                {
                    StatusHelper.SetStatusKeysColumnValuesStatus(statusKeysColumnValues, e.HResult.ToString(), e.Message);
                    return statusKeysColumnValues;
                }
            }

            StatusHelper.SetStatusKeysColumnValuesStatus(statusKeysColumnValues, StatusCode.OK);
            return statusKeysColumnValues;
        }

        /// <summary>
        ///     Gets the specified single column.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="column">The column.</param>
        /// <returns>StatusValue</returns>
        public static StatusMessageValue Get(Uri instance, string table, string key, string column)
        {
            StatusMessageValue statusMessageValue;

            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (key == null)
            {
                statusMessageValue = new StatusMessageValue
                {
                    Status = "500",
                    Value = "Internal Server Error - Null Key"
                };
                return statusMessageValue;
            }
            if (column == null)
            {
                statusMessageValue = new StatusMessageValue
                {
                    Status = "500",
                    Value = "Internal Server Error - Null Column"
                };
                return statusMessageValue;
            }

            try
            {
                statusMessageValue = DataAccessLayer.Get(instance, table, key, column);
            }
            catch (Exception e)
            {
                statusMessageValue = new StatusMessageValue
                {
                    Status = e.HResult.ToString(),
                    Value = e.Message
                };
                return statusMessageValue;
            }

            return statusMessageValue;
        }

        public static StatusKeyColumnValue GetValue(Uri instance, string table, string key, string column)
        {
            StatusKeyColumnValue statusKeyColumnValue;

            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (key == null)
            {
                statusKeyColumnValue = new StatusKeyColumnValue
                {
                    Status = "500",
                    Key = null,
                    Column = column,
                    Value = "Internal Server Error - Null Key"
                };
                return statusKeyColumnValue;
            }
            if (column == null)
            {
                statusKeyColumnValue = new StatusKeyColumnValue
                {
                    Status = "500",
                    Key = key,
                    Column = null,
                    Value = "Internal Server Error - Null Column"
                };
                return statusKeyColumnValue;
            }

            try
            {
                statusKeyColumnValue = DataAccessLayer.GetValue(instance, table, key, column);
            }
            catch (Exception e)
            {
                statusKeyColumnValue = new StatusKeyColumnValue
                {
                    Status = e.HResult.ToString(),
                    Key = key,
                    Column = column,
                    Value = e.Message
                };
                return statusKeyColumnValue;
            }

            return statusKeyColumnValue;
        }

        /// <summary>
        ///     Gets the specified columns.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="columnList">The column list.</param>
        /// <returns>StatusKeyColumnValues</returns>
        public static StatusKeyColumnValues Get(Uri instance, string table, string key, List<string> columnList)
        {
            StatusKeyColumnValues statusKeyColumnValues;

            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (key == null)
            {
                var columnValues = new Dictionary<string, string>();
                columnValues["Message"] = "Internal Server Error - Null Key";

                statusKeyColumnValues = new StatusKeyColumnValues
                {
                    Status = "500",
                    Key = null,
                    ColumnValues = columnValues
                };
                return statusKeyColumnValues;
            }

            try
            {
                statusKeyColumnValues = DataAccessLayer.Get(instance, table, key, columnList);
            }
            catch (Exception e)
            {
                var columnValues = new Dictionary<string, string>();
                columnValues["Message"] = e.Message;

                statusKeyColumnValues = new StatusKeyColumnValues
                {
                    Status = e.HResult.ToString(),
                    Key = key,
                    ColumnValues = columnValues
                };
                return statusKeyColumnValues;
            }

            return statusKeyColumnValues;
        }

        #endregion

        #region Locks

        public static void LockInstance(Uri instance)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }

            try
            {
                DataAccessLayer.LockInstance(instance);
            }
            catch (Exception)
            {
            }
        }

        public static void UnlockInstance(Uri instance)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }

            try
            {
                DataAccessLayer.UnlockInstance(instance);
            }
            catch (Exception)
            {
            }
        }

        public static void LockTable(Uri instance, string table)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }

            try
            {
                DataAccessLayer.LockTable(instance, table);
            }
            catch (Exception)
            {
            }
        }

        public static void UnlockTable(Uri instance, string table)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }

            try
            {
                DataAccessLayer.UnlockTable(instance, table);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Put

        /// <summary>
        ///     Puts the specified key column values.
        /// </summary>
        /// <param name="keyColumnValues">The key column values.</param>
        /// <returns>StatusValue</returns>
        public static StatusMessageValue Put(KeyColumnValues keyColumnValues)
        {
            var timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            return DataAccessLayer.Put(null, null, keyColumnValues, timeoutSeconds);
        }

        /// <summary>
        ///     Puts the specified key column values into a specific table using the default timeout.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="keyColumnValues">The key column values.</param>
        /// <returns>StatusValue</returns>
        public static StatusMessageValue Put(string table, KeyColumnValues keyColumnValues)
        {
            var timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            return DataAccessLayer.Put(null, table, keyColumnValues, timeoutSeconds);
        }

        /// <summary>
        ///     Puts the specified key column values into a specific table of the named instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="keyColumnValues">The key column values.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusValue</returns>
        public static StatusMessageValue Put(Uri instance, string table, KeyColumnValues keyColumnValues,
            int? timeoutSeconds)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }
            return DataAccessLayer.Put(instance, table, keyColumnValues, (int) timeoutSeconds);
        }

        /// <summary>
        ///     Puts the specified key column values into a specific table of the named instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="keyColumnValueses">The key column values.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>StatusValue</returns>
        public static StatusMessageValue Put(Uri instance, string table, List<KeyColumnValues> keyColumnValueses,
            int? timeoutSeconds)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }
            var result = new StatusMessageValue();
            foreach (var keyColumnValues in keyColumnValueses)
            {
                result = DataAccessLayer.Put(instance, table, keyColumnValues, (int) timeoutSeconds);
            }
            return result;
        }

        /// <summary>
        ///     Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusMessageValue Put(string key, string column, string value)
        {
            var instance = ZarahDBModel.GetInstance();
            var table = ZarahDBModel.GetTable();
            return Put(instance, table, key, column, value, null);
        }

        /// <summary>
        ///     Puts the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusMessageValue Put(string table, string key, string column, string value)
        {
            var instance = ZarahDBModel.GetInstance();
            return Put(instance, table, key, column, value, null);
        }

        /// <summary>
        ///     Puts the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusMessageValue Put(Uri instance, string table, string key, string column, string value)
        {
            return Put(instance, table, key, column, value, null);
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
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusMessageValue Put(Uri instance, string table, string key, string column, string value,
            int? timeoutSeconds)
        {
            StatusMessageValue statusMessageValue;

            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            if (key == null)
            {
                statusMessageValue = new StatusMessageValue
                {
                    Status = "500",
                    Value = "Internal Server Error - Null Key"
                };
                return statusMessageValue;
            }
            if (column == null)
            {
                statusMessageValue = new StatusMessageValue
                {
                    Status = "500",
                    Value = "Internal Server Error - Null Column"
                };
                return statusMessageValue;
            }
            if (timeoutSeconds == null)
            {
                timeoutSeconds = ZarahDBModel.GetTimeoutSeconds();
            }

            try
            {
                statusMessageValue = DataAccessLayer.Put(instance, table, key, column, value, (int) timeoutSeconds);
            }
            catch (Exception e)
            {
                statusMessageValue = new StatusMessageValue
                {
                    Status = e.HResult.ToString(),
                    Value = e.Message
                };
                return statusMessageValue;
            }

            return statusMessageValue;
        }

        #endregion

        #region Script

        public static StatusTransaction Script(Uri instance, string script)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            return DataAccessLayer.Script(instance, script);
        }

        public static bool PutScript(Uri instance, string scriptName, string script)
        {
            return DataAccessLayer.PutScript(instance, scriptName, script);
        }

        public static string GetScript(Uri instance, string scriptName)
        {
            return DataAccessLayer.GetScript(instance, scriptName);
        }

        public static StatusTransaction ExecuteScript(Uri instance, string scriptName,
            Dictionary<string, string> variables)
        {
            return DataAccessLayer.ExecuteScript(instance, scriptName, variables);
        }

        #endregion

        #endregion

        #region Properties

        #region Instance

        /// <summary>
        ///     Puts the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void PutInstance(Uri instance)
        {
            ZarahDBModel.PutInstance(instance);
        }

        public static void PutInstance(string instanceName = null,
            InstanceLocation rootFolder = InstanceLocation.Location)
        {
            ZarahDBModel.PutInstance(instanceName, rootFolder);
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <returns>System.Uri</returns>
        public static Uri GetInstance()
        {
            return ZarahDBModel.GetInstance();
        }

        public static StatusMessageValue GetInstanceLock(Uri instance)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            return DataAccessLayer.GetInstanceLock(instance);
        }

        #endregion

        #region Table

        /// <summary>
        ///     Puts the table.
        /// </summary>
        /// <param name="table">The table.</param>
        public static void PutTable(string table)
        {
            ZarahDBModel.PutTable(table);
        }

        /// <summary>
        ///     Gets the table.
        /// </summary>
        /// <returns>System.String</returns>
        public static string GetTable()
        {
            return ZarahDBModel.GetTable();
        }

        #endregion

        #region MaxDepth

        public static StatusMessageValue GetMaxDepth(Uri instance, string table)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            return DataAccessLayer.GetMaxDepth(instance, table);
        }

        public static StatusMessageValue SetMaxDepth(Uri instance, string table, int maxDepth)
        {
            if (instance == null)
            {
                instance = ZarahDBModel.GetInstance();
            }
            if (table == null)
            {
                table = ZarahDBModel.GetTable();
            }
            return DataAccessLayer.SetMaxDepth(instance, table, maxDepth);
        }

        #endregion

        #endregion
    }
}