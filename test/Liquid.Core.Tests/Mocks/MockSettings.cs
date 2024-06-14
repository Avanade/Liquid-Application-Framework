using Liquid.Core.Attributes;

namespace Liquid.Core.Tests.Mocks
{
    [LiquidSectionName("MockSettings")]
    public class MockSettings
    {
        public string MyProperty { get; set; }
    }

    public class MockNoAttributeSettings
    {
        public string MyProperty { get; set; }
    }
}
