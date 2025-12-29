using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace SimpLog.Databases.MSSQL.Models
{
    internal static class EnumExtensions
    {
        private static readonly ConcurrentDictionary<string, string> DisplayNameCache 
            = new ConcurrentDictionary<string, string>();

        public static string DisplayName(this Enum value)
        {
            var key = $"{value.GetType().FullName}.{value}";

            var displayName = DisplayNameCache.GetOrAdd(key, x =>
            {
                var name = (DescriptionAttribute[])value
                    .GetType()
                    .GetTypeInfo()
                    .GetField(value.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false);

                return name.Length > 0 ? name[0].Description : value.ToString();
            });

            return displayName;
        }
    }

    public enum LogType
    {
        /// <summary>
        /// TRACE level: This should be used during development to track bugs, but never committed to your VCS.
        /// </summary>
        [EnumMember(Value = "Trace")] Trace,

        /// <summary>
        /// DEBUG level: log at this level about anything that happens in the program. This is mostly used during 
        /// debugging, and I’d advocate trimming down the number of debug statement before entering the production 
        /// stage, so that only the most meaningful entries are left, and can be activated during troubleshooting.
        /// </summary>
        [EnumMember(Value = "Debug")] Debug,

        /// <summary>
        /// INFO level: log at this level all actions that are user-driven, or system 
        /// specific (ie regularly scheduled operations…)
        /// </summary>
        [EnumMember(Value = "Info")] Info,

        /// <summary>
        /// NOTICE level: this will certainly be the level at which the program will run when 
        /// in production. Log at this level all the notable events that are not considered an error.
        /// </summary>
        [EnumMember(Value = "Notice")] Notice,

        /// <summary>
        /// WARN level: log at this level all events that could potentially 
        /// become an error. For instance if one database call took more than a 
        /// predefined time, or if an in-memory cache is near capacity. This will 
        /// allow proper automated alerting, and during troubleshooting will 
        /// allow to better understand how the system was behaving before the failure.
        /// </summary>
        [EnumMember(Value = "Warn")] Warn,

        /// <summary>
        /// ERROR level: log every error condition at this level. 
        /// That can be API calls that return errors or internal error conditions.
        /// </summary>
        [EnumMember(Value = "Error")] Error,

        /// <summary>
        /// FATAL level: too bad, it’s doomsday. Use this very 
        /// scarcely, this shouldn’t happen a lot in a real program. Usually 
        /// logging at this level signifies the end of the program. 
        /// For instance, if a network daemon can’t bind a network socket, log at 
        /// this level and exit is the only sensible thing to do.
        /// </summary>
        [EnumMember(Value = "Fatal")] Fatal,
    }

    /// <summary>
    /// Extension methods for LogType to get string labels for logging.
    /// </summary>
    public static class LogTypeExtensions
    {
        public static string ToLabel(this LogType logType) => logType switch
        {
            LogType.Trace => $"{nameof(LogType.Trace).ToUpperInvariant()}:",
            LogType.Debug => $"{nameof(LogType.Debug).ToUpperInvariant()}:",
            LogType.Info => $"{nameof(LogType.Info).ToUpperInvariant()}:",
            LogType.Notice => $"{nameof(LogType.Notice).ToUpperInvariant()}:",
            LogType.Warn => $"{nameof(LogType.Warn).ToUpperInvariant()}:",
            LogType.Error => $"{nameof(LogType.Error).ToUpperInvariant()}:",
            LogType.Fatal => $"{nameof(LogType.Fatal).ToUpperInvariant()}:",
            _ => "NO TYPE:"
        };
    }

}
