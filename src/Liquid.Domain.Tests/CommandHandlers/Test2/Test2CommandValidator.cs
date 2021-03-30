using FluentValidation;

namespace Liquid.Domain.Tests.CommandHandlers.Test2
{
    public class Test2CommandValidator : AbstractValidator<Test2Command>
    {
        public Test2CommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0);
        }
    }
}