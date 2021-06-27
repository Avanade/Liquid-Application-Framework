using Castle.DynamicProxy;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Liquid.Core.Extensions
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
            services.AddTransient(typeof(ILiquidConfiguration<>), typeof(LiquidConfiguration<>));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        public static IServiceCollection AddLiquidInterceptors<TInterface, TService>(this IServiceCollection services) where TInterface : class where TService : TInterface
        {
            return services.AddTransient((provider) =>
            {
                var proxyGenerator = provider.GetService<ProxyGenerator>();
                TInterface service = provider.GetRequiredService<TService>();

                return proxyGenerator.CreateInterfaceProxyWithTarget(service, provider.GetServices<IAsyncInterceptor>().ToArray());
            });
        }
    }
}
