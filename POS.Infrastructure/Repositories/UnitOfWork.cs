using POS.Domain.Entities;
using POS.Domain.Interfaces;
using POS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<Supplier> Suppliers { get; }
        public IRepository<PurchaseOrder> PurchaseOrders { get; }
        public IStockRepository StockMovements { get; }
        public IRepository<Table> Tables { get; }
        public IRepository<Payment> Payments { get; }

        

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Products = new ProductRepository(context);
            Orders = new OrderRepository(context);
            Categories = new GenericRepository<Category>(context);
            Suppliers = new GenericRepository<Supplier>(context);
            PurchaseOrders = new GenericRepository<PurchaseOrder>(context);
            StockMovements = new StockRepository(context);
            Tables = new GenericRepository<Table>(context);
            Payments = new GenericRepository<Payment>(context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task RollBackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
