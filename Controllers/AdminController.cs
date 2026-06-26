using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopEase.Data;
using ShopEase.Models;
using ShopEase.ViewModels;

namespace ShopEase.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        // Session key
        private const string AdminSessionKey = "IsAdminLoggedIn";

        public AdminController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // ─── Helper: check if admin is logged in ───────────────────────────
        private bool IsAdminLoggedIn()
            => HttpContext.Session.GetString(AdminSessionKey) == "true";

        private IActionResult RedirectToLogin()
            => RedirectToAction("Login");

        // ─── LOGIN ─────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Login()
        {
            if (IsAdminLoggedIn())
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var adminUsername = _config["AdminCredentials:Username"] ?? "admin";
            var adminPassword = _config["AdminCredentials:Password"] ?? "admin123";

            if (vm.Username == adminUsername && vm.Password == adminPassword)
            {
                HttpContext.Session.SetString(AdminSessionKey, "true");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(vm);
        }

        // ─── LOGOUT ────────────────────────────────────────────────────────
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(AdminSessionKey);
            return RedirectToAction("Login");
        }

        // ─── DASHBOARD ─────────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            ViewBag.TotalProducts = await _db.Products.CountAsync();
            ViewBag.TotalOrders = await _db.Orders.CountAsync();
            ViewBag.TotalRevenue = await _db.Orders
                .Where(o => o.Status != "Cancelled")
                .SumAsync(o => o.TotalAmount);
            ViewBag.PendingOrders = await _db.Orders
                .CountAsync(o => o.Status == "Pending");

            var recentOrders = await _db.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentOrders = recentOrders;
            return View();
        }

        // ─── PRODUCTS ──────────────────────────────────────────────────────
        public async Task<IActionResult> Products()
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            var products = await _db.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            var vm = new AdminProductViewModel
            {
                Categories = await _db.Categories.ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(AdminProductViewModel vm)
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                vm.Categories = await _db.Categories.ToListAsync();
                return View(vm);
            }

            var product = new Product
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                DiscountPrice = vm.DiscountPrice,
                Stock = vm.Stock,
                ImageUrl = string.IsNullOrEmpty(vm.ImageUrl) ? "/images/placeholder.jpg" : vm.ImageUrl,
                CategoryId = vm.CategoryId,
                IsActive = vm.IsActive,
                CreatedAt = DateTime.Now
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Product created successfully!";
            return RedirectToAction("Products");
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            var vm = new AdminProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                Categories = await _db.Categories.ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(AdminProductViewModel vm)
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                vm.Categories = await _db.Categories.ToListAsync();
                return View(vm);
            }

            var product = await _db.Products.FindAsync(vm.Id);
            if (product == null) return NotFound();

            product.Name = vm.Name;
            product.Description = vm.Description;
            product.Price = vm.Price;
            product.DiscountPrice = vm.DiscountPrice;
            product.Stock = vm.Stock;
            product.ImageUrl = string.IsNullOrEmpty(vm.ImageUrl) ? "/images/placeholder.jpg" : vm.ImageUrl;
            product.CategoryId = vm.CategoryId;
            product.IsActive = vm.IsActive;

            await _db.SaveChangesAsync();
            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction("Products");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                product.IsActive = false;
                await _db.SaveChangesAsync();
                TempData["Success"] = "Product removed.";
            }
            return RedirectToAction("Products");
        }

        // ─── ORDERS ────────────────────────────────────────────────────────
        public async Task<IActionResult> Orders(string? status)
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            var query = _db.Orders.AsQueryable();
            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status == status);

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            ViewBag.StatusFilter = status;
            return View(orders);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            var order = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            if (!IsAdminLoggedIn()) return RedirectToLogin();

            var order = await _db.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Order #{id} status updated to {status}.";
            }
            return RedirectToAction("OrderDetail", new { id });
        }
    }
}
