using Sims.Api.Dto;
using Sims.Api.Dto.Category;
using Sims.Api.Dto.Location;

namespace Sims.Api.IRepositories
{
    public interface ILocationRepository
    {
        public Task<CommonResponseDto> CreateOrUpdateWarehouse(CreateOrUpdateWarehouseDto model, Ulid userId);
        public Task<CommonResponseDto> GetWarehouseById(long warehouseId);
        public Task<PaginationDto<WarehouseLandingDataDto>> GetAllWarehousesByShopId(string search, long shopId, int pageNo, int pageSize);
        public Task<CommonResponseDto> DeleteWarehouse(long warehouseId, Ulid userId);

    }
}
