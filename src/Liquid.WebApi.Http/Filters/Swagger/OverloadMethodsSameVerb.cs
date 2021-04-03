using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Liquid.Core.Utils;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Liquid.WebApi.Http.Filters.Swagger
{
    /// <summary>
    /// Adds the parameters to the method to avoid same method name.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    [ExcludeFromCodeCoverage]
    internal sealed class OverloadMethodsSameVerb : IOperationFilter
    {
        /// <summary>
        /// Changes the verbs by concatenating the parameters and verbs.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) return;

            var builder = new StringBuilder($"{context?.ApiDescription?.HttpMethod}_{context?.MethodInfo.Name}_{context?.ApiDescription?.RelativePath?.Replace("/", "_")}_{operation.OperationId}By");
            operation.Parameters.Each(parameter => builder.Append(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parameter.Name)));
            operation.OperationId = builder.ToString();
        }
    }
}