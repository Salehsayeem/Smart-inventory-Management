using Sims.Api.Dto;
using Sims.Api.Dto.Inventory;

namespace Sims.Api.IRepositories
{
    public interface IInventoryRepository
    {
        Task<CommonResponseDto> CreateOrUpdateInventory(CreateOrUpdateInventoryDto model, Ulid userId);
        Task<GetInventoryByIdDto> GetInventoryById(long id);
        Task<PaginationDto<GetInventoryDetailsByLocationIdLandingDto>> GetAllInventoryDetailsPagination(string search, long shopId,long locationId, int pageNumber, int pageSize);
        Task<CommonResponseDto> DeleteInventory(long id, Ulid userId);
    }
}
