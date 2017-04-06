// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 09-10-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-10-2016
// ***********************************************************************
// <copyright file="StatusList.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2016 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZarahDB_Library.Types
{
    /// <summary>
    ///     Class StatusList.
    /// </summary>
    public class StatusList
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
        ///     Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Required]
        public List<string> List { get; set; } = new List<string>();

        /// <summary>
        ///     Gets or sets the stats.
        /// </summary>
        /// <value>The stats.</value>
        [Required]
        public Statistics Statistics { get; set; } = new Statistics();
    }
}