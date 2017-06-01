namespace Cake.KeePass
{
    using System.Collections.Generic;

    public class KeePassEntryCriteria
    {
        public IEnumerable<string> GroupHierarchy { get; set; }

        public string Title { get; set; }

        public string Username { get; set; }

        public string Url { get; set; }

        public string Uuid { get; set; }
    }
}
