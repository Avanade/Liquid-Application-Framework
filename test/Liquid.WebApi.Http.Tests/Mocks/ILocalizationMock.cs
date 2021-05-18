using Liquid.Core.Localization;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Tests.Mocks
{
    /// <summary>
    /// ILocalization Mock, returns the mock interface for tests purpose.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ILocalizationMock
    {
        /// <summary>
        /// Gets the ILocalization mock.
        /// </summary>
        /// <returns></returns>
        public static ILocalization GetMock()
        {
            var mock = Substitute.For<ILocalization>();
            return mock;
        }
    }
}
