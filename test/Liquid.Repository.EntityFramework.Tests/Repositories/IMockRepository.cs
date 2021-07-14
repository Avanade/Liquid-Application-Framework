using Liquid.Repository.EntityFramework.Tests.Entities;

namespace Liquid.Repository.EntityFramework.Tests.Repositories
{
    public interface IMockRepository : ILiquidRepository<MockEntity, int>
    {
    }
}