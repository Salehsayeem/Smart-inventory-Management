using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Dto.Category;
using Sims.Api.Helper;
using Sims.Api.IRepositories;
using Sims.Api.Models;
using Sims.Api.StoredProcedure;

namespace Sims.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _settings;

        public CategoryRepository(ApplicationDbContext context, AppSettings settings)
        {
            _context = context;
            _settings = settings;
        }

        public async Task<CommonResponseDto> CreateOrUpdateCategory(CreateOrUpdateCategoryDto model, Ulid userId)
        {
            try
            {
                Ulid id = new Ulid();
                id = model.Id.Equals(string.Empty) ? Ulid.Empty : CommonHelper.StringToUlidConverter(model.Id);
                if (id == Ulid.Empty)
                {
                    // Check for duplicate active category name in the same shop
                    var exists = await _context.Categories
                        .AnyAsync(a => a.ShopId == model.ShopId && a.Name.Trim() == model.Name.Trim() && a.IsActive);
                    if (exists)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"Category '{model.Name}' already exists.",
                            Data = null,
                            StatusCode = 400
                        };
                    }

                    var data = new Category
                    {
                        Id = Ulid.NewUlid(),
                        Name = model.Name,
                        Description = model.Description,
                        ShopId = model.ShopId,
                        CreatedBy = userId,
                        IsActive = true
                    };
                    await _context.Categories.AddAsync(data);
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
                    // Update existing category
                    var category = await _context.Categories
                        .FirstOrDefaultAsync(a => a.Id == id && a.ShopId == model.ShopId && a.IsActive);
                    if (category == null)
                    {
                        return new CommonResponseDto
                        {
                            Message = "Category not found.",
                            Data = null,
                            StatusCode = 404
                        };
                    }

                    // Optional: Check for duplicate name in other categories
                    var duplicate = await _context.Categories
                        .AnyAsync(a => a.Id != id && a.ShopId == model.ShopId && a.Name.Trim() == model.Name.Trim() && a.IsActive);
                    if (duplicate)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"Another category with the name '{model.Name}' already exists.",
                            Data = null,
                            StatusCode = 400
                        };
                    }

                    category.Name = model.Name;
                    category.Description = model.Description;
                    category.ModifiedBy = userId;
                    _context.Categories.Update(category);
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

        public async Task<CommonResponseDto> GetCategoryById(Ulid categoryId)
        {
            try
            {
                var category = await _context.Categories
                    .Where(a => a.Id == categoryId && a.IsActive)
                    .Select(a => new GetCategoryByIdDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Description = a.Description!,
                        ShopId = a.ShopId
                    })
                    .FirstOrDefaultAsync();
                if (category == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Category not found.",
                        Data = null,
                        StatusCode = 404
                    };
                }
                return new CommonResponseDto
                {
                    Message = "Category retrieved successfully.",
                    Data = category,
                    StatusCode = 200
                };
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CommonResponseDto GetAllCategoryByShopId(string? search, long shopId, int pageNo, int pageSize)
        {
            try
            {
                CallStoredProcedure sp = new CallStoredProcedure();
                var data = sp.CategoryLandingPagination(search, shopId, pageNo, pageSize, _settings.ConnectionString);
                return new CommonResponseDto()
                {
                    Message = "",
                    Data = data,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> DeleteCategory(Ulid categoryId, Ulid userId)
        {
            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(a => a.Id == categoryId && a.IsActive);
                if (category == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Category not found.",
                        Data = null,
                        StatusCode = 404
                    };
                }
                category.IsActive = false;
                category.ModifiedBy = userId;
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return new CommonResponseDto
                {
                    Message = "Category deleted successfully.",
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
