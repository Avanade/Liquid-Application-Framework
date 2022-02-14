using FluentValidation;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Put
{
    class PutENTITYNAMECommandValidator : AbstractValidator<PutENTITYNAMECommand>
    {
        public PutENTITYNAMECommandValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
