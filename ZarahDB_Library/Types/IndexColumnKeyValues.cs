// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="IndexColumnKeyValues.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace ZarahDB_Library.Types
{
    public class IndexColumnKeyValues
    /// <summary>
    /// Class StatusKeysColumnValues.
    /// </summary>
    {
        public string Index { get; set; }
        public string IndexColumn { get; set; }
        public List<ColumnKeyValue> ColumnKeyValues { get; set; } = new List<ColumnKeyValue>();
    }
}