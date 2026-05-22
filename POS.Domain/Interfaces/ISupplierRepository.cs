using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier?> GetByNameAsync(string name);
        Task<IEnumerable<Supplier>> GetAllActiveAsync();
        Task<bool> HasProductsAsync(int supplierId);
    }
}
