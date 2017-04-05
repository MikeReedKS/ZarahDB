// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="foreignKeyReference.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace ZarahDB_Library.Types
{
    /// <summary>
    /// Class KeyValue.
    /// </summary>
    public class ForeignKeyReference
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Updated { get; set; }
    }
}