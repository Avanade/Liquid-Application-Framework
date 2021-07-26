using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
     public interface ILiquidPostprocessMiddleware
    {
        Task Execute();
    }
}
