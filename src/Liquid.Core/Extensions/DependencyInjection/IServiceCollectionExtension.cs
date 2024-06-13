using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Core.PipelineBehaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Liquid.Core.Extensions.DependencyInjection
{
    /// <summary>
    /// LiquidCache <see cref="IServiceCollection"/> extensions class.
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// Registers a <see cref="LiquidCache"/> service and its <see cref="LiquidTelemetryInterceptor"/> 
        /// depending on the value of <paramref name="withTelemetry"/>.
        /// </summary>
        /// <param name="services">Extended IServiceCollection.</param>
        /// <param name="withTelemetry">indicates if this method must register a <see cref="LiquidTelemetryInterceptor"/></param>
        /// <returns></returns>
        public static IServiceCollection AddLiquidDistributedCache(this IServiceCollection services, bool withTelemetry)
        {
            if (withTelemetry)
            {
                services.AddScoped<LiquidCache>();
                services.AddScopedLiquidTelemetry<ILiquidCache, LiquidCache>();
            }
            else
                services.AddScoped<ILiquidCache, LiquidCache>();

            return services;
        }

        /// <summary>
        /// Injects mediator handler, validator and Liquid native telemetry for handlers.
        /// </summary>
        /// <param name="services">Extended service collection.</param>
        /// <param name="withTelemetry">Indicates if method should inject Liquid Telemetry Behavior.</param>
        /// <param name="withValidators">Indicates if method should inject Validators.</param>
        /// <param name="assemblies">List of assemblies that contains handlers and validators implemantations to be injected.</param>
        public static void AddLiquidHandlers(this IServiceCollection services, bool withTelemetry, bool withValidators, params Assembly[] assemblies)
        {
            if (withValidators)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LiquidValidationBehavior<,>));
                services.AddValidatorsFromAssemblies(assemblies);
            }

            if (withTelemetry)
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LiquidTelemetryBehavior<,>));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        }

    }
}