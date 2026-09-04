using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Data;
using VehicleTrainingManagementSystem.Models;

namespace VehicleTrainingManagementSystem.Pages.Admin.Certificates
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        public List<Certificate> Certificates { get; set; } = new();

        public async Task OnGetAsync()
        {
            Certificates = await _db.Certificates
                .Include(c => c.TrainingSession)
                    .ThenInclude(s => s!.Trainer)
                .Include(c => c.TrainingSession)
                    .ThenInclude(s => s!.TrainingType)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }
    }
}