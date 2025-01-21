using System;

namespace NewJobSurveyAdmin.Services
{
    public class SurveyDates
    {
        public DateTime NextPullDay { get; set; }
        public DateTime InviteDate { get; set; }
        public DateTime Reminder1Date { get; set; }
        public DateTime Reminder2Date { get; set; }
        public DateTime DeadlineDate { get; set; }
    }
}