using FluentValidation;

namespace Liquid.Messaging.Kafka.Tests.Mock.HandlerMock
{
    public class MockValidator : AbstractValidator<MockRequest>
    {
        public MockValidator()
        {
            RuleFor(request => request.Message.TestMessageId).NotEmpty().NotNull();
        }
    }
}
