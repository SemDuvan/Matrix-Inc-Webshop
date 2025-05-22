using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Text.Json;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class HomeModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public Customer? Customer { get; set; }
        public List<Product> Products { get; set; } = new();
        public string? UserName { get; set; }

        public HomeModel(ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public void OnGet(string? userName)
        {
            if (string.IsNullOrEmpty(userName))
                userName = HttpContext.Session.GetString("UserName");

            if (!string.IsNullOrEmpty(userName))
            {
                Customer = _customerRepository.GetAllCustomers()
                    .FirstOrDefault(c => c.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
            }
            Products = _productRepository.GetAllProducts().ToList();
            UserName = userName;
        }

        public IActionResult OnPostAddToCart(int productId)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

            var product = _productRepository.GetProductById(productId);
            if (product != null)
            {
                var existing = cart.FirstOrDefault(c => c.ProductId == product.Id);
                if (existing != null)
                    existing.Quantity++;
                else
                    cart.Add(new CartItem { ProductId = product.Id, Name = product.Name, Price = product.Price, Quantity = 1 });
            }

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            TempData["CartMessage"] = "Product toegevoegd aan winkelwagen!";
            return RedirectToPage();
        }
    }
}
