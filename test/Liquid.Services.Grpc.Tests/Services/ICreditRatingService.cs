using Liquid.Services.Grpc.Tests.Server.Services;
using System.Threading.Tasks;

namespace Liquid.Services.Grpc.Tests.Services
{
    /// <summary>
    /// Credit Rating Service interface.
    /// </summary>
    /// <seealso cref="ILightGrpcService" />
    public interface ICreditRatingService : ILightGrpcService
    {
        /// <summary>
        /// Checks the credit with execute asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<CreditReply> CheckCreditRatingAsync(CreditRequest request);
    }
}