using Microsoft.AspNetCore.Mvc;
using POS.Application.DTOs;
using POS.Application.Interfaces;

namespace POS.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllActiveAsync();
            return View(suppliers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new CreateSupplierDto());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSupplierDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }            
            try
            {
                await _supplierService.CreateSupplierAsync(dto);
                TempData["success"] = $"The Supplier name of {dto.Name} is created successfully";
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
