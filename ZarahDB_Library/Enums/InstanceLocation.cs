// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 10-19-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 10-19-2016
// ***********************************************************************
// <copyright file="InstanceLocation.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2015 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace ZarahDB_Library.Enums
{
    /// <summary>
    /// Enum InstanceLocation
    /// </summary>
    public enum InstanceLocation
    {
        /// <summary>
        /// The common application data
        /// </summary>
        CommonApplicationData = 0,

        /// <summary>
        /// The application data
        /// </summary>
        ApplicationData = 1,

        /// <summary>
        /// The desktop directory
        /// </summary>
        DesktopDirectory = 2,

        /// <summary>
        /// The location
        /// </summary>
        Location = 3,

        /// <summary>
        /// The base directory
        /// </summary>
        BaseDirectory = 4,
        Codebase = 5
    }
}