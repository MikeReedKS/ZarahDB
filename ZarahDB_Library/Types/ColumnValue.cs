// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 07-04-2015
//
// Last Modified By : Mike.Reed
// Last Modified On : 08-08-2015
// ***********************************************************************
// <copyright file="ColumnValue.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2015 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

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
    ///     Class ColumnValue.
    /// </summary>
    public class ColumnValue
    {
        private string _value;

        /// <summary>
        ///     Gets or sets the column.
        /// </summary>
        /// <value>The column.</value>
        [Required]
        public string Column { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Required]
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                PreviousValue = _value;
                _value = value;
            }
        }

        [Required]
        public string PreviousValue { get; set; }

        [Required]
        public string Updated { get; set; }
    }
}