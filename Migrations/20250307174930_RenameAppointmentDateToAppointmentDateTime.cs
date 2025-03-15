using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendclinic.Migrations
{
    /// <inheritdoc />
    public partial class RenameAppointmentDateToAppointmentDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "Appointments",
                newName: "AppointmentDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentDateTime",
                table: "Appointments",
                newName: "AppointmentDate");
        }
    }
}
