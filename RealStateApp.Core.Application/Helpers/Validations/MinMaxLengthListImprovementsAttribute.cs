using System.Collections;
using System.ComponentModel.DataAnnotations;


namespace RealStateApp.Core.Application.Helpers.Validations
{
    public class MinMaxLengthListImprovementsAttribute : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public MinMaxLengthListImprovementsAttribute(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IList list)
            {
                if (list.Count < _minLength)
                {
                    return new ValidationResult($"Debe seleccionar al menos {_minLength} mejora.");
                }

                if (list.Count > _maxLength)
                {
                    return new ValidationResult($"No puede seleccionar más de {_maxLength} mejoras.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
