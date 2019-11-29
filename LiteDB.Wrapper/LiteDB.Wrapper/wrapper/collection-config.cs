using System;

namespace LiteDB.Wrapper
{
    /// <summary>Configuration for LiteDB.Wrapper</summary>
    public class CollectionReferenceConfig
    {
        /// <summary>LiteDB file location</summary>
        public string Location { get; }
        /// <summary>Collection name for reference</summary>
        public string Collection { get; }

        /// <summary></summary>
        internal CollectionReferenceConfig(string location, string collection)
        {
            if (string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(collection))
                throw new Exception("Failed to initialize LiteDB Repository.");
            Location = location;
            Collection = collection;
        }
    }
}