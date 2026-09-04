using System.ComponentModel.DataAnnotations;

namespace VehicleTrainingManagementSystem.Models
{
    public class Certificate
    {
        public int Id { get; set; }

        public int TrainingSessionId { get; set; }
        public TrainingSession? TrainingSession { get; set; }

        [Required, MaxLength(50)]
        public string CertificateNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string VerificationCode { get; set; } = string.Empty;

        public DateTime IssuedAt { get; set; } = DateTime.Now;

        public DateTime ExpiryDate { get; set; }

        [MaxLength(300)]
        public string? PdfFilePath { get; set; }
    }
}