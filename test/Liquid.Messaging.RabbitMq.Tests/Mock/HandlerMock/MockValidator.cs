using FluentValidation;

namespace Liquid.Messaging.RabbitMq.Tests.Mock.HandlerMock
{
    public class MockValidator : AbstractValidator<MockRequest>
    {
        public MockValidator()
        {
            RuleFor(request => request.Message.TestMessageId).NotEmpty().NotNull();
        }
    }
}
