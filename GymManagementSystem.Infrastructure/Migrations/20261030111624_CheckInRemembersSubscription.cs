using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CheckInRemembersSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "CheckIns",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_SubscriptionId",
                table: "CheckIns",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckIns_Subscriptions_SubscriptionId",
                table: "CheckIns",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckIns_Subscriptions_SubscriptionId",
                table: "CheckIns");

            migrationBuilder.DropIndex(
                name: "IX_CheckIns_SubscriptionId",
                table: "CheckIns");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "CheckIns");
        }
    }
}
