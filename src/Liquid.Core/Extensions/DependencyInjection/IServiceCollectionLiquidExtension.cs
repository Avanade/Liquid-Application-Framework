using Castle.DynamicProxy;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Liquid.Core.Extensions.DependencyInjection
{
    /// <summary>
    /// Extends <see cref="IServiceCollection"/> interface.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionLiquidExtension
    {
        /// <summary>
        /// Inject a <see cref="LiquidConfiguration{T}"/> for each configuration section entity.
        /// </summary>
        /// <param name="services">Extended IServiceCollection instance.</param>
        public static IServiceCollection AddLiquidConfiguration(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(ILiquidConfiguration<>), typeof(LiquidConfiguration<>));

            return services;
        }

        /// <summary>
        /// Register telemetry interceptor <see cref="LiquidTelemetryInterceptor"/> for defined <typeparamref name="TService"/> services. 
        /// </summary>
        /// <typeparam name="TInterface">Interface type of service that should be intercepted.</typeparam>
        /// <typeparam name="TService">Type of service that should be intercepted.</typeparam>
        /// <param name="services">Extended IServiceCollection instance.</param>
        public static IServiceCollection AddLiquidTelemetryInterceptor<TInterface, TService>(this IServiceCollection services) where TInterface : class where TService : TInterface
        {
            services.TryAddTransient(typeof(IAsyncInterceptor), typeof(LiquidTelemetryInterceptor));

            services.TryAddSingleton(new ProxyGenerator());

            return services.AddTransient((provider) =>
            {
                var proxyGenerator = provider.GetService<ProxyGenerator>();
                var service = (TInterface)provider.GetRequiredService<TService>();

                return proxyGenerator.CreateInterfaceProxyWithTarget(service, provider.GetServices<IAsyncInterceptor>().ToArray());
            });
        }

        /// <summary>
        /// Register Liquid implementation of Serializer services and Serializer provider. 
        /// <see cref="LiquidJsonSerializer"/>, <see cref="LiquidXmlSerializer"/> and <see cref="LiquidSerializerProvider"/>
        /// </summary>
        /// <param name="services">Extended IServiceCollection instance.</param>
        public static IServiceCollection AddLiquidSerializers(this IServiceCollection services)
        {
            services.AddTransient<ILiquidSerializer, LiquidJsonSerializer>();
            services.AddTransient<ILiquidSerializer, LiquidXmlSerializer>();
            services.AddTransient<ILiquidSerializerProvider, LiquidSerializerProvider>();

            return services;
        }
    }
}
