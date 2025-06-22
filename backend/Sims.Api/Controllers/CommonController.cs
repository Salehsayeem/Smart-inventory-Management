using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.IRepositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonRepository _commonRepository;

        public CommonController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        [HttpGet("CategoriesOfShopDdl")]
        public async Task<CommonResponseDto> AllCategoriesOfShopDdl(long shopId, string? search)
        {
            try
            {
                var response = await _commonRepository.AllCategoriesOfShopDdl(shopId, search);
                return new CommonResponseDto()
                {
                    Data = response,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllCategoriesOfShopDdl)}: {e.Message}", e);
            }
        }

        [HttpGet("ProductsOfShopDdl")]
        public async Task<CommonResponseDto> AllProductsOfShopDdl(long shopId, string? search)
        {
            try
            {
                var response = await _commonRepository.AllProductsOfShopDdl(shopId, search);
                return new CommonResponseDto()
                {
                    Data = response,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllProductsOfShopDdl)}: {e.Message}", e);
            }
        }
        [HttpGet("WarehousesOfShopDdl")]
        public async Task<CommonResponseDto> AllWarehousesOfShopDdl(long shopId, string? search)
        {
            try
            {
                var response = await _commonRepository.AllWarehousesOfShopDdl(shopId, search);
                return new CommonResponseDto()
                {
                    Data = response,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllWarehousesOfShopDdl)}: {e.Message}", e);
            }
        }
    }
}
