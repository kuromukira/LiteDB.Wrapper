using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Wrapper
{
    /// <summary>Collection Sort Options</summary>
    public class SortOptions
    {
        /// <summary></summary>
        public enum Order
        {
            /// <summary>Ascending</summary>
            ASC = 1,
            /// <summary>Descending</summary>
            DSC = -1
        }
        /// <summary>Ascending or Descending. Default is DSC</summary>
        public Order Sort { get; } = Order.DSC;
        /// <summary>Sort Field</summary>
        public string Field { get; }

        /// <summary></summary>
        public SortOptions(string field) => Field = field ?? string.Empty;

        /// <summary></summary>
        public SortOptions(Order sort, string field)
        {
            Sort = sort;
            Field = field ?? string.Empty;
        }
    }

    /// <summary>Collection Pagination Options</summary>
    public class PageOptions
    {
        /// <summary>Page offset</summary>
        public int Offset { get; }
        /// <summary>Total # of rows to fetch per page</summary>
        public int Rows { get; }

        /// <summary></summary>
        public PageOptions(int offset, int rows)
        {
            if (rows <= 0)
                throw new ArgumentOutOfRangeException("Total # of rows to fetch per page cannot be less than or equal to 0.");
            else if (offset < 0)
                throw new ArgumentOutOfRangeException("Page offset cannot be less than 0.");
            Offset = offset;
            Rows = rows;
        }
    }

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

    /// <summary></summary>
    public class CollectionReference<T> : IDisposable
    {
        /// <summary>Configuration for LiteDB.Wrapper</summary>
        public CollectionReferenceConfig Config { get; }

        /// <summary></summary>
        public CollectionReference(string location, string collection) => Config = new CollectionReferenceConfig(location, collection);

        /// <summary>Insert an object to the referenced collection.</summary>
        public virtual void Insert(T obj) => Insert(new List<T> { obj });

        /// <summary>Insert a list of objects to the referenced collection.</summary>
        public virtual void Insert(IList<T> objList)
        {
            try
            {
                using (LiteRepository _liteDB = new LiteRepository(Config.Location))
                {
                    _liteDB.Insert<T>(objList, Config.Collection);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>Update an object in the referenced collection.</summary>
        public virtual void Update(T obj) => Update(new List<T> { obj });

        /// <summary>Update a list of objects in the referenced collection.</summary>
        public virtual void Update(IList<T> objList)
        {
            try
            {
                using (LiteRepository _liteDB = new LiteRepository(Config.Location))
                {
                    _liteDB.Update<T>(objList, Config.Collection);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>Remove an object that matches the given id.</summary>
        public virtual void Remove(Guid id) => Remove(new List<Guid> { id });

        /// <summary>Remove a list of objects that matches the given ids.</summary>
        public virtual void Remove(IList<Guid> idList)
        {
            try
            {
                using (LiteRepository _liteDB = new LiteRepository(Config.Location))
                {
                    _liteDB.Delete<T>(Query.Where("_id", id => idList.Contains(id)), Config.Collection);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>Get an item from the referenced collection.</summary>
        public virtual T Get(Guid id)
        {
            try
            {
                using (LiteDatabase _liteDB = new LiteDatabase(Config.Location))
                {
                    var _collection = _liteDB.GetCollection<T>(Config.Collection);
                    return _collection.IncludeAll().FindById(id);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>Get a paginated list of items from the referenced collection.</summary>
        public virtual (IList<T> list, long rows) GetPaged(PageOptions pageOptions, SortOptions sortOptions)
        {
            try
            {
                using (LiteDatabase _liteDB = new LiteDatabase(Config.Location))
                {
                    var _collection = _liteDB.GetCollection<T>(Config.Collection);
                    _collection.EnsureIndex(sortOptions.Field, true);
                    long _countAll = _liteDB.GetCollection<T>(Config.Collection).Count();
                    return (_collection.IncludeAll().Find(Query.All(sortOptions.Field, (int)sortOptions.Sort), pageOptions.Offset, pageOptions.Rows).ToList(), _countAll);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>Explicitly drop currenct collection.</summary>
        public void Drop()
        {
            try
            {
                using (LiteDatabase _liteDb = new LiteDatabase(Config.Location))
                {
                    _liteDb.DropCollection(Config.Collection);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary></summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) { }
                disposedValue = true;
            }
        }

        /// <summary></summary>
        public void Dispose() => Dispose(true);
        #endregion
    }
}
