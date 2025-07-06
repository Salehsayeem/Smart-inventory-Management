using Sims.Api.Dto;

namespace Sims.Api.IRepositories
{
    public interface ISupplierRepository
    {
        Task<CommonResponseDto> CreateOrUpdateSupplier(CreateOrUpdateSupplierDto model, Ulid userId);
        Task<CommonResponseDto> GetSupplierById(long supplierId);
        Task<CommonResponseDto> DeleteSupplier(long supplierId, Ulid userId);
        Task<PaginationDto<GetSupplierDto>> GetAllSupplierPagination(string search,long shopId, int pageNo, int pageSize);
    }
}
