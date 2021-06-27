using Liquid.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Core.Implementations
{
    ///<inheritdoc/>
    public class LiquidContext : ILiquidContext
    {
        private IDictionary<string, object> _current = new Dictionary<string, object>();

        ///<inheritdoc/>
        public IDictionary<string, object> current => _current;

        ///<inheritdoc/>
        public void Upsert(string key, object value)
        {
            if (_current.ContainsKey(key))
            {
                _current[key] = value;
            }
            else
            {
                _current.TryAdd(key, value);
            }
        }

        ///<inheritdoc/>
        public object Get(string key)
        {
            try
            {
                return current[key];
            }
            catch
            {
                return null;
            }
            
        }
    }
}
