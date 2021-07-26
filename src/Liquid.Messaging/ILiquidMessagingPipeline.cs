using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    public interface ILiquidMessagingPipeline<T>
    {
        IEnumerable<ILiquidPreProcessMiddleware<T>> PreProcesses { get; set; }
        
        IEnumerable<ILiquidPostprocessMiddleware> PostProcess { get; set; }


        T ExecutePreProcessor(T input);

        Task ExecutePostProcessor<TEvent>(Task process);

    }
}
