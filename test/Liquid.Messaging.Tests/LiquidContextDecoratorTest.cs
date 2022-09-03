using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Messaging.Decorators;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Interfaces;
using Liquid.Messaging.Tests.Mock;
using NSubstitute;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.Tests
{
    public class LiquidContextDecoratorTest
    {
        private readonly ILiquidWorker<EntityMock> _inner;
        private readonly ILiquidContext _context;
        private readonly ILiquidConfiguration<ScopedContextSettings> _options;

        public LiquidContextDecoratorTest()
        {
            _inner = Substitute.For<ILiquidWorker<EntityMock>>();
            _context = new LiquidContext();
            _options = Substitute.For<ILiquidConfiguration<ScopedContextSettings>>();

            var settings = new ScopedContextSettings();
            settings.Keys.Add(new ScopedKey() { KeyName = "test", Required = true });
            settings.Culture = true;
            _options.Settings.Returns(settings);

        }

        [Fact]
        public async Task ProcessMessageAsync_WhenHeaderHasRequiredContexKey_ContextKeyInserted()
        {
            var headers = new Dictionary<string, object>();
            headers.Add("test", "sucess");

            var sut = new LiquidContextDecorator<EntityMock>(_inner, _context, _options);
            await sut.ProcessMessageAsync(new ProcessMessageEventArgs<EntityMock>() { Headers = headers }, new CancellationToken());

            Assert.Equal(headers["test"].ToString(), _context.current["test"]);
        }

        [Fact]
        public async Task ProcessMessageAsync_WhenHeaderHasntRequiredContexKey_ContextKeyInserted()
        {

            var sut = new LiquidContextDecorator<EntityMock>(_inner, _context, _options);

            await Assert.ThrowsAsync<MessagingMissingContextKeysException>(() =>
            sut.ProcessMessageAsync(new ProcessMessageEventArgs<EntityMock>(), new CancellationToken()));
        }

        [Fact]
        public async Task ProcessMessageAsync_WhenCultureTrue_ContextCultureCreated()
        {
            var headers = new Dictionary<string, object>();
            headers.Add("test", "sucess");

            var sut = new LiquidContextDecorator<EntityMock>(_inner, _context, _options);
            await sut.ProcessMessageAsync(new ProcessMessageEventArgs<EntityMock>() { Headers = headers }, new CancellationToken());

            Assert.Equal(_context.current["culture"].ToString(), CultureInfo.CurrentCulture.Name);
        }
    }
}
