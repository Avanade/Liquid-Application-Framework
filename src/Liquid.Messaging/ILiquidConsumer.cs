using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    public interface ILiquidConsumer<TEvent>
    {
        void Start();

        event Func<ProcessMessageEventArgs<TEvent>, CancellationToken, Task> ProcessMessageAsync;
        event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;
    }
}
