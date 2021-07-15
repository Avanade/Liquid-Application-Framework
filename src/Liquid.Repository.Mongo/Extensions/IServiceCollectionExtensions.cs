using Liquid.Core.Extensions;
using Liquid.Core.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

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
        public static IServiceCollection AddLiquidMongoRepository<TEntity, TIdentifier>(this IServiceCollection services)
            where TEntity : LiquidEntity<TIdentifier>, new()
        {
            if (services.FirstOrDefault(x => x.ServiceType == typeof(IMongoClientFactory)) is null)
                services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddScoped<IMongoDataContext<TEntity>, MongoDataContext<TEntity>>();

            services.AddScoped<MongoRepository<TEntity,TIdentifier>>();

            services.AddLiquidInterceptors<ILiquidRepository<TEntity, TIdentifier>, MongoRepository<TEntity, TIdentifier>>();

            return services;
        }

    }
}
