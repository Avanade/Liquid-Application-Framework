using System;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ILiquidConsumer<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        void Start();

        event Func<ProcessMessageEventArgs<TEntity>, CancellationToken, Task> ProcessMessageAsync;
        event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;
    }
}
