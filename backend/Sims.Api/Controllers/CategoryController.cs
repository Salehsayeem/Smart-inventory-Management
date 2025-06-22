using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.Dto.Category;
using Sims.Api.Helper;
using Sims.Api.IRepositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost("CreateOrUpdateCategory")]
        public async Task<CommonResponseDto> CreateOrUpdateCategory([FromBody] CreateOrUpdateCategoryDto model)
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

                return await _categoryRepository.CreateOrUpdateCategory(model, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetCategoryById")]
        public async Task<CommonResponseDto> GetCategoryById(long categoryId)
        {
            try
            {
                return await _categoryRepository.GetCategoryById(categoryId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetAllCategoriesByShopId")]
        public async Task<CommonResponseDto> GetAllCategoriesByShopId(string? search, long shopId, int pageNo, int pageSize)
        {
            try
            {
                var data = await _categoryRepository.GetAllCategoryByShopId(search, shopId, pageNo, pageSize);
                return new CommonResponseDto()
                {
                    Data = data,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("DeleteCategory")]
        public CommonResponseDto DeleteCategory(long categoryId)
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
                return _categoryRepository.DeleteCategory(categoryId, currentUserId).Result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
