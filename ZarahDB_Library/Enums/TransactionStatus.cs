// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="TransactionStatus.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace ZarahDB_Library.Enums
{
    /// <summary>
    ///     Enum TransactionStatus
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        ///     The unknown
        /// </summary>
        Unknown,

        /// <summary>
        ///     The active
        /// </summary>
        Active,

        /// <summary>
        ///     The committed
        /// </summary>
        Committed,

        /// <summary>
        ///     The rolled back
        /// </summary>
        RolledBack
    }
}