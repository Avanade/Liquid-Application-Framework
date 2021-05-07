using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;

namespace Liquid.Core.Tests.Telemetry
{
    /// <summary>
    /// Light Telemetry Test.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LightTelemetryTest
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Sets up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            IServiceCollection services = new ServiceCollection();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(services);
#pragma warning disable CS0618
            services.Configure(new Action<ConsoleLoggerOptions>(options => options.DisableColors = false));
#pragma warning restore CS0618

            services.AddSingleton(LoggerFactory.Create(builder => { builder.AddConsole(); }));
            services.AddDefaultTelemetry();
            services.AddTransient<ILightContextFactory, LightContextFactory>();
            services.AddTransient<ILightContext, LightContext>();
            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Tears down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _serviceProvider = null;
        }
        
        /// <summary>
        /// Tests the Telemetry Factory
        /// </summary>
        [Test]
        public void Verify_TelemetryFactory()
        {
            var sut = _serviceProvider.GetRequiredService<ILightTelemetryFactory>();
            var telemetry = sut.GetTelemetry();
            
            Assert.IsNotNull(telemetry);
        }

        /// <summary>
        /// Tests the Telemetry Factory
        /// </summary>
        [Test]
        public void Verify_Telemetry()
        {
            var sut = _serviceProvider.GetRequiredService<ILightTelemetryFactory>();
            var telemetry = sut.GetTelemetry();
            
            telemetry.AddContext("test");
            telemetry.AddWarningTelemetry("warning");
            telemetry.AddErrorTelemetry("error");
            telemetry.AddInformationTelemetry("information");
            telemetry.AddTelemetry(TelemetryType.Information, "telemetry information");
            telemetry.StartTelemetryStopWatchMetric("key");
            Thread.Sleep(300);
            telemetry.CollectTelemetryStopWatchMetric("key");
            telemetry.RemoveContext("test");
            telemetry.Flush();
            Assert.IsNotNull(telemetry);
        }

        /// <summary>
        /// Verifies the exceptions.
        /// </summary>
        [Test]
        public void Verify_Exceptions()
        {
            var sut = _serviceProvider.GetRequiredService<ILightTelemetryFactory>();
            var telemetry = sut.GetTelemetry();
            Assert.Catch<ArgumentNullException>(() => new LightTelemetryFactory(null));
            Assert.Catch<ArgumentNullException>(() => telemetry.AddContext(null));
            Assert.Catch<ArgumentNullException>(() => telemetry.RemoveContext(null));
            Assert.Catch<ArgumentNullException>(() => telemetry.StartTelemetryStopWatchMetric(null));
            Assert.Catch<ArgumentNullException>(() => telemetry.CollectTelemetryStopWatchMetric(null));
        }
    }
}