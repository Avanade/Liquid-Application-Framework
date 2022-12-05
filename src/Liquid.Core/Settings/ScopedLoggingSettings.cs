using Liquid.Core.Attributes;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.Settings
{
    /// <summary>
    /// Scoped logging setting properties.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [LiquidSectionName("Liquid:ScopedLogging")]
    public class ScopedLoggingSettings
    {
        /// <summary>
        /// List of keys that should be created on logger scope.
        /// </summary>
        public List<ScopedKey> Keys { get; set; } = new List<ScopedKey>();
    }

}
