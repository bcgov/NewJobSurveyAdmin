using NewJobSurveyAdmin.Services;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NewJobSurveyAdmin.Models
{
    /// <summary>
    /// A government employee who needs to receive a job survey.
    /// </summary>
    public class Employee : BaseEntity
    {
        // Keys

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public virtual string Telkey { get; set; }


        // Personal info (ID, names, etc.)

        [Sieve(CanFilter = true)] [Required] public string GovernmentEmployeeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string FirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string PreferredFirstName { get; set; }

        [Required] public Boolean PreferredFirstNameFlag { get; set; }

        [Required] public string MiddleName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.Date)] [Required] public DateTime BirthDate { get; set; }

        [Required] public string Gender { get; set; }

        [Required] public string Age { get; set; }


        // Chips information (from the PSA extract)
        [Sieve(CanFilter = true, CanSort = true)]
        public string ChipsEmail { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ChipsFirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ChipsLastName { get; set; }


        // Ldap information (from the LDAP lookup)
        [Sieve(CanFilter = true, CanSort = true)]
        public string LdapEmail { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LdapFirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LdapLastName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LdapOrganization { get; set; }

        // Contact info

        [Sieve(CanFilter = true, CanSort = true)]
        public string GovernmentEmail { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string PreferredEmail { get; set; }

        [Required] public Boolean PreferredEmailFlag { get; set; }


        // Employee job info

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string Classification { get; set; }

        [Required] public string Organization { get; set; }

        [Required] public string DepartmentId { get; set; }

        [Required] public string DepartmentIdDescription { get; set; }

        [Required] public string DevelopmentRegion { get; set; }

        [Required] public string LocationCity { get; set; }

        [Required] public string LocationGroup { get; set; }

        [Required] public string JobCode { get; set; }

        [Required] public string PositionCode { get; set; }

        [Required] public string PositionTitle { get; set; }

        [Required] public string JobClassificationGroup { get; set; }

        [Required] public string NocCode { get; set; }

        [Required] public string NocDescription { get; set; }

        [Required] public string OrganizationCount { get; set; }

        [Required] public string RegionalDistrict { get; set; }

        [Required] public string UnionCode { get; set; }


        // Hiring info

        [Sieve(CanFilter = true, CanSort = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public string AppointmentStatus { get; set; }

        [Required] public string ServiceYears { get; set; }

        [Required] public string RecordCount { get; set; }

        [Required] public string StaffingAction { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string StaffingReason { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required] public string NewHireOrInternalStaffing { get; set; }

        [Required] public string TaToPermanent { get; set; }


        // Prior job info

        [Required] public string PriorAppointmentStatus { get; set; }

        [Required] public string PriorClassification { get; set; }

        [Required] public string PriorDepartmentId { get; set; }

        [Required] public string PriorDepartmentIdDescription { get; set; }

        [Required] public string PriorEffectiveDate { get; set; }

        [Required] public string PriorEmployeeStatus { get; set; }

        [Required] public string PriorJobClassificationGroup { get; set; }

        [Required] public string PriorJobCode { get; set; }

        [Required] public string PriorNocCode { get; set; }

        [Required] public string PriorNocDescription { get; set; }

        [Required] public string PriorOrganization { get; set; }

        [Required] public string PriorPositionCode { get; set; }

        [Required] public string PriorPositionTitle { get; set; }

        [Required] public string PriorUnionCode { get; set; }


        // Calculated date fields

        [Sieve(CanFilter = true, CanSort = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime InviteDate { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Reminder1Date { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Reminder2Date { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DeadlineDate { get; set; }


        // Additional generated fields

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        public string CurrentEmployeeStatusCode { get; set; }

        public virtual EmployeeStatusEnum CurrentEmployeeStatus { get; set; }

        public virtual List<EmployeeTimelineEntry> TimelineEntries { get; set; }

        public Boolean TriedToUpdateInFinalState { get; set; }


        // Methods

        /// <summary>
        /// Method to compare properties between two Employees ("this" one, i.e.
        /// the one this method is being called on, and another candidate one).
        /// Used when we need to determine which fields have been updated on a
        /// PATCHed user. Note the intentionally excluded properties; these will
        /// never get set in a PATCH and so are not compared.
        /// </summary>
        /// <param name="candidate">The other Employee to compare this one against.</param>
        /// <returns>
        /// An enumerable list of properties that differ between the two
        /// Employees, including the property name, the value in "this"
        /// Employee, and the value in the candidate Employee.
        /// </returns>
        public IEnumerable<PropertyVariance> PropertyCompare(Employee candidate)
        {
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
                    d.PropertyInfo.Name != nameof(PreferredEmailFlag)
                );
        }

        /// <summary>
        /// Convenience method to generate an employee's full name, which is a
        /// concatenation of their first and last names.
        /// </summary>
        public string FullName
        {
            get { return $"{this.FirstName} {this.LastName}"; }
        }

        /// <summary>
        /// Retrieve an employee's first name, last name, and email from LDAP,
        /// as they may have been updated since the PSA data extract.
        /// 
        /// If the LDAP lookup finds a person with the employee's employee ID
        /// who works at BC Assessment, we must ignore the LDAP values. This is
        /// because there is a clash between employee IDs — they are not unique.
        /// </summary>
        /// <param name="infoLookupService">The <see cref="NewJobSurveyAdmin.Services.EmployeeInfoLookupService" /> to be used to look up the info.</param>
        public void UpdateInfoFromLdap(
            EmployeeInfoLookupService infoLookupService
        )
        {
            var ldapInfo = infoLookupService.GetEmployeeInfoFromLdap(GovernmentEmployeeId);

            LdapFirstName = ldapInfo.FirstName;
            LdapLastName = ldapInfo.LastName;
            LdapEmail = ldapInfo.Email;
            LdapOrganization = ldapInfo.Organization;

            if (ldapInfo.Organization.Equals("BC Assessment"))
            {
                // If the organization is "BC Assessment", we need to use the
                // CHIPS values regardless. This is due to an ID clash.
                FirstName = ChipsFirstName;
                LastName = ChipsLastName;
                GovernmentEmail = ldapInfo.EmailOverride ?? ChipsEmail;
            }
            else
            {
                // Otherwise we can use the LDAP info, defaulting back to CHIPS
                // if anything is null.
                FirstName = ldapInfo.FirstName ?? ChipsFirstName;
                LastName = ldapInfo.LastName ?? ChipsLastName;
                GovernmentEmail = ldapInfo.EmailOverride ?? ldapInfo.Email ?? ChipsEmail;
            }
        }

        /// <summary>
        /// Initialize all Preferred fields to be the equivalent of the base
        /// field, and set up all calculated date fields. This should only be
        /// run when the Employee is created.
        /// </summary>
        /// <param name="inviteDate">The date to invite the user to the survey.</param>
        /// <param name="reminder1Date">The date to send the first survey reminder.</param>
        /// <param name="reminder2Date">The date to send the second survey reminder.</param>
        /// <param name="deadlineDate">The deadline date for completing the survey.</param>
        public void InstantiateFields(DateTime inviteDate, DateTime reminder1Date, DateTime reminder2Date,
            DateTime deadlineDate)
        {
            PreferredFirstName = FirstName;
            PreferredFirstNameFlag = false;
            PreferredEmail = GovernmentEmail;
            PreferredEmailFlag = false;
            InviteDate = inviteDate;
            Reminder1Date = reminder1Date;
            Reminder2Date = reminder2Date;
            DeadlineDate = deadlineDate;
        }

        /// <summary>
        /// Identify whether the survey is open or not. For use when posting to
        /// CallWeb.
        /// </summary>
        /// <returns>
        /// "" if the survey is inactive (closed), "1" if it is active (open).
        /// </returns>
        public string SurveyWindowFlag()
        {
            return IsActive() ? "" : "1";
        }

        /// <summary>
        /// Identify whether this employee is a temporary appointment or not.
        /// For use when posting to CallWeb.
        /// </summary>
        /// <returns>
        /// "" if the employee is not a temporary appointment, or "1" if they
        /// are.
        /// </returns>
        public string TaU7Flag()
        {
            return StaffingReason.Equals("Temporary Appointment <7 Mnths")
                ? "1"
                : "";
        }

        /// <summary>
        /// Identify whether this employee is a lateral transfer or not. For use
        /// when posting to CallWeb.
        /// </summary>
        /// <returns>
        /// "" if the employee is not a lateral transfer, or "1" if they are.
        /// </returns>
        public string LatTransferFlag()
        {
            return StaffingReason.Equals("Lateral Transfer") ? "1" : "";
        }

        /// <summary>
        /// Identify whether this employee is a new hire or not. For use when
        /// posting to CallWeb.
        /// </summary>
        /// <returns>
        /// "" if the employee is not a new hire, or "1" if they are.
        /// </returns>
        public string NewHireFlag()
        {
            return NewHireOrInternalStaffing.Equals("New Hires") ? "1" : "";
        }

        /// <summary>
        /// Identify whether this employee is active or not, based on whether
        /// their <see cref="NewJobSurveyAdmin.Models.Employee.CurrentEmployeeStatusCode" />
        /// is an active code. See <see cref="NewJobSurveyAdmin.Models.EmployeeStatusEnum" />.
        /// </summary>
        /// <returns>
        /// False if the employee is not in an active state, or true if they are.
        /// </returns>
        public Boolean IsActive()
        {
            return EmployeeStatusEnum.IsActiveStatus(CurrentEmployeeStatusCode);
        }
    }
}