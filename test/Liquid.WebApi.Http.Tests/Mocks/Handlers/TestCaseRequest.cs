using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class TestCaseRequest : IRequest<TestCaseResponse>
    {
    }
}
