using Liquid.Messaging.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    ///<inheritdoc/>
    public class LiquidPipeline : ILiquidPipeline
    {
        ///<inheritdoc/>
        public async Task Execute<T>(ProcessMessageEventArgs<T> message, 
            Func<ProcessMessageEventArgs<T>, CancellationToken, Task> process,
            CancellationToken cancellationToken)
        {
            try
            {
                await process(message, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new MessagingConsumerException(ex);
            }
        }
    }
}
