using System;

namespace Liquid.Messaging.Attributes
{
    /// <summary>
    /// Light Messaging Attribute Class.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class LightMessagingAttribute : Attribute
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
        /// Initializes a new instance of the <see cref="LightMessagingAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        protected LightMessagingAttribute(string connectionId, bool compressMessage = false)
        {
            CompressMessage = compressMessage;
            ConnectionId = connectionId;
        }
    }
}