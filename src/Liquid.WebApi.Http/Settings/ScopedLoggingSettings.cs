using Liquid.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Liquid.WebApi.Http.Settings
{
    /// <summary>
    /// Scoped logging setting properties.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [LiquidSectionName("Liquid:HttpScopedLogging")]
    public class ScopedLoggingSettings
    {
        /// <summary>
        /// List of keys that should be created on logger scope.
        /// </summary>
        public List<ScopedKey> Keys { get; set; } = new List<ScopedKey>();

    }

    /// <summary>
    /// Definition of scoped key type.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ScopedKey
    {
        /// <summary>
        /// Name of the scoped key.
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Indicates if this key is mandatory.
        /// </summary>
        public bool Required { get; set; } = false;
    }
}
