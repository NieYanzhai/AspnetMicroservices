using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.Api.GrpcServices
{
    public interface ICouponsGrpcService
    {
        Task<CouponModel> GetCoupon(string productName);
    }
}