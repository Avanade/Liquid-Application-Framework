using Liquid.Core.Exceptions;
using Liquid.Core.Extensions;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Domain.Extensions;
using Liquid.WebApi.Http.Filters.Swagger;
using Liquid.WebApi.Http.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Liquid.WebApi.Http.Extensions
{
    /// <summary>
    /// Startup extension methods. Used to configure the startup application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the web API services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblies"></param>
        public static IServiceCollection AddLiquidHttp(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddScoped<LiquidContext>();
            services.AddLiquidSerializers();
            services.AddLiquidConfiguration();
            services.AddAutoMapper(assemblies);
            services.AddLiquidHandlers(true, true, assemblies);

            services.AddLiquidSwagger();
            return services;
        }

        /// <summary>
        /// Adds the liquid swagger.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddLiquidSwagger(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetService<ILiquidConfiguration<SwaggerSettings>>();
            if (configuration?.Settings == null) throw new LiquidException("'swagger' settings does not exist in appsettings.json file. Please check the file.");

            var swaggerSettings = configuration.Settings;
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerSettings.Name,
                    new OpenApiInfo
                    {
                        Title = swaggerSettings.Title,
                        Version = swaggerSettings.Version,
                        Description = swaggerSettings.Description
                    });
                options.OperationFilter<AddHeaderParameterFilter>();
                options.OperationFilter<DefaultResponseFilter>();
                options.OperationFilter<OverloadMethodsSameVerb>();

                Directory.GetFiles(AppContext.BaseDirectory, "*.xml").Each(file => options.IncludeXmlComments(file));

                options.CustomSchemaIds(x => x.FullName);
            });
            return services;
        }
    }
}