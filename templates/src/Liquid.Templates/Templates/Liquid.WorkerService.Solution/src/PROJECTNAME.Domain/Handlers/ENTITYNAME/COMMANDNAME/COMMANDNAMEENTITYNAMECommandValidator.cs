using FluentValidation;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.COMMANDNAME
{
    class COMMANDNAMEENTITYNAMECommandValidator : AbstractValidator<COMMANDNAMEENTITYNAMECommand>
    {
        public COMMANDNAMEENTITYNAMECommandValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
