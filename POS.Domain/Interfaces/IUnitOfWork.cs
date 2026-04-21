using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IRepository<Category> Categories {  get; }
        IRepository<Supplier> Suppliers { get; }
        IRepository<PurchaseOrder> PurchaseOrders { get; }
        IStockRepository StockMovements { get; }
        IRepository<Table> Tables { get; }
        IRepository<Payment> Payments { get; }
        IRepository<OrderItem> OrderItems { get; }
        Task<int> CommitAsync();
        Task RollBackAsync();
    }
}
