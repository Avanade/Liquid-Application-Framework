using Liquid.Core.Decorators;
using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Core.Tests.Mocks;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Core.Tests.Messaging
{
    public class LiquidCultureDecoratorTest
    {
        private readonly IOptions<CultureSettings> _options;
        private readonly ILiquidWorker<EntityMock> _inner;

        public LiquidCultureDecoratorTest()
        {
            _inner = Substitute.For<ILiquidWorker<EntityMock>>();
            _options = Substitute.For<IOptions<CultureSettings>>();
        }

        [Fact]
        public async Task ProcessMessageAsync_CultureSettingsIsNull_CurrentCultureNotChanged()
        {
            var settings = new CultureSettings();
            _options.Value.Returns(settings);

            var currentculture = CultureInfo.CurrentCulture.Name;

            var sut = new LiquidCultureDecorator<EntityMock>(_inner, _options);

            await sut.ProcessMessageAsync(new ConsumerMessageEventArgs<EntityMock>(), new CancellationToken());

            Assert.Equal(currentculture, CultureInfo.CurrentCulture.Name);
        }

        [Fact]
        public async Task ProcessMessageAsync_CultureSettingsIsNotNull_CurrentCultureChanged()
        {
            var settings = new CultureSettings() { DefaultCulture = "pt-BR" };
            _options.Value.Returns(settings);

            var currentculture = CultureInfo.CurrentCulture.Name;

            var sut = new LiquidCultureDecorator<EntityMock>(_inner, _options);

            await sut.ProcessMessageAsync(new ConsumerMessageEventArgs<EntityMock>(), new CancellationToken());

            Assert.NotEqual(currentculture, CultureInfo.CurrentCulture.Name);
        }
    }
}
