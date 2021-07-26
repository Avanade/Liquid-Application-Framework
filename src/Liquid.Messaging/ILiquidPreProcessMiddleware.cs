using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface ILiquidPreProcessMiddleware<TMessage>
    {
        TMessage Execute(TMessage inputMessage);
    }
}
