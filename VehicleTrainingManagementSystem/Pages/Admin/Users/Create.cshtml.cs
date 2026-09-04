using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Data;
using VehicleTrainingManagementSystem.Models;
using VehicleTrainingManagementSystem.Services;

namespace VehicleTrainingManagementSystem.Pages.Admin.Users
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
            [Required, MaxLength(50)]
            public string Username { get; set; } = string.Empty;

            [MaxLength(100)]
            public string? FullName { get; set; }

            [Required, MinLength(6)]
            public string Password { get; set; } = string.Empty;

            [Required]
            public UserRole Role { get; set; } = UserRole.Reception;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (await _db.Users.AnyAsync(u => u.Username == Input.Username))
            {
                ModelState.AddModelError("Input.Username", "Username already exists.");
                return Page();
            }

            var user = new User
            {
                Username = Input.Username,
                FullName = Input.FullName,
                PasswordHash = PasswordHasher.Hash(Input.Password),
                Role = Input.Role,
                IsActive = true
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            TempData["Message"] = $"User '{user.Username}' created successfully.";
            return RedirectToPage("Index");
        }
    }
}