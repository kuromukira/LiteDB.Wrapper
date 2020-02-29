namespace LiteDB.Wrapper
{
    /// <summary>Filter the result in the collection</summary>
    public struct FilterOptions
    {
        /// <summary>Filter Options</summary>
        public enum Options
        {
            /// <summary></summary>
            Equals,
            /// <summary></summary>
            GreaterThan,
            /// <summary></summary>
            LesserThan,
            /// <summary></summary>
            Within
        }

        /// <summary>Filter the result in the collection</summary>
        public FilterOptions(string fieldName, Options options, BsonValue fieldValue)
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
            FieldOptions = options;
        }

        /// <summary></summary>
        public Options FieldOptions { get; set; }

        /// <summary>Field name of the filter</summary>
        public string FieldName { get; set; }

        /// <summary>Value of the filter</summary>
        public BsonValue FieldValue { get; set; }
    }
}