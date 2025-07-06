using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Dto.Location;
using Sims.Api.Helper;
using Sims.Api.IRepositories;
using Sims.Api.Models;
using Sims.Api.StoredProcedure;

namespace Sims.Api.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly CallStoredProcedure _spCaller;

        public SupplierRepository(ApplicationDbContext context, CallStoredProcedure spCaller)
        {
            _context = context;
            _spCaller = spCaller;
        }

        public async Task<CommonResponseDto> CreateOrUpdateSupplier(CreateOrUpdateSupplierDto model, Ulid userId)
        {
            try
            {
                if (model.Id == 0)
                {
                    var exists = await _context.Supplier
                        .AnyAsync(a => a!.ShopId == model.ShopId && a.Phone!.Trim() == model.Phone!.Trim() && a.IsActive);
                    if (exists)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"Supplier '{model.Phone}' already exists.",
                            Data = null,
                            StatusCode = 400
                        };
                    }

                    var data = new Supplier()
                    {
                        Name = model.Name,
                        Phone = model.Phone,
                        Email = model.Email,
                        ContactPerson = model.ContactPerson,
                        Address = model.Address,
                        ShopId = model.ShopId,
                        CreatedBy = userId,
                        IsActive = true
                    };
                    await _context.Supplier.AddAsync(data);
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
                    var data = await _context.Supplier
                        .FirstOrDefaultAsync(a => a.Id == model.Id && a.ShopId == model.ShopId && a.IsActive);
                    if (data == null)
                    {
                        return new CommonResponseDto
                        {
                            Message = "Supplier not found.",
                            Data = null,
                            StatusCode = 404
                        };
                    }
                    var duplicate = await _context.Supplier
                        .AnyAsync(a => a.Id != model.Id && a.ShopId == model.ShopId && a.Phone!.Trim() == model.Phone!.Trim() && a.IsActive);
                    if (duplicate)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"Another Supplier with the phone '{model.Phone}' already exists.",
                            Data = null,
                            StatusCode = 400
                        };
                    }

                    data.Name = model.Name;
                    data.Phone = model.Phone;
                    data.Email = model.Email;
                    data.ContactPerson = model.ContactPerson;
                    data.Address = model.Address;
                    data.ModifiedBy = userId;
                    _context.Supplier.Update(data);
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

        public async Task<CommonResponseDto> GetSupplierById(long supplierId)
        {
            try
            {
                var data = await _context.Supplier
                    .Where(a => a.Id == supplierId && a.IsActive)
                    .Select(a => new GetSupplierDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Phone = a.Phone,
                        Email = a.Email,
                        ContactPerson = a.ContactPerson,
                        Address = a.Address,
                        ShopId = a.ShopId
                    })
                    .FirstOrDefaultAsync();
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

        public async Task<CommonResponseDto> DeleteSupplier(long supplierId, Ulid userId)
        {
            try
            {
                var data = await _context.Supplier
                    .FirstOrDefaultAsync(a => a.Id == supplierId && a.IsActive);
                if (data != null)
                {
                    data.IsActive = false;
                    data.ModifiedBy = userId;
                    _context.Supplier.Update(data);
                    await _context.SaveChangesAsync();
                    return new CommonResponseDto()
                    {
                        Message = $"{data.Name} deleted successfully",
                        Data = null,
                        StatusCode = 200
                    };

                }
                else
                {
                    return new CommonResponseDto()
                    {
                        Message = "Supplier not found",
                        Data = null,
                        StatusCode = 404
                    };
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaginationDto<GetSupplierDto>> GetAllSupplierPagination(string search, long shopId, int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<GetSupplierDto>(
                    CommonHelper.StoredProcedureNames.GetAllSuppliersPagination,
                    new { p0 = search, p1 = shopId, p2 = pageNo, p3 = pageSize },
                    pageNo,
                    pageSize
                );
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(GetAllSupplierPagination)}: {e.Message}", e);
            }
        }
    }
}
