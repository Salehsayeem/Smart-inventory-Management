using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Dto.Category;
using Sims.Api.Dto.Inventory;
using Sims.Api.IRepositories;
using Sims.Api.Models;
using Sims.Api.StoredProcedure;
using static Sims.Api.Helper.CommonHelper;

namespace Sims.Api.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly CallStoredProcedure _spCaller;
        public InventoryRepository(ApplicationDbContext context, CallStoredProcedure spCaller)
        {
            _context = context;
            _spCaller = spCaller;
        }
        public async Task<CommonResponseDto> CreateOrUpdateInventory(CreateOrUpdateInventoryDto model, Ulid userId)
        {
            try
            {
                if (model.Id == 0)
                {
                    var exists = await _context.Inventories
                        .AnyAsync(a => a!.ShopId == model.ShopId 
                                       && a.ProductId == model.ProductId 
                                       && a.LocationId == model.LocationId 
                                       && a.IsActive);
                    if (exists)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"Inventory of this product already exists on this warehouse.",
                            Data = null,
                            StatusCode = 400
                        };
                    }

                    var data = new Inventory()
                    {
                        ProductId = model.ProductId,
                        LocationId = model.LocationId,
                        Quantity = model.Quantity,
                        RestockThreshold = model.RestockThreshold,
                        ShopId = model.ShopId,
                        CreatedBy = userId,
                        IsActive = true
                    };
                    await _context.Inventories.AddAsync(data);
                    await _context.SaveChangesAsync();
                    return new CommonResponseDto
                    {
                        Message = $"Inventory created successfully",
                        Data = null,
                        StatusCode = 200
                    };
                }
                else
                {
                    var data = await _context.Inventories
                        .FirstOrDefaultAsync(a => a!.ShopId == model.ShopId
                                       && a.ProductId == model.ProductId
                                       && a.LocationId == model.LocationId
                                       && a.IsActive); 
                    if (data == null)
                    {
                        return new CommonResponseDto
                        {
                            Message = "Category not found.",
                            Data = null,
                            StatusCode = 404
                        };
                    }

                    data.Quantity = model.Quantity;
                    data.RestockThreshold = model.RestockThreshold;
                    data.ModifiedBy = userId;
                    _context.Inventories.Update(data);
                    await _context.SaveChangesAsync();
                    return new CommonResponseDto
                    {
                        Message = "Inventory updated successfully",
                        Data = null,
                        StatusCode = 200
                    };
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<GetInventoryByIdDto> GetInventoryById(long id)
        {
            var inventory = await _context.Inventories
                .Where(i => i.Id == id && i.IsActive)
                .Select(i => new GetInventoryByIdDto
                {
                    Id = i.Id,
                    Quantity = i.Quantity,
                    RestockThreshold = i.RestockThreshold,
                })
                .FirstOrDefaultAsync();

            return inventory!;
        }

        public async Task<PaginationDto<GetInventoryDetailsByLocationIdLandingDto>> GetAllInventoryDetailsPagination(string search, long shopId, long locationId, int pageNumber, int pageSize)
        {
            if (shopId <= 0)
                throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
            if (locationId <= 0)
                throw new ArgumentException("Shop ID must be a positive number", nameof(locationId));
            if (pageNumber < 1)
                throw new ArgumentException("Page number must be at least 1", nameof(pageNumber));
            if (pageSize < 1)
                throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

            return await _spCaller.CallPagedFunctionAsync<GetInventoryDetailsByLocationIdLandingDto>(
                StoredProcedureNames.GetAllInventoryDetailsPagination,
                new { p0 = search, p1 = shopId,p2=locationId, p3 = pageNumber, p4 = pageSize },
                pageNumber,
                pageSize
            );
        }

        public async Task<CommonResponseDto> DeleteInventory(long id, Ulid userId)
        {
            try
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
                if (inventory == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Inventory not found.",
                        Data = null,
                        StatusCode = 404
                    };
                }
                inventory.IsActive = false;
                inventory.ModifiedBy = userId;
                _context.Inventories.Update(inventory);
                await _context.SaveChangesAsync();
                return new CommonResponseDto
                {
                    Message = "Inventory deleted successfully",
                    Data = null,
                    StatusCode = 200
                };

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
