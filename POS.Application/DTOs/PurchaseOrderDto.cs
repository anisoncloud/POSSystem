using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string PurchaseOrderNumber { get; set; } = default!;
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = default!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public PurchaseStatus Status { get; set; }
        public string? Notes { get; set; }
        public List<PurchaseOrderItemDto> Items { get; set; } = new();
    }
}
