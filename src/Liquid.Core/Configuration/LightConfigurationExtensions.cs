using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Core.Configuration
{
    /// <summary>
    /// Adds the Light Json Configuration File inside application.
    /// </summary>
    public static class LightConfigurationExtensions
    {
        /// <summary>
        /// Adds All Configurations from a specific assembly.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurations(this IServiceCollection services, params Assembly[] assemblies)
        {
            var serviceTypes = assemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => t.BaseType != null &&
                            t.BaseType.Assembly.FullName.StartsWith("Liquid.Core") &&
                            t.BaseType.Name.StartsWith("LightConfiguration"));

            foreach (var serviceType in serviceTypes)
            {
                var interfaceType = serviceType.GetInterfaces().FirstOrDefault(t => 
                                                    t.Assembly.FullName.StartsWith("Liquid.Core") &&
                                                    t.Name.StartsWith("ILightConfiguration"));
                if (interfaceType != null)
                {
                    services.AddSingleton(interfaceType, serviceType);
                }
            }

            return services;
        }
    }
}