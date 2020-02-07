using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteDB.Wrapper.Test
{
    internal class WrapperModel : IDisposable
    {
        [BsonId]
        public Guid _ID { get; set; }

        [BsonField]
        public string Word { get; set; }

        [BsonField]
        public int Number { get; set; }

        [BsonField]
        public DateTime Added { get; set; }

        #region IDisposable Support
        [BsonIgnore]
        private bool disposedValue = false; // To detect redundant calls

        /// <summary></summary>
        [BsonIgnore]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) { }
                disposedValue = true;
            }
        }

        /// <summary></summary>
        [BsonIgnore]
        public void Dispose() => Dispose(true);
        #endregion
    }

    internal class WrapperData
    {
        private readonly Random _random = new Random();

        internal int Number() => _random.Next(0, 9);

        internal string Word()
        {
            int _size = _random.Next(3, 5);
            StringBuilder _builder = new StringBuilder(string.Empty);
            for (int i = 0; i < _size; i++)
                _builder.Append((char)_random.Next(65, 90));
            return _builder.ToString();
        }

        public WrapperModel GetModel() => new WrapperModel
        {
            _ID = Guid.NewGuid(),
            Word = Word(),
            Number = Number(),
            Added = DateTime.Now
        };

        public IList<WrapperModel> GetModel(int limit) => Enumerable.Range(1, limit).Select(i => GetModel()).ToList();
    }
}
