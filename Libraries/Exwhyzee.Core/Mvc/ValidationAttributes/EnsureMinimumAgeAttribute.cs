using System;
using System.ComponentModel.DataAnnotations;

namespace Exwhyzee.Core.Mvc.ValidationAttributes
{
    public class EnsureMinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public EnsureMinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dot = Convert.ToDateTime(value);
            var now = DateTime.Now;
            var current = now.AddYears(-_minimumAge);
            var compareValue = now.CompareTo(dot);
            if (compareValue >= 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
