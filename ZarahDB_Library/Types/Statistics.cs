// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="Statistics.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace ZarahDB_Library.Types
{
    /// <summary>
    ///     Class IndexColumnKeyValues.
    /// </summary>
    public class Statistics
    {
        [Required]
        /// <summary>
        /// Gets or sets the index column.
        /// </summary>
        /// <value>The index column.</value>
        public long RequestedTicks { get; set; }

        [Required]
        public long StartTicks { get; set; }

        [Required]
        public long EndTicks { get; set; }

        [Required]
        public long BlockedTicks => (StartTicks - RequestedTicks);

        [Required]
        public long ExecuteTicks => (EndTicks - StartTicks);

        [Required]
        public long TotalTicks => (EndTicks - RequestedTicks);

        [Required]
        public string Blocked => $"{(StartTicks - RequestedTicks)/10000000.0} seconds.";

        [Required]
        public string Duration => $"{(EndTicks - RequestedTicks)/10000000.0} seconds.";
    }
}