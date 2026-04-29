using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create()
        {
            //ViewBag.Categories = await 
            return View();
        }
    }
}
