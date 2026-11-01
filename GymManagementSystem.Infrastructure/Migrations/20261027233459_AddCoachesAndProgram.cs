using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoachesAndProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WithProgram",
                table: "Plans");

            migrationBuilder.AddColumn<decimal>(
                name: "ProgramPrice",
                table: "Plans",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgramPrice",
                table: "Plans");

            migrationBuilder.AddColumn<bool>(
                name: "WithProgram",
                table: "Plans",
                type: "boolean",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: false);
        }
    }
}
