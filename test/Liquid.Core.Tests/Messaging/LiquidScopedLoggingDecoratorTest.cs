using Liquid.Core.Decorators;
using Liquid.Core.Entities;
using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Core.Tests.Mocks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Core.Tests.Messaging
{
    public class LiquidScopedLoggingDecoratorTest
    {
        private readonly ILogger<LiquidScopedLoggingDecorator<EntityMock>> _logger;
        private readonly ILiquidConfiguration<ScopedLoggingSettings> _options;
        private readonly ILiquidWorker<EntityMock> _inner;


        public LiquidScopedLoggingDecoratorTest()
        {
            _logger = Substitute.For<ILogger<LiquidScopedLoggingDecorator<EntityMock>>>();
            _options = Substitute.For<ILiquidConfiguration<ScopedLoggingSettings>>();
            _inner = Substitute.For<ILiquidWorker<EntityMock>>();

            var settings = new ScopedLoggingSettings();
            settings.Keys.Add(new ScopedKey() { KeyName = "test", Required = true });

            _options.Settings.Returns(settings);
        }

        [Fact]
        public async Task ProcessMessageAsync_WhenHeaderHasRequiredScopedLoggingKey_ScopeCreated()
        {
            var headers = new Dictionary<string, object>();
            headers.Add("test", "sucess");

            var sut = new LiquidScopedLoggingDecorator<EntityMock>(_inner, _options, _logger);
            await sut.ProcessMessageAsync(new ConsumerMessageEventArgs<EntityMock>() { Headers = headers }, new CancellationToken());

            _logger.Received().BeginScope(Arg.Any<Array>());
        }

        [Fact]
        public async Task ProcessMessageAsync_WhenHeaderHasntRequiredScopedLoggingKey_ThrowMessagingException()
        {

            var sut = new LiquidScopedLoggingDecorator<EntityMock>(_inner, _options, _logger);

            await Assert.ThrowsAsync<MessagingMissingScopedKeysException>(() =>
           sut.ProcessMessageAsync(new ConsumerMessageEventArgs<EntityMock>(), new CancellationToken()));
        }
    }
}
