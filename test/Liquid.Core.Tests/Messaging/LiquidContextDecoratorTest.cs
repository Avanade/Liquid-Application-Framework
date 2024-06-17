using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Core.Decorators;
using Liquid.Core.Exceptions;
using NSubstitute;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Liquid.Core.Tests.Mocks;
using Liquid.Core.Entities;
using Microsoft.Extensions.Options;

namespace Liquid.Core.Tests.Messaging
{
    public class LiquidContextDecoratorTest
    {
        private readonly ILiquidWorker<EntityMock> _inner;
        private readonly ILiquidContext _context;
        private readonly IOptions<ScopedContextSettings> _options;

        public LiquidContextDecoratorTest()
        {
            _inner = Substitute.For<ILiquidWorker<EntityMock>>();
            _context = new LiquidContext();
            _options = Substitute.For<IOptions<ScopedContextSettings>>();

            var settings = new ScopedContextSettings();
            settings.Keys.Add(new ScopedKey() { KeyName = "test", Required = true });
            settings.Culture = true;
            _options.Value.Returns(settings);

        }

        [Fact]
        public async Task ProcessMessageAsync_WhenHeaderHasRequiredContexKey_ContextKeyInserted()
        {
            var headers = new Dictionary<string, object>();
            headers.Add("test", "sucess");

            var sut = new LiquidContextDecorator<EntityMock>(_inner, _context, _options);
            await sut.ProcessMessageAsync(new ConsumerMessageEventArgs<EntityMock>() { Headers = headers }, new CancellationToken());

            Assert.Equal(headers["test"].ToString(), _context.current["test"]);
        }

        [Fact]
        public async Task ProcessMessageAsync_WhenHeaderHasntRequiredContexKey_ContextKeyInserted()
        {

            var sut = new LiquidContextDecorator<EntityMock>(_inner, _context, _options);

            await Assert.ThrowsAsync<MessagingMissingContextKeysException>(() =>
            sut.ProcessMessageAsync(new ConsumerMessageEventArgs<EntityMock>(), new CancellationToken()));
        }

        [Fact]
        public async Task ProcessMessageAsync_WhenCultureTrue_ContextCultureCreated()
        {
            var headers = new Dictionary<string, object>();
            headers.Add("test", "sucess");

            var sut = new LiquidContextDecorator<EntityMock>(_inner, _context, _options);
            await sut.ProcessMessageAsync(new ConsumerMessageEventArgs<EntityMock>() { Headers = headers }, new CancellationToken());

            Assert.Equal(_context.current["culture"].ToString(), CultureInfo.CurrentCulture.Name);
        }
    }
}
