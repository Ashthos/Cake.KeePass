namespace Cake.KeePass
{
    using Cake.Core.IO;

    /// <summary>
    /// Database file settings
    /// </summary>
    public class KeePassDatabaseSettings
    {
        /// <summary>
        /// The file path to the database.
        /// <remarks>
        /// Path can be local or on a unc share.
        /// </remarks>
        /// </summary>
        public FilePath DatabasePath { get; set; }

        /// <summary>
        /// The Master password.
        /// </summary>
        public string MasterPassword { get; set; }
    }
}