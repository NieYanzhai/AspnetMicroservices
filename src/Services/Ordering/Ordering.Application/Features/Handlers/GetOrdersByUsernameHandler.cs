using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Queries;
using Ordering.Application.Models;

namespace Ordering.Application.Features.Handlers
{
    public class GetOrdersByUsernameHandler : IRequestHandler<GetOrdersByUsernameQuery, List<OrderVm>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public GetOrdersByUsernameHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }
        public async Task<List<OrderVm>> Handle(GetOrdersByUsernameQuery request, CancellationToken cancellationToken)
        {
            var orders = await this.orderRepository.GetOrdersByUserName(request.UserName);
            return orders == null ? null : this.mapper.Map<List<OrderVm>>(orders);
        }
    }
}