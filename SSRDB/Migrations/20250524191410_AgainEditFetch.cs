using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSRDB.Migrations
{
    /// <inheritdoc />
    public partial class AgainEditFetch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Diagnoses_DiagnosisId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DiagnosisId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DiagnosisId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Diagnoses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_AppointmentId",
                table: "Diagnoses",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Appointments_AppointmentId",
                table: "Diagnoses",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Appointments_AppointmentId",
                table: "Diagnoses");

            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_AppointmentId",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Diagnoses");

            migrationBuilder.AddColumn<int>(
                name: "DiagnosisId",
                table: "Appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DiagnosisId",
                table: "Appointments",
                column: "DiagnosisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Diagnoses_DiagnosisId",
                table: "Appointments",
                column: "DiagnosisId",
                principalTable: "Diagnoses",
                principalColumn: "DiagnosisId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
