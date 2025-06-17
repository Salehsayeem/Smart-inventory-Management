using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Dto.Product;
using Sims.Api.Helper;
using Sims.Api.IRepositories;
using Sims.Api.Models;
using Sims.Api.StoredProcedure;

namespace Sims.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _settings;

        public ProductRepository(ApplicationDbContext context, AppSettings settings)
        {
            _context = context;
            _settings = settings;
        }
        public async Task<CommonResponseDto> CreateOrUpdateProduct(CreateOrUpdateProductDto model, Ulid userId)
        {
            try
            {
                if (model.Id == 0)
                {
                    var exists = await _context.Products
                        .AnyAsync(a => a!.ShopId == model.ShopId && a.CategoryId == model.CategoryId && a.Name.Trim() == model.Name.Trim() && a.IsActive);
                    if (exists)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"Category '{model.Name}' already exists.",
                            Data = null,
                            StatusCode = 400
                        };
                    }

                    var data = new Product()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Sku = model.Sku,
                        CategoryId = model.CategoryId,
                        UnitPrice = model.UnitPrice,
                        ShopId = model.ShopId,
                        CreatedBy = userId,
                        IsActive = true
                    };
                    await _context.Products.AddAsync(data);
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
                    var data = await _context.Products
                        .FirstOrDefaultAsync(a => a.Id == model.Id && a.ShopId == model.ShopId && a.CategoryId== model.CategoryId && a.IsActive);
                    if (data == null)
                    {
                        return new CommonResponseDto
                        {
                            Message = "Category not found.",
                            Data = null,
                            StatusCode = 404
                        };
                    }
                    var duplicate = await _context.Products
                        .AnyAsync(a => a.Id != model.Id && a.ShopId == model.ShopId && a.CategoryId== model.CategoryId && a.Name.Trim() == model.Name.Trim() && a.IsActive);
                    if (duplicate)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"Another category with the name '{model.Name}' already exists.",
                            Data = null,
                            StatusCode = 400
                        };
                    }

                    data.Name = model.Name;
                    data.Description = model.Description;
                    data.Sku = model.Sku;
                    data.UnitPrice = model.UnitPrice;
                    data.ModifiedBy = userId;
                    _context.Products.Update(data);
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

        public async Task<CommonResponseDto> GetProductById(long productId)
        {
            try
            {
                var data = await _context.Products
                    .Where(a => a.Id == productId && a.IsActive)
                    .Select(a => new GetProductByIdDto
                    {
                        Id = a.Id,
                        ShopId = a.ShopId,
                        Name = a.Name,
                        Sku = a.Sku,
                        CategoryId = a.CategoryId,
                        CategoryName = _context.Categories
                            .Where(c => c.Id == a.CategoryId && c.IsActive)
                            .Select(c => c.Name)
                            .FirstOrDefault(),
                        Description = a.Description,
                        UnitPrice = a.UnitPrice
                    })
                    .FirstOrDefaultAsync();

                return new CommonResponseDto()
                {
                    Data = data,
                    Message = "",
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CommonResponseDto GetProductByShopId(string search, long shopId, int pageNo, int pageSize)
        {
            try
            {
                CallStoredProcedure sp = new CallStoredProcedure();
                var data = sp.AllProductsPagination(search, shopId, pageNo, pageSize, _settings.ConnectionString);
                return new CommonResponseDto()
                {
                    Data = data,
                    Message = "",
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CommonResponseDto GetProductByCategoryId(string search, long shopId, long categoryId, int pageNo, int pageSize)
        {
            try
            {
                CallStoredProcedure sp = new CallStoredProcedure();
                var data = sp.AllProductsByCategoryPagination(search, shopId,categoryId, pageNo, pageSize, _settings.ConnectionString);
                return new CommonResponseDto()
                {
                    Data = data,
                    Message = "",
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> DeleteProduct(long productId, Ulid userId)
        {
            try
            {
                var data = await _context.Products
                    .FirstOrDefaultAsync(a => a.Id == productId && a.IsActive);
                if (data == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Product not found.",
                        Data = null,
                        StatusCode = 404
                    };
                }
                data.IsActive = false;
                data.ModifiedBy = userId;
                _context.Products.Update(data);
                await _context.SaveChangesAsync();

                return new CommonResponseDto
                {
                    Message = "Product deleted successfully.",
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
