# ShopEase - E-Commerce Project
**Stack:** ASP.NET MVC 8 + Entity Framework Core + SQL Server + Bootstrap 5 + Vanilla JS

---

## 📦 Project Structure

```
ShopEase/
├── Controllers/
│   ├── HomeController.cs         # Landing page
│   ├── ProductController.cs      # Product listing & detail
│   ├── CartController.cs         # Session-based cart
│   ├── CheckoutController.cs     # Order placement
│   └── AdminController.cs        # Admin panel
├── Models/
│   └── Models.cs                 # Product, Category, Order, OrderItem, CartItem
├── ViewModels/
│   └── ViewModels.cs             # ProductListVM, CheckoutVM, AdminProductVM
├── Data/
│   ├── AppDbContext.cs           # EF Core DbContext + Seeder
│   └── CartHelper.cs             # Session-based cart logic
├── Views/
│   ├── Home/Index.cshtml         # Landing page
│   ├── Product/Index.cshtml      # Product listing w/ filters
│   ├── Product/Detail.cshtml     # Product detail page
│   ├── Cart/Index.cshtml         # Shopping cart
│   ├── Checkout/Index.cshtml     # Checkout form
│   ├── Checkout/Confirmation.cshtml
│   └── Admin/                    # Dashboard, Products, Orders
├── wwwroot/
│   ├── css/site.css              # Custom styles
│   └── js/site.js                # Cart AJAX & animations
└── Program.cs                    # App config & DI
```

---

## 🚀 Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full)
- Visual Studio 2022 or VS Code

### Steps

1. **Clone / Open project**
   ```bash
   cd ShopEase
   ```

2. **Update connection string** in `appsettings.json`:
   ```json
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ShopEaseDB;Trusted_Connection=True;"
   ```
   For SQL Server Express:
   ```json
   "DefaultConnection": "Server=.\\SQLEXPRESS;Database=ShopEaseDB;Trusted_Connection=True;"
   ```

3. **Restore packages**
   ```bash
   dotnet restore
   ```

4. **Run the project** (database is auto-created and seeded on first run)
   ```bash
   dotnet run
   ```

5. **Open browser** at `https://localhost:5001` or `http://localhost:5000`

---

## ✅ Features

### Customer Side
| Feature | Description |
|---------|-------------|
| 🏠 Home Page | Hero banner, featured products, categories, promo section |
| 🛍️ Product Listing | Filter by category, search, sort (price/name/newest), pagination |
| 📄 Product Detail | Images, price, stock status, quantity picker, related products |
| 🛒 Cart | Session-based cart, update qty, remove items, shipping calculation |
| 📦 Checkout | Full form validation, 3 payment methods, order summary |
| ✅ Confirmation | Order confirmed page with details |

### Admin Panel (`/Admin`)
| Feature | Description |
|---------|-------------|
| 📊 Dashboard | Stats cards (products, orders, revenue, pending) + recent orders |
| 📦 Products | List, create, edit, soft-delete products |
| 🧾 Orders | List with status filter, view details, update order status |

### Technical
- **Database:** SQL Server with Entity Framework Core, auto-migration
- **Seeded Data:** 12 products across 4 categories
- **Cart:** Session-based (no login required)
- **Validation:** Server + client-side (jQuery Unobtrusive)
- **Stock:** Auto-decremented on order placement

---

## 🗄️ Database Schema

```
Categories: Id, Name
Products: Id, Name, Description, Price, DiscountPrice, Stock, ImageUrl, CategoryId, IsActive, CreatedAt
Orders: Id, CustomerName, Email, Phone, Address, City, ZipCode, TotalAmount, Status, PaymentMethod, OrderDate
OrderItems: Id, OrderId, ProductId, Quantity, UnitPrice
```

---

## 🔗 Key URLs

| URL | Page |
|-----|------|
| `/` | Home |
| `/Product/Index` | All Products |
| `/Product/Detail/1` | Product Detail |
| `/Cart/Index` | Shopping Cart |
| `/Checkout/Index` | Checkout |
| `/Admin/Index` | Admin Dashboard |
| `/Admin/Products` | Manage Products |
| `/Admin/Orders` | Manage Orders |
