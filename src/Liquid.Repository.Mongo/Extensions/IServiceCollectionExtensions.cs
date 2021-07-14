using Liquid.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Linq;
using System.Reflection;

namespace Liquid.Repository.Mongo.Extensions
{
    /// <summary>
    /// Mongo Db Service Collection Extensions Class.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a <see cref="MongoRepository{TEntity, TIdentifier}"/> for the informed entity,
        /// injecting a new instance of its database context.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that the repository should correspond to</typeparam>
        /// <typeparam name="TIdentifier">Entity identifier type.</typeparam>
        /// <param name="services">Database name that should be context related.</param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public static IServiceCollection AddLiquidMongoRepository<TEntity,TIdentifier>(this IServiceCollection services, string databaseName)
            where TEntity : LiquidEntity<TIdentifier>, new()
        {
            services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddScoped((sp) => {
                var context = new MongoDataContext(
                    databaseName, sp.GetService<IMongoClientFactory>());
                return new MongoRepository<TEntity, TIdentifier>(context);
            });

            services.AddLiquidInterceptors<ILightRepository<TEntity, TIdentifier>, MongoRepository<TEntity, TIdentifier>>();

            return services;
        }

    }
}
