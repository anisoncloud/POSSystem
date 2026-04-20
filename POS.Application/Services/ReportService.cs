using POS.Application.Interfaces;
using POS.Domain.Enums;
using POS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _uow;

        public ReportService(IUnitOfWork uow) => _uow = uow;

        public async Task<SalesReportDto> GetDailySalesAsync(DateTime date, int branchId)
        {
            var from = date.Date;
            var to = from.AddDays(1).AddTicks(-1);
            return await BuildSalesReportAsync(from, to, branchId);
        }

        public async Task<SalesReportDto> GetMonthlySalesAsync(
            int year, int month, int branchId)
        {
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1).AddTicks(-1);
            return await BuildSalesReportAsync(from, to, branchId);
        }

        public async Task<ProfitLossDto> GetProfitLossAsync(
            DateTime from, DateTime to, int branchId)
        {
            var orders = (await _uow.Orders.GetByDateRangeAsync(from, to, branchId)).ToList();
            var revenue = orders.Sum(o => o.TotalAmount);

            decimal cogs = 0;
            var soldItems = orders
                .SelectMany(o => o.Items)
                .GroupBy(i => i.ProductId)
                .Select(g => new { ProductId = g.Key, Qty = g.Sum(i => i.Quantity) });

            foreach (var sp in soldItems)
            {
                var product = await _uow.Products.GetByIdAsync(sp.ProductId);
                if (product != null)
                    cogs += product.PurchasePrice * sp.Qty;
            }

            return new ProfitLossDto
            {
                From = from,
                To = to,
                Revenue = revenue,
                COGS = cogs,
                GrossProfit = revenue - cogs,
                GrossMargin = revenue == 0
                    ? 0
                    : Math.Round((revenue - cogs) / revenue * 100, 2),
            };
        }

        public async Task<IEnumerable<TopProductDto>> GetTopProductsAsync(
            DateTime from, DateTime to, int branchId, int top = 10)
        {
            var orders = await _uow.Orders.GetByDateRangeAsync(from, to, branchId);

            return orders
                .SelectMany(o => o.Items)
                .GroupBy(i => new { i.ProductId, i.ProductName })
                .Select(g => new TopProductDto
                {
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(i => i.Quantity),
                    Revenue = g.Sum(i => i.LineTotal),
                })
                .OrderByDescending(p => p.Revenue)
                .Take(top)
                .ToList();
        }

        public async Task<byte[]> ExportToExcelAsync(SalesReportDto report)
        {
            // Simple CSV export — replace with ClosedXML for real Excel
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Date,Total Orders,Gross Sales,Discount,Tax,Net Sales");
            sb.AppendLine($"{report.Date:yyyy-MM-dd},{report.TotalOrders}," +
                          $"{report.GrossSales},{report.TotalDiscount}," +
                          $"{report.TotalTax},{report.NetSales}");
            sb.AppendLine();
            sb.AppendLine("Product,Quantity Sold,Revenue");
            foreach (var p in report.TopProducts)
                sb.AppendLine($"{p.ProductName},{p.QuantitySold},{p.Revenue}");

            return System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        }

        private async Task<SalesReportDto> BuildSalesReportAsync(
            DateTime from, DateTime to, int branchId)
        {
            var orders = (await _uow.Orders.GetByDateRangeAsync(from, to, branchId)).ToList();

            return new SalesReportDto
            {
                Date = from,
                TotalOrders = orders.Count,
                GrossSales = orders.Sum(o => o.SubTotal),
                TotalDiscount = orders.Sum(o => o.DiscountAmount),
                TotalTax = orders.Sum(o => o.TaxAmount),
                NetSales = orders.Sum(o => o.TotalAmount),
                CashSales = orders.SelectMany(o => o.Payments)
                                     .Where(p => p.Method == PaymentMethod.Cash)
                                     .Sum(p => p.Amount),
                CardSales = orders.SelectMany(o => o.Payments)
                                     .Where(p => p.Method == PaymentMethod.Card)
                                     .Sum(p => p.Amount),
                MobileSales = orders.SelectMany(o => o.Payments)
                                     .Where(p => p.Method == PaymentMethod.MobileBanking)
                                     .Sum(p => p.Amount),
                TopProducts = orders
                    .SelectMany(o => o.Items)
                    .GroupBy(i => new { i.ProductId, i.ProductName })
                    .Select(g => new TopProductDto
                    {
                        ProductName = g.Key.ProductName,
                        QuantitySold = g.Sum(i => i.Quantity),
                        Revenue = g.Sum(i => i.LineTotal),
                    })
                    .OrderByDescending(p => p.Revenue)
                    .Take(10)
                    .ToList(),
            };
        }
    }
}
