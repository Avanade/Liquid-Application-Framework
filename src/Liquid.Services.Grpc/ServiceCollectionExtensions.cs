using System.Linq;
using System.Reflection;
using Liquid.Services.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Services.Grpc
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Grpc service clients.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblies">The assemblies that contains Grpc Service Clients declared.</param>
        public static void AddGrpcServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton<ILightServiceConfiguration<LightServiceSetting>, ServiceConfiguration>();
            var serviceTypes = assemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => t.BaseType != null &&
                            t.BaseType.Assembly.FullName.StartsWith("Liquid.Services.Grpc") &&
                            t.BaseType.Name.StartsWith("LightGrpcService"));

            foreach (var serviceType in serviceTypes)
            {
                var interfaceType = serviceType.GetInterfaces().FirstOrDefault(t =>
                    t.GetInterfaces()
                        .Any(i => i.Assembly.FullName.StartsWith("Liquid.Services.Grpc") &&
                                  i.Name.StartsWith("ILightGrpcService")));
                if (interfaceType != null)
                {
                    services.AddSingleton(interfaceType, serviceType);
                }
            }
        }
    }
}