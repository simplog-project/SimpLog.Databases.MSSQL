using System.Text.Json.Serialization;

namespace SimpLog.Databases.MSSQL.Models.AppSettings
{
    internal class Configuration
    {
        [JsonPropertyName("Database_Configuration")]
        public DatabaseConfiguration? Database_Configuration { get; set; }

        [JsonPropertyName("LogType")]
        public Log? LogType { get; set; }
    }
}
