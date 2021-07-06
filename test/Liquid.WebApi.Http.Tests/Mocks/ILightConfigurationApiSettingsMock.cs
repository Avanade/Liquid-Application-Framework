using Liquid.Core.Interfaces;
using Liquid.WebApi.Http.Configuration;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Tests.Mocks
{
    /// <summary>
    /// ILightConfigurationApiSettings Mock, returns the mock interface for tests purpose.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ILightConfigurationApiSettingsMock
    {
        /// <summary>
        /// Gets the ILightConfigurationApiSettings mock.
        /// </summary>
        /// <returns></returns>
        public static ILiquidConfiguration<ApiSettings> GetMock()
        {
            var mock = Substitute.For<ILiquidConfiguration<ApiSettings>>();
            mock.Settings.Returns(new ApiSettings { ShowDetailedException = true, TrackRequests = true });
            return mock;
        }
    }
}
