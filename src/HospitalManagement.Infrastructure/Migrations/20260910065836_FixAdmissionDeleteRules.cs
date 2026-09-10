using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAdmissionDeleteRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Chambres_ChambreId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Lits_LitId",
                table: "Admissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Chambres_ChambreId",
                table: "Admissions",
                column: "ChambreId",
                principalTable: "Chambres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Lits_LitId",
                table: "Admissions",
                column: "LitId",
                principalTable: "Lits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Chambres_ChambreId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Lits_LitId",
                table: "Admissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Chambres_ChambreId",
                table: "Admissions",
                column: "ChambreId",
                principalTable: "Chambres",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Lits_LitId",
                table: "Admissions",
                column: "LitId",
                principalTable: "Lits",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
