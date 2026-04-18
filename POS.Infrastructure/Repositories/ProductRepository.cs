using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Interfaces;
using POS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetByBarcodeAsync(string barcode)
        {
            return await _dbSet.Include(p=>p.ProductCategories)
                .ThenInclude(pc=>pc.Category)
                .FirstOrDefaultAsync(p=>p.Barcode == barcode);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet.Where(p => p.ProductCategories
                .Any(pc => pc.CategoryId == categoryId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetBySkuAsync(string sku)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.SKU == sku);
        }

        public async Task<IEnumerable<Product>> GetLowStockProductAsync(int branchId)
        {
            return _dbSet.Where(p => p.BranchId == branchId
            && p.StockQuantity <= p.LowStockThreshold
            && p.IsActive);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string term, int branchId)
        {
            return await _dbSet.Where(p=>p.BranchId == branchId
            && p.IsActive
            && (p.Name.Contains(term)) || (p.SKU.Contains(term))||(p.Barcode.Contains(term)))
                .AsNoTracking()
                .Take(20)
                .ToListAsync();                
        }
    }
}
