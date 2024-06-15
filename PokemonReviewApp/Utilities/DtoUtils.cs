using System.ComponentModel.DataAnnotations;

namespace API_Dinamis.Utilities
{
    public class DtoUtils
    {
        public class SpecificStringValueAttribute : ValidationAttribute
        {
            private readonly string[] _allowedValues;

            public SpecificStringValueAttribute(params string[] allowedValues)
            {
                _allowedValues = allowedValues;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    string stringValue = value.ToString();
                    if (!_allowedValues.Contains(stringValue))
                    {
                        return new ValidationResult($"The field {validationContext.DisplayName} must be one of the following values: {string.Join(", ", _allowedValues)}.");
                    }
                }

                return ValidationResult.Success;
            }
        }
    }
}
