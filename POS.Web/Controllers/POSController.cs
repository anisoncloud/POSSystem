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

        [HttpGet]
        public async Task<IActionResult> GetProduct(string query)
        {
            ProductDto? product = null;
            /*if (query.Length >= 8 && query.All(char.IsDigit))
                product = await _productService.GetByBarcodeAsync(query);
            else*/
                product = (await _productService.SearchAsync(query, _branchId)).FirstOrDefault();

            if (product == null)
                return Json(new { success = false, message = "Product not found" });

            return Json(new
            {
                success = true,
                product = new
                {
                    id = product.Id,
                    name = product.Name,
                    price = product.SalePrice,
                    barcode = product.Barcode,
                    stock = product.StockQuantity,
                    unit = product.UnitType.ToString(),
                    taxRate = product.TaxRate
                }
            });
        }
    }
}
