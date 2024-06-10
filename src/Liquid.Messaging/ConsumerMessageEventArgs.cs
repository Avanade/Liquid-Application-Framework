using System.Collections.Generic;

namespace Liquid.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public class ConsumerMessageEventArgs<TEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        public TEvent Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Headers { get; set; }
    }
}