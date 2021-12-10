using Liquid.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Diagnostics.CodeAnalysis;

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
            IConfiguration configurationRoot = new ConfigurationBuilder().AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json").Build();

            services.AddSingleton(configurationRoot);
            services.AddLiquidConfiguration();
            services.AddAutoMapper(GetType().Assembly);
            services.AddHttpClient();

            services.AddTransient<ILightHttpService, LightHttpService>();
            services.AddHttpServices(GetType().Assembly);
            return services.BuildServiceProvider();
        }
    }
}