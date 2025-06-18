using Sims.Api.Dto;

namespace Sims.Api.IRepositories
{
    public interface ICommonRepository
    {
        public CommonResponseDto AllCategoriesOfShopDdl(long shopId);
        public CommonResponseDto AllProductsOfShopDdl(long shopId);
    }
}
