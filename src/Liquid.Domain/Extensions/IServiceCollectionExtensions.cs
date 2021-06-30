using FluentValidation;
using Liquid.Domain.Pipelines;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Liquid.Domain.Extensions
{
    /// <summary>
    /// IServiceCollectionExtensions extension methods.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
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

            services.AddMediatR(assemblies);
        }
    }
}