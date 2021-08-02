using System.Threading.Tasks;
using Dapper;
using Discount.Api.Entites;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Api.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly string connectionString;

        public CouponRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetValue<string>("PostgresSettings:ConnectionString");
        }

        public async Task<Coupon> GetCouponAsync(string productName)
        {
            Coupon coupon = null;
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                var sql = @"Select * From Coupon
                            Where ProductName = @productName";
                coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                                    sql: sql,
                                    param: new { productName });
            }

            if (coupon == null)
            {
                return new Coupon { ProductName = "No Coupon", Description = "No Coupon Description", Amount = 0m };
            }
            return coupon;
        }

        public async Task<bool> CreateCouponAsync(Coupon coupon)
        {
            int affected = 0; ;
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                var sql = @"Insert Into Coupon(ProductName, Description, Amount)
                                Values(@ProductName, @Description, @Amount)";
                affected = await connection.ExecuteAsync(sql, coupon);
            }

            if (affected == 0) return false;
            return true;
        }

        public async Task<bool> UpdateCouponAsync(Coupon coupon)
        {
            int affected = 0;
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                var sql = @"Update Coupon
                            Set Amount=@Amount, ProductName=@ProductName, Description=@Description
                            Where Id=@Id";
                affected = await connection.ExecuteAsync(sql, coupon);
            }
            if (affected == 0) return false;
            return true;
        }

        public async Task<bool> DeleteCouponAsync(string productName)
        {
            int affected = 0;
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                var sql = @"Delete From Coupon
                            Where ProductName=@productName";
                affected = await connection.ExecuteAsync(sql, new { productName });
            }
            if (affected == 0) return false;
            return true;
        }
    }
}