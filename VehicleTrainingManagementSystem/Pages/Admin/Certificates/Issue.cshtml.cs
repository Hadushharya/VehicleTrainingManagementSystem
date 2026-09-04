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
    public class IssueModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly CertificateService _certService;

        public IssueModel(ApplicationDbContext db, CertificateService certService)
        {
            _db = db;
            _certService = certService;
        }

        [BindProperty]
        public int SessionId { get; set; }

        public TrainingSession? Session { get; set; }

        public async Task<IActionResult> OnGetAsync(int sessionId)
        {
            SessionId = sessionId;
            Session = await _db.TrainingSessions
                .Include(s => s.Trainer)
                .Include(s => s.TrainingType)
                .Include(s => s.Certificate)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (Session?.Certificate != null)
            {
                TempData["Message"] = "This session already has a certificate issued.";
                return RedirectToPage("Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var session = await _db.TrainingSessions
                .Include(s => s.Trainer)
                .Include(s => s.TrainingType)
                .FirstOrDefaultAsync(s => s.Id == SessionId);

            if (session == null || session.Trainer == null || session.TrainingType == null)
            {
                TempData["Error"] = "Session not found or incomplete.";
                return RedirectToPage("/Trainers/Index");
            }

            string certNumber;
            do
            {
                certNumber = _certService.GenerateCertificateNumber();
            }
            while (await _db.Certificates.AnyAsync(c => c.CertificateNumber == certNumber));
            var verificationCode = Guid.NewGuid().ToString("N");

            var issuedAt = DateTime.Now;
            var certificate = new Certificate
            {
                TrainingSessionId = session.Id,
                CertificateNumber = certNumber,
                VerificationCode = verificationCode,
                IssuedAt = issuedAt,
                ExpiryDate = issuedAt.AddYears(4)
            };

            _db.Certificates.Add(certificate);

            session.Status = SessionStatus.Completed;
            session.EndDate = DateTime.Now;

            await _db.SaveChangesAsync();

            TempData["Message"] = $"Certificate {certNumber} issued successfully.";
            return RedirectToPage("View", new { id = certificate.Id });
        }
    }
}