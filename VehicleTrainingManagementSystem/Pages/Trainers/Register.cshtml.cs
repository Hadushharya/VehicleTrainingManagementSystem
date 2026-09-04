using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Data;
using VehicleTrainingManagementSystem.Models;

namespace VehicleTrainingManagementSystem.Pages.Trainers
{
    [Authorize(Roles = "Admin,Reception")]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public RegisterModel(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<SelectListItem> TrainingTypeOptions { get; set; } = new();

        public class InputModel
        {
            [Required, MaxLength(100)]
            public string FullName { get; set; } = string.Empty;

            [Required]
            public Gender Gender { get; set; }

            public DateTime? DateOfBirth { get; set; }

            [MaxLength(30)]
            public string? NationalId { get; set; }

            [MaxLength(30)]
            public string? LaborId { get; set; }

            [MaxLength(20)]
            public string? PhoneNumber { get; set; }

            [MaxLength(100), EmailAddress]
            public string? Email { get; set; }

            [MaxLength(200)]
            public string? Address { get; set; }

            [MaxLength(30)]
            public string? DrivingLicenseLevel { get; set; }

            [MaxLength(30)]
            public string? DrivingLicenseNumber { get; set; }

            public IFormFile? Photo { get; set; }

            [Required(ErrorMessage = "Please select a training type.")]
            public int TrainingTypeId { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime TrainingStartDate { get; set; } = DateTime.Today;
        }

        public async Task OnGetAsync()
        {
            await LoadTrainingTypesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadTrainingTypesAsync();
                return Page();
            }

            string? photoPath = null;
            if (Input.Photo != null && Input.Photo.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var ext = Path.GetExtension(Input.Photo.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("Input.Photo", "Only JPG and PNG images are allowed.");
                    await LoadTrainingTypesAsync();
                    return Page();
                }

                if (Input.Photo.Length > 3 * 1024 * 1024) // 3 MB limit
                {
                    ModelState.AddModelError("Input.Photo", "Photo must be smaller than 3 MB.");
                    await LoadTrainingTypesAsync();
                    return Page();
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "trainers");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid():N}{ext}";
                var fullPath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await Input.Photo.CopyToAsync(stream);
                }

                photoPath = $"/uploads/trainers/{fileName}";
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var trainer = new Trainer
            {
                FullName = Input.FullName,
                Gender = Input.Gender,
                DateOfBirth = Input.DateOfBirth,
                NationalId = Input.NationalId,
                LaborId = Input.LaborId,
                PhoneNumber = Input.PhoneNumber,
                Email = Input.Email,
                Address = Input.Address,
                DrivingLicenseLevel = Input.DrivingLicenseLevel,
                DrivingLicenseNumber = Input.DrivingLicenseNumber,
                PhotoPath = photoPath,
                RegisteredByUserId = userId
            };
            _db.Trainers.Add(trainer);
            await _db.SaveChangesAsync();

            var session = new TrainingSession
            {
                TrainerId = trainer.Id,
                TrainingTypeId = Input.TrainingTypeId,
                Status = SessionStatus.InProgress,
                StartDate = Input.TrainingStartDate,
                StartedByUserId = userId
            };
            _db.TrainingSessions.Add(session);
            await _db.SaveChangesAsync();

            TempData["Message"] = $"Trainer '{trainer.FullName}' registered and training started.";
            return RedirectToPage("Index");
        }

        private async Task LoadTrainingTypesAsync()
        {
            TrainingTypeOptions = await _db.TrainingTypes
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })
                .ToListAsync();
        }
    }
}