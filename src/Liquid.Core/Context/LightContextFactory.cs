using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Core.Context
{
    /// <summary>
    /// The context factory class, responsible to return context interfaces.
    /// </summary>
    /// <seealso cref="Liquid.Core.Context.ILightContextFactory" />
    [ExcludeFromCodeCoverage]
    public class LightContextFactory : ILightContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightContextFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">container</exception>
        public LightContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        public ILightContext GetContext() => _serviceProvider.GetRequiredService<ILightContext>();
    }
}