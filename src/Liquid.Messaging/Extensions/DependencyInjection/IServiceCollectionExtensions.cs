using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Domain.Extensions.DependencyInjection;
using Liquid.Messaging.Decorators;
using Liquid.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Liquid.Messaging.Extensions.DependencyInjection
{
    /// <summary>
    /// Startup extension methods. Used to configure the startup application.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
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
        /// its mappers <see cref="IServiceCollectionAutoMapperExtensions.AddAutoMapper(IServiceCollection, Action{AutoMapper.IMapperConfigurationExpression}, Assembly[])"/>
        /// that exists in <paramref name="assemblies"/>, with telemetry and validators.
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="assemblies">Array of assemblies that contains domain handlers implementation.</param>
        public static IServiceCollection AddLiquidDomain(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddLiquidAutoMapper(assemblies);
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
