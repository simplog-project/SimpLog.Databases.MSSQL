using SimpLog.Databases.MSSQL.Entities;
using SimpLog.Databases.MSSQL.Models.AppSettings;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SimpLog.Databases.MSSQL.Services.DatabaseServices
{
    internal class DatabaseServices
    {
        private static Configuration conf = ConfigurationServices.ConfigService.BindConfigObject();
        
        private const string insertQuery = @"
            INSERT INTO StoreLog
            (
                Log_Type,
                Log_Error,
                Log_Created,
                Log_FileName,
                Log_Path,
                Log_SendEmail,
                Email_ID,
                Saved_In_Database
            )
            VALUES
            (
                @Log_Type,
                @Log_Error,
                @Log_Created,
                @Log_FileName,
                @Log_Path,
                @Log_SendEmail,
                @Email_ID,
                @Saved_In_Database
            )";

        /// <summary>
        /// Call this once at application startup to ensure DB and table exist
        /// </summary>
        public static async Task InitializeDatabase()
        {
            using var connection = new SqlConnection(conf.Database_Configuration.Connection_String);
            await connection.OpenAsync();
            await DatabaseMigrations.CreateMSSqlIfNotExists(connection);
        }

        /// <summary>
        /// Depending on the name of the DB, goes to the function for that stuff.
        /// </summary>
        /// <param name="storeLog"></param>
        public static Task SaveIntoDatabase(StoreLog storeLog)
            => InsertIntoMSSql(storeLog);

        /// <summary>
        /// Insert log into MSSql database.
        /// </summary>
        /// <param name="storeLog"></param>
        public static async Task InsertIntoMSSql(StoreLog storeLog)
        {
            using var connection = new SqlConnection(conf.Database_Configuration.Connection_String);

            await connection.OpenAsync();

            // Consider moving this to application startup
            await DatabaseMigrations.CreateMSSqlIfNotExists(connection);

            using var command = new SqlCommand(insertQuery, connection);

            command.Parameters.Add("@Log_Type", SqlDbType.NVarChar, 50).Value = storeLog.Log_Type;
            command.Parameters.Add("@Log_Error", SqlDbType.NVarChar).Value = storeLog.Log_Error;
            command.Parameters.Add("@Log_Created", SqlDbType.DateTime2).Value = storeLog.Log_Created;
            command.Parameters.Add("@Log_FileName", SqlDbType.NVarChar, 255).Value = storeLog.Log_FileName ?? (object)DBNull.Value;
            command.Parameters.Add("@Log_Path", SqlDbType.NVarChar, 500).Value = storeLog.Log_Path ?? (object)DBNull.Value;
            command.Parameters.Add("@Log_SendEmail", SqlDbType.Bit).Value = storeLog.Log_SendEmail ?? false;
            command.Parameters.Add("@Email_ID", SqlDbType.Int).Value = 0;
            command.Parameters.Add("@Saved_In_Database", SqlDbType.Bit).Value = storeLog.Saved_In_Database ?? true;

            await command.ExecuteNonQueryAsync();
        }
    }
}
