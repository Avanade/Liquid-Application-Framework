using Liquid.Core.Entities;
using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.Core.AbstractMappers
{
    /// <summary>
    /// Defines object that map data between two instance types.
    /// </summary>
    /// <typeparam name="TFrom">type of data source object.</typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class OcrResultMapper<TFrom> : ILiquidMapper<TFrom, OcrResult>
    {
        private readonly string _mapperName;

        /// <summary>
        /// Create a new instance of <see cref="OcrResultMapper{TFrom}"/>
        /// </summary>
        /// <param name="mapperName">Mapper implementation name.</param>
        public OcrResultMapper(string mapperName)
        {
            _mapperName = mapperName;
        }
        ///<inheritdoc/>
        public async Task<OcrResult> Map(TFrom dataObject)
        {
            if (dataObject is null)
            {
                throw new ArgumentNullException(nameof(dataObject));
            }

            try
            {
                return await MapImpl(dataObject);
            }
            catch (Exception e)
            {
                var msg = $"{_mapperName} throw data mapping error: '{e.Message}'";

                throw new DataMappingException(msg, e);
            }
        }
        /// <summary>
        /// Create a new instance of <see cref="OcrResult"/>
        /// with values obtained from <see cref="TFrom"/>. 
        /// </summary>
        /// <param name="dataObject">data source object instance.</param>
        protected abstract Task<OcrResult> MapImpl(TFrom dataObject);
    }
}
