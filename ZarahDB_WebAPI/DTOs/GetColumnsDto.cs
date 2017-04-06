// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-10-2016
// ***********************************************************************
// <copyright file="GetColumnsDto.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZarahDB_WebAPI
{
    /// <summary>
    ///     Class GetColumnsDto.
    /// </summary>
    public class GetColumnsDto
    {
        /// <summary>
        ///     Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        [Required]
        public string Instance { get; set; }

        /// <summary>
        ///     Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        [Required]
        public string Table { get; set; }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [Required]
        public string Key { get; set; }

        /// <summary>
        ///     Gets or sets the list of columns.
        /// </summary>
        /// <value>The column.</value>
        [Required]
        public List<string> Columns { get; set; }
    }
}