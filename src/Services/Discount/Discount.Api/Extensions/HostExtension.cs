using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Discount.Api.Extensions
{
    public static class HostExtension
    {
        public static async Task<IHost> MigrateDatabaseAsync(this IHost host, int retryCount = 0)
        {
            using (var scope = host.Services.CreateScope())
            {
                var configuration = host.Services.GetRequiredService<IConfiguration>();
                var connection = new NpgsqlConnection(configuration.GetValue<string>("PostgresSettings:ConnectionString"));
                await connection.OpenAsync();

                var sql = @"CREATE TABLE IF NOT EXISTS Coupon
                                (
                                    Id serial Primary Key,
                                    ProductName varchar(24) NOT NULL,
                                    Description varchar(100),
                                    Amount money NOT NULL
                                );";
                await connection.ExecuteAsync(sql);

                sql = @"Insert Into Coupon(ProductName, Description, Amount)
                        Values(@productName, @description, @amount);";
                await connection.ExecuteAsync(sql, new
                {
                    productName = "IPhone X",
                    description = "This is IPhone X",
                    amount = 100m
                });
                await connection.ExecuteAsync(sql, new
                {
                    productName = "IPhone 11",
                    description = "This is IPhone 11",
                    amount = 150m
                });

                await connection.CloseAsync();
            }

            return host;
        }
    }
}