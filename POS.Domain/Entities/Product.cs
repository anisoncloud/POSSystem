using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string SKU {  get; set; }= default!;
        public string Barcode { get; set; } = default!;
        public decimal PurchasePrice {  get; set; }
        public decimal SalePrice {  get; set; }
        public decimal TaxRate { get; set; } = 0;
        public int StockQuantity {  get; set; }
        public int LowStockThreshold {  get; set; } = 10;
        public UnitType TnitType { get; set; } = UnitType.Piece;
        public string? ImageUrl {  get; set; }
        public bool IsActive { get; set; } = true;
        public int BranchId {  get; set; }
        public Branch Branch { get; set; } = default!;
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set;} = new List<PurchaseOrderItem>();
        //public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        //public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    }
}
