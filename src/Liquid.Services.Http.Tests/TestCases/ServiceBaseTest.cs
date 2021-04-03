using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.DependencyInjection;
using Liquid.Core.Telemetry;
using Liquid.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;

namespace Liquid.Services.Http.Tests.TestCases
{
    /// <summary>
    /// Base Service Client Test Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class ServiceBaseTest 
    {
        /// <summary>
        /// Gets or sets the subject under test.
        /// </summary>
        /// <value>
        /// The subject under test.
        /// </value>
        protected ILightHttpService SubjectUnderTest { get; set; }

        /// <summary>
        /// Builds the service provider.
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(services);
            
            services.AddSingleton(LoggerFactory.Create(builder => { builder.AddConsole(); }));
            IConfiguration configurationRoot = new ConfigurationBuilder().AddLightConfigurationFile().Build();
            services.AddSingleton(configurationRoot);

            services.AddDefaultTelemetry();
            services.AddDefaultContext();
            services.AddAutoMapper(GetType().Assembly);
            services.AddHttpClient();

            services.AddTransient<ILightHttpService, LightHttpService>();
            services.AddHttpServices(GetType().Assembly);
            return services.BuildServiceProvider();
        }
    }
}