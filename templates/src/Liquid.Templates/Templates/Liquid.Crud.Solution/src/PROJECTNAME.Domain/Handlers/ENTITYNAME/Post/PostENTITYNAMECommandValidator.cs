using FluentValidation;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Post
{
    class PostENTITYNAMECommandValidator : AbstractValidator<PostENTITYNAMECommand>
    {
        public PostENTITYNAMECommandValidator()
        {
            RuleFor(request => request.Body.Id).NotEmpty().NotNull();
        }
    }
}
