using System.ComponentModel.DataAnnotations;

namespace MSEmail.API.Validations;

public class DateValidAttribute : ValidationAttribute
{
    public string GetErrorMessage() =>
        $"Data inválida, tente novamente.";

    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        var sendDate = (DateTime)value;

        if (!sendDate.IsNull() && sendDate.Date < DateTime.Now.Date)
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success;
    }
}