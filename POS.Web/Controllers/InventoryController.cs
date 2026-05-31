using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using System.Security.Claims;

namespace POS.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;

        public InventoryController(IInventoryService inventoryService, IProductService productService)
        {
            _inventoryService = inventoryService;
            _productService = productService;
        }
        private int branchId => int.Parse(User.FindFirstValue("BranchId") ?? "1");
        
        //-----Stock Movement List--------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movement = await _inventoryService.GetAllMovementsAsync(branchId);
            return View(movement);
        }

        //-----Stock History per product-------------------------------------------
        public async Task<IActionResult> History(int productId)
        {
            var history = await _inventoryService.GetStockHistoryAsync(productId);
            var product = await _productService.GetByIdAsync(productId);
            ViewBag.Product = product;
            return View(product);
        }

        //----Adjust Stock -----------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Adjust()
        {
            ViewBag.Products = await _productService.GetAllAsync(branchId);
            return View(new StockAdjustmentDto());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Adjust(StockAdjustmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _productService.GetAllAsync(branchId);
                return View(dto);
            }
            await _inventoryService.AdjustStockAsync(dto);
            TempData["success"] = "Stock Adjusted successfully";
            return RedirectToAction(nameof(Index));
        }

        // ── Purchase orders ───────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> PurchaseOrders()
        {
            var orders = await _inventoryService.GetPurchaseOrdersAsync(branchId);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePurchaseOrder()
        {
            ViewBag.Suppliers = await _inventoryService.GetSuppliersAsync();
            ViewBag.Products = await _productService.GetAllAsync(branchId);
            return View(new CreatePurchaseOrderDto());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePurchaseOrder(CreatePurchaseOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = await _inventoryService.GetSuppliersAsync();
                ViewBag.Products = await _productService.GetAllAsync(branchId);
                return View(dto);
            }
            await _inventoryService.CreatePurchaseOrderAsync(dto, branchId);
            TempData["success"] = "Purchase order created";
            return RedirectToAction(nameof(PurchaseOrders));
        }

        [HttpGet]
        public async Task<IActionResult> PurchaseOrderDetail(int id)
        {
            var po = (await _inventoryService
                .GetPurchaseOrdersAsync(branchId))
                .FirstOrDefault(p => p.Id == id);

            if (po == null)
                return NotFound();

            return PartialView("_PurchaseOrderDetail", po);
        }
        // ── Suppliers ─────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Suppliers()
        {
            var suppliers = await _inventoryService.GetSuppliersAsync();
            return View(suppliers);
        }

        [HttpGet]
        public IActionResult CreateSupplier() => View(new CreateSupplierDto());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSupplier(CreateSupplierDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _inventoryService.CreateSupplierAsync(dto);
            TempData["Success"] = "Supplier created.";
            return RedirectToAction(nameof(Suppliers));
        }
    }
}
