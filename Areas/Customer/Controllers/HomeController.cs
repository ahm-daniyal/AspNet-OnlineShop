using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using X.PagedList;
using Products = OnlineShop.Models.Products;

namespace OnlineShop.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index(int? page)
    {
        return View(_db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag)
            .ToList().ToPagedList(page??1,9));
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag)
            .FirstOrDefault(c => c.Id == id);
        return View(product);
    }

    [HttpPost]
    [ActionName("Details")]
    public IActionResult ProductDetails(int? id)
    {
        List<Products>products = new List<Products>();
        if (id == null)
        {
            return NotFound();
        }

        var product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag)
            .FirstOrDefault(c => c.Id == id);
        products = HttpContext.Session.Get<List<Products>>("products");
        if (products == null)
        {
            products = new List<Products>();
        }
        products.Add(product);
        HttpContext.Session.Set("products", products);
        return View(product);
    }

    [HttpPost]
    public IActionResult Remove(int? id)
    {
        List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        if (products != null)
        {
            var product = products.FirstOrDefault(c => c.Id == id);
            if (product != null)
            {
                products.Remove(product);
                HttpContext.Session.Set("products", products);
            }
        }
        return RedirectToAction(nameof(Index));
    }

    [ActionName("Remove")]
    public IActionResult RemoveToCart(int? id)
    {
        List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        if (products != null)
        {
            var product = products.FirstOrDefault(c => c.Id == id);
            if (product != null)
            {
                products.Remove(product);
                HttpContext.Session.Set("products", products);
            }
        }
        return RedirectToAction(nameof(Cart));
    }

    public IActionResult Cart()
    {
        List<Products> products = HttpContext.Session.Get<List<Products>>("products");
        if(products == null)
        {
            products = new List<Products>();
        }
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

