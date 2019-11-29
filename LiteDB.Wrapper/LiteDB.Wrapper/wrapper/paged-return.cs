using System.Collections.Generic;

namespace LiteDB.Wrapper
{
    /// <summary>Result for getting paged collection</summary>
    public struct PagedResult<T>
    {
        /// <summary>Result for getting paged collection</summary>
        public PagedResult(long totalRows, IList<T> result)
        {
            TotalRows = totalRows;
            Result = result;
        }

        /// <summary>Total number of rows in the collection</summary>
        public long TotalRows { get; set; }

        /// <summary>Paged rows</summary>
        public IList<T> Result { get; set; }
    }
}