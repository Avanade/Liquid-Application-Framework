using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Liquid.Services.Http
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the service clients.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblies">The assemblies that contains Http Service Clients declared.</param>
        public static void AddHttpServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            var serviceTypes = assemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => t.BaseType != null &&
                            t.BaseType.Assembly.FullName.StartsWith("Liquid.Services.Http") &&
                            t.BaseType.Name.StartsWith("LightHttpService"));

            foreach (var serviceType in serviceTypes)
            {
                var interfaceType = serviceType.GetInterfaces().FirstOrDefault(t =>
                    t.GetInterfaces()
                        .Any(i => i.Assembly.FullName.StartsWith("Liquid.Services.Http") &&
                                  i.Name.StartsWith("ILightHttpService")));

                if (interfaceType != null)
                {
                    services.AddSingleton(interfaceType, serviceType);
                }
            }
        }
    }
}