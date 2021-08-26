using Liquid.Messaging.Attributes;
using System;

namespace Liquid.Messaging.ServiceBus.Tests.Mock
{
    [Serializable]
    [SettingsName("test")]
    public class EntityMock
    {
        public int Id { get; set; }

        public string MyProperty { get; set; }
    }
}
