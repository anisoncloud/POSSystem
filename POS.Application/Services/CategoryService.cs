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
        private IEnumerable<CategoryDto> MapWithParentName(
            IEnumerable<Category> categories)
        {
            return _mapper.Map<IEnumerable<CategoryDto>>(categories); //**This comes form MappfingProfile
        }
        //----Get All Flat List of all Categories --------------------
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories =  await _uow.Categories.GetAllAsync();
            return MapWithParentName(categories);
        }

        //---Get All Categories with who have the parent category--------
        public async Task<IEnumerable<CategoryDto>> GetAllWithParentAsync()
        {
            var categories = await _uow.Categories.GetAllWithParentAsync();
                return MapWithParentName(categories);
        }

        //--- Get All Only Category who dont have any parent category id------
        public async Task<IEnumerable<CategoryDto>> GetTopLevelAsync()
        {
            var categories = await _uow.Categories.GetTopLevelAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
        //------ Get All SubCategories with same parent id-------------
        public async Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(int parentId)
        {
            var subCategories = await _uow.Categories.GetSubCategoriesAsync(parentId);
            return _mapper.Map<IEnumerable<CategoryDto>>(subCategories);
        }


        //-----Get Only SubCategories of A Category------------------
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int parentId)
        {
            var subCategories = await _uow.Categories.GetSubCategoriesAsync(parentId);
            return _mapper.Map<IEnumerable<CategoryDto>>(subCategories); 
        }

        //---Get A Category or SubCategory with parent by his Id-----
        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category==null)
            {
                throw new KeyNotFoundException($"Category with Id {id} not found");
            }
            var dto = _mapper.Map<CategoryDto>(category);
            if (category.ParentCategoryId.HasValue)
            {
                var parent = await _uow.Categories
                    .GetByIdAsync(category.ParentCategoryId.Value);
                dto.ParentCategoryName = parent?.Name;
            }
            return dto;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var existing = await _uow.Categories.GetByNameAsync(dto.Name);
            if (existing != null)
            {                
                throw new InvalidOperationException(
                    $"A category with the name '{dto.Name}' is already exists"
                    );

            }
            var category = _mapper.Map<Category>(dto);
            await _uow.Categories.AddAsync(category);
            await _uow.CommitAsync();
            return _mapper.Map<CategoryDto>(category);
        }        
        //--- Get All Categories ---------------
        /*public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
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
        }*/

        public Task<CategoryDto> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
