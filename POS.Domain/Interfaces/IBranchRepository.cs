using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Interfaces
{
    public interface IBranchRepository : IRepository<Branch>
    {
        Task<Branch?> GetByNameAsync(string name);
        Task<IEnumerable<Branch>> GetAllActiveAsync();
        Task<bool> HasUserAsync(int branchId);
        Task<bool> HasProductsAsync(int branchId);
    }
}
