using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public string PurchaseOrderNumber { get; set; } = default!;
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = default!;
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = default!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
        public string? Notes { get; set; }
        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}
