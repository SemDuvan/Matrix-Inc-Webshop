using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class BestellenModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public List<CartItem> Cart { get; set; } = new();

        public BestellenModel(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public void OnGet()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            Cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        public IActionResult OnPost()
        {
            var userName = HttpContext.Session.GetString("UserName");
            var customer = _customerRepository.GetAllCustomers()
                .FirstOrDefault(c => c.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (customer == null)
            {
                TempData["OrderMessage"] = "Geen geldige klant gevonden.";
                return RedirectToPage();
            }

            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                TempData["OrderMessage"] = "Je winkelwagen is leeg.";
                return RedirectToPage();
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerId = customer.Id,
                Customer = customer
            };

            foreach (var item in cart)
            {
                var product = _productRepository.GetProductById(item.ProductId);
                if (product != null)
                {
                    order.Products.Add(product);
                }
            }

            _orderRepository.AddOrder(order);

            // Leeg de winkelwagen
            HttpContext.Session.Remove("Cart");

            TempData["OrderMessage"] = "Bestelling geplaatst!";
            return RedirectToPage("/Bestellingen");
        }
    }
}
