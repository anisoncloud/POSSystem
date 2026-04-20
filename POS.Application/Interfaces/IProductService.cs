using POS.Application.DTOs;
using POS.Application.ViewModels;
using POS.Domain.Entities;
using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface IProductService
    {
        // ── CRUD ──────────────────────────────────────────────────────────────
        Task<ProductDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync(int branchId);
        Task<ProductListViewModel> GetProductListAsync(string? search, int? categoryId, int page, int pageSize, int branchId);
        Task<ProductDto> CreateProductAsync(CreateProductDto dto, int branchId);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto);
        Task DeleteProductAsync(int id);

        // ── Barcode / SKU ─────────────────────────────────────────────────────
        Task<Product?> GetByBarcodeAsync(string barcode);
        Task<Product?> GetBySkuAsync(string sku);
        Task RegenerateBarcode(int id);

        // ── Search ────────────────────────────────────────────────────────────
        Task<IEnumerable<ProductDto>> SearchAsync(string term, int branchId);

        // ── Categories ────────────────────────────────────────────────────────
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();

        // ── Stock ─────────────────────────────────────────────────────────────
        Task<IEnumerable<ProductDto>> GetLowStockAlertsAsync(int branchId);
        Task AdjustStockAsync(int productId, int qty, StockMovementType type, string? reference);
    }
}
