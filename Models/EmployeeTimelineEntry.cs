using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewJobSurveyAdmin.Models
{
    public class EmployeeTimelineEntry : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public int EmployeeId { get; set; }

        [JsonIgnore] public virtual Employee Employee { get; set; }

        [Required] public string EmployeeActionCode { get; set; }

        public virtual EmployeeActionEnum EmployeeAction { get; set; }

        [Required] public string EmployeeStatusCode { get; set; }
        public virtual EmployeeStatusEnum EmployeeStatus { get; set; }

        [Required] public string Comment { get; set; }

        public string AdminUserName { get; set; }
    }
}