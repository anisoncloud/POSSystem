using AutoMapper;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var existing = _uow.Categories.GetByNameAsync(dto.Name);
            if (existing!=null)
            {
                throw new InvalidOperationException(
                    $"A category with the name '{dto.Name}' is already exists"
                    );
            }
            var category = _mapper.Map<Category>(dto);
            await _uow.Categories.AddAsync(category);
            return _mapper.Map<CategoryDto>(dto);
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
