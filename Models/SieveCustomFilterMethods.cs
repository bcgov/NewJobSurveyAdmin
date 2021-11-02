using System.Linq;
using Sieve.Services;
using System;

namespace NewJobSurveyAdmin.Models
{
    public class SieveCustomFilterMethods : ISieveCustomFilterMethods
    {
        // The method is given the {Operator} & {Value}
        public IQueryable<Employee> BlankEmail(
            IQueryable<Employee> source, string op, string[] values)
        {
            var result = source.Where(e => e.PreferredEmail.Length == 0);

            return result; // Must return modified IQueryable<TEntity>
        }

        public IQueryable<Employee> HiringReason(
            IQueryable<Employee> source, string op, string[] values)
        {
            // We need to replace the :gt: and :lt: values with > and <
            // respectively.
            var unescapedValues = values.Select(v =>
                v.Replace(":gt:", ">")
                 .Replace(":lt:", "<")
            );

            var result = source.Where(
                e => unescapedValues.Contains(e.StaffingReason)
            );

            return result;
        }

        // Override filtering based on createTs. We only want to look at the
        // day portion of the timestamp (for filtering purposes, at least),
        // not including the hours and minutes.
        public IQueryable<Employee> ImportDate(
            IQueryable<Employee> source, string op, string[] values)
        {
            var date = DateTime.Parse(values[0]).Date;

            if (op.Equals(">="))
            {
                return source.Where(e => e.CreatedTs.Date >= date);
            }
            else // <=
            {
                return source.Where(e => e.CreatedTs.Date <= date);
            }
        }
    }
}