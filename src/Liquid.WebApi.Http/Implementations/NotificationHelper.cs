using Liquid.Core.Interfaces;
using Liquid.WebApi.Http.Interfaces;

namespace Liquid.WebApi.Http.Implementations
{
    ///<inheritdoc/>
    public class NotificationHelper : INotificationHelper
    {
        private readonly ILiquidContextNotifications _contextNotifications;

        /// <summary>
        /// Inicialize a new instace of <see cref="NotificationHelper"/>
        /// </summary>
        /// <param name="contextNotifications"></param>
        public NotificationHelper(ILiquidContextNotifications contextNotifications)
        {
            _contextNotifications = contextNotifications;
        }
        
        ///<inheritdoc/>
        public object IncludeMessages<TResponse>(TResponse response)
        {
            var messages = _contextNotifications.GetNotifications();

            return new { response, messages };

        }
    }
}
