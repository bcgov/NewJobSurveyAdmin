using Microsoft.EntityFrameworkCore;
using NewJobSurveyAdmin.Models;
using System;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Services
{
    public class SurveyDateBuilder
    {
        public static readonly DateTime BLACKOUT_DATE = new DateTime(2099, 1, 1);

        public static async Task<SurveyDates> GetDatesBasedOnAdminSettings(NewJobSurveyAdminContext context)
        {
            var adminSettings = await context.AdminSettings.ToListAsync();

            // Data pull day of week is an int, 1 = Monday, 7 = Sunday (ISO
            // standard, and it matches System.DayOfWeek values.
            bool isBlackoutPeriod = Convert.ToBoolean(
                adminSettings.Find(s => s.Key == AdminSetting.IsBlackoutPeriod).Value
            );

            if (isBlackoutPeriod)
            {
                return new SurveyDates()
                {
                    NextPullDay = BLACKOUT_DATE,
                    InviteDate = BLACKOUT_DATE,
                    Reminder1Date = BLACKOUT_DATE,
                    Reminder2Date = BLACKOUT_DATE,
                    DeadlineDate = BLACKOUT_DATE
                };
            }

            int dataPullDayOfWeek = Convert.ToInt32(
                adminSettings.Find(s => s.Key == AdminSetting.DataPullDayOfWeek).Value
            );
            int inviteDays = Convert.ToInt32(
                adminSettings.Find(s => s.Key == AdminSetting.InviteDays).Value
            );
            int reminder1Days = Convert.ToInt32(
                adminSettings.Find(s => s.Key == AdminSetting.Reminder1Days).Value
            );
            int reminder2Days = Convert.ToInt32(
                adminSettings.Find(s => s.Key == AdminSetting.Reminder2Days).Value
            );
            int deadlineDays = Convert.ToInt32(
                adminSettings.Find(s => s.Key == AdminSetting.CloseDays).Value
            );

            // Establish base dates for insert. Adapted from
            // https://stackoverflow.com/a/6346190/715870.
            DateTime today = DateTime.Today;
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntilNextPullDay = (dataPullDayOfWeek - (int)today.DayOfWeek + 7) % 7;

            DateTime nextPullDay = today.AddDays(daysUntilNextPullDay);
            DateTime inviteDate = nextPullDay.AddDays(inviteDays);
            DateTime reminder1Date = inviteDate.AddDays(reminder1Days);
            DateTime reminder2Date = reminder1Date.AddDays(reminder2Days);
            DateTime deadlineDate = reminder2Date.AddDays(deadlineDays);

            return new SurveyDates()
            {
                NextPullDay = nextPullDay,
                InviteDate = inviteDate,
                Reminder1Date = reminder1Date,
                Reminder2Date = reminder2Date,
                DeadlineDate = deadlineDate
            };
        }
    }
}