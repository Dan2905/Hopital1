using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lits_Chambres_ChambreId",
                table: "Lits");

            migrationBuilder.AddForeignKey(
                name: "FK_Lits_Chambres_ChambreId",
                table: "Lits",
                column: "ChambreId",
                principalTable: "Chambres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lits_Chambres_ChambreId",
                table: "Lits");

            migrationBuilder.AddForeignKey(
                name: "FK_Lits_Chambres_ChambreId",
                table: "Lits",
                column: "ChambreId",
                principalTable: "Chambres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
