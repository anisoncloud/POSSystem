using POS.Application.DTOs;
using POS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        public Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryDto>> GetAllSubCategoriesByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetCategoryByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> UpdateCategoryAsync(CategoryUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
