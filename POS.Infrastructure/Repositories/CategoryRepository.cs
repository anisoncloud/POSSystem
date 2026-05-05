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
                FirstOrDefaultAsync(c => 
                c.Name.ToLower() == name.ToLower() && !c.IsDeleted);
        }

        public async Task<IEnumerable<Category>> GetAllWithParentAsync()
        {
            return await _dbSet
                .Include(c => c.ParentCategory)
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.ParentCategoryId)
                .ThenBy(c => c.Name)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetTopLevelAsync()
        {
            return await _dbSet
                .Where(c=>c.ParentCategoryId == null &&  !c.IsDeleted)
                .OrderBy(c=>c.Name)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
        {
            return await _dbSet
                .Where(c => c.ParentCategoryId == parentId && !c.IsDeleted)
                .OrderBy(c => c.Name)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> HasSubCategoriesAsync(int categoryId)
        {
            return await _dbSet
                .AnyAsync(c => c.ParentCategoryId == categoryId && !c.IsDeleted);
        }
        public Task<bool> HasProductAsync(int categoryId)
        {
            return _context.ProductCategories
                .AnyAsync(c=>c.CategoryId == categoryId);
        }
    }
}
