using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.Validations
{
    public class CamelCaseValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var firstChar = value.ToString()[0].ToString();

            if(firstChar != firstChar.ToUpper())
            {
                return new ValidationResult("The first letter must be upper");
            }

            return ValidationResult.Success;
        }
    }
}
