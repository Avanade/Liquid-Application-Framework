using System.Collections.Generic;

namespace Liquid.Messaging
{
    public class ProcessMessageEventArgs<TEvent>
    {
        public TEvent Data { get; set; }

        public IDictionary<string, object> Headers { get; set; }
    }
}