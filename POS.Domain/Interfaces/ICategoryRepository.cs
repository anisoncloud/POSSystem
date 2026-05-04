using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllWithParentAsync();
        Task<IEnumerable<Category>> GetTopLevelAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);
        Task<bool> HasSubCategoriesAsync(int categoryId);
        Task<bool> HasProductAsync(int categoryId);
    }
}
