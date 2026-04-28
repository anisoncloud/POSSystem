using POS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<CategoryDto>> GetAllSubCategoriesByIdAsync(int categoryId);
        Task<CategoryDto> GetCategoryByIdAsync(int categoryId);
        Task<CategoryDto> GetCategoryByNameAsync(string name);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto);
        Task<CategoryDto> UpdateCategoryAsync(CategoryUpdateDto dto);
    }
}
