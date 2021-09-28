using System;
using Sieve.Attributes;

namespace NewJobSurveyAdmin.Models
{
    public class BaseEntity
    {
        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime CreatedTs { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime ModifiedTs { get; set; }
    }
}