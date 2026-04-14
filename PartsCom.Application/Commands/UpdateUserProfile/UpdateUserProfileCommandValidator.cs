using FluentValidation;

namespace PartsCom.Application.Commands.UpdateUserProfile;

internal sealed class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithErrorCode("UUP001")
            .WithMessage("User ID is required.");

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .WithErrorCode("UUP002")
            .WithMessage("First name is required.")
            .MaximumLength(100)
            .WithErrorCode("UUP003")
            .WithMessage("First name cannot exceed 100 characters.");

        RuleFor(c => c.LastName)
            .NotEmpty()
            .WithErrorCode("UUP004")
            .WithMessage("Last name is required.")
            .MaximumLength(100)
            .WithErrorCode("UUP005")
            .WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(c => c.PhoneNumber)
            .MaximumLength(20)
            .WithErrorCode("UUP006")
            .WithMessage("Phone number cannot exceed 20 characters.")
            .When(c => c.PhoneNumber != null);

        RuleFor(c => c.City)
            .MaximumLength(100)
            .WithErrorCode("UUP007")
            .WithMessage("City cannot exceed 100 characters.")
            .When(c => c.City != null);

        RuleFor(c => c.AvatarUrl)
            .MaximumLength(500)
            .WithErrorCode("UUP008")
            .WithMessage("Avatar URL cannot exceed 500 characters.")
            .When(c => c.AvatarUrl != null);
    }
}
