using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VehicleTrainingManagementSystem.Models;

namespace VehicleTrainingManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole(nameof(UserRole.Admin)))
                    return RedirectToPage("/Admin/Index");

                if (User.IsInRole(nameof(UserRole.Reception)))
                    return RedirectToPage("/Reception/Index");
            }

            return RedirectToPage("/Account/Login");
        }
    }
}