using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VehicleTrainingManagementSystem.Data;
using VehicleTrainingManagementSystem.Models;

namespace VehicleTrainingManagementSystem.Pages.Admin.TrainingTypes
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required, MaxLength(100)]
            public string Name { get; set; } = string.Empty;

            [MaxLength(500)]
            public string? Description { get; set; }

            [Range(1, 365)]
            public int DurationInDays { get; set; } = 1;

            public List<CompetencyInput> Competencies { get; set; } = new();
        }

        public class CompetencyInput
        {
            public string? Code { get; set; }
            public string? Name { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var trainingType = new TrainingType
            {
                Name = Input.Name,
                Description = Input.Description,
                DurationInDays = Input.DurationInDays,
                IsActive = true
            };

            int order = 0;
            foreach (var c in Input.Competencies)
            {
                if (string.IsNullOrWhiteSpace(c.Code) && string.IsNullOrWhiteSpace(c.Name))
                    continue; // skip empty rows

                trainingType.Competencies.Add(new Competency
                {
                    Code = c.Code?.Trim() ?? string.Empty,
                    Name = c.Name?.Trim() ?? string.Empty,
                    DisplayOrder = order++
                });
            }

            _db.TrainingTypes.Add(trainingType);
            await _db.SaveChangesAsync();

            TempData["Message"] = $"Training type '{Input.Name}' created with {trainingType.Competencies.Count} competencies.";
            return RedirectToPage("Index");
        }
    }
}