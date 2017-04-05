// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="IndexState.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace ZarahDB_Library.Enums
{
    /// <summary>
    /// Enum IndexState
    /// </summary>
    public enum IndexState
    {
        /// <summary>
        /// The unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The locked
        /// </summary>
        Locked = 1,
        /// <summary>
        /// The unlocked
        /// </summary>
        Unlocked = 2,
        /// <summary>
        /// The does not exist
        /// </summary>
        DoesNotExist = 3
    }
}