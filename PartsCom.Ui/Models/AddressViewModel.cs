using System.ComponentModel.DataAnnotations;
using PartsCom.Domain.Enums;

namespace PartsCom.Ui.Models;

#pragma warning disable CA1515, S6964
public class AddressViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Full name is required.")]
    [MaxLength(200, ErrorMessage = "Full name cannot exceed 200 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address line 1 is required.")]
    [MaxLength(200, ErrorMessage = "Address line 1 cannot exceed 200 characters.")]
    [Display(Name = "Address Line 1")]
    public string AddressLine1 { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "Address line 2 cannot exceed 200 characters.")]
    [Display(Name = "Address Line 2")]
    public string AddressLine2 { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Postal code is required.")]
    [MaxLength(20, ErrorMessage = "Postal code cannot exceed 20 characters.")]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required.")]
    [MaxLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
    [Display(Name = "Country")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "Address Type")]
    public AddressType Type { get; set; } = AddressType.Both;

    [Display(Name = "Set as Default")]
    public bool IsDefault { get; set; }
}
