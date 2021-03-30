using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Liquid.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Core.DependencyInjection
{
    /// <summary>
    /// Service Collection Extensions Class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the types specified inside <paramref name="assemblies" /> to the
        /// specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="assemblies">The assemblies that contains all implementation types.</param>
        /// <seealso cref="ServiceLifetime.Scoped" />
        public static IServiceCollection AddScopedAssemblies(this IServiceCollection services, Type serviceType, params Assembly[] assemblies)
        {
            var typesToRegister = TypeUtils.GetTypesToRegister(serviceType, assemblies);

            foreach (var typeToRegister in typesToRegister)
            {
                var interfaces = typeToRegister.GetInterfaces();
                var interfaceToRegister = interfaces.FirstOrDefault(t => t.GetGenericTypeDefinition() == serviceType);

                if (interfaceToRegister != null)
                {
                    services.AddScoped(interfaceToRegister, typeToRegister);
                }
            }
            return services;
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the types specified in <paramref name="implementationTypes" /> to the
        /// specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationTypes">The implementation types.</param>
        /// <seealso cref="ServiceLifetime.Scoped" />
        public static IServiceCollection AddScopedAssemblies(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes)
        {
            foreach (var typeToRegister in implementationTypes)
            {
                var interfaces = typeToRegister.GetInterfaces();
                var interfaceToRegister = interfaces.FirstOrDefault(t => t.GetGenericTypeDefinition() == serviceType);

                if (interfaceToRegister != null)
                {
                    services.AddScoped(interfaceToRegister, typeToRegister);
                }
            }
            return services;
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the types specified inside <paramref name="assemblies" /> to the
        /// specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="assemblies">The assemblies that contains all implementation types.</param>
        /// <seealso cref="ServiceLifetime.Transient" />
        public static IServiceCollection AddTransientAssemblies(this IServiceCollection services, Type serviceType, params Assembly[] assemblies)
        {
            var typesToRegister = TypeUtils.GetTypesToRegister(serviceType, assemblies);

            foreach (var typeToRegister in typesToRegister)
            {
                var interfaces = typeToRegister.GetInterfaces();
                var interfaceToRegister = interfaces.FirstOrDefault(t => t.GetGenericTypeDefinition() == serviceType);

                if (interfaceToRegister != null)
                {
                    services.AddTransient(interfaceToRegister, typeToRegister);
                }
            }
            return services;
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the types specified in <paramref name="implementationTypes" /> to the
        /// specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationTypes">The implementation types.</param>
        /// <seealso cref="ServiceLifetime.Transient" />
        public static IServiceCollection AddTransientAssemblies(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes)
        {
            foreach (var typeToRegister in implementationTypes)
            {
                var interfaces = typeToRegister.GetInterfaces();
                var interfaceToRegister = interfaces.FirstOrDefault(t => t.GetGenericTypeDefinition() == serviceType);

                if (interfaceToRegister != null)
                {
                    services.AddTransient(interfaceToRegister, typeToRegister);
                }
            }
            return services;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the types specified inside <paramref name="assemblies" /> to the
        /// specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="assemblies">The assemblies that contains all implementation types.</param>
        /// <seealso cref="ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingletonAssemblies(this IServiceCollection services, Type serviceType, params Assembly[] assemblies)
        {
            var typesToRegister = TypeUtils.GetTypesToRegister(serviceType, assemblies);

            foreach (var typeToRegister in typesToRegister)
            {
                var interfaces = typeToRegister.GetInterfaces();
                var interfaceToRegister = interfaces.FirstOrDefault(t => t.GetGenericTypeDefinition() == serviceType);

                if (interfaceToRegister != null)
                {
                    services.AddSingleton(interfaceToRegister, typeToRegister);
                }
            }
            return services;
        }
        
        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the types specified in <paramref name="implementationTypes" /> to the
        /// specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationTypes">The implementation types.</param>
        /// <seealso cref="ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingletonAssemblies(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes)
        {
            foreach (var typeToRegister in implementationTypes)
            {
                var interfaces = typeToRegister.GetInterfaces();
                var interfaceToRegister = interfaces.FirstOrDefault(t => t.GetGenericTypeDefinition() == serviceType);

                if (interfaceToRegister != null)
                {
                    services.AddSingleton(interfaceToRegister, typeToRegister);
                }
            }
            return services;
        }
    }
}