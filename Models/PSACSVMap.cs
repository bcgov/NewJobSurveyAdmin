using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using NewJobSurveyAdmin.Models;
using System;
using System.Globalization;

public class CustomDateTimeConverter : DateTimeConverter
{
    public override object ConvertFromString(
        string text, IReaderRow row, MemberMapData memberMapData
    )
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        var formatProvider = (IFormatProvider)memberMapData
                                 .TypeConverterOptions
                                 .CultureInfo
                                 .GetFormat(typeof(DateTimeFormatInfo))
                             ??
                             memberMapData.TypeConverterOptions.CultureInfo;

        var dateTimeStyle = memberMapData
                                .TypeConverterOptions
                                .DateTimeStyle
                            ??
                            DateTimeStyles.None;

        return
            (memberMapData.TypeConverterOptions.Formats == null ||
             memberMapData.TypeConverterOptions.Formats.Length == 0)
                ? DateTime.Parse(text, formatProvider, dateTimeStyle)
                : DateTime.ParseExact(
                    text, memberMapData.TypeConverterOptions.Formats,
                    formatProvider, dateTimeStyle
                );
    }
}

public class PsaCsvMap : ClassMap<Employee>
{
    public PsaCsvMap()
    {
        // Ordered as they appear in the CSV extract.
        Map(m => m.Age).Name("AGE");
        Map(m => m.AppointmentStatus).Name("APPOINTMENT STATUS");
        Map(m => m.BirthDate).Name("BIRTHDATE");
        Map(m => m.ChipsEmail).Name("CHIPS EMAIL");
        Map(m => m.ChipsFirstName).Name("FIRST NAME");
        Map(m => m.ChipsLastName).Name("LAST NAME");
        Map(m => m.ChipsCity).Name("CITY");
        Map(m => m.Classification).Name("CLASSIFICATION");
        Map(m => m.DepartmentId).Name("DEPTID");
        Map(m => m.DepartmentIdDescription).Name("DEPTID DESCR");
        Map(m => m.DevelopmentRegion).Name("DEVELOPMENT REGION");
        Map(m => m.RecordCount).Name("EMPL_RCD");
        Map(m => m.GovernmentEmployeeId).Name("EMPLID");
        Map(m => m.Gender).Name("GENDER");
        Map(m => m.StaffingAction).Name("HIRE STAFFING ACTION");
        Map(m => m.EffectiveDate).Name("HIRE STAFFING EFFDT");
        Map(m => m.StaffingReason).Name("HIRE STAFFING REASON");
        Map(m => m.JobClassificationGroup).Name("JOBCLASGRP");
        Map(m => m.JobCode).Name("JOBCODE");
        Map(m => m.LocationGroup).Name("LOCATION GROUP");
        Map(m => m.MiddleName).Name("MIDDLE NAME"); // TODO: Remove
        Map(m => m.NewHireOrInternalStaffing).Name("NEW HIRE OR INTERNAL STAFFING");
        Map(m => m.NocCode).Name("NOC CODE");
        Map(m => m.NocDescription).Name("NOC DESCR");
        Map(m => m.OrganizationCount).Name("ORG COUNT");
        Map(m => m.Organization).Name("ORGANIZATION");
        Map(m => m.PositionCode).Name("POSITION");
        Map(m => m.PositionTitle).Name("POSITION TITLE");
        Map(m => m.PriorAppointmentStatus).Name("PRIOR APPOINTMENT STATUS");
        Map(m => m.PriorClassification).Name("PRIOR CLASSIFICATION");
        Map(m => m.PriorDepartmentId).Name("PRIOR DEPTID");
        Map(m => m.PriorDepartmentIdDescription).Name("PRIOR DEPTID DESCR");
        Map(m => m.PriorEffectiveDate).Name("PRIOR EFFDT");
        Map(m => m.PriorEmployeeStatus).Name("PRIOR EMPL STATUS");
        Map(m => m.PriorJobClassificationGroup).Name("PRIOR JOBCLASGRP");
        Map(m => m.PriorJobCode).Name("PRIOR JOBCODE");
        Map(m => m.PriorNocCode).Name("PRIOR NOC CODE");
        Map(m => m.PriorNocDescription).Name("PRIOR NOC DESCR");
        Map(m => m.PriorOrganization).Name("PRIOR ORGANIZATION");
        Map(m => m.PriorPositionCode).Name("PRIOR POSITION");
        Map(m => m.PriorPositionTitle).Name("PRIOR POSITION TITLE");
        Map(m => m.PriorUnionCode).Name("PRIOR UNION CODE");
        Map(m => m.RegionalDistrict).Name("REGIONAL DISTRICT");
        Map(m => m.ServiceYears).Name("SERVICE");
        Map(m => m.TaToPermanent).Name("TA TO PERMANENT");
        Map(m => m.UnionCode).Name("UNION CODE");
    }
}