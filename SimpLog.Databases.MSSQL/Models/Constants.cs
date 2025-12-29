using System.IO;

namespace SimpLog.Databases.MSSQL.Models
{
    internal class Constants
    {
        #region File Settings
        public const string FileFormat = ".txt";
        public static readonly char PathSeparator = Path.DirectorySeparatorChar;
        #endregion

        #region Logging Settings
        public const string DateFormat = "yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'";
        public const string Separator = " | ";
        #endregion
    }
}
