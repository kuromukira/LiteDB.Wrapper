using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteDB.Wrapper
{
    /// <summary></summary>
    public class CollectionReference<T> : IDisposable
    {
        /// <summary>Configuration for LiteDB.Wrapper</summary>
        public CollectionReferenceConfig Config { get; }
        private IList<T> ToSave { get; set; } = new List<T>();
        private IList<T> ToModify { get; set; } = new List<T>();
        private IList<Guid> ToRemove { get; set; } = new List<Guid>();

        /// <summary></summary>
        public CollectionReference(string location, string collection) => Config = new CollectionReferenceConfig(location, collection);

        /// <summary>Insert an object to the referenced collection.</summary>
        public void Insert(T obj) => ToSave.Add(obj);
        /// <summary>Insert a list of objects to the referenced collection.</summary>
        public void Insert(IList<T> objs) => ToSave = ToSave.Concat(objs).ToList();

        /// <summary>Update an object in the referenced collection.</summary>
        public void Update(T obj) => ToModify.Add(obj);
        /// <summary>Update a list of objects in the referenced collection.</summary>
        public void Update(IList<T> objs) => ToModify = ToModify.Concat(objs).ToList();

        /// <summary>Remove an object that matches the given id.</summary>
        public void Remove(Guid id) => ToRemove.Add(id);
        /// <summary>Remove objects that matches the given ids.</summary>
        public void Remove(IList<Guid> ids) => ToRemove = ToRemove.Concat(ids).ToList();

        /// <summary>Commit changes made to the collection.</summary>
        public async Task Commit()
        {
            try
            {
                // For some odd reasons, current LiteDB version does not support transaction
                using (LiteRepository _liteRepo = new LiteRepository(Config.Location))
                {
                    if (ToSave.Any() || ToModify.Any())
                    {
                        IList<T> _combinedList = ToSave.Concat(ToModify).ToList();
                        _liteRepo.Upsert<T>(_combinedList, Config.Collection);
                    }
                    if (ToRemove.Any())
                        _liteRepo.Delete<T>(Query.Where("_id", id => ToRemove.Contains(id)), Config.Collection);
                }
                await Task.Run(() =>
                {
                    ToSave.Clear();
                    ToModify.Clear();
                    ToRemove.Clear();
                });
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
        public virtual PagedResult<T> GetPaged(PageOptions pageOptions, SortOptions sortOptions)
        {
            try
            {
                using (LiteDatabase _liteDB = new LiteDatabase(Config.Location))
                {
                    var _collection = _liteDB.GetCollection<T>(Config.Collection);
                    _collection.EnsureIndex(sortOptions.Field, true);
                    long _countAll = _liteDB.GetCollection<T>(Config.Collection).Count();
                    return new PagedResult<T>(_countAll, _collection.IncludeAll().Find(Query.All(sortOptions.Field, (int)sortOptions.Sort), pageOptions.Offset, pageOptions.Rows).ToList());
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
