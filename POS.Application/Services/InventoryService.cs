using AutoMapper;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public InventoryService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StockMovementDto>> GetStockHistoryAsync(int productId)
        {
            var movements = await _uow.StockMovements.GetProductHistoryAsync(productId);
            return _mapper.Map<IEnumerable<StockMovementDto>>(movements);
        }

        public async Task<IEnumerable<StockMovementDto>> GetAllMovementsAsync(int branchId)
        {
            var movements = await _uow.StockMovements.FindAsync(
                s => s.Product.BranchId == branchId);
            return _mapper.Map<IEnumerable<StockMovementDto>>(
                movements.OrderByDescending(m => m.CreatedAt));
        }

        public async Task AdjustStockAsync(StockAdjustmentDto dto)
        {
            var product = await _uow.Products.GetByIdAsync(dto.ProductId)
                ?? throw new KeyNotFoundException($"Product {dto.ProductId} not found");

            int newQty = product.StockQuantity + dto.Quantity;

            if (newQty < 0)
                throw new InvalidOperationException(
                    $"Insufficient stock for '{product.Name}'. " +
                    $"Available: {product.StockQuantity}");

            product.StockQuantity = newQty;
            product.UpdatedAt = DateTime.UtcNow;

            await _uow.Products.UpdateAsync(product);
            await _uow.StockMovements.RecordMovementAsync(
                dto.ProductId, dto.Quantity,
                dto.MovementType, dto.Reference);

            await _uow.CommitAsync();
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersAsync(int branchId)
        {
            var orders = await _uow.PurchaseOrders.FindAsync(p => p.BranchId == branchId);
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(orders);
        }

        public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(
            CreatePurchaseOrderDto dto, int branchId)
        {
            var poNumber = await GeneratePurchaseOrderNumberAsync(branchId);

            var po = new PurchaseOrder
            {
                PurchaseOrderNumber = poNumber,
                SupplierId = dto.SupplierId,
                BranchId = branchId,
                OrderDate = dto.OrderDate,
                Notes = dto.Notes,
                Status = PurchaseStatus.Pending,
            };

            foreach (var item in dto.Items)
            {
                var product = await _uow.Products.GetByIdAsync(item.ProductId)
                    ?? throw new KeyNotFoundException($"Product {item.ProductId} not found");

                po.Items.Add(new PurchaseOrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitCost,
                });
            }

            po.TotalAmount = po.Items.Sum(i => i.Quantity * i.UnitCost);

            await _uow.PurchaseOrders.AddAsync(po);
            await _uow.CommitAsync();

            return _mapper.Map<PurchaseOrderDto>(po);
        }

        public async Task ReceivePurchaseOrderAsync(int purchaseOrderId)
        {
            var po = await _uow.PurchaseOrders.GetByIdAsync(purchaseOrderId)
                ?? throw new KeyNotFoundException("Purchase order not found");

            if (po.Status == PurchaseStatus.Received)
                throw new InvalidOperationException("Purchase order already received");

            foreach (var item in po.Items)
            {
                var product = await _uow.Products.GetByIdAsync(item.ProductId)
                    ?? throw new KeyNotFoundException($"Product {item.ProductId} not found");

                product.StockQuantity += item.Quantity;
                product.UpdatedAt = DateTime.UtcNow;

                await _uow.Products.UpdateAsync(product);
                await _uow.StockMovements.RecordMovementAsync(
                    item.ProductId, item.Quantity,
                    StockMovementType.Purchase, po.PurchaseOrderNumber);
            }

            po.Status = PurchaseStatus.Received;
            await _uow.PurchaseOrders.UpdateAsync(po);
            await _uow.CommitAsync();
        }

        public async Task<IEnumerable<SupplierDto>> GetSuppliersAsync()
        {
            var suppliers = await _uow.Suppliers.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto)
        {
            var supplier = _mapper.Map<Supplier>(dto);
            await _uow.Suppliers.AddAsync(supplier);
            await _uow.CommitAsync();
            return _mapper.Map<SupplierDto>(supplier);
        }

        private async Task<string> GeneratePurchaseOrderNumberAsync(int branchId)
        {
            var today = DateTime.UtcNow;
            var prefix = $"PO-{branchId:D2}-{today:yyyyMMdd}-";
            var all = await _uow.PurchaseOrders.FindAsync(
                p => p.PurchaseOrderNumber.StartsWith(prefix));
            int seq = all.Any()
                ? all.Max(p => int.Parse(p.PurchaseOrderNumber.Split('-').Last())) + 1
                : 1;
            return $"{prefix}{seq:D4}";
        }
    }
}
