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
        

        public async Task<CategoryDto> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            //-- Find All categories by id---------------------------------------------------
            var categorie = await _uow.Categories.GetByIdAsync(id)?? throw new KeyNotFoundException($"The Item not found you requested here for id {id}");
            
            //--- Check is this inputed name already exists or not---------------------------
            var exists = await _uow.Categories.GetByNameAsync(dto.Name);
            if (exists!=null && exists.Id !=id)
            {
                throw new InvalidOperationException(
                    $"A category with the name {dto.Name} already exists.");
            }

            if (dto.ParentCategoryId.HasValue && 
                dto.ParentCategoryId==id)
            {                
                throw new InvalidOperationException(
                    $"Can not be its own parent category"
                    );                             
            }
            if (dto.ParentCategoryId.HasValue)
            {
                var isChilOfThis = await IsDescendantAsync(dto.ParentCategoryId.Value, id);
                if (isChilOfThis)
                {
                    throw new InvalidOperationException(
                        "Can not set a Subcategory as the parent"
                        + "This would create a Peference"
                        );
                }
                var parent = await _uow.Categories.GetByIdAsync(dto.ParentCategoryId.Value);
                if (parent ==null)
                {
                    throw new KeyNotFoundException(
                        $"Parent category with Id {dto.ParentCategoryId.Value} not found"
                        );
                }

                categorie.Name = dto.Name;
                categorie.Description = dto.Description;
                categorie.ImageUrl= dto.ImageUrl;
                categorie.ParentCategoryId= dto.ParentCategoryId;
                categorie.UpdatedAt = DateTime.UtcNow;

                await _uow.Categories.UpdateAsync(categorie);
                await _uow.CommitAsync();                
            }
            return await GetByIdAsync(id);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category==null)
            {
                throw new KeyNotFoundException(
                    $"Category with {id} not found"
                    );
            }
            var hasSubCategory = await _uow.Categories.HasSubCategoriesAsync(id);
            if (hasSubCategory)
            {
                throw new InvalidOperationException(
                    $"The Categoyr with Id {id} has Sub Category. Delete or Move the Sub Category first.");
            }

            await _uow.Categories.DeleteAsync(id);
            await _uow.CommitAsync();
        }

        // Recursevely check if targatedId is a decendant of ancestorId
        // Used to preven circular parent assignment 

        private async Task<bool> IsDescendantAsync(int targetdId, int ancestorId)
        {
            var children = await _uow.Categories.GetSubCategoriesAsync(ancestorId);
            foreach (var child in children)
            {
                if (child.Id == targetdId)
                {
                    return true;
                }
                if (await IsDescendantAsync(targetdId, child.Id))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
