// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 09-14-2015
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-16-2015
// ***********************************************************************
// <copyright file="ScriptDTO.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace ZarahDB_WebAPI
{
    /// <summary>
    ///     Class ScriptDTO.
    /// </summary>
    public class ScriptDto
    {
        /// <summary>
        ///     Gets or sets the script.
        /// </summary>
        /// <value>The script.</value>
        [Required]
        public string Script { get; set; }
    }
}