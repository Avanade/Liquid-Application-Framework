using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// Global context service.
    /// </summary>
    public interface ILiquidContext
    {
        /// <summary>
        /// Context itens.
        /// </summary>
        IDictionary<string, object> current { get; }

        /// <summary>
        /// Return context item value.
        /// </summary>
        /// <param name="key">key of the item to be obtained.</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Return context item value.
        /// </summary>
        /// <param name="key">key of the item to be obtained.</param>
        /// <typeparam name="T">Type of return object.</typeparam>
        T Get<T>(string key);

        /// <summary>
        /// Insert or update itens in context.
        /// </summary>
        /// <param name="key">Key of the item.</param>
        /// <param name="value">Value of the item.</param>
        void Upsert(string key, object value);
    }
}
