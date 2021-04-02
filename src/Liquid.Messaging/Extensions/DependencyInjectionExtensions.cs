using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Liquid.Core.Configuration;
using Liquid.Core.DependencyInjection;
using Liquid.Core.Utils;
using Liquid.Messaging.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Messaging.Extensions
{
    /// <summary>
    /// Container Extensions Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensions
    {
        private static IServiceCollection _services;

        /// <summary>
        /// Adds the producers and consumers.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblies">The assemblies.</param>
        public static IServiceCollection AddProducersConsumers(this IServiceCollection services, params Assembly[] assemblies)
        {
            _services = services;
            services.AddSingleton<ILightConfiguration<List<MessagingSettings>>, MessagingConfiguration>();
            services.AddSingletonAssemblies(typeof(ILightProducer<>), assemblies);
            services.AddSingletonAssemblies(typeof(ILightConsumer<>), assemblies);
            return services;
        }

        /// <summary>
        /// Starts the producers and consumers.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public static IServiceProvider StartProducersConsumers(this IServiceProvider serviceProvider)
        {
            if (_services == null) return serviceProvider;

            var types = _services.Where(sd => sd.ServiceType.Assembly.FullName.StartsWith("Liquid.Messaging") &&
                                              (sd.ServiceType.Name.StartsWith("ILightConsumer") || sd.ServiceType.Name.StartsWith("ILightProducer")));

            types.Each(type => serviceProvider.GetService(type.ServiceType));

            return serviceProvider;
        }
    }
}