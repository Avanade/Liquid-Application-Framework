using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    public interface ILiquidProducer<TEvent>
    {
        Task SendAsync(IEnumerable<TEvent> events);

        Task SendAsync(TEvent @event);
    }
}
