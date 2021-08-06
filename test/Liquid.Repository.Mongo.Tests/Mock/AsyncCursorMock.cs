using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Repository.Mongo.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    class AsyncCursorMock<TDocument> : IAsyncCursor<TDocument>
    {
        public IEnumerable<TDocument> Current { get; set; }

        public AsyncCursorMock(IEnumerable<TDocument> documents)
        {
            Current = documents;
        }

        public void Dispose()
        {
            Current.GetEnumerator().Dispose();
        }

        public bool MoveNext(CancellationToken cancellationToken = default)
        {
            return Current.GetEnumerator().MoveNext();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<bool> MoveNextAsync(CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return Current.GetEnumerator().MoveNext();
        }

        public IEnumerable<TDocument> ToEnumerable()
        {
            return Current.ToList();
        }
    }
}

