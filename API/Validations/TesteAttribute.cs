using System.ComponentModel.DataAnnotations;

namespace API.Validations;

public class TesteAttribute : ValidationAttribute
{
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (value == null || string.IsNullOrEmpty(value.ToString()))
    {
      return ValidationResult.Success;
    }

    var primeiraLetra = value.ToString()![0].ToString();
    if (primeiraLetra != primeiraLetra.ToUpper())
    {
      return new ValidationResult("A Primeira letra do nome tem que ser maiuscula");
    }

    return ValidationResult.Success;
  }
}
