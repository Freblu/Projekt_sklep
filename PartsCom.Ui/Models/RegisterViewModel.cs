using System.ComponentModel.DataAnnotations;

namespace PartsCom.Ui.Models;

#pragma warning disable CA1515
public sealed class RegisterViewModel
{
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Potwierdź hasło")]
    [Compare("Password", ErrorMessage = "Hasło i potwierdzenie hasła nie pasują.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Imię jest wymagane")]
    [Display(Name = "Imię")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [Display(Name = "Nazwisko")]
    public string LastName { get; set; } = string.Empty;
}
