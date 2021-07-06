using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class TestCaseQueryHandler : IRequestHandler<TestCaseRequest, TestCaseResponse>
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<TestCaseResponse> Handle(TestCaseRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var result = new TestCaseResponse("Successfull test.");

            return result;
        }
    }
}
