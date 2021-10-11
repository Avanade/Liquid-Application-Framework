using Liquid.Repository;

namespace Liquid.Sample.Domain.Entities
{
    public class SampleEntity : LiquidEntity<int>
    {
        public string MyProperty { get; set; }
    }
}
