using System.Text.Json.Serialization;

namespace SimpLog.Databases.MSSQL.Models.AppSettings
{
    internal class DatabaseConfiguration
    {
        [JsonPropertyName("Connection_String")]
        public string? Connection_String { get; set; }

        [JsonPropertyName("Use_OleDB")]
        public bool? Use_OleDB { get; set; }

        [JsonPropertyName("Global_Enabled_Save")]
        public bool? Global_Enabled_Save { get; set; } = true;
    }
}
