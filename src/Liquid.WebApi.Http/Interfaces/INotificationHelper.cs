namespace Liquid.WebApi.Http.Interfaces
{
    /// <summary>
    /// Abstracts the management of context notifications.
    /// </summary>
    public interface INotificationHelper
    {
        /// <summary>
        /// Add context notification messages to the response object.
        /// </summary>
        /// <typeparam name="TResponse">Type of response object obtained upon return of a request.</typeparam>
        /// <param name="response">Object obtained upon return of a request.</param>
        object IncludeMessages<TResponse>(TResponse response);
    }
}
