using POS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<StockMovementDto>> GetStockHistoryAsync(int productId);
        Task<IEnumerable<StockMovementDto>> GetAllMovementsAsync(int branchId);
        Task AdjustStockAsync(StockAdjustmentDto dto);
        Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersAsync(int branchId);
        Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto, int branchId);
        Task ReceivePurchaseOrderAsync(int purchaseOrderId);
        Task<IEnumerable<SupplierDto>> GetSuppliersAsync();
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
    }
}
