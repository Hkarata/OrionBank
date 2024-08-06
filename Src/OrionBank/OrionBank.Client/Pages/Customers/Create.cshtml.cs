using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrionBank.Client.Models;

namespace OrionBank.Client.Pages.Customers
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CustomerModel? Customer { get; set; }

        public void OnGet()
        {

        }
    }
}
