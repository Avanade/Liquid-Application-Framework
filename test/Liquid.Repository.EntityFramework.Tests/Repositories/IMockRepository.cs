using Liquid.Data.EntityFramework.Tests.Entities;
using Liquid.Data.Interfaces;

namespace Liquid.Data.EntityFramework.Tests.Repositories
{
    public interface IMockRepository : IRepository<MockEntity, int>
    {
    }
}