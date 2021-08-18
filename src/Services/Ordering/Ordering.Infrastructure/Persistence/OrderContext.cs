using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "nie";
                        entry.Entity.CreatedDate = DateTimeOffset.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = "nie";
                        entry.Entity.LastModifiedDate = DateTimeOffset.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
