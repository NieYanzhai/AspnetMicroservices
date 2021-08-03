using System.Threading.Tasks;
using Discount.Grpc.Entites;

namespace Discount.Grpc.Repository
{
    public interface ICouponRepository
    {
        Task<bool> CreateCouponAsync(Coupon coupon);
        Task<bool> DeleteCouponAsync(string productName);
        Task<Coupon> GetCouponAsync(string productName);
        Task<bool> UpdateCouponAsync(Coupon coupon);
    }
}