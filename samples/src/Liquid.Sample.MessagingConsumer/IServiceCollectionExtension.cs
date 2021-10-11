using Liquid.Messaging.Extensions.DependencyInjection;
using Liquid.Messaging.Interfaces;
using Liquid.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace Liquid.Sample.MessagingConsumer
{
    //
    // Summary:
    //     Startup extension methods. Used to configure the startup application.
    public static class IServiceCollectionExtensions
    {

        //
        // Summary:
        //     Register Liquid resources for consumers Liquid.Messaging.Extensions.DependencyInjection.IServiceCollectionExtensions.AddLiquidMessageConsumer``2(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Reflection.Assembly[])
        //     and a Liquid.Messaging.ServiceBus.ServiceBusConsumer`1 service with its dependency,
        //     with Liquid.Core.Extensions.DependencyInjection.IServiceCollectionLiquidExtension.AddLiquidTelemetryInterceptor``2(Microsoft.Extensions.DependencyInjection.IServiceCollection).
        //
        // Parameters:
        //   services:
        //     Extended service collection instance.
        //
        //   sectionName:
        //     Configuration section name.
        //
        //   assemblies:
        //     Array of assemblies that contains domain handlers implementation.
        //
        // Type parameters:
        //   TEntity:
        //     Type of entity that will be consumed by this service instance.
        //
        //   TWorker:
        //     Type of implementation from Liquid.Messaging.Interfaces.ILiquidWorker`1
        public static IServiceCollection AddLiquidServiceBusConsumerPoc<TWorker, TEntity>(this IServiceCollection services, string sectionName, params Assembly[] assemblies) where TWorker : class, ILiquidWorker<TEntity>
        {
            services.AddLiquidMessageConsumer<TWorker, TEntity>(assemblies);
            services.AddConsumerPoc<TEntity>(sectionName);
            return services;
        }


        private static IServiceCollection AddConsumerPoc<TEntity>(this IServiceCollection services, string sectionName)
        {
            services.TryAddTransient<IServiceBusFactory, ServiceBusFactory>();
            services.AddSingleton<ILiquidConsumer<TEntity>>((IServiceProvider provider) => 
            ActivatorUtilities.CreateInstance<ServiceBusConsumer<TEntity>>(provider, sectionName));
            return services;
        }
    }
}
