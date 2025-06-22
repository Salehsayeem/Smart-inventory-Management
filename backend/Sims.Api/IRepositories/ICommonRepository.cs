using Sims.Api.Dto;

namespace Sims.Api.IRepositories
{
    public interface ICommonRepository
    {
        public Task<List<CommonDdlDto>> AllCategoriesOfShopDdl(long shopId, string search);
        public Task<List<CommonDdlDto>> AllProductsOfShopDdl(long shopId, string search);
        public Task<List<CommonDdlDto>> AllWarehousesOfShopDdl(long shopId, string search);
    }
}
