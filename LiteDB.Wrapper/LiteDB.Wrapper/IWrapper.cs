using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiteDB.Wrapper.Interface
{
    /// <summary>Collection Referance interface</summary>
    public interface ICollectionRef<T>
    {
        /// <summary>Configuration for LiteDB.Wrapper</summary>
        CollectionReferenceConfig Config { get; }

        /// <summary>Insert an object to the referenced collection.</summary>
        void Insert(T obj);

        /// <summary>Insert a list of objects to the referenced collection.</summary>
        void Insert(IList<T> objs);

        /// <summary>Update an object in the referenced collection.</summary>
        void Update(T obj);

        /// <summary>Update a list of objects in the referenced collection.</summary>
        void Update(IList<T> objs);

        /// <summary>Remove an object that matches the given id.</summary>
        void Remove(Guid id);

        /// <summary>Remove objects that matches the given ids.</summary>
        void Remove(IList<Guid> ids);

        /// <summary>Commit changes made to the collection.</summary>
        Task Commit();

        /// <summary>Get an item from the referenced collection.</summary>
        T Get(Guid id);

        /// <summary>Get a paginated list of items from the referenced collection.</summary>
        PagedResult<T> GetPaged(PageOptions pageOptions, SortOptions sortOptions);

        /// <summary>Explicitly drop currenct collection.</summary>
        void Drop();
    }
}