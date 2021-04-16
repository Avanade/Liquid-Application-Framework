using Liquid.Core.Telemetry;
using Liquid.Repository.MongoDb.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.MongoDb.UnitTest.Mocks
{
    [ExcludeFromCodeCoverage]
    [MongoDb("TestEntities", "id")]
    public class TestRepository : MongoDbRepository<TestEntity, int>, ITestRepository
    {
        public TestRepository(ILightTelemetryFactory telemetryFactory, IMongoDbDataContext dataContext)
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
