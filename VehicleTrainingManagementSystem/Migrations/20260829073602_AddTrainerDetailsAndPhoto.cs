using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleTrainingManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerDetailsAndPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DrivingLicenseLevel",
                table: "Trainers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicenseNumber",
                table: "Trainers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LaborId",
                table: "Trainers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Trainers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrivingLicenseLevel",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "DrivingLicenseNumber",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "LaborId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Trainers");
        }
    }
}
