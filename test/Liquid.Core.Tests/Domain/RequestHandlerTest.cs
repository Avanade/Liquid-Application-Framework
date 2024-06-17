using FluentValidation;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.PipelineBehaviors;
using Liquid.Domain.Tests.CommandHandlers.Test1;
using Liquid.Domain.Tests.CommandHandlers.Test2;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Core.Tests.Domain
{
    /// <summary>
    /// Base Command Handler Test Class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Test.Base.TestTemplateContext{Liquid.Domain.Tests.TestEntities.TestCommandHandler}</cref>
    /// </seealso>
    [ExcludeFromCodeCoverage]
    public class RequestHandlerTest
    {
        private IServiceProvider _serviceProvider;
        private ILogger<LiquidTelemetryBehavior<Test1Command, Test1Response>> _logger = Substitute.For<ILogger<LiquidTelemetryBehavior<Test1Command, Test1Response>>>();
        private ILogger<LiquidTelemetryBehavior<Test2Command, Test2Response>> _logger2 = Substitute.For<ILogger<LiquidTelemetryBehavior<Test2Command, Test2Response>>>();


        public RequestHandlerTest()
        {
            var services = new ServiceCollection();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(services);
#pragma warning disable CS0618
            services.Configure(new Action<ConsoleLoggerOptions>(options => options.DisableColors = false));

#pragma warning restore CS0618
            services.AddSingleton(LoggerFactory.Create(builder => { builder.AddConsole(); }));

            services.AddTransient((s) => _logger);

            services.AddTransient((s) => _logger2);

            services.AddLiquidHandlers(true, true, GetType().Assembly);

            _serviceProvider = services.AddLogging().BuildServiceProvider();
        }

        [Fact]
        public async Task Test_WhenCommandHasntValidator_Sucess()
        {
            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            using var scopedTransaction = _serviceProvider.CreateScope();
            var response = await mediator.Send(new Test1Command());

            _logger.Received(2);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Test_WhenValidatorPassed_Sucess()
        {
            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            using var scopedTransaction2 = _serviceProvider.CreateScope();
            var response2 = await mediator.Send(new Test2Command { Id = 1 });

            Assert.NotNull(response2);
            _logger2.Received(2);
        }

        [Fact]
        public async Task Test_WhenValidatorThrowError_ThowException()
        {
            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                var mediator = _serviceProvider.GetRequiredService<IMediator>();
                using var scopedTransaction = _serviceProvider.CreateScope();
                await mediator.Send(new Test2Command { Id = -1 });
            });

            _logger2.Received(3);
        }
    }
}