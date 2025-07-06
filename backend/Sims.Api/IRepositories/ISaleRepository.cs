using Sims.Api.Dto;
using Sims.Api.Helper;

namespace Sims.Api.IRepositories
{
    public interface ISaleRepository
    {
        Task<CommonResponseDto> CreateSale(CreateSaleDto model, Ulid userId);
        Task<PaginationDto<SaleLandingDataDto>> GetAllSales(string search, long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize);
        Task<GetSaleDto> GetSaleById(long id);
        Task<CommonResponseDto> UpdateSaleItems(List<CreateSaleItemDto> items, Ulid userId);
        Task<CommonResponseDto> UpdateSaleStatus(long saleId, int statusId, Ulid userId);
    }
}
