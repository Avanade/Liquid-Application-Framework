using Liquid.Core.Telemetry;
using Liquid.Data.EntityFramework;
using Liquid.Repository.EntityFramework.Tests.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.EntityFramework.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class MockRepository : EntityFrameworkRepository<MockEntity, int, MockDbContext>, IMockRepository
    {
        public MockRepository(IEntityFrameworkDataContext<MockDbContext> dbContext, ILightTelemetryFactory telemetryFactory) : base(dbContext, telemetryFactory) { }
    }
}