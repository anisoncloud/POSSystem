using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface IReportService
    {
        Task<SalesReportDto> GetDailySalesAsync(DateTime date, int branchId);
        Task<SalesReportDto> GetMonthlySalesAsync(int year, int month, int branchId);
        Task<ProfitLossDto> GetProfitLossAsync(DateTime from, DateTime to, int branchId);
        Task<IEnumerable<TopProductDto>> GetTopProductsAsync(DateTime from, DateTime to, int branchId, int top = 10);
        Task<byte[]> ExportToExcelAsync(SalesReportDto report);
    }
}
