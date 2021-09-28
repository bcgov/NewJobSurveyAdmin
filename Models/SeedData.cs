using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewJobSurveyAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewJobSurveyAdmin.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new NewJobSurveyAdminContext(
                serviceProvider.GetRequiredService<DbContextOptions<NewJobSurveyAdminContext>>(),
                serviceProvider.GetRequiredService<EmployeeInfoLookupService>())
            )
            {
                // If the context contains any EmployeeStatuses already, it has
                // been seeded. Don't re-seed it.
                if (context.EmployeeStatusEnums.Any())
                {
                    return; // DB has been seeded
                }

                context.EmployeeStatusEnums.AddRange(
                    EmployeeStatusEnum.AllValues
                );

                context.EmployeeActionEnums.AddRange(
                    EmployeeActionEnum.AllValues
                );

                context.TaskEnums.AddRange(
                    TaskEnum.AllValues
                );

                context.TaskOutcomeEnums.AddRange(
                    TaskOutcomeEnum.AllValues
                );

                context.AdminSettings.AddRange(
                    new List<AdminSetting>
                    {
                        new AdminSetting()
                        {
                            Key = AdminSetting.DataPullDayOfWeek,
                            DisplayName = "Data pull day of week",
                            Value = "1"
                        },
                        new AdminSetting()
                        {
                            Key = AdminSetting.InviteDays,
                            DisplayName = "Number of days after data pull to send invitation",
                            Value = "3"
                        },
                        new AdminSetting()
                        {
                            Key = AdminSetting.Reminder1Days,
                            DisplayName = "Number of days after invitation to send first reminder",
                            Value = "3"
                        },
                        new AdminSetting()
                        {
                            Key = AdminSetting.Reminder2Days,
                            DisplayName = "Number of days after first reminder to send second reminder",
                            Value = "3"
                        },
                        new AdminSetting()
                        {
                            Key = AdminSetting.CloseDays,
                            DisplayName = "Number of days after second reminder to close survey",
                            Value = "3"
                        }
                    }
                );

                context.SaveChanges();
            }
        }
    }
}