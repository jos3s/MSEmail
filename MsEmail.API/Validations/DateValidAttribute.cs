using System.ComponentModel.DataAnnotations;

namespace MSEmail.API.Validations;

public class DateValidAttribute : ValidationAttribute
{
    public string GetErrorMessage() =>
        $"Data inválida, tente novamente.";

    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        var date = value ?? null;

        if (date.IsNull())
            return ValidationResult.Success;

        var sendDate = (DateTime)date;
        if (!sendDate.IsNull() && sendDate.Date < DateTime.Now.Date)
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success;
    }
}