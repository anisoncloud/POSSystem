using POS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<IEnumerable<CategoryDto>> GetAllWithParentAsync();
        Task<IEnumerable<CategoryDto>> GetTopLevelAsync();
        Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(int parentId);
        Task<CategoryDto> GetByIdAsync(int id);        
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto);
        Task<CategoryDto> UpdateCategoryAsync(int id, CategoryUpdateDto dto);
        Task DeleteCategoryAsync(int id);
    }
}
