using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string SKU {  get; set; }= default!;
        public string Barcode { get; set; } = default!;
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal TaxRate {  get; set; }
        public int StockQuantity {  get; set; }
        public int LowStockThreshold {  get; set; }
        public UnitType UnitType { get; set; }
        public bool IsActive {  get; set; }
        public string? ImageUrl { get; set; } 
        public int BranchId { get; set; }
        public bool IsLowStock => StockQuantity<=LowStockThreshold;
        public List<CategoryDto> Categories { get; set; } = new();

    }
}
