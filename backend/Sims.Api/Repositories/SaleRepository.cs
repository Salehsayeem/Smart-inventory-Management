using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Helper;
using Sims.Api.IRepositories;
using Sims.Api.Models;
using Sims.Api.StoredProcedure;
using static Sims.Api.Helper.CommonHelper;

namespace Sims.Api.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly CallStoredProcedure _spCaller;
        public SaleRepository(ApplicationDbContext context, CallStoredProcedure spCaller)
        {
            _context = context;
            _spCaller = spCaller;
        }
        public async Task<CommonResponseDto> CreateSale(CreateSaleDto model, Ulid userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sale = new Sale
                {
                    ShopId = model.ShopId,
                    LocationId = model.LocationId,
                    Code = new Ulid(),
                    SaleDate = model.SaleDate,
                    CreatedBy = userId,
                    Status = nameof(SalesStatus.Completed)
                };

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                var saleItems = new List<SaleItem>();
                var stockMovements = new List<StockMovement>();

                foreach (var item in model.Items)
                {
                    var inventory = await _context.Inventories.FirstOrDefaultAsync(i =>
                        i.ShopId == model.ShopId && i.ProductId == item.ProductId && i.LocationId == model.LocationId && i.IsActive);

                    if (inventory == null || inventory.Quantity < item.QuantitySold)
                        throw new Exception("Insufficient stock for product ID: " + item.ProductId);

                    inventory.Quantity -= item.QuantitySold;
                    inventory.ModifiedBy = userId;

                    saleItems.Add(new SaleItem
                    {
                        SaleId = sale.Id,
                        ProductId = item.ProductId,
                        QuantitySold = item.QuantitySold,
                        UnitPrice = item.UnitPrice,
                        CreatedBy = userId
                    });

                    stockMovements.Add(new StockMovement
                    {
                        ShopId = model.ShopId,
                        ProductId = item.ProductId,
                        LocationId = model.LocationId,
                        QuantityChange = -item.QuantitySold,
                        Reason = $"Sale #{sale.Code} completed",
                        CreatedBy = userId
                    });
                }

                _context.SaleItems.AddRange(saleItems);
                _context.StockMovements.AddRange(stockMovements);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new CommonResponseDto
                {
                    StatusCode = 200,
                    Message = "Sale created successfully",
                    Data = new { SaleId = sale.Id, Code = sale.Code }
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception("Sale creation failed: " + e.Message);
            }
        }
        public async Task<PaginationDto<SaleLandingDataDto>> GetAllSales(string search, long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<SaleLandingDataDto>(
                    StoredProcedureNames.GetAllSaleOrdersPagination,
                    new { p0 = search, p1 = shopId, p2 = fromDate, p3 = endDate, p4 = pageNo, p5 = pageSize },
                    pageNo,
                    pageSize
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<GetSaleDto> GetSaleById(long id)
        {
            try
            {
                var order = await _context.Sales.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
                if (order != null)
                {
                    var items = await _context.SaleItems
                        .Where(a => a.SaleId == id && a.IsActive)
                        .ToListAsync();
                    var productIds = items.Select(i => i.ProductId).Distinct().ToList();
                    var products = await _context.Products
                        .Where(p => productIds.Contains(p.Id))
                        .ToDictionaryAsync(p => p.Id, p => p);

                    return new GetSaleDto()
                    {
                        Code = order.Code.ToString(),
                        Id = order.Id,
                        ShopId = order.ShopId,
                        SaleDate = order.SaleDate,
                        LocationName = (order.LocationId > 0 ? _context.Locations.FirstOrDefault(l => l.Id == order.LocationId)?.Name : "Unknown Location")!,
                        Items = items.Select(item => new GetSaleItemsDto()
                        {
                            Id = item.Id,
                            SaleId = item.SaleId,
                            ProductId = item.ProductId,
                            ProductName = products.TryGetValue(item.ProductId, out var product) ? product.Name : "Unknown Product",
                            QuantitySold = item.QuantitySold,
                            UnitPrice = item.UnitPrice
                        }).OrderBy(item => item.ProductName).ToList()
                    };
                }

                throw new Exception("Purchase order not found.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> UpdateSaleItems(List<CreateSaleItemDto> items, Ulid userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var addNewItem = new List<SaleItem>();
            var updateExistingItem = new List<SaleItem>();
            try
            {
                foreach (var item in items)
                {
                    if (item.Id == 0)
                    {
                        addNewItem.Add(new SaleItem
                        {
                            ProductId = item.ProductId,
                            QuantitySold = item.QuantitySold,
                            UnitPrice = item.UnitPrice,
                            CreatedBy = userId
                        });
                    }
                    else
                    {
                        var existingItem = await _context.SaleItems
                            .FirstOrDefaultAsync(a => a.Id == item.Id && a.IsActive);
                        if (existingItem == null) continue;
                        existingItem.ProductId = item.ProductId;
                        existingItem.QuantitySold = item.QuantitySold;
                        existingItem.UnitPrice = item.UnitPrice;
                        existingItem.ModifiedBy = userId;
                        updateExistingItem.Add(existingItem);
                    }
                }
                if (addNewItem.Count > 0)
                {
                    _context.SaleItems.AddRange(addNewItem);
                }
                if (updateExistingItem.Count > 0)
                {
                    _context.SaleItems.UpdateRange(updateExistingItem);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new CommonResponseDto
                {
                    StatusCode = 200,
                    Message = "Sale items updated successfully",
                    Data = null
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> UpdateSaleStatus(long saleId, int statusId, Ulid userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == saleId && s.IsActive);
                if (sale == null) throw new Exception("Sale not found.");

                var saleItems = await _context.SaleItems.Where(si => si.SaleId == saleId && si.IsActive).ToListAsync();
                if (!saleItems.Any()) throw new Exception("No sale items found.");

                var newStatus = (SalesStatus)statusId;
                switch (newStatus)
                {
                    case SalesStatus.Refunded:
                    case SalesStatus.PartialRefund:
                    case SalesStatus.Cancelled:
                        foreach (var item in saleItems)
                        {
                            var inventory = await _context.Inventories.FirstOrDefaultAsync(i =>
                                i.ShopId == sale.ShopId && i.ProductId == item.ProductId && i.LocationId == sale.LocationId && i.IsActive);

                            if (inventory != null)
                            {
                                inventory.Quantity += item.QuantitySold;
                                inventory.ModifiedBy = userId;

                                _context.StockMovements.Add(new StockMovement
                                {
                                    ShopId = sale.ShopId,
                                    ProductId = item.ProductId,
                                    LocationId = sale.LocationId,
                                    QuantityChange = item.QuantitySold,
                                    Reason = $"Inventory restored due to {newStatus} for Sale #{sale.Code}",
                                    CreatedBy = userId
                                });
                            }
                        }
                        break;

                    case SalesStatus.Delivered:
                    case SalesStatus.InProgress:
                    case SalesStatus.Failed:
                    case SalesStatus.Pending:
                        // No inventory adjustment required
                        break;

                    default:
                        throw new Exception("Unsupported sales status.");
                }

                sale.Status = newStatus.ToString();
                sale.ModifiedBy = userId;
                sale.ModifiedAt = DateTime.UtcNow;

                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CommonResponseDto
                {
                    StatusCode = 200,
                    Message = $"Sale status updated to {newStatus}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to update sale status: " + ex.Message);
            }
        }
    }
}
