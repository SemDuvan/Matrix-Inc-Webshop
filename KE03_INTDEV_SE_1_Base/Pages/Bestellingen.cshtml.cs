using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class BestellingenModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public List<Order> Orders { get; set; } = new();

        public BestellingenModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void OnGet()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(userName))
            {
                var customer = _customerRepository.GetAllCustomers()
                    .FirstOrDefault(c => c.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
                if (customer != null && customer.Orders != null)
                {
                    Orders = customer.Orders.ToList();
                }
            }
        }
    }
}
