using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Linq;
using System.Reflection;

namespace Liquid.Repository.MongoDb
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
        public static void AddMongoDb(this IServiceCollection services, string connectionId, params Assembly[] assemblies)
        {
            //AddMongoDbContext(services, connectionId);
            AddMongoDbRepositories(services, assemblies);
        }

        /// <summary>
        /// Adds the mongo database repositories.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblies">The assemblies.</param>
        private static void AddMongoDbRepositories(IServiceCollection services, Assembly[] assemblies)
        {
            var repositoryTypes = assemblies.SelectMany(a => a.ExportedTypes)
                            .Where(t => t.BaseType != null &&
                                        t.BaseType.Assembly.FullName == Assembly.GetAssembly(typeof(MongoDbDataContext)).FullName &&
                                        t.BaseType.Name.StartsWith("MongoDbRepository"));

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
        //private static void AddMongoDbContext(IServiceCollection services, string connectionId)
        //{
        //    services.AddScoped<IMongoDbDataContext>(sp =>
        //    {
        //        var databasesConfiguration = sp.GetService<ILightConfiguration<List<LightConnectionSettings>>>();
        //        var databaseSettings = databasesConfiguration?.Settings?.GetConnectionSetting(connectionId);
        //        if (databaseSettings == null) throw new LightDatabaseConfigurationDoesNotExistException(connectionId);

        //        var mongoClient = new MongoClient(databaseSettings.ConnectionString);

        //        return new MongoDbDataContext(
        //            sp.GetService<ILightTelemetryFactory>(),
        //            connectionId,
        //            databaseSettings.ConnectionString,
        //            databaseSettings.DatabaseName);
        //    });
        //}
    }
}
