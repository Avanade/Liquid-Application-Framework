using FluentValidation;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Update
{
    class UpdateENTITYNAMECommandValidator : AbstractValidator<UpdateENTITYNAMECommand>
    {
        public UpdateENTITYNAMECommandValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
