using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Data;

namespace VehicleTrainingManagementSystem.Pages.Trainers
{
    [Authorize(Roles = "Admin,Reception")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        public List<TrainerRow> Rows { get; set; } = new();

        public class TrainerRow
        {
            public int SessionId { get; set; }
            public string TrainerName { get; set; } = string.Empty;
            public string? Phone { get; set; }
            public string? NationalId { get; set; }
            public string? PhotoPath { get; set; }
            public string TrainingTypeName { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string StatusColor { get; set; } = "secondary";
            public DateTime? StartDate { get; set; }
            public string RegisteredBy { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            var sessions = await _db.TrainingSessions
                .Include(s => s.Trainer)
                .ThenInclude(t => t!.RegisteredByUser)
                .Include(s => s.TrainingType)
                .OrderByDescending(s => s.Id)
                .ToListAsync();

            Rows = sessions.Select(s => new TrainerRow
            {
                SessionId = s.Id,
                TrainerName = s.Trainer?.FullName ?? "-",
                Phone = s.Trainer?.PhoneNumber,
                NationalId = s.Trainer?.NationalId,
                PhotoPath = s.Trainer?.PhotoPath,
                TrainingTypeName = s.TrainingType?.Name ?? "-",
                Status = s.Status.ToString(),
                StatusColor = s.Status switch
                {
                    Models.SessionStatus.Registered => "secondary",
                    Models.SessionStatus.InProgress => "warning",
                    Models.SessionStatus.Completed => "success",
                    Models.SessionStatus.Cancelled => "danger",
                    _ => "secondary"
                },
                StartDate = s.StartDate,
                RegisteredBy = s.Trainer?.RegisteredByUser?.Username ?? "-"
            }).ToList();
        }
    }
}