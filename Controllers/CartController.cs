using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopEase.Data;
using ShopEase.Models;

namespace ShopEase.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var cart = CartHelper.GetCart(HttpContext.Session);
            ViewBag.CartCount = CartHelper.GetCartCount(HttpContext.Session);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null || !product.IsActive)
                return Json(new { success = false, message = "Product not found" });

            if (product.Stock < quantity)
                return Json(new { success = false, message = "Not enough stock" });

            var item = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.FinalPrice,
                Quantity = quantity,
                ImageUrl = product.ImageUrl
            };

            CartHelper.AddToCart(HttpContext.Session, item);
            var count = CartHelper.GetCartCount(HttpContext.Session);

            return Json(new { success = true, message = "Added to cart!", cartCount = count });
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            CartHelper.RemoveFromCart(HttpContext.Session, productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            CartHelper.UpdateQuantity(HttpContext.Session, productId, quantity);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            CartHelper.ClearCart(HttpContext.Session);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            return Json(CartHelper.GetCartCount(HttpContext.Session));
        }
    }
}
