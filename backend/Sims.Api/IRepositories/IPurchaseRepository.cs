using Sims.Api.Dto;

namespace Sims.Api.IRepositories
{
    public interface IPurchaseRepository
    {
        Task<CommonResponseDto> CreatePurchaseOrder(CreatePurchaseDto model, Ulid userId);
        Task<PaginationDto<PurchaseOrderLandingDataDto>> GetAllPurchaseOrders(string search, long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize);
        Task<GetPurchaseOrderDto> GetPurchaseOrderById(long id);
        Task<CommonResponseDto> UpdatePurchaseOrderItems(List<CreatePurchaseItemDto> items, Ulid userId);
        Task<CommonResponseDto> UpdatePurchaseOrderStatus(long id, int statusId, Ulid userId);

    }
}
