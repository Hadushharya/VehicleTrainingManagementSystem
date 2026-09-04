using System.ComponentModel.DataAnnotations;

namespace VehicleTrainingManagementSystem.Models
{
    public class TrainingType
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty; // e.g. "Vehicle Driving"

        [MaxLength(500)]
        public string? Description { get; set; }

        public int DurationInDays { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Competency> Competencies { get; set; } = new List<Competency>();
    }
}