﻿using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Dto.Category;
using Sims.Api.Dto.Product;
using Sims.Api.Helper;
using Sims.Api.IRepositories;
using Sims.Api.Models;
using Sims.Api.StoredProcedure;
using static Sims.Api.Helper.CommonHelper;

namespace Sims.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly CallStoredProcedure _spCaller;

        public CategoryRepository(ApplicationDbContext context, CallStoredProcedure spCaller)
        {
            _context = context;
            _spCaller = spCaller;
        }

        public async Task<CommonResponseDto> CreateOrUpdateCategory(CreateOrUpdateCategoryDto model, Ulid userId)
        {
            try
            {
                if (model.Id == 0)
                {
                    var exists = await _context.Categories
                        .AnyAsync(a => a!.ShopId == model.ShopId && a.Name.Trim() == model.Name.Trim() && a.IsActive);
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
                    var category = await _context.Categories
                        .FirstOrDefaultAsync(a => a.Id == model.Id && a.ShopId == model.ShopId && a.IsActive);
                    if (category == null)
                    {
                        return new CommonResponseDto
                        {
                            Message = "Category not found.",
                            Data = null,
                            StatusCode = 404
                        };
                    }
                    var duplicate = await _context.Categories
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

        public async Task<CommonResponseDto> GetCategoryById(long categoryId)
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


        public async Task<PaginationDto<CategoryLandingDataDto>> GetAllCategoryByShopId(string search, long shopId, int pageNumber, int pageSize)
        {
            if (shopId <= 0)
                throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
            if (pageNumber < 1)
                throw new ArgumentException("Page number must be at least 1", nameof(pageNumber));
            if (pageSize < 1)
                throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

            return await _spCaller.CallPagedFunctionAsync<CategoryLandingDataDto>(
                StoredProcedureNames.GetCategoryPagination,
                new { p0 = search, p1 = shopId, p2 = pageNumber, p3 = pageSize },
                pageNumber,
                pageSize
            );
        }

        public async Task<CommonResponseDto> DeleteCategory(long categoryId, Ulid userId)
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
