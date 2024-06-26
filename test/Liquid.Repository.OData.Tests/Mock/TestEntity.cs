﻿using Liquid.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.OData.Tests.Mock
{
    /// <summary>
    /// Mock test entity class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Data.Entities.DataMappingBase{System.Int32}</cref>
    /// </seealso>
    [ExcludeFromCodeCoverage]
    public class TestEntity : LiquidEntity<string>
    {
        /// <summary>
        /// Gets or sets the mock title.
        /// </summary>
        /// <value>
        /// The mock title.
        /// </value>
        public string MockTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MockEntity"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

    }
}