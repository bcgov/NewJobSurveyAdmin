using NewJobSurveyAdmin.Services;
using Newtonsoft.Json;
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

        [Sieve(CanFilter = true)]
        [Required]
        [JsonProperty(Required = Required.Always)]
        public string GovernmentEmployeeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string FirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string PreferredFirstName { get; set; }

        public Boolean PreferredFirstNameFlag { get; set; }

        public string MiddleName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string Age { get; set; }

        // Chips information (from the PSA extract)
        [Sieve(CanFilter = true, CanSort = true)]
        public string ChipsEmail { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ChipsFirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ChipsLastName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ChipsCity { get; set; }

        // Ldap information (from the LDAP lookup)
        [Sieve(CanFilter = true, CanSort = true)]
        public string LdapEmail { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LdapFirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LdapLastName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LdapCity { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LdapOrganization { get; set; }

        // Contact info

        [Sieve(CanFilter = true, CanSort = true)]
        public string GovernmentEmail { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string PreferredEmail { get; set; }

        public Boolean PreferredEmailFlag { get; set; }

        // Employee job info

        [Sieve(CanFilter = true, CanSort = true)]
        public string Classification { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public string Organization { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public string DepartmentId { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public string DepartmentIdDescription { get; set; }

        public string DevelopmentRegion { get; set; }

        public string LocationCity { get; set; }

        public string LocationGroup { get; set; }

        public string JobCode { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public string PositionCode { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public string PositionTitle { get; set; }

        public string JobClassificationGroup { get; set; }

        public string NocCode { get; set; }

        public string NocDescription { get; set; }

        public string OrganizationCount { get; set; }

        public string RegionalDistrict { get; set; }

        public string UnionCode { get; set; }

        // Hiring info

        [Sieve(CanFilter = true, CanSort = true)]
        [DataType(DataType.Date)]
        [Required]
        [JsonProperty(Required = Required.Always)]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string AppointmentStatus { get; set; }

        public string ServiceYears { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public string RecordCount { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public string StaffingAction { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        [JsonProperty(Required = Required.Always)]
        public string StaffingReason { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Required]
        [JsonProperty(Required = Required.Always)]
        public string NewHireOrInternalStaffing { get; set; }

        public string TaToPermanent { get; set; }

        // Prior job info

        public string PriorAppointmentStatus { get; set; }

        public string PriorClassification { get; set; }

        public string PriorDepartmentId { get; set; }

        public string PriorDepartmentIdDescription { get; set; }

        public string PriorEffectiveDate { get; set; }

        public string PriorEmployeeStatus { get; set; }

        public string PriorJobClassificationGroup { get; set; }

        public string PriorJobCode { get; set; }

        public string PriorNocCode { get; set; }

        public string PriorNocDescription { get; set; }

        public string PriorOrganization { get; set; }

        public string PriorPositionCode { get; set; }

        public string PriorPositionTitle { get; set; }

        public string PriorUnionCode { get; set; }

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
        public string CurrentEmployeeStatusCode { get; set; }

        public virtual EmployeeStatusEnum CurrentEmployeeStatus { get; set; }

        public virtual List<EmployeeTimelineEntry> TimelineEntries { get; set; }

        public Boolean TriedToUpdateInFinalState { get; set; }

        // Methods

        public System.Collections.Generic.IEnumerable<System.Reflection.PropertyInfo> NullRequiredProperties()
        {
            var properties = this.GetType()
                .GetProperties()
                .Where(prop => prop.IsDefined(typeof(RequiredAttribute), false));

            var nullProperties = properties.Where(p => p.GetValue(this) == null);

            return nullProperties;
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
        public void UpdateInfoFromLdap(EmployeeInfoLookupService infoLookupService)
        {
            var ldapInfo = infoLookupService.GetEmployeeInfoFromLdap(GovernmentEmployeeId);

            LdapFirstName = ldapInfo.FirstName;
            LdapLastName = ldapInfo.LastName;
            LdapEmail = ldapInfo.Email;
            LdapOrganization = ldapInfo.Organization;
            LdapCity = ldapInfo.City;

            if (LdapEmail == null && (ChipsEmail == null || ChipsEmail.Equals("")))
            {
                // Throw an exception only when there's no LDAP email *and* no
                // CHIPS email.
                throw new NoInfoException(
                    $"User with ID {GovernmentEmployeeId} has neither a CHIPS nor an LDAP email."
                );
            }
            else if (ldapInfo.Organization != null && ldapInfo.Organization.Equals("BC Assessment"))
            {
                // If the organization is "BC Assessment", we need to use the
                // CHIPS values regardless. This is due to an ID clash.
                FirstName = ChipsFirstName;
                LastName = ChipsLastName;
                GovernmentEmail = ldapInfo.EmailOverride ?? ChipsEmail;
                LocationCity = ChipsCity;
            }
            else
            {
                // Otherwise we can use the LDAP info, defaulting back to CHIPS
                // if anything is null.
                FirstName = ldapInfo.FirstName ?? ChipsFirstName;
                LastName = ldapInfo.LastName ?? ChipsLastName;
                GovernmentEmail = ldapInfo.EmailOverride ?? ldapInfo.Email ?? ChipsEmail;
                LocationCity = ldapInfo.City ?? ChipsCity;
            }
        }

        /// <summary>
        /// Initialize all Preferred fields to be the equivalent of the base
        /// field, and set up all calculated date fields. This should only be
        /// run when the Employee is created.
        /// </summary>
        /// <param name="dates">The SurveyDates to use to populate the employee.</param>
        public void InstantiateFields(SurveyDates dates)
        {
            PreferredFirstName = FirstName;
            PreferredFirstNameFlag = false;
            PreferredEmail = GovernmentEmail;
            PreferredEmailFlag = false;
            SetDates(dates);
        }

        /// <summary>
        /// Set all employee date fields to the provided date fields.
        /// </summary>
        /// <param name="dates">The SurveyDates to use to populate the employee.</param>
        public void SetDates(SurveyDates dates)
        {
            InviteDate = dates.InviteDate;
            Reminder1Date = dates.Reminder1Date;
            Reminder2Date = dates.Reminder2Date;
            DeadlineDate = dates.DeadlineDate;
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
            return StaffingReason.Equals("Temporary Appointment <7 Mnths") ? "1" : "";
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

        public override string ToString()
        {
            return $"{FullName} ({GovernmentEmployeeId})";
        }
    }
}
