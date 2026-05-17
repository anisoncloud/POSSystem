using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class OrderDto
    {
        public int Id {  get; set; }
        public string InvoiceNumber { get; set; } = default!;
        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }
        //Branch
        public int BranchId {  get; set; }
        public string BranchName { get; set; } = default!;

        //Table Restaurant only
        public int? TableId {  get; set; }
        public string? TableName {  get; set; }

        //Customer
        public string? CustomerName {  get; set; }
        public string? CustomerPhone {  get; set; }

        //Cashire
        public int CashierId { get; set; } = default!;
        public string CashierName { get; set;} = default!;

        //Financials
        public decimal SubTotal {  get; set; }
        public decimal DiscountAmount {  get; set; }
        public decimal DiscountValue {  get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal TaxAmount {  get; set; }
        public decimal TotalAmount {  get; set; }
        
        public string? Notes {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        //Line items and payments
        public List<OrderItemDto> Items { get; set; } = new();
        public List<PaymentDto> Payment { get; set; } = new();

        //Computed Helpers
        public decimal TotalPaid => Payment.Sum(p => p.Amount);
        public decimal ChangeGiven => TotalPaid - TotalAmount;
        public bool IsFullyPaid => TotalPaid >= TotalAmount;


        public string StatusBadgeColor => Status switch
        {
            OrderStatus.Pending => "warning",
            OrderStatus.Cooking => "info",
            OrderStatus.Served => "primary",
            OrderStatus.Cancelled => "danger",
            OrderStatus.Completed => "success"
        };
    }
}
