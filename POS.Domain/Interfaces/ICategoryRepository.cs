using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllActiveCategoryAsync();
        Task<Category> GetCategoryByNameAsync(string name);
        Task<bool> HasProductAsync(int id);
    }
}
