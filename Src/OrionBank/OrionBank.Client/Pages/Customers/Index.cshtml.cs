using Microsoft.AspNetCore.Mvc.RazorPages;
using OrionBank.Abstractions.Entities;
using OrionBank.Client.Services;

namespace OrionBank.Client.Pages.Customers
{
    public class IndexModel(CustomerManagerService customerManager) : PageModel
    {
        public HashSet<Customer>? Customers { get; set; }

        public async Task OnGet()
        {
            Customers = await customerManager.GetAllCustomers();
        }
    }
}
