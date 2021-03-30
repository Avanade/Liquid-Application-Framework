using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Liquid.Core.DependencyInjection;

namespace Liquid.Domain.Extensions
{
    /// <summary>
    /// Domain Extensions Class.
    /// </summary>
    public static class DomainExtensions
    {
        /// <summary>
        /// Adds all mediator request handlers from assemblies.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblies">The assemblies that contains request handlers.</param>
        public static void AddDomainRequestHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var allAssemblies = GetInternalAssemblies().ToList();
            allAssemblies.AddRange(assemblies);

            services.AddSingletonAssemblies(typeof(IValidator<>), allAssemblies.ToArray());
            services.AddMediatR(allAssemblies.ToArray());
        }

        /// <summary>
        /// Gets the assemblies from some interfaces.
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<Assembly> GetInternalAssemblies()
        {
            yield return typeof(IMediator).Assembly;
            yield return typeof(DomainExtensions).Assembly;
        }
    }
}