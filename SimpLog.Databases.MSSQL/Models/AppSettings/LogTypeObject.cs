using System.Text.Json.Serialization;

namespace SimpLog.Databases.MSSQL.Models.AppSettings
{
    internal class LogTypeObject
    {
        [JsonPropertyName("SaveInDatabase")]
        public bool? SaveInDatabase { get; set; }
    }
}
