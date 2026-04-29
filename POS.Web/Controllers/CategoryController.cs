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

        public IActionResult Index()
        {
            return View();
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
    }
}
