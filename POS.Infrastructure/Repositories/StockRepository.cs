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
    public class StockRepository : GenericRepository<StockMovement>, IStockRepository
    {
        public StockRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StockMovement>> GetProductHistoryAsync(int productId)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Where(s => s.ProductId == productId)
                .OrderByDescending(s => s.CreaedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task RecordMovementAsync(int productId, int qty, StockMovementType type, string? reference)
        {
            var product = await _context.Products
                 .FirstOrDefaultAsync(p => p.Id == productId)
                 ?? throw new KeyNotFoundException($"Product{productId} not found");
            var movement = new StockMovement
            {
                ProductId = productId,
                Quantity = qty,
                MovementType = type,
                Reference = reference,
                BalanceAfter = product.StockQuantity + qty,
                CreaedAt = DateTime.UtcNow
            };
            await _dbSet.AddAsync(movement);
        }
    }
}
