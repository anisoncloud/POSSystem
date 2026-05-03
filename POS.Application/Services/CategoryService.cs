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
            var existing = await _uow.Categories.GetByNameAsync(dto.Name);
            if (existing != null)
            {
                //var categories = await _uow.Categories.GetAllAsync();
                //var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories).ToList();
                //return dots;
                throw new InvalidOperationException(
                    $"A category with the name '{dto.Name}' is already exists"
                    );

            }
            var category = _mapper.Map<Category>(dto);
            await _uow.Categories.AddAsync(category);
            await _uow.CommitAsync();
            return _mapper.Map<CategoryDto>(category);
        }

        /*public async Task<ServiceResult<CategoryDto>> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var existing = _uow.Categories.GetByNameAsync(dto.Name);
            if (existing !=null)
            {
                return new ServiceResult<CategoryDto>
                {
                    Success = false,
                    Message = "Category already exists"
                };
            }
            var category = _mapper.Map<Category>(dto);
            await _uow.Categories.AddAsync(category);
            await _uow.CommitAsync();
            return new ServiceResult<CategoryDto>
            {
                Success = true,
                Message = "Category Created Successfully"
            };
        }*/

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _uow.Categories.GetAllAsync();
            var dots = _mapper.Map<IEnumerable<CategoryDto>>(categories).ToList();
            return dots;
        }

        public Task<IEnumerable<CategoryDto>> GetAllSubCategoriesByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            var category =  await _uow.Categories.GetByIdAsync(categoryId);
            var dot = _mapper.Map<CategoryDto>(category);
            return dot;
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
