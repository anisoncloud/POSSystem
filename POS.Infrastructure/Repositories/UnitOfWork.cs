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

        public Task<int> CommitAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task RollBackAsync()
        {
            throw new NotImplementedException();
        }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Products = new ProductRepository(context);
            Orders = new OrderRepository(context);
            Categories = new GenericRepository<Category>(context);
            Suppliers = new GenericRepository<Supplier>(context);
            PurchaseOrders = new GenericRepository<PurchaseOrder>(context);
            StockMovements = new stock (context);

        }
    }
}
