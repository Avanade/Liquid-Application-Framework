using Liquid.Repository;
using Liquid.Repository.Mongo.Attributes;

namespace Liquid.Sample.Domain.Entities
{
    [Mongo("SampleCollection", "id", "MySampleDb")]
    public class SampleEntity : LiquidEntity<int>
    {
        public string MyProperty { get; set; }
    }
}
