using SimpLog.Databases.MSSQL.Models.AppSettings;
using System.Data.SqlClient;
using System.Text;

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
        public static void CreateMSSqlIfNotExists(SqlConnection connection, SqlCommand cmd)
        {
            StringBuilder query = new StringBuilder();

            query.Append($" if object_id({"'StoreLog'"}, {"'U'"}) is null ");
            query.Append($"    create table [StoreLog] ");
            query.Append($"    ( ");
            query.Append($"       [{"ID"}] int IDENTITY(1,1) PRIMARY KEY ");
            query.Append($"      ,[{"Log_Type"}] varchar(50) ");
            query.Append($"      ,[{"Log_Error"}] varchar(50) ");
            query.Append($"      ,[{"Log_Created"}] varchar(50) ");
            query.Append($"      ,[{"Log_FileName"}] varchar(50) ");
            query.Append($"      ,[{"Log_Path"}] varchar(50) ");
            query.Append($"      ,[{"Log_SendEmail"}] bit ");
            query.Append($"      ,[{"Email_ID"}] int ");
            query.Append($"      ,[{"Saved_In_Database"}] varchar(50) ");
            query.Append($"    ) ");

            query.Append($" if object_id({"'EmailLog'"}, {"'U'"}) is null ");
            query.Append($"    create table [EmailLog] ");
            query.Append($"    ( ");
            query.Append($"       [{"ID"}] int IDENTITY(1,1) PRIMARY KEY ");
            query.Append($"      ,[{"From_Email"}] varchar(50) ");
            query.Append($"      ,[{"To_Email"}] varchar(50) ");
            query.Append($"      ,[{"Bcc"}] varchar(50) ");
            query.Append($"      ,[{"Email_Subject"}] varchar(50) ");
            query.Append($"      ,[{"Email_Body"}] varchar(50) ");
            query.Append($"      ,[{"Time_Sent"}] varchar(50) ");
            query.Append($"    ) ");

            connection.Open();

            cmd.CommandText = query.ToString();
            cmd.ExecuteNonQuery();
            
            connection.Close();
        }

    }
}
