using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class HomeModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public Customer? Customer { get; set; }
        public List<Product> Products { get; set; } = new();

        public HomeModel(ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public void OnGet(string? userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                Customer = _customerRepository.GetAllCustomers()
                    .FirstOrDefault(c => c.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
            }
            Products = _productRepository.GetAllProducts().ToList();
        }
    }
}
