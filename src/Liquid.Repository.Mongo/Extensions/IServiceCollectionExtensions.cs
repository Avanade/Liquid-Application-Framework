using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
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
        /// <param name="entityOptions">MongoEntityOptions to configure how the entity will be persisted on Mongo.</param>
        public static IServiceCollection AddLiquidMongoWithTelemetry<TEntity, TIdentifier>(this IServiceCollection services, Action<MongoEntityOptions> entityOptions)
            where TEntity : LiquidEntity<TIdentifier>, new()
        {
            if (services.FirstOrDefault(x => x.ServiceType == typeof(IMongoClientFactory)) is null)
                services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddOptions();

            services.Configure<MongoEntityOptions>(typeof(TEntity).Name, entityOptions);

            services.AddScoped<IMongoDataContext<TEntity>, MongoDataContext<TEntity>>();

            services.AddScoped<MongoRepository<TEntity, TIdentifier>>();

            services.AddLiquidTelemetryInterceptor<ILiquidRepository<TEntity, TIdentifier>, MongoRepository<TEntity, TIdentifier>>();

            return services;
        }

        /// <summary>
        /// Registers a scoped <see cref="MongoRepository{TEntity, TIdentifier}"/> for any entity 
        /// that exists in project and a <see cref="MongoClientFactory"/>  if not previously registered.
        /// </summary>
        /// <param name="services">Extended ServiceCollection object.</param>
        /// <param name="mongoEntityConfigurationSection">Configuration section where entities have their options configured.</param>
        public static IServiceCollection AddLiquidMongoRepositories(this IServiceCollection services, IConfiguration mongoEntityConfigurationSection)
        {
            if (mongoEntityConfigurationSection is null) throw new MongoEntityOptionsSettingsDoesNotExistException(nameof(mongoEntityConfigurationSection));
            if (mongoEntityConfigurationSection.GetChildren().Count() == 0) throw new MongoEntityOptionsSettingsDoesNotExistException(nameof(mongoEntityConfigurationSection));

            if (services.FirstOrDefault(x => x.ServiceType == typeof(IMongoClientFactory)) is null)
                services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddOptions();

            foreach (var entityOptions in mongoEntityConfigurationSection.GetChildren())
            {
                services.Configure<MongoEntityOptions>(entityOptions.Key, entityOptions);
            }

            services.AddScoped(typeof(IMongoDataContext<>), typeof(MongoDataContext<>));

            services.AddScoped(typeof(ILiquidRepository<,>), typeof(MongoRepository<,>));

            return services;
        }
    }
}
