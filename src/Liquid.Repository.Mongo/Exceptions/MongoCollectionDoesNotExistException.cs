using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Repository.Mongo.Exceptions
{
    /// <summary>
    /// Occurs when the Mongo Db collection does not exist in database.
    /// </summary>
    /// <seealso cref="LightException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MongoCollectionDoesNotExistException : LightException, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoCollectionDoesNotExistException"/> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="databaseName">Name of the database.</param>
        public MongoCollectionDoesNotExistException(string collectionName, string databaseName)
            : base($"The Mongo Db collection {collectionName} does not exist in database {databaseName}. Please check name or create a collection.")
        {
        }
    }
}
