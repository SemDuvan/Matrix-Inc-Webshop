using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class BestellenModel : PageModel
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly CartService _cartService;

    public BestellenModel(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        CartService cartService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _cartService = cartService;
    }

    public IActionResult OnPostPlaatsBestelling()
    {
        var userName = HttpContext.Session.GetString("UserName");
        var customer = _customerRepository.GetAllCustomers()
            .FirstOrDefault(c => c.Name == userName);

        if (customer == null)
        {
            // Foutafhandeling
            return RedirectToPage("/Fout");
        }

        var order = new Order
        {
            OrderDate = DateTime.Now,
            CustomerId = customer.Id,
            Customer = customer
            // Do not assign Products here
        };

        // Add products to the existing collection
        foreach (var cartItem in _cartService.Items)
        {
            var product = _productRepository.GetProductById(cartItem.ProductId);
            if (product != null)
            {
                order.Products.Add(product);
                // Let op: voor aantallen per product is een tussentabel nodig
            }
        }



        _orderRepository.AddOrder(order);
        // Leeg de winkelkar
        // _cartService.Clear(); // Voeg deze methode toe aan CartService indien nodig

        return RedirectToPage("/BestellingBevestigd");
    }
}
