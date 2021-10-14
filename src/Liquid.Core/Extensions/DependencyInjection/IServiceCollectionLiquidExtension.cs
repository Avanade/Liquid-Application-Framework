using Castle.DynamicProxy;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Liquid.Core.Extensions.DependencyInjection
{
    /// <summary>
    /// Extends <see cref="IServiceCollection"/> interface.
    /// </summary>
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
        [Obsolete("This method will be removed in the next release. " +
            "Please use AddScopedLiquidTelemetry, AddTransientLiquidTelemetry or AddSingletonLiquidTelemetry.")]
        public static IServiceCollection AddLiquidTelemetryInterceptor<TInterface, TService>(this IServiceCollection services) 
            where TInterface : class where TService : TInterface
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
        /// Register telemetry interceptor <see cref="LiquidTelemetryInterceptor"/> for defined <typeparamref name="TService"/> services. 
        /// </summary>
        /// <typeparam name="TInterface">Interface type of service that should be intercepted.</typeparam>
        /// <typeparam name="TService">Type of service that should be intercepted.</typeparam>
        /// <param name="services">Extended IServiceCollection instance.</param>
        public static IServiceCollection AddScopedLiquidTelemetry<TInterface, TService>(this IServiceCollection services) 
            where TInterface : class where TService : TInterface
        {
            services.AddInterceptor<LiquidTelemetryInterceptor>();

            return services.AddScoped((provider) =>
            {
                var proxyGenerator = provider.GetService<ProxyGenerator>();
                var service = (TInterface)provider.GetRequiredService<TService>();

                return proxyGenerator.CreateInterfaceProxyWithTarget(service, provider.GetServices<IAsyncInterceptor>().ToArray());
            });
        }

        /// <summary>
        /// Register telemetry interceptor <see cref="LiquidTelemetryInterceptor"/> for defined <typeparamref name="TService"/> services. 
        /// </summary>
        /// <typeparam name="TInterface">Interface type of service that should be intercepted.</typeparam>
        /// <typeparam name="TService">Type of service that should be intercepted.</typeparam>
        /// <param name="services">Extended IServiceCollection instance.</param>
        public static IServiceCollection AddTransientLiquidTelemetry<TInterface, TService>(this IServiceCollection services)
            where TInterface : class where TService : TInterface
        {
            services.AddInterceptor<LiquidTelemetryInterceptor>();

            return services.AddTransient((provider) =>
            {
                var proxyGenerator = provider.GetService<ProxyGenerator>();
                var service = (TInterface)provider.GetRequiredService<TService>();

                return proxyGenerator.CreateInterfaceProxyWithTarget(service, provider.GetServices<IAsyncInterceptor>().ToArray());
            });
        }

        /// <summary>
        /// Register telemetry interceptor <see cref="LiquidTelemetryInterceptor"/> for defined <typeparamref name="TService"/> services. 
        /// </summary>
        /// <typeparam name="TInterface">Interface type of service that should be intercepted.</typeparam>
        /// <typeparam name="TService">Type of service that should be intercepted.</typeparam>
        /// <param name="services">Extended IServiceCollection instance.</param>
        public static IServiceCollection AddSingletonLiquidTelemetry<TInterface, TService>(this IServiceCollection services)
            where TInterface : class where TService : TInterface
        {
            services.AddInterceptor<LiquidTelemetryInterceptor>();

            return services.AddSingleton((provider) =>
            {
                var proxyGenerator = provider.GetService<ProxyGenerator>();
                var service = (TInterface)provider.GetRequiredService<TService>();

                return proxyGenerator.CreateInterfaceProxyWithTarget(service, provider.GetServices<IAsyncInterceptor>().ToArray());
            });
        }

        /// <summary>
        /// Register a <typeparamref name="TInterceptor"/> service instance
        /// with <see cref="IAsyncInterceptor"/> service type.
        /// </summary>
        /// <typeparam name="TInterceptor">Service implementation type.</typeparam>
        /// <param name="services">Extended IServiceCollection instance.</param>
        public static IServiceCollection AddInterceptor<TInterceptor>(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(IAsyncInterceptor), typeof(TInterceptor));

            services.TryAddSingleton(new ProxyGenerator());

            return services;
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
