namespace Cake.KeePass
{
    using System;

    using Cake.Core;
    using Cake.Core.Annotations;

    /// <summary>
    /// Contains functionality for working with KeePass.
    /// </summary>
    [CakeAliasCategory("KeePass")]
    public static class KeePassAlias
    {
        /// <summary>
        /// Reads a single entry from a Keepass database.
        /// </summary>
        /// <example>
        /// <code>
        /// var dbSettings = new KeePassDatabaseSettings { DatabasePath = File("./my-database.kdbx", MasterPassword = "Password123" };
        /// var entryCriteria = new KeePassEntryCriteria { Title = "my-entry-title" };
        /// var entry = KeePassReadEntry(databaseSettings, entryCritieria);
        /// </code>
        /// </example>
        /// <remarks>
        /// Will throw a plain System.Exception if any error occurs during key retrieval.
        /// </remarks>
        /// <param name="context">The Context.</param>
        /// <param name="databaseSettings">Database <see cref="KeePassDatabaseSettings">Settings</see>.</param>
        /// <param name="keePassEntryCriteria"><see cref="KeePassEntryCriteria">Structure</see> describing the key to load.</param>
        /// <returns>A populated <see cref="KeePassEntry"/> entry from the database.</returns>
        [CakeMethodAlias]
        public static KeePassEntry KeePassReadEntry(this ICakeContext context, KeePassDatabaseSettings databaseSettings, KeePassEntryCriteria keePassEntryCriteria)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (databaseSettings == null)
            {
                throw new ArgumentNullException(nameof(databaseSettings));
            }

            if (keePassEntryCriteria == null)
            {
                throw new ArgumentNullException(nameof(keePassEntryCriteria));
            }

            return KeePassReader.Execute(context, databaseSettings, keePassEntryCriteria);
        }
    }
}
