using System;
using System.Threading.Tasks;

namespace Liquid.Services.ResilienceHandlers
{
    /// <summary>
    /// Resilience Handler Interface.
    /// </summary>
    public interface IResilienceHandler
    {
        /// <summary>
        /// Executes the action under resilience control.
        /// </summary>
        /// <param name="operation">The operation to be executed under resilience.</param>
        /// <returns></returns>
        Task HandleAsync(Func<Task> operation);
    }
}