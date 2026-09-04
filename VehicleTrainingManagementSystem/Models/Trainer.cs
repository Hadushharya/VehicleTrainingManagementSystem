using System.ComponentModel.DataAnnotations;

namespace VehicleTrainingManagementSystem.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Trainer
    {
        public int Id { get; set; }

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

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(30)]
        public string? DrivingLicenseLevel { get; set; }

        [MaxLength(30)]
        public string? DrivingLicenseNumber { get; set; }

        [MaxLength(300)]
        public string? PhotoPath { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        public int RegisteredByUserId { get; set; }
        public User? RegisteredByUser { get; set; }

        public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();
    }
}