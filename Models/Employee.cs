using NewJobSurveyAdmin.Services;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NewJobSurveyAdmin.Models
{
    public class Employee : BaseEntity
    {

        public IEnumerable<PropertyVariance> PropertyCompare(Employee candidate)
        {
            // Compare properties. Note the intentionally excluded properties.
            return this.DetailedCompare(candidate)
                .Where(d =>
                    d.PropertyInfo.Name != nameof(Id) &&
                    d.PropertyInfo.Name != nameof(Telkey) &&
                    d.PropertyInfo.Name != nameof(CurrentEmployeeStatusCode) &&
                    d.PropertyInfo.Name != nameof(CurrentEmployeeStatus) &&
                    d.PropertyInfo.Name != nameof(TimelineEntries) &&
                    d.PropertyInfo.Name != nameof(CreatedTs) &&
                    d.PropertyInfo.Name != nameof(ModifiedTs) &&
                    d.PropertyInfo.Name != nameof(PreferredFirstName) &&
                    d.PropertyInfo.Name != nameof(PreferredEmail) &&
                    d.PropertyInfo.Name != nameof(PreferredFirstNameFlag) &&
                    d.PropertyInfo.Name != nameof(PreferredEmailFlag) &&
                    d.PropertyInfo.Name != nameof(TriedToUpdateInFinalState)
                );
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public virtual string Telkey { get; set; }


        // Personal info (ID, names, etc.)

        [Sieve(CanFilter = true)]
        [Required]
        public string GovernmentEmployeeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string FirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string PreferredFirstName { get; set; }

        [Required]
        public Boolean PreferredFirstNameFlag { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Age { get; set; }


        // Contact information

        [Sieve(CanFilter = true, CanSort = true)]
        public string ChipsEmail { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string GovernmentEmail { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string PreferredEmail { get; set; }

        [Required]
        public Boolean PreferredEmailFlag { get; set; }


        // Employee job info

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string Classification { get; set; }

        [Required]
        public string Organization { get; set; }

        [Required]
        public string DepartmentId { get; set; }

        [Required]
        public string DepartmentIdDescription { get; set; }

        [Required]
        public string DevelopmentRegion { get; set; }

        [Required]
        public string LocationCity { get; set; }

        [Required]
        public string LocationGroup { get; set; }

        [Required]
        public string JobCode { get; set; }

        [Required]
        public string PositionCode { get; set; }

        [Required]
        public string PositionTitle { get; set; }

        [Required]
        public string JobClassificationGroup { get; set; }

        [Required]
        public string NocCode { get; set; }

        [Required]
        public string NocDescription { get; set; }

        [Required]
        public string OrganizationCount { get; set; }

        [Required]
        public string RegionalDistrict { get; set; }

        [Required]
        public string UnionCode { get; set; }

        // Hiring info

        [Sieve(CanFilter = true, CanSort = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public string AppointmentStatus { get; set; }

        [Required]
        public string ServiceYears { get; set; }

        [Required]
        public string RecordCount { get; set; }

        [Required]
        public string StaffingAction { get; set; }

        [Required]
        public string StaffingReason { get; set; }

        [Required]
        public string NewHireOrInternalStaffing { get; set; }

        [Required]
        public string TaToPermanent { get; set; }

        // Prior job info

        [Required]
        public string PriorAppointmentStatus { get; set; }

        [Required]
        public string PriorClassification { get; set; }

        [Required]
        public string PriorDepartmentId { get; set; }

        [Required]
        public string PriorDepartmentIdDescription { get; set; }

        [Required]
        public string PriorEffectiveDate { get; set; }

        [Required]
        public string PriorEmployeeStatus { get; set; }

        [Required]
        public string PriorJobClassificationGroup { get; set; }

        [Required]
        public string PriorJobCode { get; set; }

        [Required]
        public string PriorNocCode { get; set; }

        [Required]
        public string PriorNocDescription { get; set; }

        [Required]
        public string PriorOrganization { get; set; }

        [Required]
        public string PriorPositionCode { get; set; }

        [Required]
        public string PriorPositionTitle { get; set; }

        [Required]
        public string PriorUnionCode { get; set; }




        // Code-specific additional fields

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string CurrentEmployeeStatusCode { get; set; }

        public virtual EmployeeStatusEnum CurrentEmployeeStatus { get; set; }

        public virtual List<EmployeeTimelineEntry> TimelineEntries { get; set; }

        public Boolean TriedToUpdateInFinalState { get; set; }


        // Methods

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string FullName
        {
            get { return $"{this.FirstName} {this.LastName}"; }
        }

        public void UpdateEmail(
            EmployeeInfoLookupService infoLookupService
        )
        {
            GovernmentEmail = infoLookupService
                .EmailByEmployeeId(GovernmentEmployeeId);
        }

        // Initialize all Preferred fields to be the equivalent of the base
        // field. This should only be run when the Employee is created.
        public void InstantiateFields()
        {
            PreferredFirstName = FirstName;
            PreferredFirstNameFlag = false;
            PreferredEmail = GovernmentEmail;
            PreferredEmailFlag = false;
            TriedToUpdateInFinalState = false;
        }

        // Update all Preferred fields to be the equivalent of the base field,
        // so long as the Preferred field has never been overwritten (i.e. the
        // corresponding `Flag` is false).
        public void UpdatePreferredFields()
        {
            if (!PreferredFirstNameFlag) PreferredFirstName = FirstName;
            if (!PreferredEmailFlag) PreferredEmail = GovernmentEmail;
        }

        // public string LeaveCode
        // {
        //     get
        //     {
        //         switch (this.Reason)
        //         {
        //             case "Just Cause":
        //             case "Redundant":
        //             case "Rejection on Probation":
        //                 return "3";
        //             case "Layoff (With Recall)":
        //             case "Job Ends/End of Recall Limit":
        //                 return "2";
        //             default: // All other cases; no need to enumerate here
        //                 return "1";
        //         }
        //     }
        // }

        public string SurveyWindowFlag()
        {
            return IsActive() ? "0" : "1";
        }

        public Boolean IsActive()
        {
            return EmployeeStatusEnum.IsActiveStatus(CurrentEmployeeStatusCode);
        }
    }
}