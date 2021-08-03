using System.Threading.Tasks;
using Discount.Grpc.Protos;
using static Discount.Grpc.Protos.Coupons;

namespace Basket.Api.GrpcServices
{
    public class CouponsGrpcService : ICouponsGrpcService
    {
        private readonly CouponsClient couponsClient;

        public CouponsGrpcService(CouponsClient couponsClient)
        {
            this.couponsClient = couponsClient;
        }

        public async Task<CouponModel> GetCoupon(string productName)
        {
            return await this.couponsClient.GetCouponAsync(
                new GetCouponRequest
                {
                    ProductName = productName
                });
        }
    }
}