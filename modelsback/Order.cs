using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string InvoiceNumber { get; set; }= default!;
        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int TableId { get; set; }
        public Table? Table { get; set; }
        public int BranchId { get; set; }
        public Branch Branch {  get; set; }=default!;
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone {  get; set; }
        public decimal SubTotal {  get; set; }
        public decimal DiscountAmount { get; set; }
        public DiscountType DiscountType { get; set; } = DiscountType.None;
        public decimal DiscountValue {  get; set; }
        public decimal TaxAmount {  get; set; }
        public decimal TotalAmount {  get; set; }
        public string? Notes {  get; set; }
        public string CashierId { get; set; } = default!;
        public AppUser Cashier { get; set; } = default!;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
