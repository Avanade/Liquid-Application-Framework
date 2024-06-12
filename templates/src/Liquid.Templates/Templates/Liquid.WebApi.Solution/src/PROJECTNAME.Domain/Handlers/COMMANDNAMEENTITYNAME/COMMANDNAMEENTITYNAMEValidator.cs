using FluentValidation;

namespace PROJECTNAME.Domain.Handlers
{
    public class COMMANDNAMEENTITYNAMEValidator : AbstractValidator<COMMANDNAMEENTITYNAMERequest>
    {
        public COMMANDNAMEENTITYNAMEValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
