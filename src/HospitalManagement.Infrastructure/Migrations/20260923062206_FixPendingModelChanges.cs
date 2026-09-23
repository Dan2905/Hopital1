using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Admissions_LitId",
                table: "Admissions");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_PatientId",
                table: "Admissions");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSortiePrevue",
                table: "Admissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_LitId_Statut",
                table: "Admissions",
                columns: new[] { "LitId", "Statut" });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientId_DateEntree",
                table: "Admissions",
                columns: new[] { "PatientId", "DateEntree" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Admissions_LitId_Statut",
                table: "Admissions");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_PatientId_DateEntree",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "DateSortiePrevue",
                table: "Admissions");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_LitId",
                table: "Admissions",
                column: "LitId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientId",
                table: "Admissions",
                column: "PatientId");
        }
    }
}
