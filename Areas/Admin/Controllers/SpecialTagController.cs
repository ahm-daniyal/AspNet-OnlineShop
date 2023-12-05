using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {

        private readonly ApplicationDbContext _db;

        public SpecialTagController(ApplicationDbContext _db)
        {
            this._db = _db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_db.SpecialTag.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpecialTag specialTagRequest)
        {
            var specialTag = new SpecialTag()
            {
                TagName = specialTagRequest.TagName
            };

            await _db.SpecialTag.AddAsync(specialTag);
            await _db.SaveChangesAsync();
            TempData["save"] = "Tag Name saved successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var specialTag = await _db.SpecialTag.FirstOrDefaultAsync(x => x.Id == id);
            if (specialTag != null)
            {
                return await Task.Run(() => View("Edit", specialTag));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SpecialTag specialTagRequest)
        {
            var specialTag = await _db.SpecialTag.FindAsync(specialTagRequest.Id);
            if (specialTag != null)
            {
                specialTag.Id = specialTagRequest.Id;
                specialTag.TagName = specialTagRequest.TagName;

                await _db.SaveChangesAsync();
                TempData["save"] = "Tag Name updated successfully";

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var specialTag = await _db.SpecialTag.FindAsync(id);
            if (specialTag != null)
            {
                _db.SpecialTag.Remove(specialTag);
                await _db.SaveChangesAsync();
                TempData["save"] = "Tag Name deleted successfully";

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}

