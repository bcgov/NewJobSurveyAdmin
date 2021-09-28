using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewJobSurveyAdmin.Models
{
    public class AdminSetting : BaseEntity
    {
        public static readonly string DataPullDayOfWeek = "DataPullDayOfWeek";
        public static readonly string InviteDays = "InviteDays";
        public static readonly string Reminder1Days = "Reminder1Days";
        public static readonly string Reminder2Days = "Reminder2Days";
        public static readonly string CloseDays = "CloseDays";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public string Key { get; set; }

        [Required] public string DisplayName { get; set; }

        [Required] public string Value { get; set; }
    }

    public class AdminSettingPatchDto
    {
        public string Value { get; set; }
    }
}