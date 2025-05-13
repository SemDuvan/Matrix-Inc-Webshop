using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public Customer? Customer { get; set; }

        public HomeModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IActionResult OnGet(string? userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                // Geen gebruikersnaam meegegeven, terug naar login
                return RedirectToPage("Index");
            }

            Customer = _customerRepository.GetAllCustomers()
                .FirstOrDefault(c => c.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (Customer == null)
            {
                // Gebruiker niet gevonden, terug naar login
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
