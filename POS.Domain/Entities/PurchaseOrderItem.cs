using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class PurchaseOrderItem : BaseEntity
    {
        public int PurchaseOrderId {  get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = default!;
        public int ProductId {  get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost => Quantity*UnitCost;
    }
}
