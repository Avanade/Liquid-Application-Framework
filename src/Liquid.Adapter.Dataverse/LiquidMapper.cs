using System.Diagnostics.CodeAnalysis;

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
        ///<inheritdoc/>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        protected abstract Task<TTo> MapImpl(TFrom dataObject, string? entityName = null);
    }
}
