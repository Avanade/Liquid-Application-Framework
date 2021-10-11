using FluentValidation;

namespace Liquid.Sample.Domain.Handlers.SamplePut
{
    public class PutCommandRequestValidator : AbstractValidator<PutCommandRequest>
    {
        public PutCommandRequestValidator()
        {
            RuleFor(request => request.Message.Id).NotEmpty().NotNull();
        }
    }
}
