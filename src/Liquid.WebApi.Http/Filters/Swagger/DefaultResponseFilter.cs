using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Liquid.WebApi.Http.Filters.Swagger
{
    /// <summary>
    /// Sets the default responses for each action call.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    [ExcludeFromCodeCoverage]
    internal sealed class DefaultResponseFilter : IOperationFilter
    {
        /// <summary>
        /// Adds the defaults headers to all responses.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.Add("400", new OpenApiResponse { Description = "The request is invalid." });
            operation.Responses.Add("404", new OpenApiResponse { Description = "Resource not found." });
            operation.Responses.Add("500", new OpenApiResponse { Description = "An internal server error has occurred." });
        }
    }
}
