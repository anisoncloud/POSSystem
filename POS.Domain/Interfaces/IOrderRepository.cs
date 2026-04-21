using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetWithItemsAsync(int orderId);
        Task<string> GenerateInvoiceNumberAsync(int branchId);
        Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime from, DateTime to, int branchId);
        Task<IEnumerable<Order>> GetActiveTableOrdersAsync(int branchId);

    }
}
