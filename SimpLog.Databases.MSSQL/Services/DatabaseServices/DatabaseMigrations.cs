using SimpLog.Databases.MSSQL.Models.AppSettings;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SimpLog.Databases.MSSQL.Services.DatabaseServices
{
    internal class DatabaseMigrations
    {
        public static Configuration conf = ConfigurationServices.ConfigService.BindConfigObject();

        /// <summary>
        /// Create MSSql tables if not exists.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cmd"></param>
        public static async Task CreateMSSqlIfNotExists(SqlConnection connection)
        {
            if(connection.State != System.Data.ConnectionState.Open) 
                await connection.OpenAsync();

            var query = new StringBuilder();

            query.Append(@"
                IF OBJECT_ID('StoreLog','U') IS NULL
                CREATE TABLE [StoreLog] (
                    [ID] INT IDENTITY(1,1) PRIMARY KEY,
                    [Log_Type] NVARCHAR(50),
                    [Log_Error] NVARCHAR(MAX),
                    [Log_Created] DATETIME2,
                    [Log_FileName] NVARCHAR(255),
                    [Log_Path] NVARCHAR(500),
                    [Log_SendEmail] BIT,
                    [Email_ID] INT,
                    [Saved_In_Database] BIT
                );

                IF OBJECT_ID('EmailLog','U') IS NULL
                CREATE TABLE [EmailLog] (
                    [ID] INT IDENTITY(1,1) PRIMARY KEY,
                    [From_Email] NVARCHAR(255),
                    [To_Email] NVARCHAR(255),
                    [Bcc] NVARCHAR(255),
                    [Email_Subject] NVARCHAR(255),
                    [Email_Body] NVARCHAR(MAX),
                    [Time_Sent] DATETIME2
                );");

            using var cmd = new SqlCommand(query.ToString(), connection);
            
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
