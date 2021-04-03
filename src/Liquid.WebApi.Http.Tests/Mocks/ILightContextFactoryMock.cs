using Liquid.Core.Context;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Tests.Mocks
{
    /// <summary>
    /// ILight Context Factory Mock, returns the mock interface for tests purpose.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ILightContextFactoryMock
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public static ILightContext Context { get; set; } = new LightContext();

        /// <summary>
        /// Gets the ILightContextFactory mock interface.
        /// </summary>
        /// <returns></returns>
        public static ILightContextFactory GetMock()
        {
            var mock = Substitute.For<ILightContextFactory>();
            mock.GetContext().ReturnsForAnyArgs(Context);
            return mock;
        }
    }
}
