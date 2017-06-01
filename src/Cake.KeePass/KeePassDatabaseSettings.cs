namespace Cake.KeePass
{
    using Cake.Core.IO;

    public class KeePassDatabaseSettings
    {
        public FilePath DatabasePath { get; set; }

        public string MasterPassword { get; set; }
    }
}