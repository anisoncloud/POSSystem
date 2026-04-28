using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Interfaces;
using POS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) 
        {
        }         

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _dbSet.
                FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public Task<bool> HasProductAsync(int categoryId)
        {
            return _context.ProductCategories
                .AnyAsync(c=>c.CategoryId == categoryId);
        }
    }
}
