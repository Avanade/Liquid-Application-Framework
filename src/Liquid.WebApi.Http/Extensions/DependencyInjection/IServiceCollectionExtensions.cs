using Liquid.Core.Exceptions;
using Liquid.Core.Extensions;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.WebApi.Http.Filters.Swagger;
using Liquid.WebApi.Http.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Liquid.WebApi.Http.Extensions.DependencyInjection
{
    /// <summary>
    /// Startup extension methods. Used to configure the startup application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        ///  Registers a <see cref="LiquidContext"/> service and execute registration methods
        ///  set mapping <see cref="IServiceCollectionAutoMapperExtensions.LiquidAddAutoMapper(IServiceCollection, Action{AutoMapper.IMapperConfigurationExpression}, Assembly[])"/>,
        ///  register domain handlers <see cref="IServiceCollectionCoreExtensions.AddLiquidHandlers(IServiceCollection, bool, bool, Assembly[])"/>, 
        ///  and swagger <see cref="AddLiquidSwagger(IServiceCollection)"/>
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="assemblies">Array of assemblies that the domain handlers are implemented.</param>
        /// <param name="sectionName">Swagger configuration section name.</param>
        /// <param name="middlewares">Indicates if middlewares options must be binded.</param>
        public static IServiceCollection AddLiquidHttp(this IServiceCollection services, string sectionName, bool middlewares = false, params Assembly[] assemblies)
        {
            if (middlewares)
            {
                services.AddOptions<ScopedContextSettings>()
                 .Configure<IConfiguration>((settings, configuration) =>
                 {
                     configuration.GetSection(sectionName + ":ScopedContext").Bind(settings);
                 });

                services.AddOptions<CultureSettings>()
                 .Configure<IConfiguration>((settings, configuration) =>
                 {
                     configuration.GetSection(sectionName + ":Culture").Bind(settings);
                 });

                services.AddOptions<ScopedLoggingSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(sectionName + ":ScopedLogging").Bind(settings);
                });
            }

            services.AddScoped<ILiquidContext, LiquidContext>();
            services.AddLiquidSerializers();

            services.AddOptions<SwaggerSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(sectionName + ":Swagger").Bind(settings);
            });

            services.LiquidAddAutoMapper(assemblies);
            services.AddLiquidHandlers(true, true, assemblies);

            services.AddLiquidSwagger();
            return services;
        }

        /// <summary>
        /// Adds swagger with liquid configuration <see cref="SwaggerSettings"/> and
        /// filters <see cref="AddHeaderParameterFilter"/>, <see cref="DefaultResponseFilter"/>
        /// and <see cref="OverloadMethodsSameVerb"/>.
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
        public static IServiceCollection AddLiquidSwagger(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetService<IOptions<SwaggerSettings>>();
            if (configuration?.Value == null) throw new LiquidException("'swagger' settings does not exist in appsettings.json file. Please check the file.");

            var swaggerSettings = configuration.Value;
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

                Directory.GetFiles(AppContext.BaseDirectory, "*.xml").AsEnumerable().Each(file => options.IncludeXmlComments(file));

                options.CustomSchemaIds(x => x.FullName);
            });
            return services;
        }
    }
}