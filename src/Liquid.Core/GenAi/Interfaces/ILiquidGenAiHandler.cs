using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Core.GenAi.Interfaces
{
    /// <summary>
    /// This service is the hub of Liquid adapter audio stream handlers for Generative AI.
    /// </summary>
    public interface ILiquidGenAiHandler
    {
        /// <summary>
        /// The task that manages receipt of incoming simplified protocol messages from the frontend client.
        /// </summary>
        /// <param name="socket"> The WebSocket connection to the client.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task HandleInputMessagesAsync(WebSocket socket, CancellationToken cancellationToken = default);

        /// <summary>
        /// The task that manages the incoming updates from realtime API messages and model responses.
        /// </summary>
        /// <param name="socket"> The WebSocket connection to the client.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task HandleUpdatesFromServiceAsync(WebSocket socket, CancellationToken cancellationToken = default);
    }
}
