using System.ComponentModel.DataAnnotations;

namespace VehicleTrainingManagementSystem.Models
{
    public class Competency
    {
        public int Id { get; set; }

        public int TrainingTypeId { get; set; }
        public TrainingType? TrainingType { get; set; }

        [Required, MaxLength(30)]
        public string Code { get; set; } = string.Empty; // e.g. "VED AUTD 001"

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty; // e.g. "Developing Defensive Driving Behavior"

        public int DisplayOrder { get; set; }
    }
}