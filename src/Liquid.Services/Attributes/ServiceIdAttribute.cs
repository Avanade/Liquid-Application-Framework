using System;

namespace Liquid.Services.Attributes
{
    /// <summary>
    /// Service Id Attribute Class.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceIdAttribute : Attribute
    {
        /// <summary>
        /// Gets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        public string ServiceId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceIdAttribute"/> class.
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        public ServiceIdAttribute(string serviceId)
        {
            ServiceId = serviceId;
        }
    }
}