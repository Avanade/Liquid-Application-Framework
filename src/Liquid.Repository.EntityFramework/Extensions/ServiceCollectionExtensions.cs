using Liquid.Core.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Liquid.Repository.EntityFramework.Extensions
{
    /// <summary>
    /// Entity Framework Service Collection Extensions Class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the entity framework database repositories and Context.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="assemblies">The assemblies.</param>
        public static void AddEntityFramework(this IServiceCollection services, string connectionId, params Assembly[] assemblies)
        {
            AddEntityContext(services, connectionId);
            AddEntityRepositories(services, assemblies);
        }

        /// <summary>
        /// Adds the entity framework database repositories.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblies">The assemblies.</param>
        private static void AddEntityRepositories(IServiceCollection services, Assembly[] assemblies)
        {
            var repositoryTypes = assemblies.SelectMany(a => a.ExportedTypes)
                            .Where(t => t.BaseType != null &&
                                        t.BaseType.Assembly.FullName == Assembly.GetAssembly(typeof(EntityFrameworkDataContext)).FullName &&
                                        t.BaseType.Name.StartsWith("EntityFrameworkRepository"));

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
        /// Adds the entity framework database context and Unit of Work.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        private static void AddEntityContext(IServiceCollection services, string connectionId)
        {
            if (services.First(x => x.ServiceType.Name == nameof(IEntityFrameworkClientFactory)) is null)
                services.AddSingleton<IEntityFrameworkClientFactory, EntityFrameworkClientFactory>();

            services.AddScoped<IEntityFrameworkDataContext>(sp =>
            {
                return new EntityFrameworkDataContext(
                    sp.GetService<ILightTelemetryFactory>(),
                    sp.GetService<IEntityFrameworkClientFactory>(),
                    connectionId);
            });
        }
    }
}
