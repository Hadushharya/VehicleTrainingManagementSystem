using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Data;
using VehicleTrainingManagementSystem.Models;

namespace VehicleTrainingManagementSystem.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        public List<User> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            Users = await _db.Users.OrderBy(u => u.Role).ThenBy(u => u.Username).ToListAsync();
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                await _db.SaveChangesAsync();
                TempData["Message"] = $"User '{user.Username}' {(user.IsActive ? "enabled" : "disabled")}.";
            }
            return RedirectToPage();
        }
    }
}