using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Domain.Interfaces;
using POS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<string> GenerateInvoiceNumberAsync(int branchId)
        {
            var today = DateTime.UtcNow;
            var prefix = $"INV-{branchId:D2}-{today:yyyyMMdd}-";
            var lastOrder = await _dbSet
                .Where(o=>o.InvoiceNumber.StartsWith(prefix))
                .OrderByDescending(o=>o.InvoiceNumber)
                .FirstOrDefaultAsync();
            int sequenc = lastOrder == null? 1: int.Parse(lastOrder.InvoiceNumber.Split('-').Last())+1;
            return $"{prefix}{sequenc:D4}";
        }

        public async Task<IEnumerable<Order>> GetActiveTableOrderAsync(int branchId)
        {
            return await _dbSet
                .Include(o => o.Table)
                .Include(o => o.Items)
                .Where(o => o.BranchId == branchId &&
                o.OrderType == OrderType.Restaurant &&
                o.Status != OrderStatus.Completed &&
                o.Status != OrderStatus.Cancelled)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByDateRanceAsync(DateTime from, DateTime to, int branchId)
        {
            return await _dbSet.Include(o => o.Items)
                .Include(o => o.Payments)
                .Where(o => o.BranchId == branchId &&
                o.CreaedAt >= from &&
                o.CreaedAt <= to &&
                o.Status == OrderStatus.Completed)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> GetWithItemsAsync(int orderId)
        {
            return await _dbSet
                .Include(o=>o.Items).ThenInclude(i=>i.Product)
                .Include(o=>o.Payments)
                .Include(o=>o.Cashier)
                .Include(o=>o.Table)
                .Include(o=>o.Branch)
                .FirstOrDefaultAsync(o=>o.Id==orderId);
        }
    }
}
