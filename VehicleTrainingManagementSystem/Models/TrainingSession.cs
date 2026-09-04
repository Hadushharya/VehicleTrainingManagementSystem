using System.ComponentModel.DataAnnotations;

namespace VehicleTrainingManagementSystem.Models
{
    public enum SessionStatus
    {
        Registered,
        InProgress,
        Completed,
        Cancelled
    }

    public class TrainingSession
    {
        public int Id { get; set; }

        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        public int TrainingTypeId { get; set; }
        public TrainingType? TrainingType { get; set; }

        public SessionStatus Status { get; set; } = SessionStatus.Registered;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int StartedByUserId { get; set; }
        public User? StartedByUser { get; set; }

        public Certificate? Certificate { get; set; }
    }
}