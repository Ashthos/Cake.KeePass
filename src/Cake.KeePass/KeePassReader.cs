namespace Cake.KeePass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Cake.Core;

    using KeePassLib;
    using KeePassLib.Interfaces;
    using KeePassLib.Keys;
    using KeePassLib.Serialization;

    /// <summary>
    /// KeePass database reader class.
    /// </summary>
    public static class KeePassReader
    {
        private const string TitleFieldName = "Title";
        private const string UserNameFieldName = "UserName";
        private const string UrlFieldName = "URL";
        private const string NotesFieldName = "Notes";
        private const string PasswordFieldName = "Password";

        /// <summary>
        /// Attempts to read from the specified database and locate the entry based on the entry search criteria.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="databaseSettings">Database <see cref="KeePassDatabaseSettings">Settings</see>.</param>
        /// <param name="entryCriteria"><see cref="KeePassEntryCriteria">Structure</see> describing the key to load.</param>
        /// <returns>A populated <see cref="KeePassEntry"/> entry from the database.</returns>
        public static KeePassEntry Execute(ICakeContext context, KeePassDatabaseSettings databaseSettings, KeePassEntryCriteria entryCriteria)
        {
            if (databaseSettings == null)
            {
                throw new ArgumentNullException(nameof(databaseSettings));
            }

            if (entryCriteria == null)
            {
                throw new ArgumentNullException(nameof(entryCriteria));
            }

            var keePassDb = OpenDb(databaseSettings);
            var groupNode = keePassDb.RootGroup;

            if (entryCriteria.GroupHierarchy == null)
            {
                entryCriteria.GroupHierarchy = new List<string>();
            }

            foreach (var group in entryCriteria.GroupHierarchy)
            {
                var subGroup = groupNode.Groups.FirstOrDefault(x => x.Name == group);

                groupNode = subGroup ?? throw new KeyNotFoundException($"Group '{group}' was not found in the hierarchy.");
            }

            var key = groupNode.Entries.FirstOrDefault(
                x =>
                    {
                        if (!Contains(x, TitleFieldName, entryCriteria.Title))
                        {
                            return false;
                        }

                        if (!Contains(x, UrlFieldName, entryCriteria.Url))
                        {
                            return false;
                        }

                        if (!Contains(x, UserNameFieldName, entryCriteria.Username))
                        {
                            return false;
                        }

                        if (!string.IsNullOrWhiteSpace(entryCriteria.Uuid))
                        {                            
                            var uuid = new PwUuid(HexStringToByteArray(entryCriteria.Uuid));
                            if (x.Uuid.CompareTo(uuid) != 0)
                            {
                                return false;
                            }
                        }                        

                        return true;
                    });

            if (key == null)
            {
                throw new KeyNotFoundException("An entry matching the criteria was not found in the target group.");
            }

            var ret = new KeePassEntry
                          {
                              Title = key.Strings.ReadSafe(TitleFieldName),
                              Notes = key.Strings.ReadSafe(NotesFieldName),
                              Password = key.Strings.ReadSafe(PasswordFieldName),
                              Url = key.Strings.ReadSafe(UrlFieldName),
                              Username = key.Strings.ReadSafe(UserNameFieldName),
                              Uuid = key.Uuid.ToHexString()
                          };

            return ret;
        }

        private static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        private static bool Contains(PwEntry entry, string fieldName, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                if (!entry.Strings.Exists(fieldName))
                {
                    return false;
                }

                var fieldValue = entry.Strings.Get(fieldName);

                if (!fieldValue.ReadString().Contains(search))
                {
                    return false;
                }
            }

            return true;
        }

        private static PwDatabase OpenDb(KeePassDatabaseSettings databaseSettings)
        {
            var ioConnection = new IOConnectionInfo { Path = databaseSettings.DatabasePath.FullPath };

            var userKey = new KcpPassword(databaseSettings.MasterPassword);

            var compositeKey = new CompositeKey();
            compositeKey.AddUserKey(userKey);

            var database = new KeePassLib.PwDatabase();

            try
            {
                database.Open(ioConnection, compositeKey, new NullStatusLogger());
            }
            catch (FormatException)
            {
                throw new Exception("Invalid database file.");
            }
            catch (InvalidCompositeKeyException)
            {
                throw new Exception("Composite Key (password) provided was invalid.");
            }

            if (!database.IsOpen)
            {
                throw new Exception("Failed to open KeePass Database");
            }

            return database;
        }
    }
}
