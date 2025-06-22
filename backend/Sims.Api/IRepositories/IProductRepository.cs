using Sims.Api.Dto.Category;
using Sims.Api.Dto;
using Sims.Api.Dto.Product;

namespace Sims.Api.IRepositories
{
    public interface IProductRepository
    {
        public Task<CommonResponseDto> CreateOrUpdateProduct(CreateOrUpdateProductDto model, Ulid userId);
        public Task<CommonResponseDto> GetProductById(long productId);
        public Task<PaginationDto<ProductLandingDataDto>> GetProductByShopId(string search, long shopId, int pageNo, int pageSize);
        public Task<PaginationDto<ProductLandingDataDto>> GetProductByCategoryId(string search, long shopId, long categoryId, int pageNo, int pageSize);
        public Task<CommonResponseDto> DeleteProduct(long productId, Ulid userId);
    }
}
