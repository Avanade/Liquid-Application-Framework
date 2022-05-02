using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Messaging.Decorators;
using Liquid.Messaging.Interfaces;
using Liquid.Messaging.Tests.Mock;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using Liquid.Messaging.Exceptions;

namespace Liquid.Messaging.Tests
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
            await sut.ProcessMessageAsync(new ProcessMessageEventArgs<EntityMock>() { Headers = headers }, new CancellationToken());

           _logger.Received().BeginScope(Arg.Any<Array>());
        }

        [Fact]
        public async Task ProcessMessageAsync_WhenHeaderHasntRequiredScopedLoggingKey_ThrowMessagingException()
        {
            
            var sut = new LiquidScopedLoggingDecorator<EntityMock>(_inner, _options, _logger);

            await Assert.ThrowsAsync<MessagingMissingScopedKeysException>(() =>
           sut.ProcessMessageAsync(new ProcessMessageEventArgs<EntityMock>(), new CancellationToken()));
        }
    }
}
