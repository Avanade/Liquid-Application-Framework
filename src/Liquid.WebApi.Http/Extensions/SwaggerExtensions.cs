using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.WebApi.Http.Configuration;
using Liquid.WebApi.Http.Filters.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Liquid.WebApi.Http.Extensions
{
    /// <summary>
    /// Adds Swagger to your application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtensions
    {
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

        /// <summary>
        /// Uses the liquid swagger.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseLiquidSwagger(this IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService<ILiquidConfiguration<SwaggerSettings>>();

            var swaggerSettings = configuration.Settings;
            app.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerSettings.SwaggerEndpoint.Url, swaggerSettings.SwaggerEndpoint.Name);
            });
            return app;
        }
    }
}