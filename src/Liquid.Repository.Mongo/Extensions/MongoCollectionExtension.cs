using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Liquid.Repository.Mongo.Extensions
{
    /// <summary>
    /// Extends <see cref="IMongoCollection{TDocument}"/> methods.
    /// </summary>
    public static class MongoCollectionExtension
    {
        /// <summary>
        ///   Inserts a single document.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="session">The transaction session.</param>
        /// <returns></returns>
        public static async Task InsertOneAsync<TDocument>(this IMongoCollection<TDocument> collection, TDocument document, IClientSessionHandle session = null)
        {
            if(session is null)
            {
                await collection.InsertOneAsync(document);
            }
            else
            {
                await collection.InsertOneAsync(session, document);
            }
        }
        /// <summary>
        /// Finds the documents matching the filter.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection"> The collection.</param>
        /// <param name="filter"> The filter.</param>
        /// <param name="session">The transaction session.</param>
        /// <returns></returns>
        public static async Task<IAsyncCursor<TDocument>> FindAsync<TDocument>(this IMongoCollection<TDocument> collection, FilterDefinition<TDocument> filter, IClientSessionHandle session = null)
        {
            if (session is null)
            {
                return await collection.FindAsync(filter, options: null, cancellationToken: default);
            }

            return await collection.FindAsync(session, filter);
        }
        /// <summary>
        /// Finds the documents matching the filter.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection"> The collection.</param>
        /// <param name="filter"> The filter.</param>
        /// <param name="session">The transaction session.</param>
        /// <returns></returns>
        public static async Task<IAsyncCursor<TDocument>> FindAsync<TDocument>(this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter, IClientSessionHandle session = null)
        {
            if (session is null)
            {
                return await collection.FindAsync(filter, options: null, cancellationToken: default);
            }

            return await collection.FindAsync(session, filter);
        }
        /// <summary>
        ///  Deletes a single document.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter</param>
        /// <param name="session">The transaction session</param>
        /// <returns></returns>
        public static async Task<DeleteResult> DeleteOneAsync<TDocument>(this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter, IClientSessionHandle session = null)
        {
            if (session is null)
            {
                return await collection.DeleteOneAsync(filter);
            }

            return await collection.DeleteOneAsync(session, filter);
        }
        /// <summary>
        /// Replaces a single document.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter"> The filter.</param>
        /// <param name="replacement">The replacement.</param>
        /// <param name="options">The options.</param>
        /// <param name="session">The transaction session.</param></param>
        /// <returns></returns>
        public static async Task<ReplaceOneResult> ReplaceOneAsync<TDocument>(this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter, TDocument replacement, ReplaceOptions options = null, IClientSessionHandle session = null)
        {
            if (session is null)
            {
                return await collection.ReplaceOneAsync(filter, replacement, options);
            }

            return await collection.ReplaceOneAsync(session, filter, replacement, options);
        }
    }
}
