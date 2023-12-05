using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext _db)
        {
            this._db = _db;
        }

        public IActionResult Index()
        {
            return View(_db.Categories.Include(c => c.Products).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["productId"] = new SelectList(_db.Products.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult getJsonData(int id)
        {
            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            var productType = _db.ProductTypes.FirstOrDefault(c => c.Id == product.ProductTypeId);
            var specialTag = _db.SpecialTag.FirstOrDefault(c => c.Id == product.SpecialTagId);

            /** To initallize the array */
            Dictionary<string, string> result = new Dictionary<string, string>();
            result["productType"] = productType.ProductType;
            result["tagName"] = specialTag.TagName;

            return Json(result);
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category CategoryRequest)
        {
            var category = new Category()
            {
                CategoryName = CategoryRequest.CategoryName,
                ProductId = CategoryRequest.ProductId,
                ProductType = CategoryRequest.ProductType,
                TagName = CategoryRequest.TagName
            };

            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            TempData["save"] = "Category saved successfully";
            return RedirectToAction("Index");
        }
    }
}

