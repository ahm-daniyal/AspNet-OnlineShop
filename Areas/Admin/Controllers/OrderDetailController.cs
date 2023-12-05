using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderDetailController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailController(ApplicationDbContext _db)
        {
            this._db = _db;
        }

        public IActionResult Index()
        {
            return View(_db.OrderDetails.Include(c => c.Order).Include(f => f.Product).ToList());
        }

        [HttpPost]
        public IActionResult Index(string orderId, string productName, string email)
        {
            var orders = _db.OrderDetails.Include(c => c.Order).Include(f => f.Product)
                .Where(c => c.Order.OrderNo == orderId || c.Product.Name == productName || c.Order.Email == email).ToList();

            return View(orders);
        }
    }
}

