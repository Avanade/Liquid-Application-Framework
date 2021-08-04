using Liquid.Core.Exceptions;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Domain.Extensions.DependencyInjection;
using Liquid.WebApi.Http.Filters.Swagger;
using Liquid.WebApi.Http.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
        ///  to configure <see cref="IServiceCollectionLiquidExtension.AddLiquidConfiguration(IServiceCollection)"/>,
        ///  set mapping <see cref="IServiceCollectionAutoMapperExtensions.AddAutoMapper(IServiceCollection, Action{AutoMapper.IMapperConfigurationExpression}, Assembly[])"/>,
        ///  register domain handlers <see cref="Domain.Extensions.DependencyInjection.IServiceCollectionExtensions.AddLiquidHandlers(IServiceCollection, bool, bool, Assembly[])"/>, 
        ///  and swagger <see cref="AddLiquidSwagger(IServiceCollection)"/>
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="assemblies">Array of assemblies that the domain handlers are implemented.</param>
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
        /// Adds swagger with liquid configuration <see cref="SwaggerSettings"/> and
        /// filters <see cref="AddHeaderParameterFilter"/>, <see cref="DefaultResponseFilter"/>
        /// and <see cref="OverloadMethodsSameVerb"/>.
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
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