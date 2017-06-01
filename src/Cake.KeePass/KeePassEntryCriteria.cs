namespace Cake.KeePass
{
    using System.Collections.Generic;

    /// <summary>
    /// The entry retrieval criteria.
    /// </summary>
    public class KeePassEntryCriteria
    {
        /// <summary>
        /// The entry's group hierarchy, with one string per hierarchical level.
        /// </summary>
        public IEnumerable<string> GroupHierarchy { get; set; }

        /// <summary>
        /// The title matching criteria.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The username matching criteria.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The Url matching criteria.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The Uuid matching criteria.
        /// </summary>
        public string Uuid { get; set; }
    }
}
