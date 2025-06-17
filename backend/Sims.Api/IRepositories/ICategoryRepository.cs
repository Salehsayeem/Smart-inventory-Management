using Sims.Api.Dto;
using Sims.Api.Dto.Category;

namespace Sims.Api.IRepositories
{
    public interface ICategoryRepository
    {
        public Task<CommonResponseDto> CreateOrUpdateCategory(CreateOrUpdateCategoryDto model, Ulid userId);
        public Task<CommonResponseDto> GetCategoryById(Ulid categoryId);
        public CommonResponseDto GetAllCategoryByShopId(string search, long shopId, int pageNo, int pageSize);
        public Task<CommonResponseDto> DeleteCategory(Ulid categoryId, Ulid userId);
    }
}
