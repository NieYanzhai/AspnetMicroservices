using MediatR;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Commands
{
    public class CheckoutOrderCommand : IRequest<int>
    {
        public Order Order { get; set; }
    }
}