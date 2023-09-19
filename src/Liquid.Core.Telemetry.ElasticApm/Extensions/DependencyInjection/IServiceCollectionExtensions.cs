using Castle.DynamicProxy;
using Elastic.Apm;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Telemetry.ElasticApm.Implementations;
using Liquid.Core.Telemetry.ElasticApm.MediatR;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Core.Telemetry.ElasticApm.Extensions.DependencyInjection
{
    /// <summary>
    /// Extends <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Register telemetry interceptor <see cref="LiquidElasticApmInterceptor"/> and behaviour <see cref="LiquidElasticApmTelemetryBehavior{TRequest, TResponse}"/> for Elastic APM. 
        /// </summary>
        /// <param name="services">Extended <see cref="IServiceCollection"/> instance.</param>
        /// <param name="configuration"><see cref="IConfiguration"/> implementation.</param>
        public static IServiceCollection AddLiquidElasticApmTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.HasElasticApmEnabled())
            {
                services.AddSingleton(s => Agent.Tracer);

                services.AddTransient<IAsyncInterceptor, LiquidElasticApmInterceptor>();

                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LiquidElasticApmTelemetryBehavior<,>));
            }

            return services;
        }

        /// <summary>
        /// Register telemetry interceptor <see cref="LiquidElasticApmInterceptor"/> and behaviour <see cref="LiquidElasticApmTelemetryBehavior{TRequest, TResponse}"/> for Elastic APM. 
        /// </summary>
        /// <param name="services">Extended <see cref="IServiceCollection"/> instance.</param>
        /// <param name="configuration"><see cref="IConfiguration"/> implementation.</param>
        /// <typeparam name="TInterface">Interface type of service that should be intercepted.</typeparam>
        /// <typeparam name="TService">Type of service that should be intercepted.</typeparam>
        public static IServiceCollection AddScopedWithLiquidElasticApmTelemetry<TInterface, TService>(this IServiceCollection services, IConfiguration configuration)
            where TInterface : class where TService : TInterface
        {
            if (configuration.HasElasticApmEnabled())
            {
                services.AddScoped(typeof(TService));
                services.AddScopedLiquidTelemetry<TInterface, TService>();
            }

            return services;
        }
    }
}
