using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Liquid.WebApi.Http.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Liquid.WebApi.Http.Filters.Swagger
{
    /// <summary>
    /// Adds headers to swagger document if the action has custom swagger attributes.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    [ExcludeFromCodeCoverage]
    internal class AddHeaderParameterFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.TryGetMethodInfo(out var methodInfo)) return;

            var controllerAttributes = methodInfo?.DeclaringType?.GetCustomAttributes<SwaggerBaseHeaderAttribute>().ToList();
            var actionAttributes = methodInfo?.GetCustomAttributes<SwaggerBaseHeaderAttribute>().ToList();
            controllerAttributes?.AddRange(actionAttributes);

            var swaggerAttributes = controllerAttributes?.Distinct();

            if (swaggerAttributes == null) return;


            foreach (var swaggerAttribute in swaggerAttributes)
            {
                if (operation.Parameters == null) { operation.Parameters = new List<OpenApiParameter>(); }

                if (!operation.Parameters.Any(p => p.Name.Equals(swaggerAttribute.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = swaggerAttribute.Name,
                        In = ParameterLocation.Header,
                        Required = swaggerAttribute.Required,
                        Description = swaggerAttribute.Description
                    });
                }
            }
        }
    }
}