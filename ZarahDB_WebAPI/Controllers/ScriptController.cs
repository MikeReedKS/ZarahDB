// ***********************************************************************
// Assembly         : ZarahDB_WebAPI
// Author           : Mike.Reed
// Created          : 07-04-2015
//
// Last Modified By : Mike.Reed
// Last Modified On : 09-16-2015
// ***********************************************************************
// <copyright file="ScriptController.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;
using ZarahDB_Library;
using ZarahDB_Library.Helpers;
using ZarahDB_Library.Types;

namespace ZarahDB_WebAPI.Controllers
{
    /// <summary>
    /// Class ScriptController.
    /// </summary>
    public class ScriptController : ApiController
    {
        /// <summary>
        /// Executes a script of commands directly.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="script">The script.</param>
        /// <returns>StatusTransaction</returns>
        [AcceptVerbs("POST")]
        [Route("Script/Run")]
        public StatusTransaction Script(
            [FromUri] string instance,
            [FromBody] ScriptDto script)
        {
            //TODO: Migrate to SecurityHelper
            if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                instance = null;
            }
            string localPath = null;
            if (instance != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
            }

            var statusTransaction = new StatusTransaction();
            try
            {
                statusTransaction = ZarahDB.Script(localPath == null ? null : new Uri(localPath), script.Script);
            }
            catch (Exception ex)
            {
                StatusHelper.SetTransactionStatus(statusTransaction, null, ex.HResult.ToString(), ex.Message);
            }

            return statusTransaction;
        }

        /// <summary>
        /// Puts the script into the instance so that it can be executed.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="scriptName">Name of the script.</param>
        /// <param name="script">The script.</param>
        /// <returns>StatusTransaction</returns>
        [AcceptVerbs("PUT")]
        [Route("Script")]
        public bool ScriptPut(
            [FromUri] string instance,
            [FromUri] string scriptName,
            [FromBody] string script)
        {
            //TODO: Migrate to SecurityHelper
            if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                instance = null;
            }
            string localPath = null;
            if (instance != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
            }

            bool result;
            try
            {
                result = ZarahDB.PutScript(localPath == null ? null : new Uri(localPath), scriptName, script);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Gets a script from the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="scriptName">The script.</param>
        /// <returns>StatusTransaction</returns>
        [AcceptVerbs("GET")]
        [Route("Script")]
        public string ScriptGet(
            [FromUri] string instance,
            [FromUri] string scriptName)
        {
            //TODO: Migrate to SecurityHelper
            if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                instance = null;
            }
            string localPath = null;
            if (instance != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
            }

            string script;
            try
            {
                script = ZarahDB.GetScript(localPath == null ? null : new Uri(localPath), scriptName);
            }
            catch (Exception ex)
            {
                script = ex.Message;
            }

            return script;
        }

        /// <summary>
        /// Execute a script from the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="scriptName">The script.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>StatusTransaction</returns>
        [AcceptVerbs("POST")]
        [Route("Script/Execute")]
        public StatusTransaction ScriptExecute(
            [FromUri] string instance,
            [FromUri] string scriptName,
            [FromBody] List<KeyValue> variables
            )
        {
            //TODO: Migrate to SecurityHelper
            if (instance.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                instance = null;
            }
            string localPath = null;
            if (instance != null)
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, instance);
            }
            var variablesDictionary = new Dictionary<string, string>();
            foreach (var variable in variables)
            {
                variablesDictionary[variable.Key] = variable.Value;
            }

            var newStatusTransaction = new StatusTransaction();
            try
            {
                newStatusTransaction = ZarahDB.ExecuteScript(localPath == null ? null : new Uri(localPath), scriptName, variablesDictionary);
            }
            catch (Exception ex)
            {
                //TODO: Mike: Work: 
                newStatusTransaction.Message = ex.Message;
            }

            return newStatusTransaction;
        }
    }
}