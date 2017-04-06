// <copyright file="ZarahDBTest.cs" company="Benchmark Solutions LLC">Copyright ©  2015 Benchmark Solutions LLC</copyright>

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

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZarahDB_Library.Types;

namespace ZarahDB_Library.Tests
{
    [TestClass]
    public partial class ZarahDBTest
    {
        public StatusMessageValue Put(
            Uri instance,
            string table,
            string key,
            string column,
            string value
            )
        {
            return ZarahDB.Put(instance, table, key, column, value);
        }

        public StatusList ListInstance(Uri instance, string table, string key)
        {
            return ZarahDB.ListInstance(null);
        }

        public StatusKeyColumnValues Get(Uri instance, string table, string key)
        {
            return ZarahDB.Get(instance, table, key);
        }

        public StatusMessageValue Get(Uri instance, string table, string key, string column)
        {
            return ZarahDB.Get(instance, table, key, column);
        }

        public StatusKeyColumnValues Get(Uri instance, string table, string key, List<string> columnList)
        {
            return ZarahDB.Get(instance, table, key, columnList);
        }

        public bool Exists(Uri instance)
        {
            return ZarahDB.Exists(instance);
        }

        public bool Exists(Uri instance, string table)
        {
            return ZarahDB.Exists(instance, table);
        }

        public bool Exists(Uri instance, string table, string key)
        {
            return ZarahDB.Exists(instance, table, key);
        }

        public StatusMessageValue Backup(Uri instance, int? timeoutSeconds)
        {
            return ZarahDB.Backup(instance, timeoutSeconds);
        }

        public StatusMessageValue DeleteInstance(Uri instance, int? timeoutSeconds)
        {
            return ZarahDB.DeleteInstance(instance, timeoutSeconds);
        }

        public StatusTransaction Transaction(Uri instance, string script)
        {
            return ZarahDB.Script(instance, script);
        }

        public StatusMessageValue SetMaxDepth(Uri instance, string table, int maxDepth)
        {
            return ZarahDB.SetMaxDepth(instance, table, maxDepth);
        }

        public StatusMessageValue GetMaxDepth(Uri instance, string table)
        {
            return ZarahDB.GetMaxDepth(instance, table);
        }

        //[PexMethod(MaxConditions = 1000)]
        //public StatusTransaction CsvFilePut(Uri csvFile, List<string> columns, string keyColumn, string fieldSeparator,
        //    string encloser, string lineTerminator, string commentLineStarter, Uri instance, string table,
        //    int? timeoutSeconds)
        //{
        //    return ZarahDB.CsvFilePut(csvFile, columns, keyColumn, fieldSeparator, encloser, lineTerminator,
        //        commentLineStarter, instance, table, timeoutSeconds);
        //}

        //[PexMethod(MaxConditions = 1000)]
        //public StatusTransaction CsvFolderPut(Uri csvFolder, List<string> columns, string keyColumn,
        //    string fieldSeparator,
        //    string encloser, string lineTerminator, string commentLineStarter, Uri instance, string table,
        //    int? timeoutSeconds)
        //{
        //    return ZarahDB.CsvFolderPut(csvFolder, columns, keyColumn, fieldSeparator, encloser, lineTerminator,
        //        commentLineStarter,
        //        instance, table, timeoutSeconds);
        //}


        public bool CsvInsert(Uri csvFolder, List<string> columns, string keyColumn, string fieldSeparator,
            string encloser, string lineTerminator, string commentLineStarter, Uri instance, string table)
        {
            return ZarahDB.CsvFolderInsert(csvFolder, columns, keyColumn, fieldSeparator,
                encloser, lineTerminator, commentLineStarter, instance, table);
        }
    }
}