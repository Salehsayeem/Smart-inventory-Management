using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.Dto.Product;
using Sims.Api.IRepositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }
        [HttpPost("CreateOrUpdateProduct")]
        public async Task<CommonResponseDto> CreateOrUpdateProduct([FromBody] CreateOrUpdateProductDto model)
        {
            try
            {
                var currentUserId = Ulid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == Ulid.Empty)
                {
                    return new CommonResponseDto()
                    {
                        Message = "Unauthorized: Please log in!",
                        Data = null,
                        StatusCode = 403,
                    };
                }
                return await _repository.CreateOrUpdateProduct(model, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetProductById")]
        public async Task<CommonResponseDto> GetProductById(long productId)
        {
            try
            {
                return await _repository.GetProductById(productId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetProductByShopId")]
        public CommonResponseDto GetProductByShopId(string? search, long shopId, int pageNo, int pageSize)
        {
            try
            {
                return _repository.GetProductByShopId(search, shopId, pageNo, pageSize);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetProductByCategoryId")]
        public CommonResponseDto GetProductByCategoryId(string? search, long shopId, long categoryId, int pageNo, int pageSize)
        {
            try
            {
                return _repository.GetProductByCategoryId(search, shopId, categoryId, pageNo, pageSize);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpDelete("DeleteProduct")]
        public async Task<CommonResponseDto> DeleteProduct(long productId)
        {
            try
            {
                var currentUserId = Ulid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == Ulid.Empty)
                {
                    return new CommonResponseDto()
                    {
                        Message = "Unauthorized: Please log in!",
                        Data = null,
                        StatusCode = 403,
                    };
                }
                return await _repository.DeleteProduct(productId, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
