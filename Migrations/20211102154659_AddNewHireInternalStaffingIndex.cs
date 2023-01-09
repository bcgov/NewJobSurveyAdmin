using Microsoft.EntityFrameworkCore.Migrations;

namespace NewJobSurveyAdmin.Migrations
{
    public partial class AddNewHireInternalStaffingIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Employees_NewHireOrInternalStaffing",
                table: "Employees",
                column: "NewHireOrInternalStaffing");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_NewHireOrInternalStaffing",
                table: "Employees");
        }
    }
}
