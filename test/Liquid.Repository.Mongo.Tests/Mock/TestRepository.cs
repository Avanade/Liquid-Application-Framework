using Liquid.Core.Telemetry;
using Liquid.Repository.Mongo.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Mongo.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    [Mongo("TestEntities", "id", "TestDatabase")]
    public class TestRepository : MongoRepository<TestEntity, int>, ITestRepository
    {
        public TestRepository(ILightTelemetryFactory telemetryFactory, IMongoDataContext dataContext)
            : base(telemetryFactory, dataContext)
        {
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
