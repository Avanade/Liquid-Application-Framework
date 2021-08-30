using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Sample.Domain.Handlers
{
    public class SampleEventRequestValidator : AbstractValidator<SampleEventRequest>
    {
        public SampleEventRequestValidator()
        {
            RuleFor(request => request.Entity.Id).NotEmpty().NotNull();
        }
    }
}
