// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="StringHelper.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace ZarahDB_Library.Helpers
{
    //Copyright 2015 Benchmark Solutions LLC
    //Originally created by Mike Reed

    //Licensed under the Apache License, Version 2.0 (the "License");
    //you may not use this file except in compliance with the License.
    //You may obtain a copy of the License at

    //    http://www.apache.org/licenses/LICENSE-2.0

    //Unless required by applicable law or agreed to in writing, software
    //distributed under the License is distributed on an "AS IS" BASIS,
    //WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    //See the License for the specific language governing permissions and
    //limitations under the License.

    /// <summary>
    ///     Class StringHelper.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        ///     Replaces the ex.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns>System.String.</returns>
        public static string ReplaceEx(string original, string pattern, string replacement)
        {
            if (original == null)
            {
                return null;
            }
            if (pattern == null)
            {
                return original;
            }
            if (replacement == null)
            {
                return original;
            }
            int startCursor, endCursor;
            var count = startCursor = 0;
            var upperString = original.ToUpper();
            var upperPattern = pattern.ToUpper();
            var inc = (original.Length/pattern.Length)*
                      (replacement.Length - pattern.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];
            while ((endCursor = upperString.IndexOf(upperPattern, startCursor, StringComparison.Ordinal)) != -1)
            {
                for (var i = startCursor; i < endCursor; ++i)
                    chars[count++] = original[i];
                foreach (var t in replacement)
                    chars[count++] = t;
                startCursor = endCursor + pattern.Length;
            }
            if (startCursor == 0) return original;
            for (var i = startCursor; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }

        /// <summary>
        ///     Nows the ticks.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string NowTicks()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }
    }
}