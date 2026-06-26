using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopEase.Data;

namespace ShopEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = await _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Stock > 0)
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync();

            var categories = await _db.Categories.ToListAsync();

            ViewBag.FeaturedProducts = featuredProducts;
            ViewBag.Categories = categories;
            ViewBag.CartCount = CartHelper.GetCartCount(HttpContext.Session);

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
