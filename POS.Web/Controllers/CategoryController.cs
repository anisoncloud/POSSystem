using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using System.Data;

namespace POS.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllWithParentAsync();
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {            
            await PopulateParentDropdownAsync(selectedId: null);
            return View(new CategoryCreateDto());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateParentDropdownAsync(dto.ParentCategoryId);
                return View(dto);
            }
            try
            {
                await _categoryService.CreateCategoryAsync(dto);                
                TempData["success"] = $"'{dto.Name}' Category Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(dto.Name), ex.Message);
                await PopulateParentDropdownAsync(dto.ParentCategoryId);
                return View(dto);
            }            
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                // Map CategoryDto → CategoryUpdateDto
                var dto = new CategoryUpdateDto
                {
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    ParentCategoryId = category.ParentCategoryId
                };

                // Populate dropdown — exclude current category
                // so it cannot select itself as parent
                await PopulateParentDropdownAsync(
                    selectedId: category.ParentCategoryId,
                    excludedId: id);
                ViewBag.CategoryId = id;
                ViewBag.CategoryName = category.Name;
                return View(dto);

            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateParentDropdownAsync(
                    selectedId: dto.ParentCategoryId,
                    excludedId: id
                    );
                ViewBag.CategoryId = id;
                return View(dto);
            }
            try
            {
                await _categoryService.UpdateCategoryAsync(id, dto);
                TempData["success"] = $"{dto.Name} Updated successfylly";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidExpressionException ex)
            {
                // Duplicate name or circular reference error
                ModelState.AddModelError(nameof(dto.Name), ex.Message);
                await PopulateParentDropdownAsync(
                    selectedId: dto.ParentCategoryId,
                    excludedId: id);
                ViewBag.CategoryId = id;
                return View(dto);
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
            
        }
        private async Task PopulateParentDropdownAsync(int? selectedId, int? excludedId=null)
        {
            var allCategories = await _categoryService.GetAllAsync();

            // Exclude the current category being edited
            // so it cannot be set as its own parent
            var filtered = allCategories.Where(c => c.Id != excludedId).ToList();

            var items = filtered.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.DisplayName,
                Selected = c.Id == selectedId,
            }).ToList();

            // Add black option at top
            items.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "---None (Top Level Category) ----",
                Selected = selectedId == null
            });
            ViewBag.ParentCategories = items;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                TempData["success"] = $"Category deleted successfully";
            }
            catch (KeyNotFoundException)
            {
                TempData["error"] = $"Category with this {id} not found";
            }
            catch (InvalidOperationException ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
