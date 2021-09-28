using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NewJobSurveyAdmin.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedTs = table.Column<DateTime>(nullable: false),
                    ModifiedTs = table.Column<DateTime>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AdminSettings", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "EmployeeActionEnums",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_EmployeeActionEnums", x => x.Code); });

            migrationBuilder.CreateTable(
                name: "EmployeeStatusEnums",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_EmployeeStatusEnums", x => x.Code); });

            migrationBuilder.CreateTable(
                name: "TaskEnums",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_TaskEnums", x => x.Code); });

            migrationBuilder.CreateTable(
                name: "TaskOutcomeEnums",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_TaskOutcomeEnums", x => x.Code); });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedTs = table.Column<DateTime>(nullable: false),
                    ModifiedTs = table.Column<DateTime>(nullable: false),
                    Telkey = table.Column<string>(nullable: true),
                    GovernmentEmployeeId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    PreferredFirstName = table.Column<string>(nullable: false),
                    PreferredFirstNameFlag = table.Column<bool>(nullable: false),
                    MiddleName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    Age = table.Column<string>(nullable: false),
                    ChipsEmail = table.Column<string>(nullable: true),
                    GovernmentEmail = table.Column<string>(nullable: true),
                    PreferredEmail = table.Column<string>(nullable: true),
                    PreferredEmailFlag = table.Column<bool>(nullable: false),
                    Classification = table.Column<string>(nullable: false),
                    Organization = table.Column<string>(nullable: false),
                    DepartmentId = table.Column<string>(nullable: false),
                    DepartmentIdDescription = table.Column<string>(nullable: false),
                    DevelopmentRegion = table.Column<string>(nullable: false),
                    LocationCity = table.Column<string>(nullable: false),
                    LocationGroup = table.Column<string>(nullable: false),
                    JobCode = table.Column<string>(nullable: false),
                    PositionCode = table.Column<string>(nullable: false),
                    PositionTitle = table.Column<string>(nullable: false),
                    JobClassificationGroup = table.Column<string>(nullable: false),
                    NocCode = table.Column<string>(nullable: false),
                    NocDescription = table.Column<string>(nullable: false),
                    OrganizationCount = table.Column<string>(nullable: false),
                    RegionalDistrict = table.Column<string>(nullable: false),
                    UnionCode = table.Column<string>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    AppointmentStatus = table.Column<string>(nullable: false),
                    ServiceYears = table.Column<string>(nullable: false),
                    RecordCount = table.Column<string>(nullable: false),
                    StaffingAction = table.Column<string>(nullable: false),
                    StaffingReason = table.Column<string>(nullable: false),
                    NewHireOrInternalStaffing = table.Column<string>(nullable: false),
                    TaToPermanent = table.Column<string>(nullable: false),
                    PriorAppointmentStatus = table.Column<string>(nullable: false),
                    PriorClassification = table.Column<string>(nullable: false),
                    PriorDepartmentId = table.Column<string>(nullable: false),
                    PriorDepartmentIdDescription = table.Column<string>(nullable: false),
                    PriorEffectiveDate = table.Column<string>(nullable: false),
                    PriorEmployeeStatus = table.Column<string>(nullable: false),
                    PriorJobClassificationGroup = table.Column<string>(nullable: false),
                    PriorJobCode = table.Column<string>(nullable: false),
                    PriorNocCode = table.Column<string>(nullable: false),
                    PriorNocDescription = table.Column<string>(nullable: false),
                    PriorOrganization = table.Column<string>(nullable: false),
                    PriorPositionCode = table.Column<string>(nullable: false),
                    PriorPositionTitle = table.Column<string>(nullable: false),
                    PriorUnionCode = table.Column<string>(nullable: false),
                    InviteDate = table.Column<DateTime>(nullable: false),
                    Reminder1Date = table.Column<DateTime>(nullable: false),
                    Reminder2Date = table.Column<DateTime>(nullable: false),
                    DeadlineDate = table.Column<DateTime>(nullable: false),
                    CurrentEmployeeStatusCode = table.Column<string>(nullable: false),
                    TriedToUpdateInFinalState = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeStatusEnums_CurrentEmployeeStatusCode",
                        column: x => x.CurrentEmployeeStatusCode,
                        principalTable: "EmployeeStatusEnums",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskLogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedTs = table.Column<DateTime>(nullable: false),
                    ModifiedTs = table.Column<DateTime>(nullable: false),
                    TaskCode = table.Column<string>(nullable: false),
                    TaskOutcomeCode = table.Column<string>(nullable: false),
                    Comment = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskLogEntries_TaskEnums_TaskCode",
                        column: x => x.TaskCode,
                        principalTable: "TaskEnums",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskLogEntries_TaskOutcomeEnums_TaskOutcomeCode",
                        column: x => x.TaskOutcomeCode,
                        principalTable: "TaskOutcomeEnums",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTimelineEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedTs = table.Column<DateTime>(nullable: false),
                    ModifiedTs = table.Column<DateTime>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    EmployeeActionCode = table.Column<string>(nullable: false),
                    EmployeeStatusCode = table.Column<string>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    AdminUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTimelineEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeTimelineEntries_EmployeeActionEnums_EmployeeActionC~",
                        column: x => x.EmployeeActionCode,
                        principalTable: "EmployeeActionEnums",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeTimelineEntries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeTimelineEntries_EmployeeStatusEnums_EmployeeStatusC~",
                        column: x => x.EmployeeStatusCode,
                        principalTable: "EmployeeStatusEnums",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CurrentEmployeeStatusCode",
                table: "Employees",
                column: "CurrentEmployeeStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_GovernmentEmployeeId",
                table: "Employees",
                column: "GovernmentEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTimelineEntries_EmployeeActionCode",
                table: "EmployeeTimelineEntries",
                column: "EmployeeActionCode");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTimelineEntries_EmployeeId",
                table: "EmployeeTimelineEntries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTimelineEntries_EmployeeStatusCode",
                table: "EmployeeTimelineEntries",
                column: "EmployeeStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogEntries_TaskCode",
                table: "TaskLogEntries",
                column: "TaskCode");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogEntries_TaskOutcomeCode",
                table: "TaskLogEntries",
                column: "TaskOutcomeCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminSettings");

            migrationBuilder.DropTable(
                name: "EmployeeTimelineEntries");

            migrationBuilder.DropTable(
                name: "TaskLogEntries");

            migrationBuilder.DropTable(
                name: "EmployeeActionEnums");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "TaskEnums");

            migrationBuilder.DropTable(
                name: "TaskOutcomeEnums");

            migrationBuilder.DropTable(
                name: "EmployeeStatusEnums");
        }
    }
}