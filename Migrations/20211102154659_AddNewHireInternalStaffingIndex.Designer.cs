﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewJobSurveyAdmin.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NewJobSurveyAdmin.Migrations
{
    [DbContext(typeof(NewJobSurveyAdminContext))]
    [Migration("20211102154659_AddNewHireInternalStaffingIndex")]
    partial class AddNewHireInternalStaffingIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("NewJobSurveyAdmin.Models.AdminSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedTs")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedTs")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AdminSettings");
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Age")
                        .HasColumnType("text");

                    b.Property<string>("AppointmentStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ChipsCity")
                        .HasColumnType("text");

                    b.Property<string>("ChipsEmail")
                        .HasColumnType("text");

                    b.Property<string>("ChipsFirstName")
                        .HasColumnType("text");

                    b.Property<string>("ChipsLastName")
                        .HasColumnType("text");

                    b.Property<string>("Classification")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedTs")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CurrentEmployeeStatusCode")
                        .HasColumnType("text");

                    b.Property<DateTime>("DeadlineDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DepartmentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DepartmentIdDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DevelopmentRegion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GovernmentEmail")
                        .HasColumnType("text");

                    b.Property<string>("GovernmentEmployeeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("InviteDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("JobClassificationGroup")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("JobCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("LdapCity")
                        .HasColumnType("text");

                    b.Property<string>("LdapEmail")
                        .HasColumnType("text");

                    b.Property<string>("LdapFirstName")
                        .HasColumnType("text");

                    b.Property<string>("LdapLastName")
                        .HasColumnType("text");

                    b.Property<string>("LdapOrganization")
                        .HasColumnType("text");

                    b.Property<string>("LocationCity")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LocationGroup")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedTs")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("NewHireOrInternalStaffing")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NocCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NocDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Organization")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OrganizationCount")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PositionCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PositionTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PreferredEmail")
                        .HasColumnType("text");

                    b.Property<bool>("PreferredEmailFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("PreferredFirstName")
                        .HasColumnType("text");

                    b.Property<bool>("PreferredFirstNameFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("PriorAppointmentStatus")
                        .HasColumnType("text");

                    b.Property<string>("PriorClassification")
                        .HasColumnType("text");

                    b.Property<string>("PriorDepartmentId")
                        .HasColumnType("text");

                    b.Property<string>("PriorDepartmentIdDescription")
                        .HasColumnType("text");

                    b.Property<string>("PriorEffectiveDate")
                        .HasColumnType("text");

                    b.Property<string>("PriorEmployeeStatus")
                        .HasColumnType("text");

                    b.Property<string>("PriorJobClassificationGroup")
                        .HasColumnType("text");

                    b.Property<string>("PriorJobCode")
                        .HasColumnType("text");

                    b.Property<string>("PriorNocCode")
                        .HasColumnType("text");

                    b.Property<string>("PriorNocDescription")
                        .HasColumnType("text");

                    b.Property<string>("PriorOrganization")
                        .HasColumnType("text");

                    b.Property<string>("PriorPositionCode")
                        .HasColumnType("text");

                    b.Property<string>("PriorPositionTitle")
                        .HasColumnType("text");

                    b.Property<string>("PriorUnionCode")
                        .HasColumnType("text");

                    b.Property<string>("RecordCount")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RegionalDistrict")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Reminder1Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Reminder2Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ServiceYears")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StaffingAction")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StaffingReason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TaToPermanent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Telkey")
                        .HasColumnType("text");

                    b.Property<bool>("TriedToUpdateInFinalState")
                        .HasColumnType("boolean");

                    b.Property<string>("UnionCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CurrentEmployeeStatusCode");

                    b.HasIndex("GovernmentEmployeeId");

                    b.HasIndex("NewHireOrInternalStaffing");

                    b.HasIndex("StaffingReason");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.EmployeeActionEnum", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("EmployeeActionEnums");
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.EmployeeStatusEnum", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("EmployeeStatusEnums");
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.EmployeeTimelineEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AdminUserName")
                        .HasColumnType("text");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedTs")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EmployeeActionCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<string>("EmployeeStatusCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedTs")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeActionCode");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeStatusCode");

                    b.ToTable("EmployeeTimelineEntries");
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.TaskEnum", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("TaskEnums");
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.TaskLogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedTs")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ModifiedTs")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("TaskCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TaskOutcomeCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TaskCode");

                    b.HasIndex("TaskOutcomeCode");

                    b.ToTable("TaskLogEntries");
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.TaskOutcomeEnum", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("TaskOutcomeEnums");
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.Employee", b =>
                {
                    b.HasOne("NewJobSurveyAdmin.Models.EmployeeStatusEnum", "CurrentEmployeeStatus")
                        .WithMany("Employees")
                        .HasForeignKey("CurrentEmployeeStatusCode")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.EmployeeTimelineEntry", b =>
                {
                    b.HasOne("NewJobSurveyAdmin.Models.EmployeeActionEnum", "EmployeeAction")
                        .WithMany("TimelineEntries")
                        .HasForeignKey("EmployeeActionCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NewJobSurveyAdmin.Models.Employee", "Employee")
                        .WithMany("TimelineEntries")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NewJobSurveyAdmin.Models.EmployeeStatusEnum", "EmployeeStatus")
                        .WithMany("TimelineEntries")
                        .HasForeignKey("EmployeeStatusCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("NewJobSurveyAdmin.Models.TaskLogEntry", b =>
                {
                    b.HasOne("NewJobSurveyAdmin.Models.TaskEnum", "Task")
                        .WithMany("TaskLogEntries")
                        .HasForeignKey("TaskCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NewJobSurveyAdmin.Models.TaskOutcomeEnum", "TaskOutcome")
                        .WithMany("TaskLogEntries")
                        .HasForeignKey("TaskOutcomeCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
