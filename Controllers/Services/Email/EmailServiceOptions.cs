namespace NewJobSurveyAdmin.Services
{
    public class EmailServiceOptions
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ToName { get; set; }
        public string ToAddress { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
    }
}