using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopEase.Data;
using ShopEase.Models;
using ShopEase.ViewModels;

namespace ShopEase.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _db;

        public CheckoutController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var cart = CartHelper.GetCart(HttpContext.Session);
            if (!cart.Any())
                return RedirectToAction("Index", "Cart");

            var vm = new CheckoutViewModel
            {
                CartItems = cart,
                TotalAmount = cart.Sum(c => c.SubTotal)
            };

            ViewBag.CartCount = CartHelper.GetCartCount(HttpContext.Session);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel vm)
        {
            var cart = CartHelper.GetCart(HttpContext.Session);

            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                vm.CartItems = cart;
                vm.TotalAmount = cart.Sum(c => c.SubTotal);
                ViewBag.CartCount = CartHelper.GetCartCount(HttpContext.Session);
                return View("Index", vm);
            }

            var order = new Order
            {
                CustomerName = vm.CustomerName,
                Email = vm.Email,
                Phone = vm.Phone,
                Address = vm.Address,
                City = vm.City,
                ZipCode = vm.ZipCode,
                PaymentMethod = vm.PaymentMethod,
                TotalAmount = cart.Sum(c => c.SubTotal),
                Status = "Pending",
                OrderDate = DateTime.Now
            };

            foreach (var item in cart)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });

                // Update stock
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product != null && product.Stock >= item.Quantity)
                    product.Stock -= item.Quantity;
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            CartHelper.ClearCart(HttpContext.Session);

            return RedirectToAction("Confirmation", new { orderId = order.Id });
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return NotFound();

            ViewBag.CartCount = 0;
            return View(order);
        }
    }
}
