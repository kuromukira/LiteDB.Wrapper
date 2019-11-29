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
}