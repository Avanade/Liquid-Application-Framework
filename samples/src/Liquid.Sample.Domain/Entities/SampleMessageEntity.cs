using Liquid.Messaging.Attributes;
using Liquid.Repository;
using Liquid.Repository.Mongo.Attributes;
using System;

namespace Liquid.Sample.Domain.Entities
{
    [SettingsName("test")]
    [Serializable]
    public class SampleMessageEntity
    {
        public int Id { get; set; }
        public string MyProperty { get; set; }
    }
}
