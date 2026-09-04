using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Data;
using VehicleTrainingManagementSystem.Models;
using VehicleTrainingManagementSystem.Services;

namespace VehicleTrainingManagementSystem.Pages.Admin.Certificates
{
    [Authorize(Roles = "Admin")]
    public class ViewModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly CertificateService _certService;

        public ViewModel(ApplicationDbContext db, CertificateService certService)
        {
            _db = db;
            _certService = certService;
        }

        public Certificate? Certificate { get; set; }
        public string QrCodeBase64 { get; set; } = string.Empty;

        public async Task OnGetAsync(int id)
        {
            Certificate = await _db.Certificates
                .Include(c => c.TrainingSession)
                    .ThenInclude(s => s!.Trainer)
                .Include(c => c.TrainingSession)
                    .ThenInclude(s => s!.TrainingType)
                        .ThenInclude(t => t!.Competencies)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Certificate != null)
            {
                var verifyUrl = Url.Page(
                    "/Verify/Index",
                    pageHandler: null,
                    values: new { code = Certificate.VerificationCode },
                    protocol: Request.Scheme,
                    host: Request.Host.Value);

                QrCodeBase64 = _certService.GenerateQrCodeBase64(verifyUrl!);
            }
        }
    }
}