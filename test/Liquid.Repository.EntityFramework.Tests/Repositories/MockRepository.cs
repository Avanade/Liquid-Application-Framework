using Liquid.Core.Telemetry;
using Liquid.Data.EntityFramework.Tests.Entities;

namespace Liquid.Data.EntityFramework.Tests.Repositories
{
    public class MockRepository : EntityFrameworkRepository<MockEntity, int>, IMockRepository
    {
        public MockRepository(MockDbContext dbContext, ILightTelemetryFactory telemetryFactory) : base(dbContext, telemetryFactory) { }
    }
}