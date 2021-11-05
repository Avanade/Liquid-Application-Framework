using Grpc.Core;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.Services.Grpc.Tests.Server.Services
{
    [ExcludeFromCodeCoverage]
    public class CreditRatingCheckService : CreditRatingCheck.CreditRatingCheckBase
    {
        private static readonly Dictionary<string, int> CustomerTrustedCredit = new Dictionary<string, int>
        {
            {"id0201", 10000},
            {"id0417", 5000},
            {"id0306", 15000}
        };

        public override Task<CreditReply> CheckCreditRequest(CreditRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CreditReply
            {
                IsAccepted = IsEligibleForCredit(request.CustomerId, request.Credit)
            });
        }

        private bool IsEligibleForCredit(string customerId, int credit)
        {
            var isEligible = false;

            if (CustomerTrustedCredit.TryGetValue(customerId, out int maxCredit))
            {
                isEligible = credit <= maxCredit;
            }

            return isEligible;
        }
    }
}