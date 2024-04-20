using SimpLog.Databases.MSSQL.Entities;
using SimpLog.Databases.MSSQL.Models;
using SimpLog.Databases.MSSQL.Models.AppSettings;
using System;
using System.Data.SqlClient;

namespace SimpLog.Databases.MSSQL.Services.DatabaseServices
{
    internal class DatabaseServices
    {
        public static Configuration conf = ConfigurationServices.ConfigService.BindConfigObject();

        /// <summary>
        /// Depending on the name of the DB, goes to the function for that stuff.
        /// </summary>
        /// <param name="storeLog"></param>
        public static void SaveIntoDatabase(StoreLog storeLog)
            => InsertIntoMSSql(storeLog);

        /// <summary>
        /// Insert log into MSSql database.
        /// </summary>
        /// <param name="storeLog"></param>
        public static void InsertIntoMSSql(StoreLog storeLog)
        {
            SqlConnection connection = new SqlConnection(conf.Database_Configuration.Connection_String);
            SqlCommand cmd = new SqlCommand(null, connection);

            DatabaseMigrations.CreateMSSqlIfNotExists(connection, cmd);

            connection.Open();

            int EmailID = 0;

            string query = string.Empty;

            query = "INSERT INTO StoreLog(Log_Type, Log_Error, Log_Created, Log_FileName, Log_Path, Log_SendEmail, Email_ID, Saved_In_Database) " +
                "VALUES(@Log_Type, @Log_Error, @Log_Created, @Log_FileName, @Log_Path, @Log_SendEmail, @Email_ID, @Saved_In_Database)";

            cmd.Parameters.AddWithValue("@Log_Type", storeLog.Log_Type);
            cmd.Parameters.AddWithValue("@Log_Error", storeLog.Log_Error);
            cmd.Parameters.AddWithValue("@Log_Created", storeLog.Log_Created);
            cmd.Parameters.AddWithValue("@Log_FileName", storeLog.Log_FileName);
            cmd.Parameters.AddWithValue("@Log_Path", storeLog.Log_Path);
            cmd.Parameters.AddWithValue("@Log_SendEmail", storeLog.Log_SendEmail);
            cmd.Parameters.AddWithValue("@Email_ID", EmailID);
            cmd.Parameters.AddWithValue("@Saved_In_Database", DateTime.UtcNow.ToString());

            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
