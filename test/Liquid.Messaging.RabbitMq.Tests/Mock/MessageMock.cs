using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Messaging.RabbitMq.Tests.Mock
{
    /// <summary>
    /// Test Message Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MessageMock
    {
        /// <summary>
        /// Gets or sets the test message identifier.
        /// </summary>
        /// <value>
        /// The test message identifier.
        /// </value>
        public int TestMessageId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public double Amount { get; set; }
    }
}