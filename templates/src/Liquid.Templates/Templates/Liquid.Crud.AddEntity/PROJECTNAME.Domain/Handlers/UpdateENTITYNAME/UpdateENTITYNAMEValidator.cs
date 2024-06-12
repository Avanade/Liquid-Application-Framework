using FluentValidation;

namespace PROJECTNAME.Domain.Handlers
{
    public class UpdateENTITYNAMEValidator : AbstractValidator<UpdateENTITYNAMERequest>
    {
        public UpdateENTITYNAMEValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
