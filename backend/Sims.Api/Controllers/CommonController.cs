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
        public CommonResponseDto AllCategoriesOfShopDdl(long shopId)
        {
            try
            {
                var response = _commonRepository.AllCategoriesOfShopDdl(shopId);
                return response;
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllCategoriesOfShopDdl)}: {e.Message}", e);
            }
        }

        [HttpGet("ProductsOfShopDdl")]
        public CommonResponseDto AllProductsOfShopDdl(long shopId)
        {
            try
            {
                var response = _commonRepository.AllProductsOfShopDdl(shopId);
                return response;
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllProductsOfShopDdl)}: {e.Message}", e);
            }
        }
    }
}
