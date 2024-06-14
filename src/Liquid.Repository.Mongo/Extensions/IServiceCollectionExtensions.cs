using Liquid.Core.Entities;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Repository.Mongo.Configuration;
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
        /// <param name="entitiesConfigurationRootSectionName">Name of the configuration section where all entities have their repository settings configured. Default: "Liquid:RepositorySettings:Entities".</param>
        /// <param name="activateTelemetry">Specifies whether the telemetry should be activated or not for this repository. Default: True.</param>
        public static IServiceCollection AddLiquidMongoRepository<TEntity, TIdentifier>(this IServiceCollection services, string entitiesConfigurationRootSectionName = "Liquid:RepositorySettings:Entities", bool activateTelemetry = true)
            where TEntity : LiquidEntity<TIdentifier>, new()
        {
            services.TryAddSingleton<IMongoClientFactory, MongoClientFactory>();
            services.TryAddSingleton<IMongoEntitySettingsFactory>(provider => { return ActivatorUtilities.CreateInstance<MongoEntitySettingsFactory>(provider, entitiesConfigurationRootSectionName); });

            services.AddScoped<IMongoDataContext<TEntity>, MongoDataContext<TEntity>>();


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
