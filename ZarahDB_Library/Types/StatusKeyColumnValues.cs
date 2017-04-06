// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 07-27-2015
//
// Last Modified By : Mike.Reed
// Last Modified On : 08-08-2015
// ***********************************************************************
// <copyright file="StatusKeyColumnValues.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2015 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZarahDB_Library.Types
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
    ///     Class StatusKeyColumnValues.
    /// </summary>
    public class StatusKeyColumnValues
    {
        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Required]
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [Required]
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Required]
        public string Value { get; set; }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [Required]
        public string Key { get; set; }

        /// <summary>
        ///     Gets or sets the column values.
        /// </summary>
        /// <value>The column values.</value>
        [Required]
        public Dictionary<string, string> ColumnValues { get; set; } = new Dictionary<string, string>();

        /// <summary>
        ///     Gets or sets the statistics.
        /// </summary>
        /// <value>The statistics.</value>
        [Required]
        public Statistics Statistics { get; set; } = new Statistics();
    }
}