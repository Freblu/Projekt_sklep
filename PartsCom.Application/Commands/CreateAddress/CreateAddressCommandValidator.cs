using FluentValidation;

namespace PartsCom.Application.Commands.CreateAddress;

internal sealed class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithErrorCode("CA001")
            .WithMessage("User ID is required.");

        RuleFor(c => c.FullName)
            .NotEmpty()
            .WithErrorCode("CA002")
            .WithMessage("Full name is required.")
            .MaximumLength(200)
            .WithErrorCode("CA003")
            .WithMessage("Full name cannot exceed 200 characters.");

        RuleFor(c => c.AddressLine1)
            .NotEmpty()
            .WithErrorCode("CA004")
            .WithMessage("Address line 1 is required.")
            .MaximumLength(200)
            .WithErrorCode("CA005")
            .WithMessage("Address line 1 cannot exceed 200 characters.");

        RuleFor(c => c.AddressLine2)
            .MaximumLength(200)
            .WithErrorCode("CA006")
            .WithMessage("Address line 2 cannot exceed 200 characters.")
            .When(c => c.AddressLine2 != null);

        RuleFor(c => c.City)
            .NotEmpty()
            .WithErrorCode("CA007")
            .WithMessage("City is required.")
            .MaximumLength(100)
            .WithErrorCode("CA008")
            .WithMessage("City cannot exceed 100 characters.");

        RuleFor(c => c.PostalCode)
            .NotEmpty()
            .WithErrorCode("CA009")
            .WithMessage("Postal code is required.")
            .MaximumLength(20)
            .WithErrorCode("CA010")
            .WithMessage("Postal code cannot exceed 20 characters.");

        RuleFor(c => c.Country)
            .NotEmpty()
            .WithErrorCode("CA011")
            .WithMessage("Country is required.")
            .MaximumLength(100)
            .WithErrorCode("CA012")
            .WithMessage("Country cannot exceed 100 characters.");

        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .WithErrorCode("CA013")
            .WithMessage("Phone number is required.")
            .MaximumLength(20)
            .WithErrorCode("CA014")
            .WithMessage("Phone number cannot exceed 20 characters.");

        RuleFor(c => c.Type)
            .IsInEnum()
            .WithErrorCode("CA015")
            .WithMessage("Invalid address type.");
    }
}
