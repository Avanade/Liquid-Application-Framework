using System;

namespace Liquid.Messaging.Parameters
{
    /// <summary>
    /// Light Messaging Parameter base class.
    /// </summary>
    public abstract class LightMessagingParameter : Attribute
    {
        /// <summary>
        /// Gets the connection identifier.
        /// </summary>
        /// <value>
        /// The connection identifier.
        /// </value>
        public string ConnectionId { get; }

        /// <summary>
        /// Gets a value indicating whether [compress message].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [compress message]; otherwise, <c>false</c>.
        /// </value>
        public bool CompressMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightMessagingParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        protected LightMessagingParameter(string connectionId, bool compressMessage = false)
        {
            ConnectionId = connectionId ?? throw new ArgumentNullException(nameof(connectionId));
            CompressMessage = compressMessage;
        }
    }
}