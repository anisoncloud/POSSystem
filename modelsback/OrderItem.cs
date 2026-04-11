using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId {  get; set; }
        public Order Order { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal LineTotal { get; set; }
        public string? Note { get; set; }
        public OrderItemStatus Status { get; set; } = OrderItemStatus.Pending;

    }
}
