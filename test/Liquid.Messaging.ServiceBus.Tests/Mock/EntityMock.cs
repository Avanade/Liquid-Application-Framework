using System;

namespace Liquid.Messaging.ServiceBus.Tests.Mock
{
    [Serializable]
    public class EntityMock
    {
        public int Id { get; set; }

        public string MyProperty { get; set; }
    }
}
