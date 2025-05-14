using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICustomerRepository _customerRepository;

        [BindProperty]
        public string UserName { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public void OnGet()
        {
            // Alleen login tonen
        }

        public IActionResult OnPost()
        {
            var customer = _customerRepository.GetAllCustomers()
                .FirstOrDefault(c => c.Name.Equals(UserName, StringComparison.OrdinalIgnoreCase));

            if (customer != null)
            {
                // Zet de gebruikersnaam in de session
                HttpContext.Session.SetString("UserName", customer.Name);

                // Redirect naar Home met gebruikersnaam als querystring
                return RedirectToPage("Home", new { UserName = customer.Name });
            }
            else
            {
                ErrorMessage = "Gebruikersnaam niet gevonden.";
                return Page();
            }
        }

    }
}
