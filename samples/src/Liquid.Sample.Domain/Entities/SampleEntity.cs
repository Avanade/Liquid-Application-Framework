using Liquid.Repository;
using System;

namespace Liquid.Sample.Domain.Entities
{
    public class SampleEntity : LiquidEntity<Guid>
    {
        public string MyProperty { get; set; }
    }
}
