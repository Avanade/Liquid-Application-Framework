using FluentValidation;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Update
{
    class PutENTITYNAMECommandValidator : AbstractValidator<PutENTITYNAMECommand>
    {
        public PutENTITYNAMECommandValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
