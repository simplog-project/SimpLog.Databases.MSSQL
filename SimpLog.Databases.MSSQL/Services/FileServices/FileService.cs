using SimpLog.Databases.MSSQL.Entities;
using SimpLog.Databases.MSSQL.Models;
using SimpLog.Databases.MSSQL.Models.AppSettings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static SimpLog.Databases.MSSQL.Models.Constants;

namespace SimpLog.Databases.MSSQL.Services.FileServices
{
    internal class FileService
    {
        /// <summary>
        /// FullPath + FileName is the key and value is what should be saved into the log
        /// </summary>
        internal static ConcurrentDictionary<string, StringBuilder> Logs = new ();

        internal static readonly Configuration configuration = ConfigurationServices.ConfigService.CurrentConfiguration;

        private static bool GetLogTypeEnabled(bool? value) => value ?? true;

        internal readonly bool _Trace_Db = GetLogTypeEnabled(configuration.LogType.Trace.SaveInDatabase);
        internal readonly bool _Debug_Db = GetLogTypeEnabled(configuration.LogType.Debug.SaveInDatabase);
        internal readonly bool _Info_Db = GetLogTypeEnabled(configuration.LogType.Info.SaveInDatabase);
        internal readonly bool _Notice_Db = GetLogTypeEnabled(configuration.LogType.Notice.SaveInDatabase);
        internal readonly bool _Warn_Db = GetLogTypeEnabled(configuration.LogType.Warn.SaveInDatabase);
        internal readonly bool _Error_Db = GetLogTypeEnabled(configuration.LogType.Error.SaveInDatabase);
        internal readonly bool _Fatal_Db = GetLogTypeEnabled(configuration.LogType.Fatal.SaveInDatabase);

        private static readonly Dictionary<LogType, bool> _dbLogEnabledByType = new()
        {
            { LogType.Trace,  configuration.LogType.Trace.SaveInDatabase ?? true },
            { LogType.Debug,  configuration.LogType.Debug.SaveInDatabase ?? true },
            { LogType.Info,   configuration.LogType.Info.SaveInDatabase ?? true },
            { LogType.Notice, configuration.LogType.Notice.SaveInDatabase ?? true },
            { LogType.Warn,   configuration.LogType.Warn.SaveInDatabase ?? true },
            { LogType.Error,  configuration.LogType.Error.SaveInDatabase ?? true },
            { LogType.Fatal,  configuration.LogType.Fatal.SaveInDatabase ?? true }
        };

        /// <summary>
        /// Converts message type from enum to string.
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal string MessageType(LogType logType) => logType.ToLabel();

        /// <summary>
        /// Distributes what type of save is it configured. File, Email of Database.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="saveInDatabase"></param>
        /// <returns></returns>
        internal async Task Save(
            string message, 
            LogType logType, 
            bool? saveInDatabase = null,
            string? saveType = null,
            bool? isSentEmail = null,
            string? path_to_save_log = null,
            string? log_file_name = null)
        {
            try
            {
                //  Send into a database
                if (ShouldSaveInDb(saveInDatabase, logType))
                {
                    await DatabaseServices.DatabaseServices.SaveIntoDatabase(
                        storeLog(message, isSentEmail, logType, saveInDatabase, saveType, path_to_save_log, log_file_name));
                }
            }
            catch(Exception ex)
            {
                //await SaveSimpLogError(ex.Message);
                //Dispose();
            }
        }

        /// <summary>
        /// Checks the configurations for saving in Db at all.
        /// </summary>
        /// <param name="saveInDatabase"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal bool ShouldSaveInDb(bool? saveInDatabase, LogType logType)
        {
            //  Check if the db log is active at global level.
            if(saveInDatabase is false)
                return false;

            var dbConfig = configuration.Database_Configuration;

            if (dbConfig?.Global_Enabled_Save == false ||
                string.IsNullOrWhiteSpace(dbConfig?.Connection_String))
                return false;

            return _dbLogEnabledByType.TryGetValue(logType, out var enabled) && enabled;
        }

        /// <summary>
        /// Populates the object for StoreLog in database table
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isEmailSent"></param>
        /// <param name="logType"></param>
        /// <param name="saveInDatabase"></param>
        /// <param name="saveType"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        internal StoreLog storeLog(
            string message, 
            bool? isEmailSent, 
            LogType? logType, 
            bool? saveInDatabase, 
            string? saveType, 
            string? path_to_save_log,
            string? log_file_name)
        {
            StoreLog storeLog = new StoreLog()
            {
                Log_Created = DateTime.UtcNow.ToString(),
                Log_Error = message,
                Log_SendEmail = isEmailSent,
                Log_Type = logType.ToString(),
                Saved_In_Database = saveInDatabase,
                Log_File_Save_Type = saveType,
                Log_Path = path_to_save_log,
                Log_FileName = log_file_name
            };

            return storeLog;
        }
    }
}
