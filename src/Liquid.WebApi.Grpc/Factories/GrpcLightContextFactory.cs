using Liquid.Core.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Grpc.Factories
{
    /// <summary>
    /// Gets the current context from the current web scope.
    /// </summary>
    /// <seealso cref="ILightContextFactory" />
    [ExcludeFromCodeCoverage]
    internal class GrpcLightContextFactory : ILightContextFactory
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcLightContextFactory"/> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public GrpcLightContextFactory(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        public ILightContext GetContext()
        {
            return _contextAccessor.HttpContext.RequestServices.GetService<ILightContext>();
        }
    }
}