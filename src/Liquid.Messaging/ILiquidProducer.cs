using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    public interface ILiquidProducer<TEntity>
    {
        Task SendAsync(IEnumerable<TEntity> events);

        Task SendAsync(TEntity @event);
    }
}
