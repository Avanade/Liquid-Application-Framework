using Liquid.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Liquid.Core.Settings
{
    /// <summary>
    /// Context key settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [LiquidSectionName("Liquid:ScopedContext")]
    public class ScopedContextSettings
    {
        /// <summary>
        /// List of keys that should be created on context.
        /// </summary>
        public List<ScopedKey> Keys { get; set; } = new List<ScopedKey>();

        /// <summary>
        /// Indicates if the current culture must be included on context keys.
        /// </summary>
        public bool Culture { get; set; } = false;

    }
}