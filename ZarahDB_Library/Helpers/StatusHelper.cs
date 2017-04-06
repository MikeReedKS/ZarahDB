// ***********************************************************************
// Assembly         : ZarahDB_Library
// Author           : Mike.Reed
// Created          : 04-18-2016
//
// Last Modified By : Mike.Reed
// Last Modified On : 04-02-2017
// ***********************************************************************
// <copyright file="StatusHelper.cs" company="Benchmark Solutions LLC">
//     Copyright ©  2017 Benchmark Solutions LLC
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using ZarahDB_Library.Enums;
using ZarahDB_Library.Types;

namespace ZarahDB_Library.Helpers
{
    /// <summary>
    ///     Class StatusHelper.
    /// </summary>
    public static class StatusHelper
    {
        /// <summary>
        ///     Sets the status and message.
        /// </summary>
        /// <param name="statusKeysColumnValues">The status keys column values.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public static void SetStatusKeysColumnValuesStatus(StatusKeysColumnValues statusKeysColumnValues,
            StatusCode status, StatusCode message)
        {
            statusKeysColumnValues.Status = ((int) status).ToString();
            statusKeysColumnValues.Message = nameof(message);
        }

        /// <summary>
        ///     Sets the status and message.
        /// </summary>
        /// <param name="statusKeysColumnValues">The status keys column values.</param>
        /// <param name="statusCode">The status code.</param>
        public static void SetStatusKeysColumnValuesStatus(StatusKeysColumnValues statusKeysColumnValues,
            StatusCode statusCode)
        {
            statusKeysColumnValues.Status = ((int) statusCode).ToString();
            statusKeysColumnValues.Message = nameof(statusCode);
        }

        /// <summary>
        ///     Sets the status and message.
        /// </summary>
        /// <param name="statusKeysColumnValues">The status keys column values.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public static void SetStatusKeysColumnValuesStatus(StatusKeysColumnValues statusKeysColumnValues, string status,
            string message)
        {
            statusKeysColumnValues.Status = status;
            statusKeysColumnValues.Message = message;
        }

        /// <summary>
        ///     Sets the command and transaction status.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="commandStatus">The command status.</param>
        /// <param name="commandMessage">The command message.</param>
        /// <param name="transactionStatus">The transaction status.</param>
        public static void SetCommandAndTransactionStatus(StatusTransaction statusTransaction,
            StatusCode commandStatus, TransactionStatus commandMessage, StatusCode transactionStatus)
        {
            var newCommandWithResult = new CommandWithResult {Command = statusTransaction.Command};
            var newStatusKeyColumnValues = new StatusKeyColumnValues
            {
                Status = commandStatus.ToString(),
                Message = commandMessage.ToString()
            };
            newCommandWithResult.Result.Add(newStatusKeyColumnValues);
            statusTransaction.Status = ((int) transactionStatus).ToString();
            statusTransaction.Message = nameof(transactionStatus);
        }

        /// <summary>
        ///     Sets the status value.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusMessageValue SetStatusMessageValue(StatusCode statusCode)
        {
            return SetStatusMessageValue(((int) statusCode).ToString(), statusCode.ToString(), "");
        }

        /// <summary>
        ///     Sets the status key value.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>StatusKeyColumnValue.</returns>
        public static StatusKeyColumnValue SetStatusKeyColumnValue(StatusCode statusCode)
        {
            return SetStatusKeyColumnValue(((int) statusCode).ToString(), statusCode.ToString(), "");
        }

        /// <summary>
        ///     Sets the status list.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>StatusList.</returns>
        public static StatusList SetStatusList(StatusCode statusCode)
        {
            return SetStatusList(((int) statusCode).ToString(), statusCode.ToString(), "");
        }

        /// <summary>
        ///     Sets the status list.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="list">The list.</param>
        /// <returns>StatusList.</returns>
        public static StatusList SetStatusList(StatusCode statusCode, List<string> list)
        {
            return SetStatusList(((int) statusCode).ToString(), statusCode.ToString(), list);
        }

        /// <summary>
        ///     Sets the status value.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="value">The value.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusMessageValue SetStatusMessageValue(StatusCode statusCode, string value)
        {
            return SetStatusMessageValue(((int) statusCode).ToString(), statusCode.ToString(), value);
        }

        /// <summary>
        ///     Sets the status value.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        /// <param name="value">The value.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusMessageValue SetStatusMessageValue(string status, string message, string value)
        {
            var statusValue = new StatusMessageValue
            {
                Status = status,
                Message = message,
                Value = value
            };
            return statusValue;
        }

        /// <summary>
        ///     Sets the status key column value.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        /// <param name="value">The value.</param>
        /// <returns>StatusKeyColumnValue.</returns>
        public static StatusKeyColumnValue SetStatusKeyColumnValue(string status, string message, string value)
        {
            var statusValue = new StatusKeyColumnValue
            {
                Status = status,
                Message = message,
                Value = value
            };
            return statusValue;
        }

        /// <summary>
        ///     Sets the status value.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        /// <param name="value">The value.</param>
        /// <returns>ZarahDB_Library.StatusValue</returns>
        public static StatusList SetStatusList(string status, string message, string value)
        {
            var statusList = new StatusList
            {
                Status = status,
                Message = message,
                Value = value
            };
            return statusList;
        }

        /// <summary>
        ///     Sets the status list.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        /// <param name="list">The list.</param>
        /// <returns>StatusList.</returns>
        public static StatusList SetStatusList(string status, string message, List<string> list)
        {
            var statusList = new StatusList
            {
                Status = status,
                Message = message,
                Value = list.Count.ToString(),
                List = list
            };
            return statusList;
        }

        /// <summary>
        ///     Sets the transaction status.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public static void SetTransactionStatus(StatusTransaction statusTransaction, string status,
            string message)
        {
            SetTransactionStatus(statusTransaction, null, status, message);
        }

        /// <summary>
        ///     Sets the transaction status.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public static void SetTransactionStatus(StatusTransaction statusTransaction, StatusCode status,
            StatusCode message)
        {
            SetTransactionStatus(statusTransaction, null, ((int) status).ToString(), message.ToString());
        }

        /// <summary>
        ///     Sets the transaction status.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public static void SetTransactionStatus(StatusTransaction statusTransaction, StatusCode status,
            string message)
        {
            SetTransactionStatus(statusTransaction, null, ((int) status).ToString(), message);
        }

        /// <summary>
        ///     Sets the transaction status.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="keyColumnValues">The key column values.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public static void SetTransactionStatus(StatusTransaction statusTransaction, KeyColumnValues keyColumnValues,
            StatusCode status, StatusCode message)
        {
            SetTransactionStatus(statusTransaction, keyColumnValues, ((int) status).ToString(), nameof(message));
        }

        /// <summary>
        ///     Sets the transaction status.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        /// <param name="keyColumnValues">The key column values.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public static void SetTransactionStatus(StatusTransaction statusTransaction, KeyColumnValues keyColumnValues,
            string status, string message)
        {
            var newCommandWithResult = new CommandWithResult
            {
                Command = statusTransaction.Command
            };
            var newStatusKeyColumnValues = new StatusKeyColumnValues
            {
                Status = status,
                Message = message
            };
            if (keyColumnValues != null && keyColumnValues.ColumnValues.Any())
            {
                foreach (var columnValue in keyColumnValues.ColumnValues)
                {
                    newStatusKeyColumnValues.ColumnValues.Add(columnValue.Column, columnValue.Value);
                }
            }
            newCommandWithResult.Result.Add(newStatusKeyColumnValues);
            statusTransaction.Transaction.Add(newCommandWithResult);
            statusTransaction.Status = status;
            statusTransaction.Message = message;
        }

        /// <summary>
        ///     Sets the start ticks.
        /// </summary>
        /// <param name="statistics">The statistics.</param>
        public static void SetStartTicks(Statistics statistics)
        {
            var now = DateTime.UtcNow.Ticks;
            if (statistics.RequestedTicks == 0)
            {
                statistics.RequestedTicks = now;
            }
            if (statistics.StartTicks == 0)
            {
                statistics.StartTicks = now;
            }
        }

        /// <summary>
        ///     Sets the start ticks.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        public static void SetStartTicks(StatusTransaction statusTransaction)
        {
            if (statusTransaction.Statistics.StartTicks == 0)
            {
                statusTransaction.Statistics.StartTicks = DateTime.UtcNow.Ticks;
            }
        }

        /// <summary>
        ///     Sets the start ticks.
        /// </summary>
        /// <param name="statusMessageValue">The status message value.</param>
        public static void SetStartTicks(StatusMessageValue statusMessageValue)
        {
            if (statusMessageValue.Statistics.StartTicks == 0)
            {
                statusMessageValue.Statistics.StartTicks = DateTime.UtcNow.Ticks;
            }
        }

        /// <summary>
        ///     Sets the end ticks.
        /// </summary>
        /// <param name="statusTransaction">The status transaction.</param>
        public static void SetEndTicks(StatusTransaction statusTransaction)
        {
            statusTransaction.Statistics.EndTicks = DateTime.UtcNow.Ticks;
        }

        /// <summary>
        ///     Finalizes the stats.
        /// </summary>
        /// <param name="requestedTicks">The requested ticks.</param>
        /// <param name="startTicks">The start ticks.</param>
        /// <returns>Statistics.</returns>
        public static Statistics FinalizeStats(string requestedTicks, string startTicks)
        {
            if (string.IsNullOrEmpty(requestedTicks))
            {
                requestedTicks = StringHelper.NowTicks();
            }
            if (string.IsNullOrEmpty(startTicks))
            {
                startTicks = StringHelper.NowTicks();
            }
            var newStats = new Statistics
            {
                RequestedTicks = Convert.ToInt64(requestedTicks),
                StartTicks = Convert.ToInt64(startTicks),
                EndTicks = Convert.ToInt64(StringHelper.NowTicks())
            };
            return newStats;
        }

        /// <summary>
        ///     Sets the status transaction.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>StatusTransaction.</returns>
        public static StatusTransaction SetStatusTransaction(StatusCode statusCode)
        {
            return SetStatusTransaction(((int) statusCode).ToString(), statusCode.ToString());
        }

        /// <summary>
        ///     Sets the status transaction.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        /// <returns>StatusTransaction.</returns>
        public static StatusTransaction SetStatusTransaction(string statusCode, string message)
        {
            var newStatusTransaction = new StatusTransaction
            {
                Status = statusCode,
                Message = message
            };

            return newStatusTransaction;
        }

        /// <summary>
        ///     Sets the status key column values.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>StatusKeyColumnValues.</returns>
        public static StatusKeyColumnValues SetStatusKeyColumnValues(StatusCode statusCode)
        {
            var newStatusKeyColumnValues = new StatusKeyColumnValues
            {
                Status = ((int) statusCode).ToString(),
                Message = statusCode.ToString()
            };

            return newStatusKeyColumnValues;
        }

        /// <summary>
        ///     Sets the requested ticks.
        /// </summary>
        /// <param name="statistics">The statistics.</param>
        public static void SetRequestedTicks(Statistics statistics)
        {
            if (statistics.RequestedTicks == 0)
            {
                statistics.RequestedTicks = DateTime.UtcNow.Ticks;
            }
        }
    }
}