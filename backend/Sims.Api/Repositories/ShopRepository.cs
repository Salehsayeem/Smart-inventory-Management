using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                var shop = new Shop()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Address = model.Address,
                };
                if (model.Id == 0)
                {
                    shop.CreatedBy = userId;
                    await _context.Shops.AddAsync(shop);
                }
                else
                {
                    shop.ModifiedBy = userId;
                    _context.Shops.Update(shop);
                }
                await _context.SaveChangesAsync();
                return new CommonResponseDto()
                {
                    Message = "Shop created/updated successfully",
                    Data = shop,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> GetShopById(GetShopByIdDto model)
        {
            try
            {
                var shop = await _context.Shops.FindAsync(model.Id);
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