using Liquid.Core.Telemetry;
using Liquid.Data.EntityFramework;
using Liquid.Repository.EntityFramework.Tests.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.EntityFramework.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class MockRepository : EntityFrameworkRepository<MockEntity, int>, IMockRepository
    {
        public MockRepository(IEntityFrameworkDataContext dbContext, ILightTelemetryFactory telemetryFactory) : base(dbContext, telemetryFactory) { }
    }
}