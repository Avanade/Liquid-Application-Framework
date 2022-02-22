using FluentValidation;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Create
{
    class CreateENTITYNAMECommandValidator : AbstractValidator<CreateENTITYNAMECommand>
    {
        public CreateENTITYNAMECommandValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
