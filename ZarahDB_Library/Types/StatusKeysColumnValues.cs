// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="StatusKeysColumnValues.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace ZarahDB_Library.Types
{
    /// <summary>
    /// Class StatusKeysColumnValues.
    /// </summary>
    public class StatusKeysColumnValues
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the column values.
        /// </summary>
        /// <value>A list of Keys, each with associated column values.</value>
        public List<KeyColumnValues> KeysColumnValues { get; set; } = new List<KeyColumnValues>();

        /// <summary>
        /// Gets or sets the ticks.
        /// Ticks are a high accuracy timestamp of when the object was created.
        /// </summary>
        /// <value>The ticks.</value>
        public string Ticks { get; set; } = DateTime.Now.Ticks.ToString();
    }
}
