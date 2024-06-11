using Liquid.Repository;
using System;

namespace Liquid.Sample.Domain.Entities
{
    [Serializable]
    public class SampleMessageEntity
    {
        public string Id { get; set; }
        public string MyProperty { get; set; }
    }
}
