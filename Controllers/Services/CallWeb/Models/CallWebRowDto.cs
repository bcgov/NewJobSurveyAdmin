namespace NewJobSurveyAdmin.Services.CallWeb
{
    public partial class CallWebRowDto
    {
        public string Telkey { get; set; }
        public string PreferredEmail { get; set; }
        public string PreferredFirstName { get; set; }
        public string LastName { get; set; }
        public string EffectiveDate { get; set; }
        public string CurrentStatus { get; set; }
        public string HireReason { get; set; }
        public string InviteDate { get; set; }
        public string Reminder1Date { get; set; }
        public string Reminder2Date { get; set; }
        public string DeadlineDate { get; set; }
        public string IsSurveyComplete { get; set; }
        public string SurveyWindowFlag { get; set; }
        public string TaU7Flag { get; set; }
        public string LatTransferFlag { get; set; }
    }
}