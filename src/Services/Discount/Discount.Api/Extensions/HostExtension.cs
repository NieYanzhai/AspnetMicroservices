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
        public async Task<IHost> MigrateDatabaseAsync<T>(this IHost host, int retryCount = 0)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var configuration = host.Services.GetRequiredService<IConfiguration>())
                {
                    var connection = new NpgsqlConnection(configuration.GetValue<string>("PostgresSettings:ConnectionString"));
                    await connection.OpenAsync();

                    var sql = @"CREATE TABLE IF NOT EXISTS Coupon
                                (
                                    Id serial int,
                                    ProductName character varying(24)[] NOT NULL,
                                    Description character varying(100)[],
                                    Amount money NOT NULL,
                                    PRIMARY KEY(Id)
                                ); ";
                    await connection.ExecuteAsync(sql);
                }
            }

            return host;
        }
    }
}