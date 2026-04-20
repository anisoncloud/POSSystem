using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class SalesReportDto
    {
        public DateTime Date { get; set; }
        public int TotalOrders { get; set; }
        public decimal GrossSales { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal NetSales { get; set; }
        public decimal CashSales { get; set; }
        public decimal CardSales { get; set; }
        public decimal MobileSales { get; set; }
        public List<TopProductDto> TopProducts { get; set; } = new();
    }
}
