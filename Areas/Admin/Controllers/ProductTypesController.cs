using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Controllers;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
	public class ProductTypesController : Controller
	{
        private readonly ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext _db)
        {
            this._db = _db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_db.ProductTypes.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductTypes productTypesRequest)
        {
            var productType = new ProductTypes()
            {
                ProductType = productTypesRequest.ProductType
            };

            await _db.ProductTypes.AddAsync(productType);
            await _db.SaveChangesAsync();
            TempData["save"] = "Product Type saved successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var productType = await _db.ProductTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (productType != null)
            {
                return await Task.Run(() => View("Edit", productType));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductTypes productTypesRequest)
        {
            var productTypes = await _db.ProductTypes.FindAsync(productTypesRequest.Id);
            if (productTypes != null)
            {
                productTypes.Id = productTypesRequest.Id;
                productTypes.ProductType = productTypesRequest.ProductType;

                await _db.SaveChangesAsync();
                TempData["save"] = "Product Type updated successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var productType = await _db.ProductTypes.FindAsync(id);
            if (productType != null)
            {
                _db.ProductTypes.Remove(productType);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Type deleted successfully";

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}

