using Liquid.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Core.UnitTests.Mocks
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
