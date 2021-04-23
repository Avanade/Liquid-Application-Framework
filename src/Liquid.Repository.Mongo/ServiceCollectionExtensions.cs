using Liquid.Core.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Reflection;

namespace Liquid.Repository.Mongo
{
    /// <summary>
    /// Mongo Db Service Collection Extensions Class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the mongo database repositories and Context.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="assemblies">The assemblies.</param>
        public static void AddMongo(this IServiceCollection services, string connectionId, params Assembly[] assemblies)
        {
            AddMongoContext(services, connectionId);
            AddMongoRepositories(services, assemblies);
        }

        /// <summary>
        /// Adds the mongo database repositories.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblies">The assemblies.</param>
        private static void AddMongoRepositories(IServiceCollection services, Assembly[] assemblies)
        {
            var repositoryTypes = assemblies.SelectMany(a => a.ExportedTypes)
                            .Where(t => t.BaseType != null &&
                                        t.BaseType.Assembly.FullName == Assembly.GetAssembly(typeof(MongoDataContext)).FullName &&
                                        t.BaseType.Name.StartsWith("MongoRepository"));

            foreach (var repositoryType in repositoryTypes)
            {
                var interfaceType = repositoryType.GetInterfaces().FirstOrDefault(t =>
                    t.GetInterfaces()
                        .Any(i => i.Assembly.FullName == Assembly.GetAssembly(typeof(ILightDataContext)).FullName &&
                                  i.Name.StartsWith("ILightRepository")));

                if (interfaceType != null) services.AddScoped(interfaceType, repositoryType);
            }
        }

        /// <summary>
        /// Adds the mongo database context and Unit of Work.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns></returns>
        private static void AddMongoContext(IServiceCollection services, string connectionId)
        {
            services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddScoped<IMongoDataContext>(sp =>
            {
                return new MongoDataContext(
                    sp.GetService<ILightTelemetryFactory>(),
                    connectionId, sp.GetService<IMongoClientFactory>());
            });
        }
    }
}
