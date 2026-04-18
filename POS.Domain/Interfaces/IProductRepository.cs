using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByBarcodeAsync(string barcode);
        Task<Product?> GetBySkuAsync(string sku);
        Task<IEnumerable<Product>> GetLowStockProductAsync(int branchId);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SearchAsync(string term, int branchId);
    }
}
