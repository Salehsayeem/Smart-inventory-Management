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
        public async Task<CommonResponseDto> GetCategoryById(string categoryId)
        {
            try
            {
                return await _categoryRepository.GetCategoryById(CommonHelper.StringToUlidConverter(categoryId));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetAllCategoriesByShopId")]
        public CommonResponseDto GetAllCategoriesByShopId(string? search, long shopId, int pageNo, int pageSize)
        {
            try
            {
                return _categoryRepository.GetAllCategoryByShopId(search, shopId, pageNo, pageSize);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("DeleteCategory")]
        public CommonResponseDto DeleteCategory(string categoryId)
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
                return _categoryRepository.DeleteCategory(CommonHelper.StringToUlidConverter(categoryId), currentUserId).Result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
