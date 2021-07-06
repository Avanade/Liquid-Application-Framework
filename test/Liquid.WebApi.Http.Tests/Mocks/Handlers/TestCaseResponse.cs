using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class TestCaseResponse
    {
        public string MyProperty { get; set; }

        public TestCaseResponse(string myProperty)
        {
            MyProperty = myProperty;
        }
    }
}
