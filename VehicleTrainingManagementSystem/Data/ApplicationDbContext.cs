using Microsoft.EntityFrameworkCore;
using VehicleTrainingManagementSystem.Models;

namespace VehicleTrainingManagementSystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<TrainingType> TrainingTypes => Set<TrainingType>();
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<Competency> Competencies => Set<Competency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Certificate>()
            .HasIndex(c => c.CertificateNumber)
            .IsUnique();

        modelBuilder.Entity<Certificate>()
            .HasIndex(c => c.VerificationCode)
            .IsUnique();

        modelBuilder.Entity<TrainingSession>()
            .HasOne(t => t.Certificate)
            .WithOne(c => c.TrainingSession)
            .HasForeignKey<Certificate>(c => c.TrainingSessionId);

        // Avoid multiple cascade paths (SQL Server will reject them otherwise)
        modelBuilder.Entity<Trainer>()
            .HasOne(t => t.RegisteredByUser)
            .WithMany()
            .HasForeignKey(t => t.RegisteredByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TrainingSession>()
            .HasOne(t => t.StartedByUser)
            .WithMany()
            .HasForeignKey(t => t.StartedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}