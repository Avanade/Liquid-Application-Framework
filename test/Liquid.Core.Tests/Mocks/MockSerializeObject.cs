namespace Liquid.Core.Tests.Mocks
{
    public class MockSerializeObject
    {
        public MockSerializeObject()
        {

        }

        public MockSerializeObject(int intProperty, string stringProperty)
        {
            IntProperty = intProperty;
            StringProperty = stringProperty;
        }

        public int IntProperty { get; set; }

        public string StringProperty { get; set; }
    }
}
