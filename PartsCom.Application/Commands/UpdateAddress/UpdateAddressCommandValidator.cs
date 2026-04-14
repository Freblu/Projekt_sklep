using FluentValidation;

namespace PartsCom.Application.Commands.UpdateAddress;

internal sealed class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(c => c.AddressId)
            .NotEmpty()
            .WithErrorCode("UA001")
            .WithMessage("Address ID is required.");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithErrorCode("UA002")
            .WithMessage("User ID is required.");

        RuleFor(c => c.FullName)
            .NotEmpty()
            .WithErrorCode("UA003")
            .WithMessage("Full name is required.")
            .MaximumLength(200)
            .WithErrorCode("UA004")
            .WithMessage("Full name cannot exceed 200 characters.");

        RuleFor(c => c.AddressLine1)
            .NotEmpty()
            .WithErrorCode("UA005")
            .WithMessage("Address line 1 is required.")
            .MaximumLength(200)
            .WithErrorCode("UA006")
            .WithMessage("Address line 1 cannot exceed 200 characters.");

        RuleFor(c => c.AddressLine2)
            .MaximumLength(200)
            .WithErrorCode("UA007")
            .WithMessage("Address line 2 cannot exceed 200 characters.")
            .When(c => c.AddressLine2 != null);

        RuleFor(c => c.City)
            .NotEmpty()
            .WithErrorCode("UA008")
            .WithMessage("City is required.")
            .MaximumLength(100)
            .WithErrorCode("UA009")
            .WithMessage("City cannot exceed 100 characters.");

        RuleFor(c => c.PostalCode)
            .NotEmpty()
            .WithErrorCode("UA010")
            .WithMessage("Postal code is required.")
            .MaximumLength(20)
            .WithErrorCode("UA011")
            .WithMessage("Postal code cannot exceed 20 characters.");

        RuleFor(c => c.Country)
            .NotEmpty()
            .WithErrorCode("UA012")
            .WithMessage("Country is required.")
            .MaximumLength(100)
            .WithErrorCode("UA013")
            .WithMessage("Country cannot exceed 100 characters.");

        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .WithErrorCode("UA014")
            .WithMessage("Phone number is required.")
            .MaximumLength(20)
            .WithErrorCode("UA015")
            .WithMessage("Phone number cannot exceed 20 characters.");
    }
}
