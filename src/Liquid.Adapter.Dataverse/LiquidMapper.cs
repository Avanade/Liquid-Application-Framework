using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Adapter.Dataverse
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    public abstract class LiquidMapper<TFrom, TTo> : ILiquidMapper<TFrom, TTo>
    {
        private readonly string _mapperName;

        /// <summary>
        /// Create a new instance of <see cref="LiquidMapper{TFrom, TTo}"/>
        /// </summary>
        /// <param name="mapperName">Mapper implementation name.</param>
        public LiquidMapper(string mapperName)
        {
            _mapperName = mapperName;
        }
        public async Task<TTo> Map(TFrom dataObject, string? entityName = null)
        {
            if (dataObject is null)
            {
                throw new ArgumentNullException(nameof(dataObject));
            }

            try
            {
                return await MapImpl(dataObject, entityName);
            }
            catch (Exception e)
            {
                var msg = $"{_mapperName} throw data mapping error: '{e.Message}'";

                throw new DataMappingException(msg, e);
            }
        }

        protected abstract Task<TTo> MapImpl(TFrom dataObject, string? entityName = null);
    }
}
