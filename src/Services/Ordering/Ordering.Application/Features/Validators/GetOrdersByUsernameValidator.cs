using FluentValidation;
using Ordering.Application.Features.Queries;

namespace Ordering.Application.Features.Validators
{
    public class GetOrdersByUsernameValidator : AbstractValidator<GetOrdersByUsernameQuery>
    {
        public GetOrdersByUsernameValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .NotNull();
        }
    }
}