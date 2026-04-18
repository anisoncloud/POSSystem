using POS.Domain.Entities;
using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Interfaces
{
    public interface IStockRepository : IRepository<StockMovement>
    {
        Task<IEnumerable<StockMovement>> GetProductHistoryAsync(int productId);
        Task RecordMovementAsync(int productId, int qty, StockMovementType type, string? reference);
    }
}
