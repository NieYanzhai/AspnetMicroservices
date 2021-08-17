using System.Collections.Generic;
using MediatR;
using Ordering.Application.Models;

namespace Ordering.Application.Features.Queries
{
    public class GetOrdersByUsernameQuery : IRequest<List<OrderVm>>
    {
        public string UserName { get; set; }
    }

    
}