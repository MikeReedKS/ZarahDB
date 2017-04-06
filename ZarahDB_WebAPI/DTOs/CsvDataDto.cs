// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 09-15-2015
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-15-2015
// ***********************************************************************
// <copyright file="CsvDataDto.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZarahDB_WebAPI
{
    /// <summary>
    ///     Class CsvDataDto.
    /// </summary>
    public class CsvDataDto
    {
        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        /// <value>The columns. If null or empty, the columns will be read from the first line of CSV data.</value>
        public List<string> Columns { get; set; }

        /// <summary>
        ///     Gets or sets the key column.
        /// </summary>
        /// <value>The key column.</value>
        [Required]
        public string KeyColumn { get; set; }

        /// <summary>
        ///     Gets or sets the field separator.
        /// </summary>
        /// <value>The field separator.</value>
        [Required]
        public string FieldSeparator { get; set; }

        /// <summary>
        ///     Gets or sets the encloser.
        /// </summary>
        /// <value>The encloser.</value>
        [Required]
        public string Encloser { get; set; }

        /// <summary>
        ///     Gets or sets the line terminator.
        /// </summary>
        /// <value>The line terminator.</value>
        [Required]
        public string LineTerminator { get; set; }

        /// <summary>
        ///     Gets or sets the comment line starter.
        /// </summary>
        /// <value>The comment line starter.</value>
        [Required]
        public string CommentLineStarter { get; set; }

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
        ///     Gets or sets the CSV data.
        /// </summary>
        /// <value>The CSV data.</value>
        [Required]
        public string CSVData { get; set; }
    }
}