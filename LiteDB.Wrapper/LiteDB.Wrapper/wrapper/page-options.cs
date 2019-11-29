using System;

namespace LiteDB.Wrapper
{
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
}