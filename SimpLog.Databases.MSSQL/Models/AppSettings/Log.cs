namespace SimpLog.Databases.MSSQL.Models.AppSettings
{
    internal class Log
    {
        public LogTypeObject? Trace { get; set; }

        public LogTypeObject? Debug { get; set; }

        public LogTypeObject? Info { get; set; }

        public LogTypeObject? Notice { get; set; }

        public LogTypeObject? Warn { get; set; }

        public LogTypeObject? Error { get; set; }

        public LogTypeObject? Fatal { get; set; }
    }
}
