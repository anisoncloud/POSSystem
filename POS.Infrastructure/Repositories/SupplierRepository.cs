using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Supplier>> GetAllActiveAsync()
        {
            return await _dbSet
                .Where(s=>!s.IsDeleted)
                .OrderByDescending(s=>s.Id)
                .ToListAsync();
        }

        public async Task<Supplier?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(nameof(name)))
            {
                return null;
            }
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Name.Replace(" ", "").ToLower() == name.Replace(" ", "").ToLower());
        }
    }
}
