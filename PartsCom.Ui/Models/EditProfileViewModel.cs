using System.ComponentModel.DataAnnotations;

namespace PartsCom.Ui.Models;

#pragma warning disable CA1515
public class EditProfileViewModel
{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Avatar URL cannot exceed 500 characters.")]
    [Display(Name = "Avatar URL")]
    public string AvatarUrl { get; set; } = string.Empty;
}
