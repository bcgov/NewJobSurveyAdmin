using Microsoft.EntityFrameworkCore.Migrations;

namespace NewJobSurveyAdmin.Migrations
{
    public partial class AddCities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChipsCity",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LdapCity",
                table: "Employees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChipsCity",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LdapCity",
                table: "Employees");
        }
    }
}
