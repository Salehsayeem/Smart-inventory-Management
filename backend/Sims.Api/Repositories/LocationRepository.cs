using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Dto.Category;
using Sims.Api.Dto.Location;
using Sims.Api.Helper;
using Sims.Api.IRepositories;
using Sims.Api.Models;
using Sims.Api.StoredProcedure;

namespace Sims.Api.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly CallStoredProcedure _spCaller;

        public LocationRepository(ApplicationDbContext context, CallStoredProcedure spCaller)
        {
            _context = context;
            _spCaller = spCaller;
        }

        public async Task<CommonResponseDto> CreateOrUpdateWarehouse(CreateOrUpdateWarehouseDto model, Ulid userId)
        {
            try
            {
                if (model.Id == 0)
                {
                    var exists = await _context.Locations
                        .AnyAsync(a => a!.ShopId == model.ShopId && a.Name.Trim() == model.Name.Trim() && a.IsActive);
                    if (exists)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"Warehouse '{model.Name}' already exists.",
                            Data = null,
                            StatusCode = 400
                        };
                    }

                    var data = new Location()
                    {
                        Name = model.Name,
                        Address = model.Address,
                        ShopId = model.ShopId,
                        CreatedBy = userId,
                        IsActive = true
                    };
                    await _context.Locations.AddAsync(data);
                    await _context.SaveChangesAsync();
                    return new CommonResponseDto
                    {
                        Message = $"{model.Name} created successfully",
                        Data = null,
                        StatusCode = 200
                    };
                }
                else
                {
                    var data = await _context.Locations
                        .FirstOrDefaultAsync(a => a.Id == model.Id && a.ShopId == model.ShopId && a.IsActive);
                    if (data == null)
                    {
                        return new CommonResponseDto
                        {
                            Message = "Location not found.",
                            Data = null,
                            StatusCode = 404
                        };
                    }
                    var duplicate = await _context.Locations
                        .AnyAsync(a => a.Id != model.Id && a.ShopId == model.ShopId && a.Name.Trim() == model.Name.Trim() && a.IsActive);
                    if (duplicate)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"Another category with the name '{model.Name}' already exists.",
                            Data = null,
                            StatusCode = 400
                        };
                    }

                    data.Name = model.Name;
                    data.Address = model.Address;
                    data.ModifiedBy = userId;
                    _context.Locations.Update(data);
                    await _context.SaveChangesAsync();
                    return new CommonResponseDto
                    {
                        Message = $"{model.Name} updated successfully",
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

        public async Task<CommonResponseDto> GetWarehouseById(long warehouseId)
        {
            try
            {
                var warehouse = await _context.Locations
                    .Where(a => a.Id == warehouseId && a.IsActive)
                    .Select(a => new GetWarehouseByIdDto()
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Address = a.Address!,
                        ShopId = a.ShopId
                    })
                    .FirstOrDefaultAsync();
                if (warehouse == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Warehouse not found.",
                        Data = null,
                        StatusCode = 404
                    };
                }
                return new CommonResponseDto
                {
                    Data = warehouse,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(GetWarehouseById)}: {e.Message}", e);

            }

        }

        public async Task<PaginationDto<WarehouseLandingDataDto>> GetAllWarehousesByShopId(string search, long shopId, int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<WarehouseLandingDataDto>(
                    CommonHelper.StoredProcedureNames.GetAllLocationsPagination,
                    new { p0 = search, p1 = shopId, p2 = pageNo, p3 = pageSize },
                    pageNo,
                    pageSize
                );
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(GetAllWarehousesByShopId)}: {e.Message}", e);
            }

        }

        public async Task<CommonResponseDto> DeleteWarehouse(long warehouseId, Ulid userId)
        {
            try
            {
                var warehouse = await _context.Locations
                    .FirstOrDefaultAsync(a => a.Id == warehouseId && a.IsActive);
                if (warehouse == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Warehouse not found.",
                        Data = null,
                        StatusCode = 404
                    };
                }
                warehouse.IsActive = false;
                warehouse.ModifiedBy = userId;
                _context.Locations.Update(warehouse);
                await _context.SaveChangesAsync();
                return new CommonResponseDto
                {
                    Message = "Warehouse deleted successfully.",
                    Data = null,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(DeleteWarehouse)}: {e.Message}", e);
            }
        }
    }
}
