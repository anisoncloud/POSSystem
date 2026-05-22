using Microsoft.AspNetCore.Mvc;
using POS.Application.DTOs;

namespace POS.Web.Controllers
{
    public class SupplierController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View(new CreateSupplierDto());
        }
    }
}
