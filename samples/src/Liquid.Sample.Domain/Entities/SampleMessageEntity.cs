using Liquid.Repository;
using System;

namespace Liquid.Sample.Domain.Entities
{
    [Serializable]
    public class SampleMessageEntity
    {
        public int Id { get; set; }
        public string MyProperty { get; set; }
    }
}
