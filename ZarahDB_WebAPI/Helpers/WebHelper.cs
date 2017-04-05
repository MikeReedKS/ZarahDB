using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace ZarahDB_WebAPI
{
    /// <summary>
    /// Class WebHelper.
    /// </summary>
    static public class WebHelper
    {
        /// <summary>
        /// Converts the physical path to a file to a URI relative to the site.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        static public string PathToUri(string baseUri, string path)
        {
            //Convert local physical path to a web based URI
            if (HttpContext.Current.Request.PhysicalApplicationPath != null)
                path = Path.Combine(baseUri, path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, string.Empty)).Replace(@"\", @"/");

            return path;
        }

        /// <summary>
        /// Gets the instance path.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>System.String.</returns>
        public static string GetInstancePath(string instance)
        {
            if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                instance = null;
            }
            var instancesRootFolder = "";
            try
            {
                if (HttpContext.Current.Request.PhysicalApplicationPath == null) return instance;
                instancesRootFolder = ConfigurationManager.AppSettings["InstancesRootFolder"] ??
                                          Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "zdb");
            }
            catch
            {
                instancesRootFolder = ConfigurationManager.AppSettings["InstancesRootFolder"] ?? @"D:\zdb";
            }

            return instance == null ? instancesRootFolder : Path.Combine(instancesRootFolder, instance);
        }
    }
}
