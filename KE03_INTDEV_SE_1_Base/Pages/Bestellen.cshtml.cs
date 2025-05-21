using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class BestellenModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public BestellenModel(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        [BindProperty]
        public string CartJson { get; set; } = "";

        public IActionResult OnPostPlaatsBestelling()
        {
            var userName = HttpContext.Session.GetString("UserName");
            var customer = _customerRepository.GetAllCustomers()
                .FirstOrDefault(c => c.Name == userName);

            if (customer == null)
            {
                return new JsonResult(new { success = false, message = "Geen geldige klant gevonden." });
            }

            List<CartItem>? cartItems = null;
            try
            {
                cartItems = JsonSerializer.Deserialize<List<CartItem>>(CartJson);
            }
            catch
            {
                return new JsonResult(new { success = false, message = "Ongeldige winkelwagen data." });
            }

            if (cartItems == null || cartItems.Count == 0)
            {
                return new JsonResult(new { success = false, message = "Winkelwagen is leeg." });
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerId = customer.Id,
                Customer = customer
            };

            foreach (var cartItem in cartItems)
            {
                var product = _productRepository.GetProductById(cartItem.ProductId);
                if (product != null)
                {
                    order.Products.Add(product);
                }
            }

            _orderRepository.AddOrder(order);

            // Geef een JSON-resultaat terug in plaats van een redirect
            return new JsonResult(new { success = true, message = "Bestelling geplaatst!" });
        }
    }
}
