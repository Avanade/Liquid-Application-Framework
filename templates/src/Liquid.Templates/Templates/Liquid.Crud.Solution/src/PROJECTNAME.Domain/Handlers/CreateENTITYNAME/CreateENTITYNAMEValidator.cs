using FluentValidation;

namespace PROJECTNAME.Domain.Handlers
{
    public class CreateENTITYNAMEValidator : AbstractValidator<CreateENTITYNAMERequest>
    {
        public CreateENTITYNAMEValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
