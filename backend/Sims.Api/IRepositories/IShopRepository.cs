using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sims.Api.Dto;
using Sims.Api.Dto.Shop;

namespace Sims.Api.IRepositories
{
    public interface IShopRepository
    {
        public Task<CommonResponseDto> CreateOrUpdateShop(CreateOrUpdateShopDto model, Ulid userId);
        public Task<CommonResponseDto> GetShopById(long shopId);
        public Task<CommonResponseDto> DeleteShop(long shopId, Ulid userId);
    }
}