// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 07-05-2015
//
// Last Modified By : Mike.Reed
// Last Modified On : 08-28-2015
// ***********************************************************************
// <copyright file="FileAccessLayer.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2015 Benchmark Solutions LLC
// </copyright>
// <summary>
// This is the FAL (File Access Layer).
// This class contains all methods which access or modify files within the file system.
// This class is exposed internally, with no public methods. Some private methods contain 
// common routines used across the FAL.
// This class is aware of the file system itself.
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZarahDB_Library.Enums;
using ZarahDB_Library.Helpers;
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

    internal static class FileAccessLayer
    {
        #region Key Index

        /// <summary>
        ///     Creates the index. The index is a fully qualified path to the key's json file.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.String</returns>
        internal static string CreateKeyIndex(Uri instance, string table, string key)
        {
            var index = instance.AbsolutePath;
            table = DirectoryHelper.CreateLegalDirectoryName(table, key);
            if (!string.IsNullOrEmpty(table)) index = Path.Combine(index, table);

            var fileName = CreateLegalJSONFilename(key);

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (!string.IsNullOrEmpty(fileNameWithoutExtension))
            {
                var nameLength = fileNameWithoutExtension.Length;
                var zdbFolders = "";

                var maxDepth = GetMaxDepth(instance, table);
                var depth = Math.Min(nameLength, maxDepth - 1);
                for (var i = depth; i >= 0; i--)
                {
                    if (nameLength > i) zdbFolders = fileName.Substring(i, 1) + @"\" + zdbFolders;
                }

                zdbFolders = zdbFolders.Replace(" ", "_");
                index = Path.Combine(index, zdbFolders);
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                index = Path.Combine(index, fileName);
            }

            index = StringHelper.ReplaceEx(index, @"\__\", @"\");
            return index;
        }

        #endregion

        #region Insert

        private static bool InsertKey(string index, KeyColumnValues keyColumnValues)
        {
            var keyColumnValueses = new List<KeyColumnValues> {keyColumnValues};
            try
            {
                var json = JsonConvert.SerializeObject(keyColumnValueses);
                json = "{ " + $"\"Keys\":{json}}}";

                DirectoryHelper.AssureDirectoryExists(Path.GetDirectoryName(index));
                File.WriteAllText(index, json);
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        #region JSON

        /// <summary>
        ///     Creates the legal json filename.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String</returns>
        internal static string CreateLegalJSONFilename(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return key;
            }

            //Make the cleaned text all lower case
            var legalJSONFilename = key.ToLowerInvariant();

            //Remove illegal characters
            var regexRemovePunctuation =
                new Regex(@"[\`!@#$%^&*\(\)_\+\-=\{\}\[\]\|\\:;\" + "\"" + @"\'<>,.?\™\©\…\“\‘\-\.\0]");
            legalJSONFilename = regexRemovePunctuation.Replace(legalJSONFilename, "");

            //Only process if there is something to process
            if (string.IsNullOrEmpty(legalJSONFilename.Trim())) return "null.json";

            //Remove Linefeed
            legalJSONFilename = StringHelper.ReplaceEx(legalJSONFilename, "\n", " ");

            //Remove Carriage Return
            legalJSONFilename = StringHelper.ReplaceEx(legalJSONFilename, "\r", " ");

            //Remove Tab
            legalJSONFilename = StringHelper.ReplaceEx(legalJSONFilename, "\t", " ");

            //Remove Backslash
            legalJSONFilename = StringHelper.ReplaceEx(legalJSONFilename, "\\", " ");

            //Remove Foreword Slash
            legalJSONFilename = StringHelper.ReplaceEx(legalJSONFilename, @"/", " ");

            //Remove any extra spaces
            while (legalJSONFilename.Contains("  "))
            {
                legalJSONFilename = legalJSONFilename.Replace("  ", " ");
            }

            try
            {
                legalJSONFilename = Path.ChangeExtension(legalJSONFilename, ".json");
            }
            catch (ArgumentException)
            {
                return "invalid.JSON";
            }

            //After processing, make sure we still have a filename
            if (string.IsNullOrEmpty(legalJSONFilename.Trim())) return "null.json";

            return legalJSONFilename;
        }

        #endregion

        #region Create

        public static StatusMessageValue CreateInstance(Uri instance)
        {
            StatusMessageValue statusMessageValue;
            try
            {
                var alreadyExists = Exists(instance);
                if (alreadyExists)
                {
                    statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Internal_Server_Error,
                        "Instance already exists");
                }
                else
                {
                    DirectoryHelper.AssureDirectoryExists(instance.AbsolutePath);
                    statusMessageValue = Exists(instance)
                        ? StatusHelper.SetStatusMessageValue(StatusCode.OK, "")
                        : StatusHelper.SetStatusMessageValue(StatusCode.Internal_Server_Error, "Instance not created");
                }
            }
            catch (Exception e)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Exception, e.Message);
            }

            return statusMessageValue;
        }

        public static StatusMessageValue CreateTable(Uri instance, int maxDepth)
        {
            return null;
        }

        #endregion

        #region List

        public static StatusList ListInstance(string instancesRootFolder)
        {
            StatusList statusList;
            try
            {
                var childFolders = DirectoryHelper.ChildInstances(instancesRootFolder);
                statusList = StatusHelper.SetStatusList(StatusCode.OK, childFolders);
            }
            catch (Exception e)
            {
                statusList = StatusHelper.SetStatusList(StatusCode.Exception.ToString(), e.Message, "0");
            }
            return statusList;
        }

        public static StatusList ListTable(Uri instance, int? timeoutSeconds)
        {
            StatusList statusList;
            try
            {
                var childFolders = DirectoryHelper.ChildFolders(instance.AbsolutePath);
                statusList = StatusHelper.SetStatusList(StatusCode.OK, childFolders);
            }
            catch (Exception e)
            {
                statusList = StatusHelper.SetStatusList(StatusCode.Exception.ToString(), e.Message, "0");
            }
            return statusList;
        }

        #endregion

        #region Backup

        /// <summary>
        ///     Back up the specified instance. This creates a .zip file in the root folder of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        internal static StatusMessageValue Backup(Uri instance)
        {
            StatusMessageValue statusMessageValue;
            try
            {
                //Get the root path to the instance
                var index = CreateKeyIndex(instance, null, null);

                //Remove any existing backups
                const string backupWildcard = "*.zip";
                var dir = new DirectoryInfo(index);
                foreach (var file in dir.EnumerateFiles(backupWildcard))
                {
                    file.Delete();
                }

                //Create a new zip file in a temp directory
                var tempFilename = Path.GetTempFileName();
                // ReSharper disable StringLiteralTypo
                const string timestampformat = "yyyyMMddhhmmssffff";
                // ReSharper restore StringLiteralTypo
                var now = DateTime.UtcNow.ToString(timestampformat);
                var finalFilename = $"{Path.GetFileName(instance.AbsolutePath)}_{now}.zip";
                var backupFilePath = Path.Combine(index, finalFilename);
                File.Delete(tempFilename);
                ZipFile.CreateFromDirectory(index, tempFilename, CompressionLevel.Optimal, false);

                //Move the backup into the root of the instance
                File.Move(tempFilename, backupFilePath);

                //Return success and the path to the backup file
                statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.OK, backupFilePath);
            }
            catch (Exception e)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Exception, e.Message);
            }
            return statusMessageValue;
        }

        /// <summary>
        ///     Restores the specified instance from an instance backup .zip file.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="instanceBackup">The instance backup .zip file.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        internal static StatusMessageValue Restore(Uri instance, Uri instanceBackup)
        {
            StatusMessageValue statusMessageValue;
            try
            {
                var tempFilename = Path.GetTempFileName();
                File.Delete(tempFilename);
                File.Move(instanceBackup.AbsolutePath, tempFilename);
                DirectoryHelper.DeleteDirectory(instance.AbsolutePath);
                var index = CreateKeyIndex(instance, null, null);
                ZipFile.ExtractToDirectory(tempFilename, instance.AbsolutePath);
                File.Move(tempFilename, instanceBackup.AbsolutePath);
                statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.OK, index);
            }
            catch (Exception ex)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Exception, ex.Message);
            }
            return statusMessageValue;
        }

        #endregion

        #region CSV

        public static StatusMessageValue CsvPut(string csvData, List<string> columns, string keyColumn,
            string fieldSeparator, string encloser, string lineTerminator, string commentLineStarter, Uri instance,
            string table, int timeoutSeconds)
        {
            StatusMessageValue statusMessageValue;

            DirectoryHelper.AssureDirectoryExists(instance.AbsolutePath);

            var strReader = new StringReader(csvData);
            while (true)
            {
                string line;
                try
                {
                    line = strReader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Exception, ex.Message);
                    return statusMessageValue;
                }

                try
                {
                    if (line.StartsWith(commentLineStarter)) continue;
                    if (!columns.Any())
                    {
                        columns = ParseCsvLine(line, fieldSeparator, encloser, lineTerminator, commentLineStarter);
                        continue;
                    }
                    var values = ParseCsvLine(line, fieldSeparator, encloser, lineTerminator, commentLineStarter);
                    if (columns.Count() != values.Count())
                    {
                        statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Internal_Server_Error,
                            "Different number of columns and values.");
                        return statusMessageValue;
                    }
                    PutColumns(columns, keyColumn, values, instance, table, timeoutSeconds);
                }
                catch (Exception ex)
                {
                    statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Exception, ex.Message);
                    return statusMessageValue;
                }
            }

            statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.OK, instance.AbsolutePath);
            return statusMessageValue;
        }

        public static StatusMessageValue CsvFilePut(Uri csvFile, List<string> columns, string keyColumn,
            string fieldSeparator, string encloser, string lineTerminator, string commentLineStarter, Uri instance,
            string table, int timeoutSeconds)
        {
            var statusMessageValue = new StatusMessageValue();
            StatusHelper.SetStartTicks(statusMessageValue);

            DirectoryHelper.AssureDirectoryExists(instance.AbsolutePath);

            var reader = new StreamReader(File.OpenRead(csvFile.AbsolutePath));
            while (!reader.EndOfStream)
            {
                string line;
                try
                {
                    line = reader.ReadLine();
                }
                catch (Exception ex)
                {
                    statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Exception, ex.Message);
                    return statusMessageValue;
                }

                try
                {
                    if (line == null || line.StartsWith(commentLineStarter)) continue;
                    if (!columns.Any())
                    {
                        columns = ParseCsvLine(line, fieldSeparator, encloser, lineTerminator, commentLineStarter);
                        continue;
                    }
                    var values = ParseCsvLine(line, fieldSeparator, encloser, lineTerminator, commentLineStarter);
                    if (columns.Count() != values.Count())
                    {
                        statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Internal_Server_Error,
                            "Different number of columns and values.");
                        return statusMessageValue;
                    }
                    PutColumns(columns, keyColumn, values, instance, table, timeoutSeconds);
                }
                catch (Exception ex)
                {
                    statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Exception, ex.Message);
                    return statusMessageValue;
                }
            }

            statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.OK, instance.AbsolutePath);
            return statusMessageValue;
        }

        private static List<string> ParseCsvLine(string line, string fieldSeparator, string encloser,
            string lineTerminator, string commentLineStarter)
        {
            var newLineValues = new List<string>();
            if (line.StartsWith(commentLineStarter)) return newLineValues;

            try
            {
                var nextFieldSeparatorLength = fieldSeparator.Length;
                var cursor = 0;
                while (cursor < line.Length)
                {
                    var nextFieldSeparator = line.IndexOf(fieldSeparator, cursor, StringComparison.Ordinal);
                    if (nextFieldSeparator < cursor)
                    {
                        nextFieldSeparator = line.IndexOf(lineTerminator, StringComparison.Ordinal);
                        nextFieldSeparatorLength = lineTerminator.Length;
                    }
                    if (nextFieldSeparator < cursor)
                    {
                        nextFieldSeparator = line.Length;
                        nextFieldSeparatorLength = 0;
                    }
                    var newLineValue = line.Substring(cursor, nextFieldSeparator - cursor);
                    if (!string.IsNullOrEmpty(encloser))
                    {
                        if (newLineValue.StartsWith(encloser))
                        {
                            if (newLineValue.EndsWith(encloser))
                            {
                                newLineValue = newLineValue.Substring(encloser.Length,
                                    newLineValue.Length - (encloser.Length*2));
                            }
                        }
                    }
                    newLineValues.Add(newLineValue);
                    cursor = nextFieldSeparator + nextFieldSeparatorLength;
                }
                return newLineValues;
            }
            catch (Exception)
            {
                return newLineValues;
            }
        }

        public static bool CsvFileInsert(Uri csvFile, List<string> columns, string keyColumn, string fieldSeparator,
            string encloser, string lineTerminator, string commentLineStarter, Uri instance, string table)
        {
            DirectoryHelper.AssureDirectoryExists(instance.AbsolutePath);

            var keyIndex = -1;
            var keyColumnValueses = new KeyColumnValues();
            var reader = new StreamReader(File.OpenRead(csvFile.AbsolutePath));
            while (!reader.EndOfStream)
            {
                string line;
                try
                {
                    line = reader.ReadLine();
                }
                catch
                {
                    return false;
                }

                try
                {
                    if (line == null || line.StartsWith(commentLineStarter)) continue;
                    if (!columns.Any())
                    {
                        columns = ParseCsvLine(line, fieldSeparator, encloser, lineTerminator, commentLineStarter);
                        continue;
                    }
                    if (keyIndex == -1)
                    {
                        for (var i = 0; i < columns.Count; i++)
                        {
                            if (!keyColumn.Equals(columns[i], StringComparison.OrdinalIgnoreCase)) continue;
                            keyIndex = i;
                            break;
                        }
                    }
                    var values = ParseCsvLine(line, fieldSeparator, encloser, lineTerminator, commentLineStarter);
                    if (columns.Count() != values.Count())
                    {
                        return false;
                    }

                    var now = StringHelper.NowTicks();

                    keyColumnValueses.ColumnValues.Clear();

                    for (var i = 0; i < columns.Count; i++)
                    {
                        var newColumnValue = new ColumnValue
                        {
                            Column = columns[i],
                            Value = values[i],
                            Updated = now
                        };
                        keyColumnValueses.ColumnValues.Add(newColumnValue);
                    }

                    keyColumnValueses.Key = values[keyIndex];
                    var index = CreateKeyIndex(instance, table, keyColumnValueses.Key);

                    var result = InsertKey(index, keyColumnValueses);
                    if (!result)
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Delete

        /// <summary>
        ///     Deletes the instance.
        ///     This removes all data from the instance, but the root folder of the instance and any backups remain.
        ///     Any transaction folders are retained.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>System.Boolean</returns>
        internal static StatusMessageValue DeleteInstance(Uri instance)
        {
            StatusMessageValue statusMessageValue;
            try
            {
                var index = CreateKeyIndex(instance, null, null);
                const string directoryWildcard = "*.";
                var dir = new DirectoryInfo(index);

                //Delete the instance contents
                foreach (var folder in dir.EnumerateDirectories(directoryWildcard, SearchOption.TopDirectoryOnly))
                {
                    //TODO: Add a comment here to describe why "transaction_" is checked for.
                    if (folder.FullName.IndexOf("transaction_", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        DirectoryHelper.DeleteDirectory(folder.FullName);
                    }
                }

                //Delete the instance folder
                DirectoryHelper.DeleteDirectory(index);

                //Return success and the path to the backup file
                statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.OK, dir.Name);
            }
            catch (Exception e)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.Exception, e.Message);
            }

            return statusMessageValue;
        }

        /// <summary>
        ///     Deletes the table. The folder representing the table and all folders and files it contains are deleted.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <returns>System.Boolean</returns>
        internal static bool DeleteTable(Uri instance, string table)
        {
            var index = CreateKeyIndex(instance, null, null);
            try
            {
                var dir = new DirectoryInfo(index);
                foreach (var folder in dir.EnumerateDirectories(table, SearchOption.TopDirectoryOnly))
                {
                    Directory.Delete(folder.FullName, true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Exists

        /// <summary>
        ///     Tests if the specified instance exists.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>System.Boolean</returns>
        internal static bool Exists(Uri instance)
        {
            try
            {
                return DirectoryHelper.DirectoryExists(instance.AbsolutePath);
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
        internal static bool Exists(Uri instance, string table)
        {
            var index = CreateKeyIndex(instance, table, "");
            try
            {
                var result = DirectoryHelper.DirectoryExists(index);
                return result;
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
        /// <returns><c>true</c> if the key exists, <c>false</c> if it does not.</returns>
        internal static bool Exists(Uri instance, string table, string key, bool? checkExactMatch = true)
        {
            var index = CreateKeyIndex(instance, table, key);
            if (key == null)
            {
                return false;
            }
            try
            {
                if (checkExactMatch == false)
                {
                    return File.Exists(index);
                }
                var indexKey = Path.GetFileNameWithoutExtension(index);
                if (string.IsNullOrEmpty(indexKey))
                {
                    return false;
                }
                if (indexKey.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return File.Exists(index);
                }

                var keyColumnValues = GetAllKeyColumnValues(index);
                if (keyColumnValues == null)
                {
                    return false;
                }
                foreach (var keyColumnValue in keyColumnValues)
                {
                    if (keyColumnValue.Key == key)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Tests if any of a list of keys exist.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="keyList">The keys.</param>
        /// <param name="checkExactMatch">
        ///     Key collisions are possible. if set to <c>true</c> the file is checked to contain the
        ///     exact key if there is any chance of collision. Set this to <c>false</c> for the fastest execution when key
        ///     collisions are known not to exist.
        /// </param>
        /// <returns><c>true</c> if the key exists, <c>false</c> if it does not.</returns>
        internal static KeyList Exists(Uri instance, string table, KeyList keyList, bool? checkExactMatch = true)
        {
            var newKeys = new KeyList();
            foreach (var key in keyList.Keys)
            {
                if (Exists(instance, table, key, checkExactMatch))
                {
                    newKeys.Keys.Add(key);
                }
            }
            return newKeys;
        }

        #endregion

        #region Get

        /// <summary>
        ///     Gets all key column values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.Collections.Generic.List&lt;ZarahDB_Library.KeyColumnValues&gt;</returns>
        internal static List<KeyColumnValues> GetAllKeyColumnValues(Uri instance, string table, string key)
        {
            var index = CreateKeyIndex(instance, table, key);
            return GetAllKeyColumnValues(index);
        }

        /// <summary>
        ///     Gets all key column values. If there is a key collision, it may return multiple keys from a single json file.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>System.Collections.Generic.List&lt;ZarahDB_Library.KeyColumnValues&gt;</returns>
        private static List<KeyColumnValues> GetAllKeyColumnValues(string index)
        {
            var keyColumnValues = new List<KeyColumnValues>();

            if (!File.Exists(index)) return keyColumnValues;

            var json = File.ReadAllText(index);
            var keys = JObject.Parse(json)["Keys"];

            foreach (var key in keys)
            {
                var newKeyColumnValues = new KeyColumnValues {Key = key["Key"].ToString()};
                foreach (var columnValue in key["ColumnValues"])
                {
                    var newColumnValue = new ColumnValue
                    {
                        Column = columnValue["Column"].ToString(),
                        Value = columnValue["Value"].ToString(),
                        PreviousValue = columnValue["PreviousValue"].ToString(),
                        Updated = columnValue["Updated"].ToString()
                    };
                    newKeyColumnValues.ColumnValues.Add(newColumnValue);
                }
                keyColumnValues.Add(newKeyColumnValues);
            }

            return keyColumnValues;
        }

        #endregion

        #region Value Indexes

        private static void UpdateIndexes(Uri instance, string table, string key,
            IEnumerable<KeyColumnValues> keyColumnValueses)
        {
            //Key is required for an index
            if (string.IsNullOrEmpty(key)) return;

            //Find all existing indexes
            var indexFolder = CreateKeyIndex(instance, table, "");
            indexFolder = Path.Combine(indexFolder, "Index");
            DirectoryHelper.AssureDirectoryExists(indexFolder);
            var indexes = new List<string>(Directory.EnumerateDirectories(indexFolder));
            if (!indexes.Any()) return;

            //Build a list of existing indexes with composite indexes broken down to individual columns
            var indexColumnKeyValues = new List<IndexColumnKeyValues>();
            foreach (var index in indexes)
            {
                var fi = new FileInfo(index);
                var indexColumn = fi.Name;
                var indexColumns = indexColumn.Split(new[] {'~'}, StringSplitOptions.RemoveEmptyEntries);
                var newIndexColumnKeyValues = new IndexColumnKeyValues();
                foreach (var column in indexColumns)
                {
                    newIndexColumnKeyValues.Index = index;
                    newIndexColumnKeyValues.IndexColumn = indexColumn;
                    var newColumnKeyValues = new ColumnKeyValue {Column = column};
                    newIndexColumnKeyValues.ColumnKeyValues.Add(newColumnKeyValues);
                }
                indexColumnKeyValues.Add(newIndexColumnKeyValues);
            }

            var keyColumnValuesArray = keyColumnValueses as KeyColumnValues[] ?? keyColumnValueses.ToArray();
            var activeIndexes = new List<IndexColumnKeyValues>();
            foreach (var indexColumnKeyValue in indexColumnKeyValues)
            {
                foreach (var columnKeyValue in indexColumnKeyValue.ColumnKeyValues)
                {
                    foreach (var keyColumnValues in keyColumnValuesArray)
                    {
                        foreach (var columnValue in keyColumnValues.ColumnValues)
                        {
                            if (columnKeyValue.Column != DirectoryHelper.CreateLegalDirectoryName(columnValue.Column))
                                continue;
                            columnKeyValue.Key = keyColumnValues.Key;
                            columnKeyValue.Column = columnValue.Column;
                            columnKeyValue.Value = columnValue.Value;
                        }
                    }
                }
                var found = true;
                foreach (var columnKeyValue in indexColumnKeyValue.ColumnKeyValues)
                {
                    if (!string.IsNullOrEmpty(columnKeyValue.Key)) continue;
                    found = false;
                    break;
                }
                if (found)
                {
                    activeIndexes.Add(indexColumnKeyValue);
                }
            }

            if (!activeIndexes.Any()) return;

            UpdateIndex(activeIndexes);
        }

        private static void UpdateIndex(List<IndexColumnKeyValues> indexColumnKeyValueses)
        {
            var newValueKeys = new ValueKeys();

            foreach (var indexColumnKeyValues in indexColumnKeyValueses)
            {
                foreach (var indexColumnKeyValue in indexColumnKeyValues.ColumnKeyValues)
                {
                    var newForeinKeyReference = new ForeignKeyReference();
                    var indexFile = CreateKeyIndex(new Uri(indexColumnKeyValues.Index), "", indexColumnKeyValue.Value);
                    if (File.Exists(indexFile))
                    {
                        //TODO: Mike: Work: load existing index and update it 

                        //Read existing index file

                        //Check for matching Foreign Key (FK)

                        //If FK found, then update

                        //If FK not found, add it

                        //Write updated index file to disk

                        continue;
                    }

                    DeleteForeignKey(indexColumnKeyValue);

                    //Create new Index
                    newForeinKeyReference.Key = indexColumnKeyValue.Key;
                    newForeinKeyReference.Updated = StringHelper.NowTicks();
                    newValueKeys.Value = indexColumnKeyValue.Value;
                    newValueKeys.Keys.Clear();
                    newValueKeys.Keys.Add(newForeinKeyReference);
                    var json = JsonConvert.SerializeObject(newValueKeys);
                    json = "{ " + $"\"Values\":{json}}}";
                    DirectoryHelper.AssureDirectoryExists(Path.GetDirectoryName(indexFile));
                    File.WriteAllText(indexFile, json);
                }
            }
        }

        private static void DeleteForeignKey(ColumnKeyValue indexColumnKeyValue)
        {
            if (indexColumnKeyValue == null) throw new ArgumentNullException(nameof(indexColumnKeyValue));
            //TODO: Mike: Work: Delete the old index entry, which related to the previous value.
        }

        #endregion

        #region Locks

        /// <summary>
        ///     Tests if the specified Key is locked.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.Boolean</returns>
        internal static bool KeyLocked(Uri instance, string table, string key)
        {
            var index = CreateKeyIndex(instance, table, key);
            if (!File.Exists(index))
            {
                return false;
            }
            var fi = new FileInfo(index);
            return !fi.IsReadOnly;
        }

        /// <summary>
        ///     Locks the key. This removes the Read-Only attribute from the Key's json file.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        internal static void LockKey(Uri instance, string table, string key)
        {
            var index = CreateKeyIndex(instance, table, key);
            if (!File.Exists(index))
            {
                return;
            }
            try
            {
                // ReSharper disable once UseObjectOrCollectionInitializer
                var fi = new FileInfo(index);
                fi.IsReadOnly = false;
            }
            catch (Exception e)
            {
                throw new ApplicationException(@"Failed to lock {index}", e);
            }
        }

        /// <summary>
        ///     Unlocks the key. This sets the Read-Only attribute on the Key's json file.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        internal static void UnlockKey(Uri instance, string table, string key)
        {
            var index = CreateKeyIndex(instance, table, key);
            UnlockKey(index);
        }

        internal static void UnlockKey(string index)
        {
            try
            {
                // ReSharper disable once UseObjectOrCollectionInitializer
                var fi = new FileInfo(index);
                fi.IsReadOnly = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(@"Failed to lock {index}", e);
            }
        }

        /// <summary>
        ///     Tests if the Instances is locked.
        ///     This is done by checking for the existence of an "instance.locked" file in the root folder of the instance
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>System.Boolean</returns>
        internal static bool InstanceLocked(Uri instance)
        {
            var index = CreateKeyIndex(instance, null, null);
            if (!Directory.Exists(index))
            {
                return false;
            }
            return File.Exists(Path.Combine(index, "instance.locked"));
        }

        /// <summary>
        ///     Locks the instance. This creates an "instance.locked" file in the root of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        internal static void LockInstance(Uri instance)
        {
            var index = CreateKeyIndex(instance, null, null);
            DirectoryHelper.AssureDirectoryExists(index);
            if (!Directory.Exists(index))
            {
                return;
            }
            try
            {
                File.Create(Path.Combine(index, "instance.locked")).Close();
            }
            catch (Exception e)
            {
                throw new ApplicationException(@"Failed to lock {index}", e);
            }
        }

        /// <summary>
        ///     Unlocks the instance. This deletes any existing "instance.locked" file in the root of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        internal static void UnlockInstance(Uri instance)
        {
            var index = CreateKeyIndex(instance, null, null);
            try
            {
                var lockFilename = Path.Combine(index, "instance.locked");
                if (File.Exists(lockFilename))
                {
                    File.Delete(lockFilename);
                }
            }
            catch (Exception e)
            {
                //TODO: Mike: Consider: Is this the pattern to follow for real exceptions?
                throw new ApplicationException(@"Failed to unlock {index}", e);
            }
        }

        /// <summary>
        ///     Gets the instance lock status.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetInstanceLock(Uri instance)
        {
            var index = CreateKeyIndex(instance, null, null);
            try
            {
                var lockFilename = Path.Combine(index, "instance.locked");
                return File.Exists(lockFilename);
            }
            catch (Exception e)
            {
                //TODO: Mike: Consider: Is this the pattern to follow for real exceptions?
                throw new ApplicationException(@"Failed to unlock {index}", e);
            }
        }

        /// <summary>
        ///     Locks a table. This creates a "table.locked" file in the root of a table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        internal static void LockTable(Uri instance, string table)
        {
            //TODO: Mike: Work: Implement
            //var index = CreateIndex(instance, null, null);
            //DirectoryHelper.AssureDirectoryExists(index);
            //if (!Directory.Exists(index))
            //{
            //    return;
            //}
            //try
            //{
            //    File.Create(Path.Combine(index, "instance.locked")).Close();
            //}
            //catch (Exception e)
            //{
            //    throw new ApplicationException(@"Failed to lock {index}", e);
            //}
        }

        /// <summary>
        ///     Unlocks a table. This deletes any existing "table.locked" file in the root of a table.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        internal static void UnlockTable(Uri instance, string table)
        {
            //TODO: Mike: Work: Implement - Handle the lock file and update any PUT operations to check for it
            //var index = CreateIndex(instance, null, null);
            //try
            //{
            //    var lockFilename = Path.Combine(index, "instance.locked");
            //    if (File.Exists(lockFilename))
            //    {
            //        File.Delete(lockFilename);
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw new ApplicationException(@"Failed to unlock {index}", e);
            //}
        }

        #endregion

        #region MaxDepth

        internal static int GetMaxDepth(Uri instance, string table)
        {
            var index = CreateKeyIndex(instance, table, null);
            var maxDepthFilename = Path.Combine(index, "Max.Depth");
            if (File.Exists(maxDepthFilename))
            {
                using (TextReader reader = File.OpenText(maxDepthFilename))
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        return int.Parse(line);
                    }
                }
            }
            SetMaxDepth(instance, table, 5);
            return 5;
        }

        internal static void SetMaxDepth(Uri instance, string table, int maxDepth)
        {
            DirectoryHelper.AssureDirectoryExists(instance.AbsolutePath);
            var index = CreateKeyIndex(instance, table, null);
            if (index != instance.AbsolutePath)
            {
                DirectoryHelper.AssureDirectoryExists(index);
            }
            var maxDepthFilename = Path.Combine(index, "Max.Depth");
            if (File.Exists(maxDepthFilename))
            {
                if (index != instance.AbsolutePath)
                {
                    return;
                }
            }
            File.WriteAllText(maxDepthFilename, maxDepth.ToString());
        }

        #endregion

        #region Put

        /// <summary>
        ///     Puts all key column values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <param name="keyColumnValueses">The key column valueses.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        internal static StatusMessageValue PutAllKeyColumnValues(Uri instance, string table, string key,
            List<KeyColumnValues> keyColumnValueses)
        {
            var index = CreateKeyIndex(instance, table, key);
            UpdateIndexes(instance, table, key, keyColumnValueses);
            return PutAllKeyColumnValues(index, keyColumnValueses);
        }

        /// <summary>
        ///     Puts all key column values.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="keyColumnValueses">
        ///     The key column values. This can put multiple keys as long as there is a valid key
        ///     collision.
        /// </param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        internal static StatusMessageValue PutAllKeyColumnValues(string index, List<KeyColumnValues> keyColumnValueses)
        {
            StatusMessageValue statusMessageValue;
            try
            {
                var json = JsonConvert.SerializeObject(keyColumnValueses);
                json = "{ " + $"\"Keys\":{json}}}";

                DirectoryHelper.AssureDirectoryExists(Path.GetDirectoryName(index));
                File.WriteAllText(index, json);
            }
            catch (Exception e)
            {
                statusMessageValue = StatusHelper.SetStatusMessageValue(e.HResult.ToString(), e.Message, "");
                return statusMessageValue;
            }

            statusMessageValue = StatusHelper.SetStatusMessageValue(StatusCode.OK);
            return statusMessageValue;
        }

        internal static void PutColumns(IReadOnlyList<string> columns, string keyColumn, IReadOnlyList<string> values,
            Uri instance, string table, int timeoutSeconds)
        {
            var newColumnValues = new List<ColumnValue>();
            var key = "";
            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                if (column.Equals(keyColumn, StringComparison.OrdinalIgnoreCase))
                {
                    key = values[i];
                }
                var newColumnValue = new ColumnValue
                {
                    Column = column,
                    Value = values[i]
                };
                newColumnValues.Add(newColumnValue);
            }
            foreach (var newColumnValue in newColumnValues)
            {
                var result = Put(instance, table, key, newColumnValue.Column, newColumnValue.Value, timeoutSeconds);
                if (result.Status != "200")
                {
                    break;
                }
            }
        }

        internal static StatusMessageValue Put(Uri instance, string table, string key, string column, string value,
            int timeoutSeconds)
        {
            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(timeoutSeconds);
            while (KeyLocked(instance, table, key) || InstanceLocked(instance))
            {
                if (DateTime.Now > endTime)
                {
                    var statusValue = new StatusMessageValue
                    {
                        Status = "597",
                        Value = $"Network timeout error (Persistant Lock for more than {timeoutSeconds} seconds.)"
                    };
                    return statusValue;
                }
                Thread.Sleep(100);
            }
            LockKey(instance, table, key);

            var keyColumnValueses = GetAllKeyColumnValues(instance, table, key);

            var ticks = StringHelper.NowTicks();

            var valueSet = false;
            var keyExists = false;
            foreach (var keyColumnValues in keyColumnValueses)
            {
                if (keyColumnValues.Key == null) continue;

                if (keyColumnValues.Key != key) continue;

                //The unique key exists
                keyExists = true;

                foreach (var columnValue in keyColumnValues.ColumnValues)
                {
                    if (columnValue.Column != column) continue;

                    //Check that we don't have an outside lock
                    if (string.IsNullOrEmpty(ticks) ||
                        string.Compare(columnValue.Updated, ticks, StringComparison.Ordinal) > 0)
                    {
                        return StatusHelper.SetStatusMessageValue(StatusCode.Outside_Lock);
                    }

                    //The column already exists, so update it
                    columnValue.Value = value;
                    valueSet = true;
                    break;
                }
            }

            if (!valueSet)
            {
                if (!keyExists)
                {
                    //Create a new key
                    var newKeyColumnValues = new KeyColumnValues {Key = key};
                    var newColumnValues = new ColumnValue
                    {
                        Column = column,
                        Value = value,
                        Updated = ticks
                    };
                    newKeyColumnValues.ColumnValues.Add(newColumnValues);
                    keyColumnValueses.Add(newKeyColumnValues);
                }
                else
                {
                    foreach (var keyColumnValues in keyColumnValueses)
                    {
                        if (keyColumnValues.Key == null) continue;

                        if (keyColumnValues.Key != key) continue;

                        var newColumnValues = new ColumnValue
                        {
                            Column = column,
                            Value = value,
                            Updated = ticks
                        };
                        keyColumnValues.ColumnValues.Add(newColumnValues);
                    }
                }
            }

            var result = PutAllKeyColumnValues(instance, table, key, keyColumnValueses);

            UnlockKey(instance, table, key);

            return result;
        }

        internal static StatusMessageValue Put(Uri instance, string table, KeyColumnValues keyColumnValues,
            int timeoutSeconds)
        {
            var newStatusMessageValue = new StatusMessageValue();
            foreach (var columnValue in keyColumnValues.ColumnValues)
            {
                newStatusMessageValue = Put(instance, table, keyColumnValues.Key, columnValue.Column, columnValue.Value,
                    timeoutSeconds);
            }
            //TODO: Mike: Work: Convert the next method to accept keyColumnValue, check the ticks and call from here.
            return newStatusMessageValue;
        }

        #endregion

        #region Script

        public static bool PutScript(Uri instance, string scriptName, string script)
        {
            if (string.IsNullOrEmpty(instance.AbsolutePath))
            {
                return false;
            }
            if (string.IsNullOrEmpty(scriptName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(script))
            {
                return false;
            }
            var indexFolder = CreateScriptIndex(instance, scriptName);
            DirectoryHelper.AssureDirectoryExists(indexFolder);
            var filename = $"{scriptName}.script";
            var indexFile = Path.Combine(indexFolder, filename);
            File.WriteAllText(indexFile, script);
            return true;
        }

        public static string GetScript(Uri instance, string scriptName)
        {
            if (string.IsNullOrEmpty(instance.AbsolutePath))
            {
                return "";
            }
            if (string.IsNullOrEmpty(scriptName))
            {
                return "";
            }
            var indexFolder = CreateScriptIndex(instance, scriptName);
            DirectoryHelper.AssureDirectoryExists(indexFolder);
            var filename = $"{scriptName}.script";
            var indexFile = Path.Combine(indexFolder, filename);
            var script = File.ReadAllText(indexFile);
            return script;
        }

        /// <summary>
        ///     Creates the index. The index is a fully qualified path to the key's json file.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="scriptName">Name of the script.</param>
        /// <returns>System.String</returns>
        internal static string CreateScriptIndex(Uri instance, string scriptName)
        {
            var index = instance.AbsolutePath;
            index = Path.Combine(index, "_Script_");

            var fileName = $"{scriptName}.script";

            if (!string.IsNullOrEmpty(scriptName))
            {
                var nameLength = scriptName.Length;
                var zdbFolders = "";

                var maxDepth = GetMaxDepth(instance, "");
                var depth = Math.Min(nameLength, maxDepth - 1);
                for (var i = depth; i >= 0; i--)
                {
                    if (nameLength > i) zdbFolders = fileName.Substring(i, 1) + @"\" + zdbFolders;
                }

                zdbFolders = zdbFolders.Replace(" ", "_");
                index = Path.Combine(index, zdbFolders);
            }
            index = StringHelper.ReplaceEx(index, @"\__\", @"\");
            return index;
        }

        #endregion

        #region Transaction

        /// <summary>
        ///     Begins the transaction.
        ///     Creates a folder at the root of the instance to support the transaction.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <returns>ZarahDB_Library.StatusTransaction</returns>
        internal static StatusTransaction BeginTransaction(StatusTransaction statusTransaction)
        {
            // ReSharper disable StringLiteralTypo
            const string timestampformat = "yyyyMMddhhmmssffff";
            // ReSharper restore StringLiteralTypo
            var now = DateTime.UtcNow.ToString(timestampformat);
            statusTransaction.TransactionTimestamp = now;
            statusTransaction.Status = nameof(StatusCode.OK);
            statusTransaction.Message = nameof(TransactionStatus.Active);
            statusTransaction.TransactionIndex = Path.Combine(statusTransaction.Instance.AbsolutePath,
                $"Transaction_{statusTransaction.TransactionTimestamp}");
            DirectoryHelper.AssureDirectoryExists(statusTransaction.TransactionIndex);
            return statusTransaction;
        }

        /// <summary>
        ///     Commits the transaction.
        ///     Copies the structure in the transaction to the instance, then removes the transaction folder.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <returns>ZarahDB_Library.StatusTransaction</returns>
        internal static StatusTransaction CommitTransaction(StatusTransaction statusTransaction)
        {
            DirectoryHelper.MoveDirectory(statusTransaction.TransactionIndex, statusTransaction.Instance.AbsolutePath);
            foreach (var lockedInstanceKey in statusTransaction.LockedInstanceKeys)
            {
                UnlockKey(lockedInstanceKey);
            }
            //TODO: Mike: Consider: Should we return to the caller the fill list of locked keys? If not, clear them.
            //statusTransaction.LockedInstanceKeys.Clear();
            StatusHelper.SetCommandAndTransactionStatus(statusTransaction, StatusCode.OK, TransactionStatus.Committed,
                StatusCode.OK);
            return statusTransaction;
        }

        /// <summary>
        ///     Rollbacks the transaction.
        ///     Removes the transaction folder and it's contents without affecting the instance.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <returns>ZarahDB_Library.StatusTransaction</returns>
        internal static StatusTransaction RollbackTransaction(StatusTransaction statusTransaction)
        {
            foreach (var lockedInstanceKey in statusTransaction.LockedInstanceKeys)
            {
                UnlockKey(lockedInstanceKey);
            }
            //TODO: Mike: Consider: Should we return to the caller the fill list of locked keys? If not, clear them.
            //statusTransaction.LockedInstanceKeys.Clear();
            DirectoryHelper.DeleteDirectory(statusTransaction.TransactionIndex);
            StatusHelper.SetCommandAndTransactionStatus(statusTransaction, StatusCode.Rolled_Back,
                TransactionStatus.RolledBack, StatusCode.OK);
            return statusTransaction;
        }

        /// <summary>
        ///     Locks the Transactions Key.
        ///     This locks the instance Key file, then copies it to the transaction.
        ///     If the Key file has already been copied, it does nothing.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        internal static void TransactionLockKey(StatusTransaction statusTransaction, string table, string key)
        {
            var index = CreateKeyIndex(statusTransaction.Instance, table, key);
            if (!File.Exists(index))
            {
                return;
            }
            try
            {
                var newIndex = CreateKeyIndex(new Uri(statusTransaction.TransactionIndex), table, key);

                //Only lock and copy the key a single time
                if (File.Exists(newIndex)) return;

                //Record the key to make it easy to unlock them in bulk later
                statusTransaction.LockedInstanceKeys.Add(index);

                //Copy the key into the transaction instance
                DirectoryHelper.AssureDirectoryExists(Path.GetDirectoryName(newIndex));
                File.Copy(index, newIndex);

                //Lock the original key while the transaction key is being updated
                // ReSharper disable once UseObjectOrCollectionInitializer
                var fi = new FileInfo(index);
                fi.IsReadOnly = false;
            }
            catch (Exception e)
            {
                throw new ApplicationException(@"Failed to perform transaction lock on {index}", e);
            }
        }

        #endregion
    }
}