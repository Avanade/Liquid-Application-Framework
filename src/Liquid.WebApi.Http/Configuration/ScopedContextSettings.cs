using Liquid.Core.Attributes;
using System.Collections.Generic;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Context key settings.
    /// </summary>
    [LiquidSectionName("Liquid:HttpScopedContext")]
    public class ScopedContextSettings
    {
        /// <summary>
        /// List of keys that should be created on context.
        /// </summary>
        public List<ContextKey> Keys { get; set; } = new List<ContextKey>();
        
        /// <summary>
        /// Indicates if the current culture must be included on context keys.
        /// </summary>
        public bool Culture { get; set; } = false;

    }
    /// <summary>
    /// Definition of context key type.
    /// </summary>
    public class ContextKey
    {
        /// <summary>
        /// Name of the context key.
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Indicates if this key is mandatory.
        /// </summary>
        public bool Required { get; set; } = false;
    }
}
