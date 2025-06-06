using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Dto.Shop;
using Sims.Api.IRepositories;
using Sims.Api.Models;

namespace Sims.Api.Repositories
{
    public class ShopRepository : IShopRepository
    {        private readonly ApplicationDbContext _context;

        public ShopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommonResponseDto> CreateOrUpdateShop(CreateOrUpdateShopDto model, Ulid userId)
        {
            try
            {
                if (model.Id == 0)
                {
                    var data = new Shop()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Address = model.Address,
                        CreatedBy = userId
                    };
                    await _context.Shops.AddAsync(data);
                }
                else
                {
                    var existingData = await _context.Shops.FindAsync(model.Id);
                    if (existingData != null)
                    {
                        existingData.Name = model.Name;
                        existingData.Address = model.Address;
                        existingData.ModifiedBy = userId;
                        existingData.ModifiedAt = DateTime.UtcNow;
                        _context.Shops.Update(existingData);
                    }
                }
                await _context.SaveChangesAsync();
                return new CommonResponseDto()
                {
                    Message = model.Id == 0? $"{model.Name}  created successfully": $"{model.Name} updated successfully",
                    Data = null,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> GetShopById(long shopId)
        {
            try
            {
                var shop = await _context.Shops.FindAsync(shopId);
                if (shop == null)
                {
                    return new CommonResponseDto()
                    {
                        Message = "Shop not found",
                        Data = null,
                        StatusCode = 404,
                    };
                }
                return new CommonResponseDto()
                {
                    Message = "Shop retrieved successfully",
                    Data = shop,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> DeleteShop(long shopId , Ulid userId)
        {
            try
            {
                var data = await _context.Shops.FindAsync(shopId);
                if (data == null)
                {
                    return new CommonResponseDto()
                    {
                        Message = "Shop not found",
                        Data = null,
                        StatusCode = 404,
                    };
                }
                data.IsActive = false;
                data.ModifiedBy = userId;
                data.ModifiedAt = DateTime.UtcNow;
                _context.Shops.Update(data);
                await _context.SaveChangesAsync();
                return new CommonResponseDto()
                {
                    Message = "Shop deleted successfully",
                    Data = null,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}