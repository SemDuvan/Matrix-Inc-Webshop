using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class BestellingenModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        public List<Order> Orders { get; set; } = new();

        public BestellingenModel(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public void OnGet()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(userName))
            {
                var customer = _customerRepository.GetAllCustomers()
                    .FirstOrDefault(c => c.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
                if (customer != null)
                {
                    Orders = _orderRepository.GetAllOrders()
                        .Where(o => o.CustomerId == customer.Id)
                        .OrderByDescending(o => o.OrderDate) 
                        .ToList();
                }
            }
        }

    }
}
