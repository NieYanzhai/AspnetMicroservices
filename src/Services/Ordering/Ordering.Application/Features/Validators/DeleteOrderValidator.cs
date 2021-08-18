using FluentValidation;
using Ordering.Application.Features.Commands;

namespace Ordering.Application.Features.Validators
{
    public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}