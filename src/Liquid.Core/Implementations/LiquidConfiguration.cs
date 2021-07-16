using Liquid.Core.Attributes;
using Liquid.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;

namespace Liquid.Core.Implementations
{
    ///<inheritdoc/>
    public class LiquidConfiguration<T> : ILiquidConfiguration<T> where T : class, new()
    {
        ///<inheritdoc/>
        public T Settings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidConfiguration{T}"/> class.
        /// </summary>
        /// <param name="options">Configured application settigs instance.</param>
        /// <param name="configuration">A set of key/value application settigs properties.</param>
        public LiquidConfiguration(IOptions<T> options, IConfiguration configuration)
        {
            Settings = configuration.GetSection(GetSectionName(typeof(T))).Get<T>() ?? options.Value;

            if (Settings is null)
                throw new ArgumentNullException(GetSectionName(typeof(T)));
        }

        private string GetSectionName(Type type)
        {
            var sectionAttibute = type.GetCustomAttribute<LiquidSectionNameAttribute>();

            return sectionAttibute?.SectionName ?? type.Name;
        }
    }
}