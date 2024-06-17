using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Core.PipelineBehaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using System;
using Liquid.Core.Decorators;

namespace Liquid.Core.Extensions.DependencyInjection
{
    /// <summary>
    /// LiquidCache <see cref="IServiceCollection"/> extensions class.
    /// </summary>
    public static class IServiceCollectionCoreExtensions
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

        /// <summary>
        /// Register Liquid Worker Service <see cref="AddLiquidWorkerService{TService, TEntity}(IServiceCollection)"/>
        /// with messaging pipeline <see cref="AddLiquidPipeline(IServiceCollection)"/>,
        /// and domain handlers <see cref="AddLiquidDomain(IServiceCollection, Assembly[])"/>
        /// that exists in <paramref name="assemblies"/>, with telemetry and validators.
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="assemblies">Array of assemblies that contains domain handlers implementation.</param>
        /// <returns></returns>
        public static IServiceCollection AddLiquidMessageConsumer<TWorker, TEntity>(this IServiceCollection services, params Assembly[] assemblies)
            where TWorker : class, ILiquidWorker<TEntity>
        {
            services.AddLiquidWorkerService<TWorker, TEntity>();
            services.AddLiquidPipeline<TEntity>();
            services.AddLiquidDomain(assemblies);
            return services;
        }
        /// <summary>
        /// Register <see cref="ILiquidWorker{TEntity}"/> implementation service  
        /// <typeparamref name="TWorker"/> and <see cref="LiquidBackgroundService{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TWorker"><see cref="ILiquidWorker{TEntity}"/> implementation type.</typeparam>
        /// <typeparam name="TEntity">Type of entity that will be consumed by this service.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        public static IServiceCollection AddLiquidWorkerService<TWorker, TEntity>(this IServiceCollection services)
            where TWorker : class, ILiquidWorker<TEntity>
        {
            services.AddScoped<ILiquidWorker<TEntity>, TWorker>();

            services.AddHostedService<LiquidBackgroundService<TEntity>>();

            return services;
        }

        /// <summary>
        /// Register domain handlers <see cref="AddLiquidDomain(IServiceCollection, Assembly[])"/> and
        /// its mappers <see cref="IServiceCollectionAutoMapperExtensions.LiquidAddAutoMapper(IServiceCollection, Action{AutoMapper.IMapperConfigurationExpression}, Assembly[])"/>
        /// that exists in <paramref name="assemblies"/>, with telemetry and validators.
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="assemblies">Array of assemblies that contains domain handlers implementation.</param>
        public static IServiceCollection AddLiquidDomain(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.LiquidAddAutoMapper(assemblies);
            services.AddLiquidHandlers(withTelemetry: true, withValidators: true, assemblies);

            return services;
        }

        /// <summary>
        /// Register <see cref="LiquidContext"/> and aditional behaviors
        /// <see cref="LiquidContextDecorator{TEntity}"/>, <see cref="LiquidScopedLoggingDecorator{TEntity}"/>
        /// and <see cref="LiquidCultureDecorator{TEntity}"/>. These additional behaviors will be 
        /// performed in the reverse order they were registered.
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
        public static IServiceCollection AddLiquidPipeline<TEntity>(this IServiceCollection services)
        {
            services.AddScoped<ILiquidContext, LiquidContext>();
            services.Decorate<ILiquidWorker<TEntity>, LiquidContextDecorator<TEntity>>();
            services.Decorate<ILiquidWorker<TEntity>, LiquidScopedLoggingDecorator<TEntity>>();
            services.Decorate<ILiquidWorker<TEntity>, LiquidCultureDecorator<TEntity>>();

            return services;
        }
        /// <summary>
        /// Changes the previously registered <typeparamref name="TInterface"/> service descriptor 
        /// to the <typeparamref name="TDecorator"/> service.
        /// </summary>
        /// <typeparam name="TInterface">Interface type of service that should be decorated.</typeparam>
        /// <typeparam name="TDecorator">Type of decorator service implementation.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        public static IServiceCollection Decorate<TInterface, TDecorator>(this IServiceCollection services)
           where TInterface : class
           where TDecorator : class, TInterface
        {
            ServiceDescriptor innerDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TInterface));

            if (innerDescriptor == null) { throw new InvalidOperationException($"{typeof(TInterface).Name} is not registered"); }

            var objectFactory = ActivatorUtilities.CreateFactory(
              typeof(TDecorator),
              new[] { typeof(TInterface) });

            services.Replace(ServiceDescriptor.Describe(
              typeof(TInterface),
              s => (TInterface)objectFactory(s, new[] { s.CreateInstance(innerDescriptor) }), innerDescriptor.Lifetime)
            );

            return services;
        }

        private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
                return descriptor.ImplementationInstance;

            if (descriptor.ImplementationFactory != null)
                return descriptor.ImplementationFactory(services);

            return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }

    }
}