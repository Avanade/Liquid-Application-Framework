using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class TestCaseResponse(string myProperty)
    {
        public string MyProperty { get; set; } = myProperty;
    }
}
