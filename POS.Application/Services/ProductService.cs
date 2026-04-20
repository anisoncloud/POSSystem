using AutoMapper;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Application.ViewModels;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IBarcodeService _barcodeService;
        private readonly IMapper _mapper;

        public ProductService(
            IUnitOfWork uow,
            IBarcodeService barcodeService,
            IMapper mapper)
        {
            _uow = uow;
            _barcodeService = barcodeService;
            _mapper = mapper;
        }

        // ── Get by ID ─────────────────────────────────────────────────────────
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _uow.Products.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

            return _mapper.Map<ProductDto>(product);
        }

        // ── Get all for a branch ──────────────────────────────────────────────
        public async Task<IEnumerable<ProductDto>> GetAllAsync(int branchId)
        {
            var products = await _uow.Products.FindAsync(p => p.BranchId == branchId && p.IsActive);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        // ── Paged list for Product/Index view ─────────────────────────────────
        public async Task<ProductListViewModel> GetProductListAsync(
            string? search,
            int? categoryId,
            int page,
            int pageSize,
            int branchId)
        {
            IEnumerable<Product> products;

            if (!string.IsNullOrWhiteSpace(search))
                products = await _uow.Products.SearchAsync(search, branchId);
            else if (categoryId.HasValue)
                products = await _uow.Products.GetByCategoryAsync(categoryId.Value);
            else
                products = await _uow.Products.FindAsync(p => p.BranchId == branchId && p.IsActive);

            var totalCount = products.Count();
            var paged = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ProductListViewModel
            {
                Products = _mapper.Map<IEnumerable<ProductDto>>(paged),
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Search = search,
                CategoryId = categoryId,
            };
        }

        // ── Create ────────────────────────────────────────────────────────────
        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto, int branchId)
        {
            var sku = GenerateSku(dto.Name, dto.CategoryIds.FirstOrDefault());
            var barcode = _barcodeService.GenerateEan13();

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                SKU = sku,
                Barcode = barcode,
                PurchasePrice = dto.PurchasePrice,
                SalePrice = dto.SalePrice,
                TaxRate = dto.TaxRate,
                StockQuantity = dto.InitialStock,
                LowStockThreshold = dto.LowStockThreshold,
                UnitType = dto.UnitType,
                BranchId = branchId,
                IsActive = true,
            };

            foreach (var catId in dto.CategoryIds)
                product.ProductCategories.Add(new ProductCategory { CategoryId = catId });

            await _uow.Products.AddAsync(product);
            await _uow.CommitAsync();

            // Record initial stock movement after product is saved (needs product.Id)
            if (dto.InitialStock > 0)
            {
                await _uow.StockMovements.RecordMovementAsync(
                    product.Id,
                    dto.InitialStock,
                    StockMovementType.Adjustment,
                    "Initial stock");

                await _uow.CommitAsync();
            }

            return _mapper.Map<ProductDto>(product);
        }

        // ── Update ────────────────────────────────────────────────────────────
        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _uow.Products.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.PurchasePrice = dto.PurchasePrice;
            product.SalePrice = dto.SalePrice;
            product.TaxRate = dto.TaxRate;
            product.LowStockThreshold = dto.LowStockThreshold;
            product.UnitType = dto.UnitType;
            product.IsActive = dto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            // Sync categories
            product.ProductCategories.Clear();
            foreach (var catId in dto.CategoryIds)
                product.ProductCategories.Add(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = catId
                });

            await _uow.Products.UpdateAsync(product);
            await _uow.CommitAsync();

            return _mapper.Map<ProductDto>(product);
        }

        // ── Delete (soft) ─────────────────────────────────────────────────────
        public async Task DeleteProductAsync(int id)
        {
            await _uow.Products.DeleteAsync(id);
            await _uow.CommitAsync();
        }

        // ── Barcode lookup ────────────────────────────────────────────────────
        public async Task<Product?> GetByBarcodeAsync(string barcode)
            => await _uow.Products.GetByBarcodeAsync(barcode);

        public async Task<Product?> GetBySkuAsync(string sku)
            => await _uow.Products.GetBySkuAsync(sku);

        public async Task RegenerateBarcode(int id)
        {
            var product = await _uow.Products.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

            product.Barcode = _barcodeService.GenerateEan13();
            product.UpdatedAt = DateTime.UtcNow;

            await _uow.Products.UpdateAsync(product);
            await _uow.CommitAsync();
        }

        // ── Search ────────────────────────────────────────────────────────────
        public async Task<IEnumerable<ProductDto>> SearchAsync(string term, int branchId)
        {
            var products = await _uow.Products.SearchAsync(term, branchId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        // ── Categories ────────────────────────────────────────────────────────
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _uow.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        // ── Low stock alerts ──────────────────────────────────────────────────
        public async Task<IEnumerable<ProductDto>> GetLowStockAlertsAsync(int branchId)
        {
            var products = await _uow.Products.GetLowStockProductsAsync(branchId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        // ── Stock adjustment ──────────────────────────────────────────────────
        public async Task AdjustStockAsync(
            int productId,
            int qty,
            StockMovementType type,
            string? reference)
        {
            var product = await _uow.Products.GetByIdAsync(productId)
                ?? throw new KeyNotFoundException($"Product with ID {productId} not found.");

            var newQty = product.StockQuantity + qty;
            if (newQty < 0)
                throw new InvalidOperationException(
                    $"Insufficient stock for '{product.Name}'. Available: {product.StockQuantity}, Requested: {Math.Abs(qty)}");

            product.StockQuantity = newQty;
            product.UpdatedAt = DateTime.UtcNow;

            await _uow.Products.UpdateAsync(product);
            await _uow.StockMovements.RecordMovementAsync(productId, qty, type, reference);
            await _uow.CommitAsync();
        }

        // ── Private helpers ───────────────────────────────────────────────────
        private static string GenerateSku(string name, int categoryId)
        {
            var prefix = new string(
                name.Where(char.IsLetter).Take(3).ToArray()).ToUpper();

            if (prefix.Length < 3)
                prefix = prefix.PadRight(3, 'X');

            return $"{prefix}-{categoryId:D3}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds() % 100000}";
        }
    }
}
