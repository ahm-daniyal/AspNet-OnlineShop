using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using OnlineShop.Utility;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender emailSender;
        private readonly IUnitOfWork unitOfWork;

        public OrderController(ApplicationDbContext db, IEmailSender emailSender, IUnitOfWork unitOfWork)
        {
            _db = db;
            this.emailSender = emailSender;
            this.unitOfWork = unitOfWork;
        }

        // Get Checkout Action Method
        public IActionResult Checkout()
        {
            return View();
        }

        // Post Checkout Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }

            // To Verify Payment Method
            try
            {
                decimal amount = 829;
                string returnUrl = "https://localhost:7269/Customer/Home/Cart";
                string cancelUrl = "https://localhost:7269/Customer/Home/Cart";

                // Create a paypal Order
                var createdPayment = await unitOfWork.PaypalServices.CreateOrderAsync(amount, returnUrl, cancelUrl);

                // Get the paypal approave url
                string approvalUrl = createdPayment.links.FirstOrDefault(x => x.rel.ToLower() == "approval_url")?.href;

                // Redirect the user to Paypal approval url
                if (!string.IsNullOrEmpty(approvalUrl))
                {
                    return Redirect(approvalUrl);
                }
                else
                {
                    TempData["error"] = "Failed to initiate Paypal payment";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            anOrder.OrderNo = GetOrderNo();
            _db.Orders.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Products>());

            var receiverEmail = "mibimay848@frandin.com";
            var subject = "Order Placed Successfully!";
            var message = "You will receiveyour order in 10 days.";
            await emailSender.SendEmailAsync(
                receiverEmail,
                subject,
                message
            );
            return RedirectToAction("Cart", "Home", new { area = "Customer" });
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count + 1;
            return rowCount.ToString("000");
        }
    }
}

