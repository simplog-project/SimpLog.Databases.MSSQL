using SimpLog.Databases.MSSQL.Models.AppSettings;
using System;
using System.IO;
using System.Text.Json;

namespace SimpLog.Databases.MSSQL.Services.ConfigurationServices
{
    internal static class ConfigService
    {
        internal static Configuration CurrentConfiguration { get; private set; }

        static ConfigService()
        {
            var configPath = Path.Combine(Environment.CurrentDirectory, "simplog.json");

            CurrentConfiguration = File.Exists(configPath)
                ? JsonSerializer.Deserialize<Configuration>(File.ReadAllText(configPath), new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip })
                : new Configuration(); // defaults
        }

        internal static bool PathCheck(string? path) => !string.IsNullOrEmpty(path) && Directory.Exists(path);

        internal static Configuration BindConfigObject() => CurrentConfiguration;
    }
}
