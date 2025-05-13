using Liquid.Core.Entities;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Repository.Mongo.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        /// This method may also registers <see cref="LiquidTelemetryInterceptor"/> for MongoRepository instance.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that the repository should correspond to</typeparam>
        /// <typeparam name="TIdentifier">Entity identifier type.</typeparam>
        /// <param name="services">Extended ServiceCollection object.</param>
        /// <param name="sectionName">Name of the configuration section where all entities have their repository settings configured.</param>
        /// <param name="collectionName">Name of the collection in the database that the repository should correspond to.</param>
        /// <param name="activateTelemetry">Specifies whether the telemetry should be activated or not for this repository. Default: True.</param>
        public static IServiceCollection AddLiquidMongoRepository<TEntity, TIdentifier>(this IServiceCollection services, string sectionName, string collectionName, bool activateTelemetry = true)
            where TEntity : LiquidEntity<TIdentifier>, new()
        {
            services.TryAddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddOptions<MongoDbSettings>()
             .Configure<IConfiguration>((settings, configuration) =>
             {
                 configuration.GetSection(sectionName).Bind(settings);
             });

            services.AddScoped<IMongoDataContext<TEntity>>((provider) =>
            {
                return ActivatorUtilities.CreateInstance<MongoDataContext<TEntity>>(provider, collectionName);
            });


            if (activateTelemetry)
            {
                services.AddScoped<MongoRepository<TEntity, TIdentifier>>();
                services.AddScopedLiquidTelemetry<ILiquidRepository<TEntity, TIdentifier>, MongoRepository<TEntity, TIdentifier>>();
            }
            else
            {
                services.AddScoped<ILiquidRepository<TEntity, TIdentifier>, MongoRepository<TEntity, TIdentifier>>();
            }


            return services;
        }

    }
}
