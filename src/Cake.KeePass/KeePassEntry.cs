namespace Cake.KeePass
{
    /// <summary>
    /// A entry retrieved from the KeePass database.
    /// </summary>
    public class KeePassEntry
    {
        /// <summary>
        /// The entry title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The entry Username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The entry password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The entry Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The entry notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// The Uuid of the entry.
        /// </summary>
        public string Uuid { get; set; }
    }
}
