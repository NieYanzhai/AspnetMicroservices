using FluentValidation;
using Ordering.Application.Features.Commands;

namespace Ordering.Application.Features.Validators
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.Order.UserName)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Order.TotalPrice)
                .GreaterThan(0);
        }
    }
}