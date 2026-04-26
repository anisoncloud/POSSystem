using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Interfaces;
using POS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Infrastructure.Repositories
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Branch>> GetAllActiveAsync()
        {
            return await _dbSet
                .Where(b=>b.IsActive==true)
                .OrderBy(b=>b.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Branch?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.Name == name);                
        }

        public async Task<bool> HasProductsAsync(int branchId)
        {
            return await _context.Products
                .AnyAsync(p => p.BranchId == branchId && !p.IsDeleted);
        }

        public async Task<bool> HasUserAsync(int branchId)
        {
            return await _context.Users
                .AnyAsync(u=>u.BranchId== branchId) ;
        }
    }
}
