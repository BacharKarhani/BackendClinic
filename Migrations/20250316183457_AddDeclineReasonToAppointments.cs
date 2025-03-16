using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendclinic.Migrations
{
    /// <inheritdoc />
    public partial class AddDeclineReasonToAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Users_UserId",
                table: "HealthRecords");

            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_UserId",
                table: "HealthRecords");

            migrationBuilder.AddColumn<string>(
                name: "DeclineReason",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeclineReason",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_UserId",
                table: "HealthRecords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Users_UserId",
                table: "HealthRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
