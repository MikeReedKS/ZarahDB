// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="DirectoryHelper.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ZarahDB_Library.Helpers
{
    /// <summary>
    ///     Class DirectoryHelper.
    /// </summary>
    internal static class DirectoryHelper
    {
        /// <summary>
        ///     Creates the name of the legal directory.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        internal static string CreateLegalDirectoryName(string table, string key = "")
        {
            if (table == null & key == null)
            {
                table = "";
            }
            if (table == null & key != null)
            {
                table = "[default]";
            }
            while (table.Length < 2)
            {
                table = table + "_";
            }
            table = StringHelper.ReplaceEx(table, " ", "_");
            if (table == null || table.Equals("__"))
            {
                table = "";
            }
            return table;
        }

        /// <summary>
        ///     Assures the folder exists. If it does not, it creates it.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <exception cref="System.ApplicationException"></exception>
        internal static void AssureDirectoryExists(string directoryPath)
        {
            if (DirectoryExists(directoryPath)) return;
            try
            {
                Directory.CreateDirectory(directoryPath);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message, e);
            }
        }

        /// <summary>
        ///     Tests if the Folder exists.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>System.Boolean</returns>
        internal static bool DirectoryExists(string directoryPath)
        {
            try
            {
                var possibleFilename = Path.GetFileName(directoryPath);
                if (possibleFilename != null && possibleFilename.IndexOf('.') >= 0)
                {
                    directoryPath = Path.GetDirectoryName(directoryPath);
                }
                var result = directoryPath != null && Directory.Exists(directoryPath);
                return result;
            }
            catch
            {
                return (false);
            }
        }

        /// <summary>
        ///     Moves the directory.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ApplicationException"></exception>
        internal static void MoveDirectory(string source, string target)
        {
            var sourcePath = source.TrimEnd('\\', ' ');
            var targetPath = target.TrimEnd('\\', ' ');
            var files = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories)
                .GroupBy(s => Path.GetDirectoryName(s));
            foreach (var folder in files)
            {
                var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                Directory.CreateDirectory(targetFolder);
                foreach (var file in folder)
                {
                    if (file == null) continue;
                    var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));

                    var tries = 0;
                    while (File.Exists(targetFile) && tries < 3)
                    {
                        try
                        {
                            tries++;
                            if (File.Exists(targetFile)) File.Delete(targetFile);
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(100);
                        }
                    }

                    try
                    {
                        File.Move(file, targetFile);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.Message, ex);
                    }
                }
            }
            Directory.Delete(source, true);
        }

        /// <summary>
        ///     Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        internal static void DeleteDirectory(string path)
        {
            if (!DirectoryExists(path)) return;

            var directory = new DirectoryInfo(path) {Attributes = FileAttributes.Normal};

            foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }

            directory.Delete(true);
        }

        /// <summary>
        ///     Childs the folders.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        internal static List<string> ChildFolders(string path)
        {
            var result = new List<string>();
            var directory = new DirectoryInfo(path);
            var directories = directory.GetDirectories();

            foreach (var folder in directories)
                result.Add(folder.Name);

            return result;
        }

        /// <summary>
        ///     Childs the instances.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        internal static List<string> ChildInstances(string path)
        {
            var result = new List<string>();
            var directory = new DirectoryInfo(path);
            var directories = directory.GetDirectories();

            foreach (var folder in directories)
            {
                var maxDepthFilename = Path.Combine(folder.FullName, "[default]", "Max.Depth");
                if (File.Exists(maxDepthFilename))
                {
                    result.Add(folder.Name);
                }
            }

            return result;
        }
    }
}