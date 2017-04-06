// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="OperatorWithOperands.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace ZarahDB_Library.Types
{
    /// <summary>
    ///     Class OperatorWithOperands.
    /// </summary>
    public class OperatorWithOperands
    {
        /// <summary>
        ///     Gets or sets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public string Operator { get; set; }

        /// <summary>
        ///     Gets or sets the operands.
        /// </summary>
        /// <value>The operands.</value>
        public List<string> Operands { get; set; } = new List<string>();
    }
}