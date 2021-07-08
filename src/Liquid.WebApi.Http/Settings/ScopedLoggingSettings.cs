using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.WebApi.Http.Settings
{
    /// <summary>
    /// Scoped logging setting properties.
    /// </summary>
    public class ScopedLoggingSettings
    {
        /// <summary>
        /// List of keys that should be created on logging..
        /// </summary>
        public List<ScopedKey> Keys { get; set; } = new List<ScopedKey>();

    }

    /// <summary>
    /// Definition of scoped key type.
    /// </summary>
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
