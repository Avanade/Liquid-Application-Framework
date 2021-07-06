using Liquid.Core.Interfaces;
using Liquid.WebApi.Http.Interfaces;
using System;

namespace Liquid.WebApi.Http.Implementations
{
    ///<inheritdoc/>
    public class LiquidNotificationHelper : ILiquidNotificationHelper
    {
        private readonly ILiquidContextNotifications _contextNotifications;

        /// <summary>
        /// Inicialize a new instace of <see cref="LiquidNotificationHelper"/>
        /// </summary>
        /// <param name="contextNotifications"></param>
        public LiquidNotificationHelper(ILiquidContextNotifications contextNotifications)
        {
            _contextNotifications = contextNotifications ?? throw new ArgumentNullException(nameof(contextNotifications));
        }
        
        ///<inheritdoc/>
        public object IncludeMessages<TResponse>(TResponse response)
        {
            var messages = _contextNotifications.GetNotifications();

            if (messages is null)
                return response;

            return new { response, messages };

        }
    }
}
