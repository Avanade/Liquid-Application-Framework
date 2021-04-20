using Liquid.Repository.EntityFramework.Tests.Entities;

namespace Liquid.Repository.EntityFramework.Tests.Repositories
{
    public interface IMockRepository : ILightRepository<MockEntity, int>
    {
    }
}