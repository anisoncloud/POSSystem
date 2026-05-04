using Microsoft.AspNetCore.Mvc;
using POS.Application.DTOs;
using POS.Application.Interfaces;

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
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            if (!ModelState.IsValid)
            {
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
                return View(dto);
            }            
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return View(category);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryUpdateDto dto)
        {
            return View();
        }
    }
}
