using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Messaging.Decorators;
using Liquid.Messaging.Interfaces;
using Liquid.Messaging.Tests.Mock;
using NSubstitute;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.Tests
{
    public class LiquidCultureDecoratorTest
    {
        private readonly ILiquidConfiguration<CultureSettings> _options;
        private readonly ILiquidWorker<EntityMock> _inner;

        public LiquidCultureDecoratorTest()
        {
            _inner = Substitute.For<ILiquidWorker<EntityMock>>();
            _options = Substitute.For<ILiquidConfiguration<CultureSettings>>();
        }

        [Fact]
        public async Task ProcessMessageAsync_CultureSettingsIsNull_CurrentCultureNotChanged()
        {
            var settings = new CultureSettings();
            _options.Settings.Returns(settings);

            var currentculture = CultureInfo.CurrentCulture.Name;

            var sut = new LiquidCultureDecorator<EntityMock>(_inner, _options);

            await sut.ProcessMessageAsync(new ProcessMessageEventArgs<EntityMock>(), new CancellationToken());

            Assert.Equal(currentculture, CultureInfo.CurrentCulture.Name);
        }

        [Fact]
        public async Task ProcessMessageAsync_CultureSettingsIsNotNull_CurrentCultureChanged()
        {
            var settings = new CultureSettings() { DefaultCulture = "pt-BR" };
            _options.Settings.Returns(settings);

            var currentculture = CultureInfo.CurrentCulture.Name;

            var sut = new LiquidCultureDecorator<EntityMock>(_inner, _options);

            await sut.ProcessMessageAsync(new ProcessMessageEventArgs<EntityMock>(), new CancellationToken());

           Assert.NotEqual(currentculture, CultureInfo.CurrentCulture.Name);
        }
    }
}
