using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopEase.Data;
using ShopEase.ViewModels;

namespace ShopEase.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private const int PageSize = 9;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId, string sortBy = "newest", int page = 1)
        {
            var query = _db.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name" => query.OrderBy(p => p.Name),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var total = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var vm = new ProductListViewModel
            {
                Products = products,
                Categories = await _db.Categories.ToListAsync(),
                SearchQuery = search,
                CategoryId = categoryId,
                SortBy = sortBy,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)PageSize),
                TotalCount = total
            };

            ViewBag.CartCount = CartHelper.GetCartCount(HttpContext.Session);
            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null) return NotFound();

            var related = await _db.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != id && p.IsActive)
                .Take(4)
                .ToListAsync();

            var vm = new ProductDetailViewModel
            {
                Product = product,
                RelatedProducts = related
            };

            ViewBag.CartCount = CartHelper.GetCartCount(HttpContext.Session);
            return View(vm);
        }
    }
}
