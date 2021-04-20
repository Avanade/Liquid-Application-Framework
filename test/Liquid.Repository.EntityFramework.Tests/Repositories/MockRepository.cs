using Liquid.Core.Telemetry;
using Liquid.Data.EntityFramework;
using Liquid.Data.EntityFramework.Tests;
using Liquid.Repository.EntityFramework.Tests.Entities;

namespace Liquid.Repository.EntityFramework.Tests.Repositories
{
    public class MockRepository : EntityFrameworkRepository<MockEntity, int>, IMockRepository
    {
        public MockRepository(MockDbContext dbContext, ILightTelemetryFactory telemetryFactory) : base(dbContext, telemetryFactory) { }
    }
}