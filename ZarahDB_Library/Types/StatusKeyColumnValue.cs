// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 09-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-18-2016
// ***********************************************************************
// <copyright file="StatusKeyColumnValue.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2015 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace ZarahDB_Library.Types
{
    /// <summary>
    ///     Class StatusKeyColumnValue.
    /// </summary>
    public class StatusKeyColumnValue
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
        ///     Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [Required]
        public string Key { get; set; }

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
        public string Value { get; set; }

        /// <summary>
        ///     Gets or sets the previous value.
        /// </summary>
        /// <value>The previous value.</value>
        [Required]
        public string PreviousValue { get; set; }

        /// <summary>
        ///     Gets or sets the ticks for when the new value replaced the previous value.
        /// </summary>
        /// <value>The updated.</value>
        [Required]
        public long Updated { get; set; }

        /// <summary>
        ///     Gets or sets the stats.
        /// </summary>
        /// <value>The stats.</value>
        [Required]
        public Statistics Statistics { get; set; } = new Statistics();
    }
}