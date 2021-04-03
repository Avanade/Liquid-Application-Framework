using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Liquid.Core.Utils;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Liquid.WebApi.Http.Filters.Swagger
{
    /// <summary>
    /// The Swagger document custom sort filter class.
    /// </summary>
    /// <seealso cref="IDocumentFilter" />
    [ExcludeFromCodeCoverage]
    internal sealed class DocumentSortFilter : IDocumentFilter
    {
        /// <summary>
        /// Applies the specified swagger document.
        /// </summary>
        /// <param name="swaggerDoc">The swagger document.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.OrderBy(e => e.Key);
            swaggerDoc.Paths = new OpenApiPaths();
            paths.Each(path => swaggerDoc.Paths.Add(path.Key, path.Value));
        }
    }
}