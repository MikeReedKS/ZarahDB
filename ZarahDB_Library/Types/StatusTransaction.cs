// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="StatusTransaction.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZarahDB_Library.Types
{
    /// <summary>
    /// Class StatusTransaction.
    /// </summary>
    public class StatusTransaction
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Required]
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [Required]
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Required]
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        [Required]
        public Uri Instance { get; set; }
        /// <summary>
        /// Gets or sets the index of the transaction.
        /// </summary>
        /// <value>The index of the transaction.</value>
        [Required]
        public string TransactionIndex { get; set; }
        /// <summary>
        /// Gets or sets the script.
        /// </summary>
        /// <value>The script.</value>
        [Required]
        public string Script { get; set; }
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        [Required]
        public string Command { get; set; }
        /// <summary>
        /// Gets or sets the variables.
        /// </summary>
        /// <value>The variables.</value>
        [Required]
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// Gets or sets the transaction timestamp.
        /// </summary>
        /// <value>The transaction timestamp.</value>
        [Required]
        public string TransactionTimestamp { get; set; }
        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [Required]
        public List<CommandWithResult> Transaction { get; set; } = new List<CommandWithResult>();
        /// <summary>
        /// Gets or sets the locked instance keys.
        /// </summary>
        /// <value>The locked instance keys.</value>
        [Required]
        public List<string> LockedInstanceKeys { get; set; } = new List<string>();
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        [Required]
        public List<KeyColumnValues> Results { get; set; } = new List<KeyColumnValues>();
        /// <summary>
        /// Gets or sets the statistics.
        /// </summary>
        /// <value>The statistics.</value>
        [Required]
        public Statistics Statistics { get; set; } = new Statistics();
    }
}