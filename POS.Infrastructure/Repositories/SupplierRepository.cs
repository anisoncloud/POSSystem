using POS.Domain.Entities;
using POS.Domain.Interfaces;
using POS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Infrastructure.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AppDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<Supplier>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Supplier?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasProductsAsync(int supplierId)
        {
            throw new NotImplementedException();
        }
    }
}
