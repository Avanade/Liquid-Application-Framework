using Liquid.Core.Interfaces;
using Liquid.Core.Localization;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Tests.Mocks
{
    /// <summary>
    /// ILightConfigurationCultureSettings Mock, returns the mock interface for tests purpose.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ILightConfigurationCultureSettingsMock
    {
        /// <summary>
        /// Gets the ILightConfigurationCultureSettings mock.
        /// </summary>
        /// <returns></returns>
        public static ILiquidConfiguration<CultureSettings> GetMock()
        {
            var mock = Substitute.For<ILiquidConfiguration<CultureSettings>>();
            mock.Settings.Returns(new CultureSettings { DefaultCulture = "en-US" });
            return mock;
        }
    }
}
