using ShopEase.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopEase.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public string? SearchQuery { get; set; }
        public int? CategoryId { get; set; }
        public string SortBy { get; set; } = "newest";
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

    public class ProductDetailViewModel
    {
        public Product Product { get; set; } = null!;
        public IEnumerable<Product> RelatedProducts { get; set; } = new List<Product>();
    }

    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Full Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Zip Code is required")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a payment method")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = "COD";

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }
    }

    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class AdminProductViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 999999)]
        public decimal Price { get; set; }

        [Range(0, 999999)]
        public decimal? DiscountPrice { get; set; }

        [Required]
        [Range(0, 10000)]
        public int Stock { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        public bool IsActive { get; set; } = true;
    }
}
