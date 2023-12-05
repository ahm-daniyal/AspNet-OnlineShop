using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _he;
        private string productImage;

        public ProductController(ApplicationDbContext _db, IHostingEnvironment _he)
        {
            this._db = _db;
            this._he = _he;
        }

        public IActionResult Index()
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).ToList());
        }

        [HttpPost]
        public IActionResult Index(decimal lowAmount, decimal largeAmount)
        {
            var product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag)
                .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).ToList();
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["specialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Products productRequest, IFormFile image)
        {
            var searchableProducts = _db.Products.FirstOrDefault(c => c.Name == productRequest.Name);
            if (searchableProducts != null)
            {
                ViewBag.message = "This product name already exists.";
                ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                ViewData["specialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
                return View(productRequest);
            }

            if (image != null)
            {
                var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                await image.CopyToAsync(new FileStream(name, FileMode.Create));
                productRequest.Image = "Images/" + image.FileName;
            }

            if (image == null)
            {
                productRequest.Image = "Images/noimage.PNG";
            }

            var product = new Products()
            {
                Name = productRequest.Name,
                Price = productRequest.Price,
                Image = productRequest.Image,
                ProductColor = productRequest.ProductColor,
                IsAvailable = productRequest.IsAvailable,
                ProductTypeId = productRequest.ProductTypeId,
                SpecialTagId = productRequest.SpecialTagId
            };

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            TempData["save"] = "Product saved successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["specialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                .FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Products productRequest, IFormFile image)
        {
            if (image != null)
            {
                var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                await image.CopyToAsync(new FileStream(name, FileMode.Create));
                productRequest.Image = "Images/" + image.FileName;
            }

            if (image == null)
            {
                productRequest.Image = "Images/noimage.PNG";
            }

            _db.Products.Update(productRequest);
            await _db.SaveChangesAsync();
            TempData["save"] = "Product updated successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product deleted successfully";

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}

