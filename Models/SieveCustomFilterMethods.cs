using Sieve.Services;
using System;
using System.Linq;

namespace NewJobSurveyAdmin.Models
{
    public class SieveCustomFilterMethods : ISieveCustomFilterMethods
    {
        // Override filtering based on createTs. We need to convert the provided
        // date (which will be in Pacific time) to UTC, and filter correctly.
        protected IQueryable<T> FilterByCreateDate<T>(
            IQueryable<T> source,
            string op,
            string[] values
        )
            where T : NewJobSurveyAdmin.Models.BaseEntity
        {
            TimeZoneInfo pacificZone = TimeZoneInfo.FindSystemTimeZoneById("America/Vancouver");

            var dayStartPacific = TimeZoneInfo.ConvertTimeToUtc(
                DateTime.Parse(values[0]),
                pacificZone
            );

            var dayEndPacific = dayStartPacific + new TimeSpan(23, 59, 59);

            if (op.Equals(">="))
            {
                return source.Where(item => item.CreatedTs >= dayStartPacific);
            }
            else // i.e. <=
            {
                return source.Where(item => item.CreatedTs <= dayEndPacific);
            }
        }

        // Override filtering based on modifiedTs. We need to convert the provided
        // date (which will be in Pacific time) to UTC, and filter correctly.
        protected IQueryable<T> FilterByModifiedDate<T>(
            IQueryable<T> source,
            string op,
            string[] values
        )
            where T : NewJobSurveyAdmin.Models.BaseEntity
        {
            TimeZoneInfo pacificZone = TimeZoneInfo.FindSystemTimeZoneById("America/Vancouver");

            var dayStartPacific = TimeZoneInfo.ConvertTimeToUtc(
                DateTime.Parse(values[0]),
                pacificZone
            );

            var dayEndPacific = dayStartPacific + new TimeSpan(23, 59, 59);

            if (op.Equals(">="))
            {
                return source.Where(item => item.ModifiedTs >= dayStartPacific);
            }
            else // i.e. <=
            {
                return source.Where(item => item.ModifiedTs <= dayEndPacific);
            }
        }

        // The method is given the {Operator} & {Value}
        public IQueryable<Employee> BlankEmail(
            IQueryable<Employee> source,
            string op,
            string[] values
        )
        {
            var result = source.Where(e => e.PreferredEmail.Length == 0);

            return result; // Must return modified IQueryable<TEntity>
        }

        public IQueryable<Employee> HiringReason(
            IQueryable<Employee> source,
            string op,
            string[] values
        )
        {
            // We need to replace the :gt: and :lt: values with > and <
            // respectively.
            var unescapedValues = values.Select(v => v.Replace(":gt:", ">").Replace(":lt:", "<"));

            var result = source.Where(e => unescapedValues.Contains(e.StaffingReason));

            return result;
        }

        public IQueryable<Employee> ImportDate(
            IQueryable<Employee> source,
            string op,
            string[] values
        )
        {
            return FilterByCreateDate(source, op, values);
        }

        public IQueryable<TaskLogEntry> LogDate(
            IQueryable<TaskLogEntry> source,
            string op,
            string[] values
        )
        {
            return FilterByCreateDate(source, op, values);
        }

        public IQueryable<Employee> LastModifiedDate(
            IQueryable<Employee> source,
            string op,
            string[] values
        )
        {
            return FilterByModifiedDate(source, op, values);
        }
    }
}
