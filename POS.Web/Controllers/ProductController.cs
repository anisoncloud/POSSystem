using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using System.Security.Claims;

namespace POS.Web.Controllers
{
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _env;

        public ProductController(
            IProductService productService,
            IWebHostEnvironment env)
        {
            _productService = productService;
            _env = env;
        }

        private int BranchId =>
            int.Parse(User.FindFirstValue("BranchId") ?? "1");

        // ── Index ─────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Index(
            string? search, int? categoryId, int page = 1)
        {
            var vm = await _productService.GetProductListAsync(
                search, categoryId, page, 20, BranchId);
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            return View(vm);
        }

        // ── Create ────────────────────────────────────────────────────────────
        [HttpGet]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            return View(new CreateProductDto());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(
            CreateProductDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                return View(dto);
            }

            dto.ImageUrl = await SaveImageAsync(imageFile);

            await _productService.CreateProductAsync(dto, BranchId);
            TempData["Success"] = "Product created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ── Edit ──────────────────────────────────────────────────────────────
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            ViewBag.Categories = await _productService.GetCategoriesAsync();

            var dto = new UpdateProductDto
            {
                Name = product.Name,
                Description = product.Description,
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice,
                TaxRate = product.TaxRate,
                LowStockThreshold = product.LowStockThreshold,
                UnitType = product.UnitType,
                IsActive = product.IsActive,
                CategoryIds = product.Categories.Select(c => c.Id).ToList(),
                ImageUrl = product.ImageUrl,
            };
            return View(dto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(
            int id, UpdateProductDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                return View(dto);
            }

            if (imageFile != null)
                dto.ImageUrl = await SaveImageAsync(imageFile);

            await _productService.UpdateProductAsync(id, dto);
            TempData["Success"] = "Product updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ── Delete ────────────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            TempData["Success"] = "Product deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ── Barcode label ─────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> BarcodeLabel(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> RegenerateBarcode(int id)
        {
            await _productService.RegenerateBarcode(id);
            TempData["Success"] = "Barcode regenerated.";
            return RedirectToAction(nameof(BarcodeLabel), new { id });
        }

        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var folder = Path.Combine(_env.WebRootPath, "uploads", "products");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var path = Path.Combine(folder, fileName);

            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/products/{fileName}";
        }
    }
}
