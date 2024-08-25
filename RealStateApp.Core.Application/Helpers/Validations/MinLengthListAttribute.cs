using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Helpers.Validations
{
    public class MinLengthListAttribute : ValidationAttribute
    {
        private readonly int _minLength;

        public MinLengthListAttribute(int minLength)
        {
            _minLength = minLength;
            ErrorMessage = $"The list must contain at least {minLength} items.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IList list && list.Count >= _minLength)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
