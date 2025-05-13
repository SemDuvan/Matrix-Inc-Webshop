using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public IList<Customer> Customers { get; set; }
        public IList<Product> Products { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            Customers = new List<Customer>();
            Products = new List<Product>();
        }

        public void OnGet()
        {
            Customers = _customerRepository.GetAllCustomers().ToList();
            Products = _productRepository.GetAllProducts().ToList();

            _logger.LogInformation($"Getting all {Customers.Count} customers and {Products.Count} products");
        }
    }

}
