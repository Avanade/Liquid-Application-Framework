using Liquid.Core.Extensions;
using Liquid.Core.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Repository.Mongo.Extensions
{
    /// <summary>
    /// Mongo Db Service Collection Extensions Class.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a <see cref="MongoRepository{TEntity, TIdentifier}"/> for the entity <typeparamref name="TEntity"/>,
        /// and a <see cref="MongoClientFactory"/>  if not previously registered.
        /// This method also registers <see cref="LiquidTelemetryInterceptor"/> for MongoRepository instance.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that the repository should correspond to</typeparam>
        /// <typeparam name="TIdentifier">Entity identifier type.</typeparam>
        /// <param name="services">Extended ServiceCollection object.</param>
        /// <param name="databaseName">Database name that should be context related.</param>
        /// <returns></returns>
        public static IServiceCollection AddLiquidMongoRepository<TEntity, TIdentifier>(this IServiceCollection services, string databaseName)
            where TEntity : LiquidEntity<TIdentifier>, new()
        {
            services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddScoped((sp) =>
            {
                var context = new MongoDataContext(
                    databaseName, sp.GetService<IMongoClientFactory>());
                return new MongoRepository<TEntity, TIdentifier>(context);
            });

            services.AddLiquidInterceptors<ILiquidRepository<TEntity, TIdentifier>, MongoRepository<TEntity, TIdentifier>>();

            return services;
        }

    }
}
