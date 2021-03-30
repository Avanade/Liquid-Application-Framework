using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentValidation;
using Liquid.Core.Context;
using Liquid.Core.DependencyInjection;
using Liquid.Core.Telemetry;
using Liquid.Domain.Extensions;
using Liquid.Domain.Tests.CommandHandlers.Test1;
using Liquid.Domain.Tests.CommandHandlers.Test2;
using MediatR;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;

namespace Liquid.Domain.Tests.TestCases
{
    /// <summary>
    /// Base Command Handler Test Class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Test.Base.TestTemplateContext{Liquid.Domain.Tests.TestEntities.TestCommandHandler}</cref>
    /// </seealso>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RequestHandlerTest
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Establishes the context.
        /// </summary>
        /// <returns></returns>
        [SetUp]
        protected void EstablishContext()
        {
            var services = new ServiceCollection();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(services);
            services.Configure(new Action<ConsoleLoggerOptions>(options => options.DisableColors = false));
            services.AddSingleton(LoggerFactory.Create(builder => { builder.AddConsole(); }));

            services.AddDomainRequestHandlers(GetType().Assembly);
            services.AddAutoMapper(GetType().Assembly);
            services.AddSingleton<ILightTelemetryFactory, LightTelemetryFactory>();
            services.AddScoped<ILightTelemetry, LightTelemetry>();
            services.AddSingleton<ILightContextFactory, LightContextFactory>();
            services.AddScoped<ILightContext, LightContext>();
            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Tests the cleanup.
        /// </summary>
        [TearDown]
        protected void TestCleanup()
        {
            _serviceProvider = null;
        }

        /// <summary>
        /// Verifies the request handlers execution.
        /// </summary>
        [Test]
        public async Task Test_ExecuteCommandHandler()
        {
            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            using var scopedTransaction = _serviceProvider.CreateScope();
            var response = await mediator.Send(new Test1Command());
            Assert.IsNotNull(response);

            using var scopedTransaction2 = _serviceProvider.CreateScope();
            var response2 = await mediator.Send(new Test2Command { Id = 1 });
            Assert.IsNotNull(response2);
        }

        /// <summary>
        /// Tests the exceptions.
        /// </summary>
        [Test]
        public void Test_Exceptions()
        {
            Assert.ThrowsAsync<ValidationException>( async () =>
            {
                var mediator = _serviceProvider.GetRequiredService<IMediator>();
                using var scopedTransaction = _serviceProvider.CreateScope();
                await mediator.Send(new Test2Command { Id = -1 });
            });
        }
    }
}