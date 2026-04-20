using Microsoft.AspNetCore.Mvc;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using System.Security.Claims;

namespace POS.Web.Controllers
{
    public class ProductController : Controller
    {
        // POS.Web/Controllers/ProductController.cs
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _productService.GetCategoriesAsync();
                return View(dto);
            }

            // Handle image upload here in the Web layer
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                dto.ImageUrl = $"/uploads/products/{uniqueFileName}";
            }

            var branchId = int.Parse(User.FindFirstValue("BranchId")!);
            await _productService.CreateProductAsync(dto, branchId);

            TempData["Success"] = "Product created successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
