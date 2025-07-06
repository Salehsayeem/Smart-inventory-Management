using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Dto.Category;
using Sims.Api.Helper;
using Sims.Api.IRepositories;
using Sims.Api.Models;
using Sims.Api.StoredProcedure;
using static Sims.Api.Helper.CommonHelper;

namespace Sims.Api.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly CallStoredProcedure _spCaller;
        public PurchaseRepository(ApplicationDbContext context, CallStoredProcedure spCaller)
        {
            _context = context;
            _spCaller = spCaller;
        }
        public async Task<CommonResponseDto> CreatePurchaseOrder(CreatePurchaseDto model, Ulid userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new PurchaseOrder();
                var items = new List<PurchaseOrderItem>();
                order.ShopId = model.ShopId;
                order.SupplierId = model.SupplierId;
                order.LocationId = model.LocationId;
                order.Code = Ulid.NewUlid();
                order.OrderDate = model.OrderDate;
                order.Status = nameof(PurchaseOrderStatus.Pending);
                order.CreatedBy = userId;

                foreach (var item in model.Items)
                {
                    items.Add(new PurchaseOrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        CreatedBy = userId
                    });
                }

                _context.PurchaseOrders.Add(order);
                await _context.SaveChangesAsync();
                foreach (var item in items)
                {
                    item.PurchaseOrderId = order.Id;
                }
                _context.PurchaseOrderItems.AddRange(items);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new CommonResponseDto
                {
                    StatusCode = 200,
                    Message = "Purchase order created successfully",
                    Data = new { OrderId = order.Id, Code = order.Code }
                };

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error creating purchase order: {e.Message}", e);
            }
        }

        public async Task<PaginationDto<PurchaseOrderLandingDataDto>> GetAllPurchaseOrders(string search, long shopId, DateOnly fromDate, DateOnly endDate,  int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<PurchaseOrderLandingDataDto>(
                    StoredProcedureNames.GetAllPurchaseOrdersPagination,
                    new { p0 = search, p1 = shopId, p2 = fromDate, p3 = endDate,  p4 = pageNo, p5 = pageSize },
                    pageNo,
                    pageSize
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<GetPurchaseOrderDto> GetPurchaseOrderById(long id)
        {
            try
            {
                var order = await _context.PurchaseOrders.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
                if (order != null)
                {
                    var items = await _context.PurchaseOrderItems
                        .Where(a => a.PurchaseOrderId == id && a.IsActive)
                        .ToListAsync();
                    var productIds = items.Select(i => i.ProductId).Distinct().ToList();
                    var products = await _context.Products
                        .Where(p => productIds.Contains(p.Id))
                        .ToDictionaryAsync(p => p.Id, p => p);

                    return new GetPurchaseOrderDto
                    {
                        Code = order.Code.ToString(),
                        Id = order.Id,
                        ShopId = order.ShopId,
                        OrderDate = order.OrderDate,
                        SupplierDetails = order.SupplierId > 0
                            ? _context.Supplier.FirstOrDefault(s => s.Id == order.SupplierId)?.Name ?? "Unknown Supplier"
                            : "No Supplier",
                        LocationName = (order.LocationId > 0 ? _context.Locations.FirstOrDefault(l => l.Id == order.LocationId)?.Name : "Unknown Location")!,
                        Items = items.Select(item => new GetPurchaseItemsDto
                        {
                            Id = item.Id,
                            PurchaseOrderId = item.PurchaseOrderId,
                            ProductId = item.ProductId,
                            ProductName = products.TryGetValue(item.ProductId, out var product) ? product.Name : "Unknown Product",
                            Quantity = item.Quantity,
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

        public async Task<CommonResponseDto> UpdatePurchaseOrderItems(List<CreatePurchaseItemDto> items, Ulid userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var addNewItem = new List<PurchaseOrderItem>();
            var updateExistingItem = new List<PurchaseOrderItem>();
            try
            {
                foreach (var item in items)
                {
                    if (item.Id == 0)
                    {
                        addNewItem.Add(new PurchaseOrderItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            CreatedBy = userId
                        });
                    }
                    else
                    {
                        var existingItem = await _context.PurchaseOrderItems
                            .FirstOrDefaultAsync(a => a.Id == item.Id && a.IsActive);
                        if (existingItem == null) continue;
                        existingItem.ProductId = item.ProductId;
                        existingItem.Quantity = item.Quantity;
                        existingItem.UnitPrice = item.UnitPrice;
                        existingItem.ModifiedBy = userId;
                        updateExistingItem.Add(existingItem);
                    }
                }
                if (addNewItem.Count > 0)
                {
                    _context.PurchaseOrderItems.AddRange(addNewItem);
                }
                if (updateExistingItem.Count > 0)
                {
                    _context.PurchaseOrderItems.UpdateRange(updateExistingItem);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new CommonResponseDto
                {
                    StatusCode = 200,
                    Message = "Purchase order items updated successfully",
                    Data = null
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> UpdatePurchaseOrderStatus(long id, int statusId, Ulid userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.PurchaseOrders
                    .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);

                if (order == null)
                    throw new Exception("Purchase order not found");

                var orderItems = await _context.PurchaseOrderItems
                    .Where(a => a.PurchaseOrderId == id && a.IsActive)
                    .ToListAsync();

                if (orderItems.Count == 0)
                    throw new Exception("No items found in this purchase order.");

                var inventoriesToUpdate = new List<Inventory>();
                var inventoriesToInsert = new List<Inventory>();
                var stockMovements = new List<StockMovement>();

                var status = (PurchaseOrderStatus)statusId;
                var locationId = 0;

                switch (status)
                {
                    case PurchaseOrderStatus.Received:
                        foreach (var item in orderItems)
                        {
                            var inventory = await _context.Inventories.FirstOrDefaultAsync(a =>
                                a.ProductId == item.ProductId &&
                                a.ShopId == order.ShopId &&
                                a.LocationId == locationId &&
                                a.IsActive);

                            if (inventory != null)
                            {
                                inventory.Quantity += item.Quantity;
                                inventory.ModifiedBy = userId;
                                inventory.ModifiedAt = DateTime.UtcNow;
                                inventoriesToUpdate.Add(inventory);
                            }
                            else
                            {
                                inventoriesToInsert.Add(new Inventory
                                {
                                    ProductId = item.ProductId,
                                    ShopId = order.ShopId,
                                    LocationId = locationId,
                                    Quantity = item.Quantity,
                                    RestockThreshold = 10,
                                    CreatedBy = userId,
                                    CreatedAt = DateTime.UtcNow
                                });
                            }

                            stockMovements.Add(new StockMovement
                            {
                                ShopId = order.ShopId,
                                ProductId = item.ProductId,
                                LocationId = locationId,
                                QuantityChange = item.Quantity,
                                Reason = $"Purchase order #{order.Code} received",
                                CreatedBy = userId,
                                CreatedAt = DateTime.UtcNow
                            });
                        }

                        break;

                    case PurchaseOrderStatus.Cancelled:
                        if (order.Status == nameof(PurchaseOrderStatus.Received))
                            throw new Exception("Cannot cancel an already received order.");
                        // No inventory changes, just status update
                        break;

                    case PurchaseOrderStatus.Returned:
                        if (order.Status != nameof(PurchaseOrderStatus.Received))
                            throw new Exception("Only received orders can be returned.");

                        foreach (var item in orderItems)
                        {
                            var inventory = await _context.Inventories.FirstOrDefaultAsync(a =>
                                a.ProductId == item.ProductId &&
                                a.ShopId == order.ShopId &&
                                a.LocationId == locationId &&
                                a.IsActive);

                            if (inventory == null || inventory.Quantity < item.Quantity)
                                throw new Exception($"Not enough inventory to return Product ID {item.ProductId}");

                            inventory.Quantity -= item.Quantity;
                            inventory.ModifiedBy = userId;
                            inventory.ModifiedAt = DateTime.UtcNow;
                            inventoriesToUpdate.Add(inventory);

                            stockMovements.Add(new StockMovement
                            {
                                ShopId = order.ShopId,
                                ProductId = item.ProductId,
                                LocationId = locationId,
                                QuantityChange = -item.Quantity,
                                Reason = $"Purchase order #{order.Code} returned",
                                CreatedBy = userId,
                                CreatedAt = DateTime.UtcNow
                            });
                        }

                        break;

                    default:
                        throw new Exception("Invalid purchase order status");
                }

                // Update order status and audit
                order.Status = status.ToString();
                order.ModifiedBy = userId;
                order.ModifiedAt = DateTime.UtcNow;
                _context.PurchaseOrders.Update(order);

                if (inventoriesToInsert.Count > 0)
                    _context.Inventories.AddRange(inventoriesToInsert);

                if (inventoriesToUpdate.Count > 0)
                    _context.Inventories.UpdateRange(inventoriesToUpdate);

                if (stockMovements.Count > 0)
                    _context.StockMovements.AddRange(stockMovements);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CommonResponseDto
                {
                    StatusCode = 200,
                    Message = $"Purchase order updated to '{status}' successfully.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
