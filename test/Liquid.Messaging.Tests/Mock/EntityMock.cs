using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.Tests.Mock
{
    [SettingsName("test")]
    public class EntityMock
    {
        public int Property1 { get; set; }

        public string Property2 { get; set; }
    }
}
