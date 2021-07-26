using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Utils;

namespace Liquid.Messaging.Extensions
{
    /// <summary>
    /// Container Extensions Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Starts the producers and consumers from all messaging.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public static IServiceProvider StartMessaging(this IServiceProvider serviceProvider)
        {
            var services = serviceProvider.GetAllServiceDescriptors();

            var types = services.Where(sd => sd.Key.Assembly.FullName.StartsWith("Liquid.Messaging") &&
                                              (sd.Key.Name.StartsWith("ILightConsumer") || sd.Key.Name.StartsWith("ILightProducer")));

            types.Each(type => serviceProvider.GetService(type.Key));

            return serviceProvider;
        }
    }
}