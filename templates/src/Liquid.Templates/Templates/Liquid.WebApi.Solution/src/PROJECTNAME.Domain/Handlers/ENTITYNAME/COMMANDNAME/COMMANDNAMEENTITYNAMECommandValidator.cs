using FluentValidation;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.COMMANDNAME
{
    public class COMMANDNAMEENTITYNAMECommandValidator : AbstractValidator<COMMANDNAMEENTITYNAMECommand>
    {
        public COMMANDNAMEENTITYNAMECommandValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
