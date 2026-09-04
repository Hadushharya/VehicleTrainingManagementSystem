using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Data;
using VehicleTrainingManagementSystem.Models;

namespace VehicleTrainingManagementSystem.Pages.Verify
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        public Certificate? Certificate { get; set; }
        public bool IsExpired { get; set; }

        public async Task OnGetAsync(string? code)
        {
            if (string.IsNullOrEmpty(code)) return;

            Certificate = await _db.Certificates
                .Include(c => c.TrainingSession)
                    .ThenInclude(s => s!.Trainer)
                .Include(c => c.TrainingSession)
                    .ThenInclude(s => s!.TrainingType)
                .FirstOrDefaultAsync(c => c.VerificationCode == code);

            if (Certificate != null)
            {
                IsExpired = Certificate.ExpiryDate < DateTime.Now;
            }
        }
    }
}