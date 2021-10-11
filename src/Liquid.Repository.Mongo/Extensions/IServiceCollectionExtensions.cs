using Liguid.Repository.Configuration;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
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
        /// <param name="mongoDatabasesConfigurationSection">Configuration section where databases have their options configured.</param>
        /// <param name="entityOptions">MongoEntityOptions to configure how the entity will be persisted on Mongo.</param>
        /// <param name="activateTelemetry">Specifies whether the telemetry should be activated or not for this repository. Default: True.</param>
        public static IServiceCollection AddLiquidMongoRepository<TEntity, TIdentifier>(this IServiceCollection services, IConfiguration mongoDatabasesConfigurationSection, Action<MongoEntityOptions> entityOptions, bool activateTelemetry = true)
            where TEntity : LiquidEntity<TIdentifier>, new()
        {
            if (mongoDatabasesConfigurationSection is null ||
                !mongoDatabasesConfigurationSection.GetChildren().Any()) throw new LiquidDatabaseSettingsDoesNotExistException(nameof(mongoDatabasesConfigurationSection));

            if (entityOptions is null) throw new MongoEntityOptionsSettingsDoesNotExistException(nameof(entityOptions));

            services.AddOptions();

            foreach (var databaseSettings in mongoDatabasesConfigurationSection.GetChildren())
            {
                services.Configure<DatabaseSettings>(databaseSettings.Key, databaseSettings);
            }

            services.Configure<MongoEntityOptions>(typeof(TEntity).Name, entityOptions);

            if (services.FirstOrDefault(x => x.ServiceType == typeof(IMongoClientFactory)) is null)
                services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddScoped<IMongoDataContext<TEntity>, MongoDataContext<TEntity>>();

            services.AddScoped<MongoRepository<TEntity, TIdentifier>>();

            if (activateTelemetry)
                services.AddLiquidTelemetryInterceptor<ILiquidRepository<TEntity, TIdentifier>, MongoRepository<TEntity, TIdentifier>>();

            return services;
        }

        /// <summary>
        /// Registers a scoped <see cref="MongoRepository{TEntity, TIdentifier}"/> for any entity 
        /// that exists in project and a <see cref="MongoClientFactory"/>  if not previously registered.
        /// </summary>
        /// <param name="services">Extended ServiceCollection object.</param>
        /// <param name="mongoDatabasesConfigurationSection">Configuration section where databases have their options configured.</param>
        /// <param name="mongoEntityConfigurationSection">Configuration section where entities have their options configured.</param>
        /// <param name="activateTelemetry">Specifies whether the telemetry should be activated or not for this repository. Default: True.</param>
        public static IServiceCollection AddLiquidMongoRepositories(this IServiceCollection services, IConfiguration mongoDatabasesConfigurationSection, IConfiguration mongoEntityConfigurationSection, bool activateTelemetry = true)
        {
            if (mongoDatabasesConfigurationSection is null ||
                !mongoDatabasesConfigurationSection.GetChildren().Any()) throw new LiquidDatabaseSettingsDoesNotExistException(nameof(mongoDatabasesConfigurationSection));

            if (mongoEntityConfigurationSection is null || 
                !mongoEntityConfigurationSection.GetChildren().Any()) throw new MongoEntityOptionsSettingsDoesNotExistException(nameof(mongoEntityConfigurationSection));

            services.AddOptions();

            foreach (var databaseSettings in mongoDatabasesConfigurationSection.GetChildren())
            {
                services.Configure<DatabaseSettings>(databaseSettings.Key, databaseSettings);
            }

            foreach (var entityOptions in mongoEntityConfigurationSection.GetChildren())
            {
                services.Configure<MongoEntityOptions>(entityOptions.Key, entityOptions);
            }

            if (services.FirstOrDefault(x => x.ServiceType == typeof(IMongoClientFactory)) is null)
                services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddScoped(typeof(IMongoDataContext<>), typeof(MongoDataContext<>));

            services.AddScoped(typeof(ILiquidRepository<,>), typeof(MongoRepository<,>));

            if (activateTelemetry) 
            {
                // TODO: services.AddLiquidTelemetryInterceptor for all Repositories
            }

            return services;
        }
    }
}
