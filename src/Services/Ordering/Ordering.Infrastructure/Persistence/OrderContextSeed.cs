using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static void Seed(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.AddRange(GetFakedOrders());
                orderContext.SaveChanges();
                logger.LogInformation($"Seed database associated with context {typeof(OrderContext).Name}");
            }
        }

        private static IEnumerable<Order> GetFakedOrders()
        {
            return new List<Order>{
                new Order() {
                    UserName = "swn",
                    FirstName = "Mehmet",
                    LastName = "Ozkaya",
                    EmailAddress = "ezozkme@gmail.com",
                    AddressLine = "Bahcelievler",
                    Country = "Turkey",
                    TotalPrice = 350 },
                new Order() {
                    UserName = "nyz",
                    FirstName = "nie",
                    LastName = "yanzhai",
                    EmailAddress = "yanzhai.nie@outlook.com",
                    AddressLine = "wuxi",
                    Country = "China",
                    TotalPrice = 100 }
            };
        }
    }
}