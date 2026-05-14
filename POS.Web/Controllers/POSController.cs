using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Application.ViewModels;
using POS.Domain.Entities;
using System.Security.Claims;

namespace POS.Web.Controllers
{
    public class POSController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly int _branchId;
        public POSController(IOrderService orderService, 
            IProductService productService, 
            IMapper mapper, 
            IHttpContextAccessor httpContext)
        {
            _orderService = orderService;
            _productService = productService;
            _mapper = mapper;
            _branchId = int.Parse(httpContext.HttpContext.User.FindFirstValue("BranchId") ?? "1");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _productService.GetCategoriesAsync();
            var tables = await _orderService.GetAvailableTablesAsync(_branchId);
            var vm = new PosScreenViewModel
            {
                Categories = categories,
                Tables = tables
            };
            return View(vm);
        }

        // ── AJAX: scan barcode or search product ──────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetProduct(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new
                {
                    success = false,
                    message = "Please enter a product name or barcode."
                });

            ProductDto? product = null;

            // Barcode scan: 8+ digit string from USB scanner
            if (query.Length >= 8 && query.All(char.IsDigit))
            {
                product = await _productService
                    .GetByBarcodeAsDtoAsync(query);
            }
            else
            {
                // Text search: name, SKU or partial barcode
                var results = await _productService
                    .SearchAsync(query, _branchId);

                product = results.FirstOrDefault();
            }

            if (product == null)
                return Json(new
                {
                    success = false,
                    message = $"No product found for '{query}'."
                });

            if (!product.IsActive)
                return Json(new
                {
                    success = false,
                    message = $"'{product.Name}' is inactive."
                });

            if (product.StockQuantity <= 0)
                return Json(new
                {
                    success = false,
                    message = $"'{product.Name}' is out of stock."
                });

            return Json(new
            {
                success = true,
                product = new
                {
                    id = product.Id,
                    name = product.Name,
                    price = product.SalePrice,
                    barcode = product.Barcode,
                    sku = product.SKU,
                    stock = product.StockQuantity,
                    unit = product.UnitType.ToString(),
                    taxRate = product.TaxRate,
                    imageUrl = product.ImageUrl ?? string.Empty,
                }
            });
        }

        // ── AJAX: product grid for category filter ────────────────────────
        [HttpGet]
        public async Task<IActionResult> ProductGrid(
            int? categoryId)
        {
            IEnumerable<ProductDto> products;
            if (categoryId.HasValue)
                products = await _productService
                    .GetProductsByCategoryAsync(categoryId.Value, _branchId);
            else
                products = await _productService.GetAllAsync(_branchId);
            return PartialView("_ProductGrid", products);
        }
    }
}
