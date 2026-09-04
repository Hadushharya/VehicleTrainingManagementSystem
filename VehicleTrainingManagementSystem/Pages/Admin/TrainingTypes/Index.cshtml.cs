using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Data;
using VehicleTrainingManagementSystem.Models;

namespace VehicleTrainingManagementSystem.Pages.Admin.TrainingTypes
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        public List<TrainingType> Types { get; set; } = new();

        public async Task OnGetAsync()
        {
            Types = await _db.TrainingTypes
                .Include(t => t.Competencies)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(int id)
        {
            var type = await _db.TrainingTypes.FindAsync(id);
            if (type != null)
            {
                type.IsActive = !type.IsActive;
                await _db.SaveChangesAsync();
                TempData["Message"] = $"'{type.Name}' {(type.IsActive ? "enabled" : "disabled")}.";
            }
            return RedirectToPage();
        }
    }
}