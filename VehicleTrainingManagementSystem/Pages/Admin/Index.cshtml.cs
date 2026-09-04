using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Data;
using VehicleTrainingManagementSystem.Models;

namespace VehicleTrainingManagementSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        public int ReceptionCount { get; set; }
        public int TrainingTypeCount { get; set; }
        public int TrainerCount { get; set; }
        public int CertificateCount { get; set; }

        public async Task OnGetAsync()
        {
            ReceptionCount = await _db.Users.CountAsync(u => u.Role == UserRole.Reception && u.IsActive);
            TrainingTypeCount = await _db.TrainingTypes.CountAsync(t => t.IsActive);
            TrainerCount = await _db.Trainers.CountAsync();
            CertificateCount = await _db.Certificates.CountAsync();
        }
    }
}